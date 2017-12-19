using Movie.Models;
using System;
using System.Data.Entity;
using System.Diagnostics;

namespace Movie.DataLayer {
    public class MyContext : DbContext {
        private static System.Data.Entity.SqlServer.SqlProviderServices _instance =
           System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        /*public MyContext() : base(String.Format("Data Source=(LocalDB)\\v11.0;" +
                "AttachDbFilename=\"{0}\\Movie.mdf\";Integrated Security=True;Connect Timeout=30", 
                System.IO.Directory.GetCurrentDirectory())) {
        }*/

        /*Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\student\Documents\movie.mdf;
        Integrated Security=True;Connect Timeout=30*/

        public MyContext() : base("MovieDB2") {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
            Debug.Write(Database.Connection.ConnectionString);
            Database.CreateIfNotExists();
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Popular> Populars { get; set; }
    }
}
