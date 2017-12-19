using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using Movie.Models;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.Find;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace Movie.ConnectionLayer {
    public class Connector {
        private static Connector _instance;
        public string ApiKey = @"ef7e1eac6f5470cf7aa4b130a93c27f4";
        public TMDbClient Client;
        public void FetchConfig() {
            FileInfo configJson = new FileInfo("config.json");

            if (configJson.Exists && configJson.LastWriteTimeUtc >= DateTime.UtcNow.AddHours(-1)) {
                string json = File.ReadAllText(configJson.FullName, Encoding.UTF8);
                Client.SetConfig(JsonConvert.DeserializeObject<TMDbConfig>(json));
            }
            else {
                Client.GetConfig();
                string json = JsonConvert.SerializeObject(Client.Config);
                File.WriteAllText(configJson.FullName, json, Encoding.UTF8);
            }
        }

        public static Connector Instance() {
            return _instance ?? (_instance = new Connector());
        }
        
        private Connector() {
            Client = new TMDbClient(ApiKey);
            FetchConfig();
        }

        public string GetJson(SearchMovie movie) {
            var details = new TMDbClient(ApiKey).GetMovieAsync(movie.Id).Result;
            var trailer = string.Format("http://api.themoviedb.org/3/movie/{0}/videos?api_key={1}", details.Id, ApiKey);
            var imdb = string.Format("http://www.omdbapi.com/?i={0}&plot=full&r=json&tomatoes=true", details.ImdbId);

            var req = new WebClient().DownloadString(imdb);
            var trailerdetails = new WebClient().DownloadString(trailer);

            int tx = trailerdetails.IndexOf("key");
            string ss = "";
            if (tx < trailerdetails.Length) {
                for (int i = tx + 6; trailerdetails[i] != '\"'; i++) {
                    ss += trailerdetails[i];
                }
            }

            req = req.Remove(req.Length - 1);
            req += ",\"Tagline\":\"" + details.Tagline + "\"";
            req += ",\"Budget\":\"" + details.Budget.ToString() + "\"";
            req += ",\"Trailer\":\"" + ss + "\"}";

            return req;
        }

        public MovieInfo GenerateInfo(string req) {
            MovieInfo movieInfo = JsonConvert.DeserializeObject<MovieInfo>(req);

            if (movieInfo.Response.Equals("False")) return null;
            return movieInfo;
        }

        public SearchContainer<SearchMovie> GetPopular() {
            return Client.DiscoverMoviesAsync().OrderBy(DiscoverMovieSortBy.PopularityDesc)
                .Query().Result;
        }

        public SearchContainer<SearchMovie> GetBrowse(int year,int order) {
            //"Popularity","Rating","Release Date","Title(A-Z)"
            if (order == 0)
                return Client.DiscoverMoviesAsync()
                        .WherePrimaryReleaseIsInYear(year)
                        .OrderBy(DiscoverMovieSortBy.PopularityDesc)
                        .Query().Result;
            if(order==1)
                return Client.DiscoverMoviesAsync()
                        .WherePrimaryReleaseIsInYear(year)
                        .WhereVoteCountIsAtLeast(50)
                        .OrderBy(DiscoverMovieSortBy.VoteAverageDesc)
                        .Query().Result;
            if (order == 2)
                return Client.DiscoverMoviesAsync()
                        .WherePrimaryReleaseIsInYear(year)
                        .WhereVoteAverageIsAtLeast(7.0)
                        .WhereVoteCountIsAtLeast(50)
                        .OrderBy(DiscoverMovieSortBy.PrimaryReleaseDateDesc)
                        .Query().Result;
            return Client.DiscoverMoviesAsync()
                    .WherePrimaryReleaseIsInYear(year)
                    .WhereVoteAverageIsAtLeast(7.0)
                    .WhereVoteCountIsAtLeast(50)
                    .OrderBy(DiscoverMovieSortBy.OriginalTitleDesc)
                    .Query().Result;
        }

        public  Bitmap GetImage(string url) {
            try {
                var image=Image.FromStream(new WebClient().OpenRead(url));
                return new Bitmap(image);
            }
            catch (Exception ex) {
                return null;
            }
        }

        public SearchContainer<SearchMovie> SearchInfo(string text) {
            return Client.SearchMovieAsync(text).Result;
        }

        public FindContainer FindByImdb(string imdb) {
            return Client.FindAsync(FindExternalSource.Imdb, imdb).Result;
        }
        /*public void PopulateHomeAsync() {
            var populars = Client.DiscoverMoviesAsync().OrderBy(DiscoverMovieSortBy.PopularityDesc)
                .Query().Result;
            foreach (var movie in populars.Results) {
                if(HomeTileList.Count==10) break;

                HomeTileList.Add(GenerateInfo(movie));
            }
        }*/
    }
}
