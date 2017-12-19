using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Movie.Models;
//using Movie.LogicLayer;
using Movie.LogicLayer;
namespace Movie.UI {
    public partial class StartupSignupControl : UserControl {
        public Startup _ParentForm { get; set; }
        private MyRepo _repo;
        public StartupSignupControl() {
            InitializeComponent();
            _repo = new MyRepo();
        }

        private void metroButton1_Click(object sender, EventArgs e) {
            this.nameBox.Clear(); this.passBox.Clear();
            this.passBox2.Clear(); this.emailBox.Clear();
            this.userNameBox.Clear();

            _ParentForm.Text = "Log In";
            _ParentForm.Refresh();
            _ParentForm.metroPanel1.Controls.Clear();
            _ParentForm.metroPanel1.Controls.Add(_ParentForm.LoginControl);
        }

        private void createAccountButton_Click(object sender, EventArgs e) {
            if (!passBox.Text.Equals(passBox2.Text)) {
                MetroFramework.MetroMessageBox.Show(this, "Passwords don't match!!", "Error Occured",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            User user = new User();
            user.Name = nameBox.Text;
            user.Email = emailBox.Text;
            user.UserName = userNameBox.Text;
            user.Password = passBox.Text;
            string error;
            if(!_repo.AddUser(user,out error)) {
                MetroFramework.MetroMessageBox.Show(this, error, "Error Occured", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                MetroFramework.MetroMessageBox.Show(this, "Account Created!!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                metroButton1_Click(sender, e);
            }
        }

        private void passBox2_Click(object sender, EventArgs e) {

        }
    }
}
