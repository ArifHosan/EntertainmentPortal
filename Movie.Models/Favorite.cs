using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie.Models {
    public class Favorite {
        public int Id { get; set; }
        public string Json { get; set; }
        [ForeignKey("User")]
        public string UserName { get; set; }

        public virtual User User { get; set; }
        //public string Title { get; set; }
    }
}
