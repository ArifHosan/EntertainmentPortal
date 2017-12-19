using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Models {
    public class Popular {
        public Popular() {
            
        }
        public Popular(string temp) {
            Json = temp;
        }

        public int Id { get; set; }
        public string Json { get; set; }
    }
}
