using System.Collections.Generic;
using MetroFramework.Controls;
using MetroFramework.Forms;

namespace MoviePortal.v1._1
{
    public partial class Browse : MetroForm
    {
        public List<MetroTile> popularTiles = new List<MetroTile>();
        public List<MetroTile> releaseTiles = new List<MetroTile>();
        public List<MetroTile> TheatreTiles = new List<MetroTile>();

        public List<MetroTile> PopularTvTiles = new List<MetroTile>();
        public List<MetroTile> TopTvTiles = new List<MetroTile>();
        public List<MetroTile> NewTvTiles = new List<MetroTile>();

        public List<MetroTile> Row1 = new List<MetroTile>();
        public List<MetroTile> Row2 = new List<MetroTile>();
        public List<MetroTile> Row3 = new List<MetroTile>();

        //public List<>

        public Browse()
        {
            InitializeComponent();

            Row1.Add(metroTile1); Row2.Add(metroTile8); Row3.Add(metroTile15);
            Row1.Add(metroTile2); Row2.Add(metroTile9); Row3.Add(metroTile16);
            Row1.Add(metroTile3); Row2.Add(metroTile10); Row3.Add(metroTile17);
            Row1.Add(metroTile4); Row2.Add(metroTile11); Row3.Add(metroTile18);
            Row1.Add(metroTile5); Row2.Add(metroTile12); Row3.Add(metroTile19);
            Row1.Add(metroTile6); Row2.Add(metroTile13); Row3.Add(metroTile20);
            Row1.Add(metroTile7); Row2.Add(metroTile14); Row3.Add(metroTile21);

            //PopulateStartupTiles();
        }

        /*public async void PopulateStartupTiles() {
            TMDbClient client = new TMDbClient(Bridge.ApiKey);
            Bridge.FetchConfig(client);
            var populars = client.DiscoverMoviesAsync().OrderBy(DiscoverMovieSortBy.PopularityDesc).Query().Result;
            var releases = client.DiscoverMoviesAsync()
                .OrderBy(DiscoverMovieSortBy.ReleaseDateDesc)
                .WhereReleaseDateIsBefore(DateTime.Today.AddYears(1))
                .Query().Result;
            var theatres =
                client.DiscoverMoviesAsync()
                    .OrderBy(DiscoverMovieSortBy.PrimaryReleaseDateDesc)
                    .WherePrimaryReleaseDateIsAfter(DateTime.Today.AddDays(-15))
                    .WhereReleaseDateIsBefore(DateTime.Today)
                    .Query().Result;

            var topTvs = client.DiscoverTvShowsAsync()
                .OrderBy(DiscoverTvShowSortBy.PopularityDesc)
                .Query().Result;

            var tv2 = client.DiscoverTvShowsAsync()
                .WhereVoteAverageIsAtLeast(8.0)
                .Query().Result;
            var tv3 = client.DiscoverTvShowsAsync()
                .WhereFirstAirDateIsInYear(2016)
                .Query().Result;

            for (int i = 0; i < 7; i++) {
                popularTiles.Add(new MetroTile());
                releaseTiles.Add(new MetroTile());
                TheatreTiles.Add(new MetroTile());

                TopTvTiles.Add(new MetroTile());
                PopularTvTiles.Add(new MetroTile());
                NewTvTiles.Add(new MetroTile());

                await Bridge.SetImage(populars.Results[i], popularTiles[i], client);
                await Bridge.SetImage(releases.Results[i], releaseTiles[i], client);
                await Bridge.SetImage(theatres.Results[i], TheatreTiles[i], client);

                await Bridge.SetImage(topTvs.Results[i], TopTvTiles[i], client);
                await Bridge.SetImage(tv2.Results[i], PopularTvTiles[i], client);
                await Bridge.SetImage(tv3.Results[i], NewTvTiles[i], client);
            }
            //this.Refresh();
        }

        public void SelectSource(int index) {
            if (index == 1) {
                metroLabel1.Text = "Popular Movies";
                metroLabel2.Text = "Upcoming Release";
                metroLabel3.Text = "In Theatres";
                for (int i = 0; i < 7; i++) {
                    Row1[i].Text = popularTiles[i].Text;
                    Row1[i].Tag = popularTiles[i].Tag;
                    Row1[i].TileImage = popularTiles[i].TileImage;

                    Row2[i].Text = releaseTiles[i].Text;
                    Row2[i].Tag = releaseTiles[i].Tag;
                    Row2[i].TileImage = releaseTiles[i].TileImage;

                    Row3[i].Text = TheatreTiles[i].Text;
                    Row3[i].Tag = TheatreTiles[i].Tag;
                    Row3[i].TileImage = TheatreTiles[i].TileImage;
                }
            }
            else {
                metroLabel1.Text = "Popular TV Shows";
                metroLabel2.Text = "Top TV Shows";
                metroLabel3.Text = "Recent TV Shows";
                for (int i = 0; i < 7; i++) {
                    Row1[i].Text = PopularTvTiles[i].Text;
                    Row1[i].Tag = PopularTvTiles[i].Tag;
                    Row1[i].TileImage = PopularTvTiles[i].TileImage;

                    Row2[i].Text = TopTvTiles[i].Text;
                    Row2[i].Tag = TopTvTiles[i].Tag;
                    Row2[i].TileImage = TopTvTiles[i].TileImage;

                    Row3[i].Text = NewTvTiles[i].Text;
                    Row3[i].Tag = NewTvTiles[i].Tag;
                    Row3[i].TileImage = NewTvTiles[i].TileImage;
                }
            }
        }*/

        private void Browse_Load(object sender, System.EventArgs e)
        {

        }
        /*
        private void metroTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bridge.Navigate(this, metroTabControl1.SelectedTab.Text);
        }

        private void metroTile1_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile1);
        }
        private void metroTile2_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile2);
        }
        private void metroTile3_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile3);
        }
        private void metroTile4_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile4);
        }
        private void metroTile5_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile5);
        }
        private void metroTile6_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile6);
        }
        private void metroTile7_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile7);
        }
        private void metroTile8_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile8);
        }
        private void metroTile9_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile9);
        }
        private void metroTile10_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile10);
        }
        private void metroTile11_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile11);
        }
        private void metroTile12_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile12);
        }
        private void metroTile13_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile13);
        }
        private void metroTile14_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile14);
        }
        private void metroTile15_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile15);
        }
        private void metroTile16_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile16);
        }
        private void metroTile17_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile17);
        }
        private void metroTile18_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile18);
        }
        private void metroTile19_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile19);
        }
        private void metroTile20_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile20);
        }
        private void metroTile21_Click(object sender, EventArgs e){
            Bridge.MovieDetails(metroTile21);
        }

        private void Browse_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e) {
            //Form1.form1.Show();
        }*/

    }
}
