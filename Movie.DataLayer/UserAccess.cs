using Movie.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.DataLayer {
    public class UserAccess {
        public UserAccess() {
            try {
                using (var context = new MyContext()) {
                    context.Database.Exists();
                }
            }
            catch (Exception ex) {
            }
        }
        public bool InsertUserDb(User user, out string error) {
            error = string.Empty;
            try {
                using (var context=new MyContext()) {
                    context.Users.Add(user);
                    return Convert.ToInt32(context.SaveChanges()) > 0;
                }
            } catch(Exception ex) {
                error = ex.Message;
                return false;
            }
        }

        public bool isUser(string username) {
            try {
                using (var context = new MyContext()) {
                    return context.Users.Any(c => c.UserName.Equals(username));
                }
            }
            catch (Exception e) {
                return false;
            }
        }

        public bool verifyPass(string name, string pass) {
            try {
                using(var context=new MyContext()) {
                    var p=context.Users.Where(c => c.UserName.Equals(name))
                        .Select(c=>c.Password)
                        .ToList();
                    return p.Any(c => c.Equals(pass));
                }
            }
            catch(Exception e) {
                return false;
            }
        }
    }
}
