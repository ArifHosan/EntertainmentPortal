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

namespace Movie.UI {
    public partial class StartupLoginControl : UserControl {
        public Startup _ParentForm { get; set; }
        private  MyRepo _repo;
        public StartupLoginControl() {
            InitializeComponent();
            _repo = new MyRepo();
        }

        private void loginButton_Click(object sender, EventArgs e) {
            string error;
            if(!_repo.verifyLogin(nameBox.Text,passBox.Text, out error)) {
                MetroFramework.MetroMessageBox.Show(this, error, "Error Occured",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MetroFramework.MetroMessageBox.Show(this, "Login Success!!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            _ParentForm.Visible = false;
            new Home(nameBox.Text).ShowDialog();
            _ParentForm.Close();
        }

        private void metroButton1_Click(object sender, EventArgs e) {
            this.passBox.Clear(); this.nameBox.Clear();
            _ParentForm.Text = "Sign Up";
            _ParentForm.Refresh();
            _ParentForm.metroPanel1.Controls.Clear();
            _ParentForm.metroPanel1.Controls.Add(_ParentForm.SignupControl);
        }
    }
}
