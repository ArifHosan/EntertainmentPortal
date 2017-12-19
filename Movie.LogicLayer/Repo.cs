using Movie.DataLayer;
using Movie.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Movie.ConnectionLayer;

namespace Movie.LogicLayer {
    public partial class MyRepo {
        private readonly UserAccess _userAccess;
        private readonly FavoriteAccess _favoriteAccess;
        private readonly PopularAccess _popularAccess;
        private static Regex NamePattern,UserNamePattern;

        private bool verifyName(string name, ref string error) {
            if (String.IsNullOrEmpty(name)) {
                error = "Name cannot be empty!!";
                return false;
            }
            if (!NamePattern.IsMatch(name)) {
                error = "Name is not valid!!";
                return false;
            }
            return true;
        } 

        private bool verifyEmail(string email, ref string error) {
            if (String.IsNullOrEmpty(email)) {
                error = "Email can't be empty!!";
                return false;
            }
            try {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch (Exception ex) {
                error = "Email is invalid";
                return false;
            }
        }

        private bool verifyUserName(string userName, ref string error) {
            if (String.IsNullOrEmpty(userName)) {
                error = "User Name cannot be empty!!";
                return false;
            }
            if (!UserNamePattern.IsMatch(userName)) {
                error = "User Name is not valid!!";
                return false;
            }
            if (_userAccess.isUser(userName)) {
                error = "User Name is not available!!";
                return false;
            }
            return true;
        }

        private bool verifyPass(string password,ref string error) {
            if(string.IsNullOrEmpty(password) || password.Length<6) {
                error = "Password must contain at least six characters!!";
                return false;
            }
            return true;
        }

        public MyRepo() {
            _tomatoImage = new Bitmap("icons-v2.png");
            _userAccess = new UserAccess();
            _favoriteAccess=new FavoriteAccess();
            _popularAccess=new PopularAccess();
            _connector = Connector.Instance();
            NamePattern = new Regex(@"^[A-Za-z]+$");
            UserNamePattern = new Regex(@"^[A-Za-z0-9]+$");
        }

        public bool AddUser(User user,out string error) {
            error = string.Empty;
            if (!verifyName(user.Name, ref error) || !verifyEmail(user.Email, ref error) ||
                !verifyUserName(user.UserName, ref error) || !verifyPass(user.Password, ref error)) {
                return false;
            }
            if (!_userAccess.InsertUserDb(user, out error)) return false;
            return true;
        }
        
        public bool verifyLogin(string name,string pass, out string error) {
            error = string.Empty;
            if (!_userAccess.isUser(name)) {
                error = "User name could not be found!!";
                return false;
            }
            if(!_userAccess.verifyPass(name, pass)) {
                error = "User name and Password don't match!!";
                return false;
            }
            return true;
        }

        public bool SetFav(string imdb,string userName) {
            Favorite fav = new Favorite { Json = imdb,UserName = userName};
            if (!IsFav(imdb,userName)) {
                return _favoriteAccess.InsertDb(fav);
            }
            return _favoriteAccess.RemoveDb(imdb,userName);
        }

        public bool IsFav(string imdb,string userName) {
            return _favoriteAccess.FindInFavorite(imdb,userName);
        }

        public List<string> GetAllFavorites(string userName) {
            return _favoriteAccess.GetFavorite(userName);
        }
    }
}
