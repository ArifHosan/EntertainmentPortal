using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movie.Models;

namespace Movie.DataLayer {
    public class PopularAccess {
        public List<Popular> GetAllPopulars() {
            try {
                using (var context = new MyContext()) {
                    return context.Populars
                        .ToList();
                }
            }
            catch (Exception ex) {
                return null;
            }
        }

        public void AddPopulars(List<Popular>jsonList ) {
            try {
                using (var context = new MyContext()) {
                    context.Populars.AddRange(jsonList);
                    context.SaveChanges();
                }
            }
            catch (Exception ex) { }
        }
    }
}
