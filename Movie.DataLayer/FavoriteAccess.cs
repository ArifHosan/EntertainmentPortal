using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movie.Models;

namespace Movie.DataLayer {
    public class FavoriteAccess {

        public FavoriteAccess() {
            try {
                using (var context = new MyContext()) {
                    context.Database.Exists();
                }
            }
            catch (Exception ex) {
            }
        }

        public bool InsertDb(Favorite fav) {
            try {
                using (var context = new MyContext()) {
                    context.Favorites.Add(fav);
                    return context.SaveChanges()>0;
                }
            }
            catch (Exception ex) {
                return false;
            }
        }

        public bool FindInFavorite(string text,string userName) {
            try {
                using (var context = new MyContext()) {
                    var res= context.Favorites
                        .Where(c => c.Json.Equals(text) && c.UserName.Equals(userName));
                    return res.Any();
                }
            }
            catch (Exception ex) {
                return false;
            }
        }

        public bool RemoveDb(string imdb,string userName) {
            try {
                using (var context = new MyContext()) {
                    context.Favorites
                        .Remove(context.Favorites.First(c => c.Json.Equals(imdb) && c.UserName.Equals(userName)));
                    return context.SaveChanges() > 0;
                }
            }
            catch (Exception ex) {
                return true;
            }
        }

        public List<string> GetFavorite(string userName) {
            try {
                using (var context = new MyContext()) {
                    return context.Favorites
                        .Where(c=>c.UserName.Equals(userName))
                        .Select(c=>c.Json)
                        .ToList();
                }
            }
            catch (Exception ex) {
                return null;
            }
        }
    }
}
