using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework.Forms;

namespace MoviePortal.v1._1
{
    public partial class Browse : MetroForm {
        public static int CurIndex = 0;
        public List<MetroTile> tileList=new List<MetroTile>();

        public Browse()
        {
            InitializeComponent();
            tileList.Add(metroTile1); tileList.Add(metroTile2); tileList.Add(metroTile3); tileList.Add(metroTile4);
            tileList.Add(metroTile5); tileList.Add(metroTile6); tileList.Add(metroTile7); tileList.Add(metroTile8);
            tileList.Add(metroTile9); tileList.Add(metroTile10); tileList.Add(metroTile11); tileList.Add(metroTile12);
            tileList.Add(metroTile13); tileList.Add(metroTile14); tileList.Add(metroTile15); tileList.Add(metroTile16);
            tileList.Add(metroTile17); tileList.Add(metroTile18); tileList.Add(metroTile19); tileList.Add(metroTile20);
            tileList.Add(metroTile21);
        }

        private void Browse_Load(object sender, EventArgs e) {
            //Medium.PopulateDatabase();
            Task.Run(() => Medium.PopulateDatabase());
            Medium.PopulateHomeTiles(tileList);
            metroLabel1.Text = "Currently Trending";
            metroLabel2.Text = "Newly Released Movies";
            metroLabel3.Text = "New TV Shows";
            metroTabControl1.SelectedIndex = 0;
            Refresh();
        }

        private void metroTile1_MouseHover(object sender, EventArgs e) {
            MetroTile metro = (MetroTile) sender;
            //
        }

        private void metroTabControl1_SelectedIndexChanged(object sender, EventArgs e) {
            if (CurIndex == metroTabControl1.SelectedIndex) return;
            LoadPage(metroTabControl1.SelectedIndex);
            CurIndex = metroTabControl1.SelectedIndex;
        }

        private void LoadPage(int selectedIndex) {
            if (selectedIndex == 0) {
                Medium.PopulateHomeTiles(tileList);
                metroLabel1.Text = "Currently Trending";
                metroLabel2.Text = "Newly Released Movies";
                metroLabel3.Text = "New TV Shows";
                metroTabControl1.SelectedIndex = 0;
                Refresh();
            }
            else if (selectedIndex == 1) {
                Medium.PopulateMoviesTiles(tileList);
                metroLabel1.Text = "Currently Trending Movies";
                metroLabel2.Text = "";
                metroLabel3.Text = "Newly Released Movies";
                metroTabControl1.SelectedIndex = 1;
                Refresh();
            }
            else if (selectedIndex == 2) {
                Medium.PopulateTVTiles(tileList);
                metroLabel1.Text = "Currently Trending TV Shows";
                metroLabel2.Text = "";
                metroLabel3.Text = "New TV Shows";
                metroTabControl1.SelectedIndex = 2;
                Refresh();
            }
        }
    }
}
