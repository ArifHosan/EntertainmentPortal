using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie.Models {
    public class User {
        public string Name { get; set; }
        public string Email { get; set; }
        [Key]
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
