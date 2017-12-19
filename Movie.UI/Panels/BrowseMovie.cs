using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using Movie.LogicLayer;

namespace Movie.UI.Panels {
    public partial class BrowseMovie : MetroUserControl {
        public string Year;
        public int Order;
        private readonly MyRepo _repo;
        //public List<HomeMovieInfoShort> BrowsePanels;
        private Home _ParentForm { get; set; }
        public BrowseMovie(Home homeForm) {
            InitializeComponent();
            _repo=new MyRepo();
            _ParentForm = homeForm;
            //BrowsePanels=new List<HomeMovieInfoShort>();
        }

        private void BrowseMovie_Load(object sender, EventArgs e) {
            metroComboBox1.MaxDropDownItems = 5;
            metroComboBox2.MaxDropDownItems = 5;
            for (int i = DateTime.Today.Year; i >= 1950; i--) {
                metroComboBox1.Items.Add(i.ToString());
            }
            metroComboBox2.Items.AddRange(new object[] {"Popularity","Rating","Release Date","Title(A-Z)"});

            metroComboBox1.SelectedIndex = 0;
            metroComboBox2.SelectedIndex = 0;
        }

        private void metroButton1_Click(object sender, EventArgs e) {
            Year = metroComboBox1.SelectedItem.ToString();
            Order = metroComboBox2.SelectedIndex;
            backgroundWorker1.RunWorkerAsync(_ParentForm.LoadingForm);

            //_ParentForm.LoadingForm.Show(Parent as Home);
            _ParentForm.LoadingForm.ShowDialog(Parent as Home);
            _ParentForm.LoadingForm.SetLocation();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            //BrowsePanels.Clear();
            _ParentForm.infoList.Clear();
            var browseMovie = _repo.GetBrowseList(Year,Order);
            for (int i = 0; i < Math.Min(10, browseMovie.Count); i++) {
                 _ParentForm.infoList.Add(new HomeMovieInfoShort(browseMovie[i],_ParentForm.UserName));
            }
            e.Result = e.Argument;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            Task.Run(() => _ParentForm.LoadPanels());

            var d = e.Result as Loading;
            //d?.Hide();
            if (d != null) d.Hide();
        }
    }
}
