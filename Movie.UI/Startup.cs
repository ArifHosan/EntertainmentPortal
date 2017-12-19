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
using Movie.LogicLayer;

namespace Movie.UI {
    public partial class Startup : MetroForm {
        public StartupLoginControl LoginControl;
        public StartupSignupControl SignupControl;

        public Startup() {
            InitializeComponent();
            LoginControl = new StartupLoginControl();
            SignupControl = new StartupSignupControl();
            SignupControl._ParentForm = this;
            LoginControl._ParentForm = this;
            this.metroPanel1.Controls.Add(LoginControl);
        }
    }
}
