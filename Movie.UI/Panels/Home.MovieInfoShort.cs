using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using Movie.LogicLayer;
using Movie.Models;
using TMDbLib.Objects.Search;

namespace Movie.UI.Panels {
    public partial class HomeMovieInfoShort : UserControl {
        private readonly MovieInfo _movie;
        private readonly string movieJson;
        public delegate void RefreshDelegate();
        public delegate Task SetImageDelegate(MetroTile tile,string url);
        private readonly MyRepo _repo;

        public HomeMovieInfoShort() {
            InitializeComponent();
            _repo=new MyRepo();
        }

        public HomeMovieInfoShort(string json,string name) {
            InitializeComponent();
            movieJson = json;
            _repo = new MyRepo();
            _movie = _repo.GetMovieInfo(json);
            UserName = name;
        }

        public string UserName { get; set; }

        private void metroLabel3_Click(object sender, EventArgs e) {

        }

        public bool IsValid() {
            return _movie != null;
        }

        private void HomeMovieInfoShort_Load(object sender, EventArgs e) {
            try {
                metroLabel1.Text = _movie.Title;
                metroLabel2.Text = _movie.Genre;
                metroLabel3.Text = "(" + _movie.Year + ")";
                metroLabel4.Text = _movie.Plot + "...";
                if (!string.IsNullOrEmpty(_movie.Poster) && _movie.Poster.Length > 5) {
                    Task.Run(() => ApplyImageAsync(metroTile1, _movie.Poster));
                }
                this.Refresh();
            }
            catch(Exception ex) { }
        }

        private void metroButton1_Click(object sender, EventArgs e) {
            new Details(movieJson,UserName).ShowDialog();
        }

        public async Task ApplyImageAsync(MetroTile tile, string url) {
            if (tile.InvokeRequired) {
                SetImageDelegate d = ApplyImageAsync;
                Invoke(d, tile, url);
            }
            else {
                var image = await Task.Run(()=> _repo.GetImage(url));
                tile.TileImage = image;
                if (InvokeRequired) {
                    RefreshDelegate refresh = Refresh;
                    Invoke(refresh);
                }
                else Refresh();
            }
            //var image = _repo.GetImage(url);
            //return image.Result;
        }
    }
}
