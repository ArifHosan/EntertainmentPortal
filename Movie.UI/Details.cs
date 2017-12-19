using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework.Forms;
using Movie.LogicLayer;
using Movie.Models;

namespace Movie.UI {
    public partial class Details : MetroForm {
        private readonly MovieInfo _movie;
        private string Json;
        private readonly MyRepo _repo;

        public delegate void RefreshDelegate();
        public delegate void SetTextDelegate(MetroLabel lable,string s);
        public delegate void SetTextDelegate2(MetroTextBox lable, string s);
        public delegate void SetImageDelegate(MetroTile tile, Bitmap image);
        public Details(string json,string name) {
            InitializeComponent();
            Json = json;
            _repo=new MyRepo();
            _movie = _repo.GetMovieInfo(json);
            UserName = name;
        }

        public string UserName { get; set; }

        private void Details_Load(object sender, EventArgs e) {
            tableLayoutPanel1.HorizontalScroll.Visible = false;
            tableLayoutPanel1.HorizontalScroll.Enabled = false;
            tableLayoutPanel1.HorizontalScroll.Maximum = 0;
            tableLayoutPanel1.AutoScroll = true;

            flowLayoutPanel3.HorizontalScroll.Visible = false;
            flowLayoutPanel3.HorizontalScroll.Enabled = false;
            flowLayoutPanel3.HorizontalScroll.Maximum = 0;
            flowLayoutPanel3.AutoScroll = true;

            //tableLayoutPanel1.Visible = false;
            //metroTile1.Visible = false;
            metroLabel7.Text = "Director";
            metroLabel11.Text = "Writer";
            metroLabel12.Text = "Cast";
            metroLabel6.Text = "IMDB Details";
            metroLabel16.Text = "Rating";
            metroLabel17.Text = "RottenTomato Details";
            axShockwaveFlash1.Movie = "https://www.youtube.com/v/" + _movie.Trailer;
            axShockwaveFlash1.Height = 300;
            Task.Run(()=>Populate());
        }

        public void SetText(MetroLabel label, string text) {
            if (label.InvokeRequired) {
                SetTextDelegate d = SetText;
                Invoke(d, label, text);
            }
            else label.Text = text;
        }
        public void SetText2(MetroTextBox label, string text) {
            if (label.InvokeRequired) {
                SetTextDelegate2 d = SetText2;
                Invoke(d, label, text);
            }
            else label.Text = text;
        }

        public void SetImage(MetroTile tile, Bitmap image) {
            if (tile.InvokeRequired) {
                SetImageDelegate d = SetImage;
                Invoke(d, tile,image);
            }
            else
                tile.TileImage = image;
        }

        public void Populate() {
            SetText(metroLabel1, _movie.Title);
            SetText(metroLabel2, "(" + _movie.Year + ")");
            SetText(metroLabel3, _movie.Genre);
            SetText(metroLabel4, _movie.Rated);
            SetText(metroLabel5, _movie.Tagline);
            SetText2(metroTextBox1, _movie.Plot);
            SetText(metroLabel8, _movie.Writer);
            SetText(metroLabel9, _movie.Actors);
            SetText(metroLabel10, _movie.Director);
            SetText(metroLabel13, _movie.Awards);
            SetText(metroLabel15,_movie.ImdbRating+"/10");
            SetText(metroLabel14,"("+_movie.ImdbVotes+")");
            SetText(metroLabel25, _movie.TomatoMeter+"%");
            SetText2(metroTextBox2, _movie.TomatoConsensus);
            SetText(metroLabel26, _movie.TomatoUserMeter+"%");
            SetText(metroLabel23, "Fresh: "+_movie.TomatoFresh+"\nRotten: "+_movie.TomatoRotten+
                "\nTomato Reviews: "+_movie.TomatoReviews);
            SetText(metroLabel22, "Language: "+_movie.Language);
            SetText(metroLabel28, "Runtime: " + _movie.Runtime);
            SetText(metroLabel29, "Country: " + _movie.Country);
            SetText(metroLabel30, "Release: " + _movie.Released);
            SetText(metroLabel31, "Budget: " + _movie.Budget);
            SetText(metroLabel32, "Box Office: " + _movie.BoxOffice);
            
            if (_repo.IsFav(Json,UserName)) SetText(metroLabel21,"Remove From Favorites");

            SetImage(metroTile2, _movie.TomatoImage.Equals("rotten") ? _repo.TomatoImage(1, 0) : _repo.TomatoImage(0, 0));
            SetImage(metroTile3, _repo.TomatoImage(2,1));

            SetImage(metroTile1,ApplyImage(_movie.Poster));


            RefreshDelegate refresh = Refresh;
            Invoke(refresh);
            /*metroLabel1.Text =_movie.Title;
            metroLabel2.Text ="("+_movie.Year+")";
            metroLabel3.Text =_movie.Genre;
            metroLabel4.Text =_movie.Rated;
            metroLabel5.Text =_movie.Tagline;
            metroLabel6.Text =_movie.Plot;
            metroLabel8.Text = _movie.Writer;
            metroLabel9.Text =_movie.Actors;
            metroLabel10.Text =_movie.Director;
            metroLabel13.Text =_movie.Awards;*/
        }

        private void metroLabel21_Click(object sender, EventArgs e) {
            _repo.SetFav(Json,UserName);
            if (_repo.IsFav(Json,UserName)) {
                SetText(metroLabel21, "Remove From Favorites");
            }
            else SetText(metroLabel21, "Add To Favorites");
        }

        public Bitmap ApplyImage(string url) {
            var image = _repo.GetImage(url);
            return image;
        }
    }
}
