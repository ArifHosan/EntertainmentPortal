using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using Movie.LogicLayer;

namespace Movie.UI.Panels {
    public partial class SearchMovie : UserControl {
        private readonly MyRepo _repo;
        private Home _ParentForm { get; set; }
        public SearchMovie(Home home) {
            InitializeComponent();
            _repo=new MyRepo();
            _ParentForm = home;
            
        }

        private void metroButton1_Click(object sender, EventArgs e) {
            /*_ParentForm.infoList.Clear();
            string error;
            var searchList= _repo.GetSearchInfo(metroTextBox1.Text, out error);
            if (searchList == null) MetroMessageBox.Show(this, error, "An Error Occured!");*/
            backgroundWorker1.RunWorkerAsync(_ParentForm.LoadingForm);

            //_ParentForm.LoadingForm.Show(Parent as Home);
            _ParentForm.LoadingForm.ShowDialog(Parent as Home);
            _ParentForm.LoadingForm.SetLocation();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            _ParentForm.infoList.Clear();
            string error;
            var searchList = _repo.GetSearchInfo(metroTextBox1.Text, out error);
            if (searchList == null) MetroMessageBox.Show(this, error, "An Error Occured!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            else if(searchList.Count==0) MetroMessageBox.Show(this, "No Data Found", "An Error Occured!",MessageBoxButtons.OK,MessageBoxIcon.Information);
            else {
                for (int i = 0; i < Math.Min(10, searchList.Count); i++) {
                    var panel = new HomeMovieInfoShort(searchList[i], _ParentForm.UserName);
                    if(panel.IsValid())_ParentForm.infoList.Add(panel);
                }
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
