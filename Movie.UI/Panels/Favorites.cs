using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Movie.LogicLayer;

namespace Movie.UI.Panels {
    public partial class Favorites : UserControl {
        private readonly MyRepo _repo;
        private Home _ParentForm { get; set; }
        public Favorites(Home home) {
            InitializeComponent();
            _ParentForm = home;
            _repo=new MyRepo();

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            _ParentForm.infoList.Clear();
            string error;
            var allfavs = _repo.GetAllFavorites(_ParentForm.UserName);
            //var browseMovie = _repo.GetSearchInfo();
            for (int i = 0; i < Math.Min(10, allfavs.Count); i++) {
                _ParentForm.infoList.Add(new HomeMovieInfoShort(allfavs[i],_ParentForm.UserName));
            }
            e.Result = e.Argument;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            Task.Run(() => _ParentForm.LoadPanels());

            var d = e.Result as Loading;
            //d?.Hide();
            if (d != null) d.Hide();
        }

        public void ShowFavorites() {
            backgroundWorker1.RunWorkerAsync(_ParentForm.LoadingForm);

            //_ParentForm.LoadingForm.Show(Parent as Home);
            _ParentForm.LoadingForm.ShowDialog(Parent as Home);
            _ParentForm.LoadingForm.SetLocation();
        }
    }
}
