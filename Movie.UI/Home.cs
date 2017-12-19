using MetroFramework.Forms;
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
using Movie.LogicLayer;
using Movie.UI.Panels;
using TMDbLib.Objects.Search;

namespace Movie.UI {
    public partial class Home : MetroForm {
        private readonly MyRepo _repo;
        public readonly string UserName;
        //public delegate void AddControlDelegate(MetroPanel panel);
        public delegate void AddControlDelegate(HomeMovieInfoShort moviePanel);
        public delegate void ClearControlDelegate();
        public delegate void RefreshDelegate();
        public delegate void SetTextDelegate(string s);
        public List<MetroPanel> PanelList;
        public List<HomeMovieInfoShort> HomeTileList, infoList;

        //public Task LoadPanelTask;
        public Loading LoadingForm;
        public Home(string userName) {
            UserName = userName;
            InitializeComponent();
            _repo=new MyRepo();
            LoadingForm = new Loading();
            HomeTileList=new List<HomeMovieInfoShort>();
            infoList=new List<HomeMovieInfoShort>();
            PanelList = new List<MetroPanel> {
                metroPanel2,
                metroPanel1,
                metroPanel3,
                metroPanel4,
                metroPanel6,
                metroPanel5,
                metroPanel7,
                metroPanel8,
                metroPanel9,
                metroPanel10
            };
        }

        /*public Home(string nameBoxText) {
            throw new NotImplementedException();
        }*/

        private void Home_FormClosed(object sender, FormClosedEventArgs e) {

        }

        private void Home_Load(object sender, EventArgs e) { 
            tableLayoutPanel1.HorizontalScroll.Visible = false;
            tableLayoutPanel1.HorizontalScroll.Enabled = false;
            tableLayoutPanel1.HorizontalScroll.Maximum = 0;
            tableLayoutPanel1.AutoScroll = true;
            metroTabControl1.SelectedIndex = 0;
            metroTabControl1.TabPages[0].Text = "Home";
            metroTabControl1.TabPages[1].Text = "Browse";
            metroTabControl1.TabPages[2].Text = "Search";
            metroTabControl1.TabPages[3].Text = "Favorite";
            metroTabControl1.TabPages[4].Text = "About";
            //LoadPanelTask=Task.Run(() =>LoadPanels(HomeTileList));
            //SetText("Loading...Please Wait..");
            //LoadingForm.ShowDialog();
            //backgroundWorker1.RunWorkerAsync(LoadingForm);
            //LoadingForm.ShowDialog();
        }

        private void backgroundWorker1_DoWorkAsync(object sender, DoWorkEventArgs e) {
            /*var popularMovie = _repo.GetPopularList();
            for (int i = 0; i < Math.Min(10, popularMovie.Count); i++) {
                if (popularMovie[i] != null) {
                    var movie = _repo.GetMovieInfo(popularMovie[i]);
                    if (movie != null)
                        HomeTileList.Add(new HomeMovieInfoShort(movie,UserName));
                }
            }*/
            var pops = _repo.GetPopularList();
            for (int i = 0; i < Math.Min(10, pops.Count); i++) {
                if (pops[i] != null) {
                    HomeTileList.Add(new HomeMovieInfoShort(pops[i],UserName));
                }
            }
            e.Result = e.Argument;
            //await Task.Run(() => LoadHomePanels());
            //LoadHomePanels();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            //metroLabel1.Text = "";
            Task.Run(() => LoadHomePanels());
            var d = e.Result as Loading;
            //d?.Hide();
            if (d != null) d.Hide();
        }

        public void LoadPanels() {
            foreach (MetroPanel panel in PanelList) {
                ClearControlDelegate cd = panel.Controls.Clear;
                Invoke(cd);
            }
            //SetText("Loading..Please Wait!!");
            for (int i = 0; i < infoList.Count; i++) {
                if (PanelList[i].InvokeRequired) {
                    AddControlDelegate d = (PanelList[i].Controls.Add);
                    Invoke(d, infoList[i]);
                }
            }
            //SetText("");
            //LoadingForm.Hide();
        }

        public void LoadHomePanels() {
            //SetText("Loading..Please Wait!!");
            for (int i = 0; i < HomeTileList.Count; i++) {
                if (PanelList[i].InvokeRequired) {
                    ClearControlDelegate cd = PanelList[i].Controls.Clear;
                    Invoke(cd);
                    AddControlDelegate d = (PanelList[i].Controls.Add);
                    Invoke(d, HomeTileList[i]);
                }
            }
            //SetText("");
            //LoadingForm.Hide();
        }

        /*private void SetText(string text) {
            if (this.metroLabel1.InvokeRequired) {
                SetTextDelegate d = SetText;
                Invoke(d, text);
            }
            else this.metroLabel1.Text = text;
        }*/

        private void metroTabControl1_SelectedIndexChanged(object sender, EventArgs e) {
            tableLayoutPanel1.VerticalScroll.Value = 0;
            if (metroTabControl1.SelectedIndex == 1) {
                metroPanel11.Controls.Clear();
                metroPanel11.Controls.Add(new BrowseMovie(this));
                //tableLayoutPanel1.VerticalScroll.Value = 0;
                Task.Run(() => LoadHomePanels());
            }
            else if (metroTabControl1.SelectedIndex == 0) {
                //tableLayoutPanel1.VerticalScroll.Value = 0;
                metroPanel11.Controls.Clear();
                Task.Run(() => LoadHomePanels());

                //LoadingForm.Show(Parent as Home);
                //LoadingForm.SetLocation();

            }
            else if (metroTabControl1.SelectedIndex == 2) {
                foreach (var panel in PanelList) {
                    panel.Controls.Clear();
                }
                metroPanel11.Controls.Clear();
                metroPanel11.Controls.Add(new Panels.SearchMovie(this));
            }

            else if (metroTabControl1.SelectedIndex == 3) {
                metroPanel11.Controls.Clear();
                var fav = new Favorites(this);
                metroPanel11.Controls.Add(fav);
                fav.ShowFavorites();
            }
            else if (metroTabControl1.SelectedIndex == 4) {
                new About().ShowDialog();
                metroTabControl1.SelectedIndex = 0;
                metroPanel11.Controls.Clear();
                Task.Run(() => LoadHomePanels());
            }
        }

        private void Home_Shown(object sender, EventArgs e) {
            backgroundWorker1.RunWorkerAsync(LoadingForm);
            
            //LoadingForm.Show(Parent as Home);
            LoadingForm.ShowDialog(Parent as Home);
            LoadingForm.SetLocation();
        }

        private void Home_Resize(object sender, EventArgs e) {
            if (LoadingForm != null && LoadingForm.Visible) {
                LoadingForm.SetLocation();
            }
        }

        private void Home_Leave(object sender, EventArgs e) {

        }

        private void Home_Move(object sender, EventArgs e) {
            if (LoadingForm != null && LoadingForm.Visible) {
                LoadingForm.SetLocation();
            }
        }

    }
}
