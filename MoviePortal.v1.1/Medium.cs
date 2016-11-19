using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MetroFramework.Controls;
using Newtonsoft.Json;
using TMDbLib.Client;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.TvShows;

namespace MoviePortal.v1._1 {
    class Medium {

        public static string ApiKey = @"ef7e1eac6f5470cf7aa4b130a93c27f4";

        public static void FetchConfig(TMDbClient client) {
            FileInfo configJson = new FileInfo("config.json");

            if (configJson.Exists && configJson.LastWriteTimeUtc >= DateTime.UtcNow.AddHours(-1)) {
                string json = File.ReadAllText(configJson.FullName, Encoding.UTF8);
                client.SetConfig(JsonConvert.DeserializeObject<TMDbConfig>(json));
            }
            else {
                client.GetConfig();

                string json = JsonConvert.SerializeObject(client.Config);
                File.WriteAllText(configJson.FullName, json, Encoding.UTF8);
            }
        }

        /*public static bool IsFoundInMovie(string name) {
            using (var context = new MovieFoundEntities()) {
                var list = context.MovieInfo1
                    .Where(c => c.Title.Equals(name));
                return (list.Any());
            }
        }*/

        public static bool IsFoundInPopular(string imdbid) {
            using (var context = new MovieFoundEntities()) {
                var list = context.MovieInfoes_PopularMovieInfo
                    .Where(c => c.MovieInfo.imdbId.Equals(imdbid));
                return (list.Any());
            }
        }

        public static bool IsFoundInRelease(string imdbid) {
            using (var context = new MovieFoundEntities()) {
                var list = context.MovieInfoes_ReleaseMovie
                    .Where(c => c.MovieInfo.imdbId.Equals(imdbid));
                return (list.Any());
            }
        }

        private static bool IsFoundInTopTv(string tvsDetailsName) {
            using (var context = new MovieFoundEntities()) {
                var list = context.MovieInfoes_TopTV
                    .Where(c => c.MovieInfo.Title.Equals(tvsDetailsName));
                return (list.Any());
            }
        }

        private static bool IsFoundInNewTv(string tvsDetailsName) {
            using (var context = new MovieFoundEntities()) {
                var list = context.MovieInfoes_NewTV
                    .Where(c => c.MovieInfo.Title.Equals(tvsDetailsName));
                return (list.Any());
            }
        }

        public static MovieInfo1 returnMovie(string name) {
            using (var context = new MovieFoundEntities()) {
                var list = context.MovieInfo1
                    .Where(c => c.Title.Equals(name))
                    .Where()
                foreach (var a in list) {
                    return a;
                }
            }
            return null;
        }

        public static Bitmap GetImage(string poster,string poster2, Size size) {
            Image image = null;
            if (poster != null && poster.Length > 10) 
                image = Image.FromStream(new WebClient().OpenRead(poster));
            else if(poster2!=null)
                image = Image.FromStream(new WebClient().OpenRead(poster2));

            try {
                Bitmap newBitmap = new Bitmap(image, size);
                return newBitmap;
            }
            catch (Exception e) {
                return null;
            }
        }

        public static MovieInfo1 BuildMovie(TMDbClient client,Movie moviesDetails,Movie moviesImages) {
            string search = string.Format("http://www.omdbapi.com/?i={0}&plot=full&r=json&tomatoes=true", moviesDetails.ImdbId);
            var req = new WebClient().DownloadString(search);

            MovieInfo1 movie = JsonConvert.DeserializeObject<MovieInfo1>(req);
            if (movie.Response.Equals("False")) return null;
            //MovieInfoes_PopularMovieInfo popularMovie=new MovieInfoes_PopularMovieInfo();
            //popularMovie.MovieInfo = movie;
            movie.Tagline = moviesDetails.Tagline;
            movie.Budget=moviesDetails.Budget.ToString();
            //int x = client.Config.Images.PosterSizes.Count;
            //int y = moviesImages.Images.Posters.Count;
            if (client.Config.Images.PosterSizes.Count > 0 && moviesImages.Images.Posters.Count > 0) {
                var size = client.Config.Images.PosterSizes[1];
                var path = moviesImages.Images.Posters[0].FilePath;
                var add = client.GetImageUrl(size, path);
                movie.FurtherImage = add.ToString();
            }
            return movie;
        }

        public static MovieInfo1 BuildTv(TMDbClient client, TvShow moviesDetails, TvShow moviesImages) {
            string search = string.Format("http://www.omdbapi.com/?t={0}&plot=full&r=json&tomatoes=true", moviesDetails.Name);
            var req = new WebClient().DownloadString(search);

            MovieInfo1 tv = JsonConvert.DeserializeObject<MovieInfo1>(req);
            if (tv.Response.Equals("False")) return null;
            //MovieInfoes_PopularMovieInfo popularMovie=new MovieInfoes_PopularMovieInfo();
            //popularMovie.MovieInfo = movie;
            if (moviesDetails.Networks.Any()) tv.Network = moviesDetails.Networks[0].Name;
            tv.Status = moviesDetails.Status;
            //int x = client.Config.Images.PosterSizes.Count;
            //int y = moviesImages.Images.Posters.Count;
            if (client.Config.Images.PosterSizes.Count > 0 && moviesImages.Images.Posters.Count > 0) {
                var size = client.Config.Images.PosterSizes[1];
                var path = moviesImages.Images.Posters[0].FilePath;
                var add = client.GetImageUrl(size, path);
                tv.FurtherImage = add.ToString();
            }
            return tv;
        }

        public static void PopulateDatabase() {
            TMDbClient client = new TMDbClient(ApiKey);
            FetchConfig(client);

            var PopularMovies = client.DiscoverMoviesAsync()
                .WherePrimaryReleaseIsInYear(DateTime.Today.Year)
                .OrderBy(DiscoverMovieSortBy.PopularityDesc)
                .Query().Result;
            var popularMoviesList = PopularMovies.Results.Take(Math.Min(15,PopularMovies.Results.Count)).Reverse();

            var releaseMovies = client.DiscoverMoviesAsync()
                .WherePrimaryReleaseIsInYear(DateTime.Today.Year)
                .OrderBy(DiscoverMovieSortBy.ReleaseDateDesc)
                .Query().Result;
            var releaseMoviesList = releaseMovies.Results.Take(Math.Min(15, releaseMovies.Results.Count)).Reverse();

            var newTvs = client.DiscoverTvShowsAsync()
                .WhereFirstAirDateIsInYear(DateTime.Today.Year)
                .OrderBy(DiscoverTvShowSortBy.FirstAirDateDesc)
                .Query().Result;
            var newTvList = newTvs.Results.Take(Math.Min(15, newTvs.Results.Count)).Reverse();

            var topTvs = client.DiscoverTvShowsAsync()
                .OrderBy(DiscoverTvShowSortBy.PopularityDesc)
                .Query().Result;
            var topTVList = topTvs.Results.Take(Math.Min(15, topTvs.Results.Count)).Reverse();

            foreach (var a in popularMoviesList) {
                var moviesImages = client.GetMovieAsync(a.Id, MovieMethods.Images).Result;
                var moviesDetails = client.GetMovieAsync(a.Id).Result;

                if (!IsFoundInPopular(moviesDetails.ImdbId)) {
                    var movie = returnMovie(moviesDetails.Title);
                    MovieInfoes_PopularMovieInfo movieInfo = new MovieInfoes_PopularMovieInfo();
                    if (movie != null) movieInfo.MovieInfo = movie;
                    else movieInfo.MovieInfo = BuildMovie(client, moviesDetails, moviesImages);
                    if (movieInfo.MovieInfo != null) {
                        using (var context = new MovieFoundEntities()) {
                            context.MovieInfoes_PopularMovieInfo.Add(movieInfo);
                            //Task.Run(() => context.SaveChangesAsync());
                            context.SaveChanges();
                        }
                    }
                }
            }
            foreach (var a in releaseMoviesList) {
                var moviesImages = client.GetMovieAsync(a.Id, MovieMethods.Images).Result;
                var moviesDetails = client.GetMovieAsync(a.Id).Result;

                if (!IsFoundInPopular(moviesDetails.ImdbId)) {
                    var movie = returnMovie(moviesDetails.Title);
                    MovieInfoes_ReleaseMovie movieInfo = new MovieInfoes_ReleaseMovie();
                    if (movie != null && returnMovie(movie.imdbId)==null) movieInfo.MovieInfo = movie;
                    else movieInfo.MovieInfo = BuildMovie(client, moviesDetails, moviesImages);
                    if (movieInfo.MovieInfo != null) {
                        using (var context = new MovieFoundEntities()) {
                            context.MovieInfoes_ReleaseMovie.Add(movieInfo);
                            //Task.Run(() => context.SaveChangesAsync());
                            context.SaveChanges();
                        }
                    }
                }
            }
            foreach (var a in newTvList) {
                var tvsImage = client.GetTvShowAsync(a.Id, TvShowMethods.Images).Result;
                var tvsDetails = client.GetTvShowAsync(a.Id).Result;

                if (!IsFoundInNewTv(tvsDetails.Name)) {
                    var movie = returnMovie(tvsDetails.Name);
                    MovieInfoes_NewTV movieInfo = new MovieInfoes_NewTV();
                    if (movie != null) movieInfo.MovieInfo = movie;
                    else movieInfo.MovieInfo = BuildTv(client, tvsDetails, tvsImage);
                    if (movieInfo.MovieInfo != null) { 
                        using (var context = new MovieFoundEntities()) {
                            context.MovieInfoes_NewTV.Add(movieInfo);
                            //Task.Run(() => context.SaveChangesAsync());
                            context.SaveChanges();
                        }
                    }
                }
            }
            foreach (var a in topTVList) {
                var tvsImage = client.GetTvShowAsync(a.Id, TvShowMethods.Images).Result;
                var tvsDetails = client.GetTvShowAsync(a.Id).Result;

                if (!IsFoundInTopTv(tvsDetails.Name)) {
                    var movie = returnMovie(tvsDetails.Name);
                    MovieInfoes_TopTV movieInfo = new MovieInfoes_TopTV();
                    if (movie != null) movieInfo.MovieInfo = movie;
                    else movieInfo.MovieInfo = BuildTv(client, tvsDetails, tvsImage);
                    if (movieInfo.MovieInfo != null) {
                        using (var context = new MovieFoundEntities()) {
                            context.MovieInfoes_TopTV.Add(movieInfo);
                            //Task.Run(() => context.SaveChangesAsync());
                            context.SaveChanges();
                        }
                    }
                }
            }
            
            /*using (var context = new MovieFoundEntities()) {
                try {
                    for (int i = 0; i < 15; i++) {
                        if (popularMoviesList.Count() > i) {
                            var moviesImages = client.GetMovieAsync(popularMoviesList.Id, MovieMethods.Images).Result;
                            var moviesDetails = client.GetMovieAsync(PopularMovies.Results[i].Id).Result;

                            if (!IsFoundInPopular(moviesDetails.ImdbId)) {
                                MovieInfoes_PopularMovieInfo movieInfo = new MovieInfoes_PopularMovieInfo();
                                movieInfo.MovieInfo = BuildMovie(client, moviesDetails, moviesImages);
                                if (movieInfo.MovieInfo != null) {
                                    //context.MovieInfo1.Add(movieInfo.MovieInfo);
                                    context.MovieInfoes_PopularMovieInfo.Add(movieInfo);
                                    //Task.Run(() => context.SaveChangesAsync());
                                    context.SaveChanges();
                                }
                                /*try {
                                    Console.WriteLine(context.SaveChanges());
                                }
                                catch (DbEntityValidationException e) {
                                    foreach (var eve in e.EntityValidationErrors) {
                                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                        foreach (var ve in eve.ValidationErrors) {
                                            Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                                ve.PropertyName,
                                                eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                                ve.ErrorMessage);
                                        }
                                    }
                                    throw;
                                }
        }
                        }

                        if (releaseMovies.Results.Count > i) {
                            var moviesImages = client.GetMovieAsync(releaseMovies.Results[i].Id, MovieMethods.Images).Result;
                            var moviesDetails = client.GetMovieAsync(releaseMovies.Results[i].Id).Result;
                            if (!IsFoundInRelease(moviesDetails.ImdbId)) {
                                MovieInfoes_ReleaseMovie movieInfo = new MovieInfoes_ReleaseMovie();
                                movieInfo.MovieInfo = BuildMovie(client, moviesDetails, moviesImages);
                                if (movieInfo.MovieInfo != null) {
                                    context.MovieInfoes_ReleaseMovie.Add(movieInfo);
                                    context.SaveChanges();
                                }
                            }
                        }
                        if (topTvs.Results.Count > i) {
                            var tvsImages = client.GetTvShowAsync(topTvs.Results[i].Id, TvShowMethods.Images).Result;
                            var tvsDetails = client.GetTvShowAsync(topTvs.Results[i].Id).Result;
                            if (!IsFoundInTopTv(tvsDetails.Name)) {
                                MovieInfoes_TopTV movieInfo = new MovieInfoes_TopTV();
                                movieInfo.MovieInfo = BuildTv(client, tvsDetails, tvsImages);
                                if (movieInfo.MovieInfo != null) {
                                    context.MovieInfoes_TopTV.Add(movieInfo);
                                    context.SaveChanges();
                                }
                            }
                        }
                        if (newTvs.Results.Count > i) {
                            var tvsImages = client.GetTvShowAsync(newTvs.Results[i].Id, TvShowMethods.Images).Result;
                            var tvsDetails = client.GetTvShowAsync(newTvs.Results[i].Id).Result;
                            if (!IsFoundInNewTv(tvsDetails.Name)) {
                                MovieInfoes_NewTV movieInfo = new MovieInfoes_NewTV();
                                movieInfo.MovieInfo = BuildTv(client, tvsDetails, tvsImages);
                                if (movieInfo.MovieInfo != null) {
                                    context.MovieInfoes_NewTV.Add(movieInfo);
                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                }
                catch (DbEntityValidationException e) {
                    foreach (var eve in e.EntityValidationErrors) {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors) {
                            Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                ve.PropertyName,
                                eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }*/
            //Console.WriteLine("All done");
        }

        public static async void PopulateHomeTiles(List<MetroTile> tileList) {
            using (var context = new MovieFoundEntities()) {
                var popularMovieList = context.MovieInfoes_PopularMovieInfo
                    .Select(c => c.MovieInfo)
                    //.OrderByDescending(c => c.ImdbRating);
                    .OrderByDescending(c => c.Id);

                var releaseMovieList = context.MovieInfoes_PopularMovieInfo
                    .Select(c => c.MovieInfo)
                    //.OrderByDescending(c => c.Released);
                    .OrderByDescending(c => c.Id);

                var topTvList = context.MovieInfoes_TopTV
                    .Select(c => c.MovieInfo)
                    //.OrderByDescending(c => c.ImdbRating);
                    .OrderByDescending(c => c.Id);

                var newTvList = context.MovieInfoes_NewTV
                    .Select(c => c.MovieInfo)
                    //.OrderByDescending(c => c.Released);
                    .OrderByDescending(c => c.Id);

                int i = 0;
                foreach (var a in popularMovieList) {
                    if (i == 4) break;
                    tileList[i].Text = a.Title;
                    tileList[i].Tag = a;
                    Bitmap image = await Task.Run(() => GetImage(a.Poster, a.FurtherImage, tileList[i].Size));
                    tileList[i].TileImage = image;
                    i++;
                }
                i = 4;
                foreach (var a in topTvList) {
                    if (i == 7) break;
                    tileList[i].Text = a.Title;
                    tileList[i].Tag = a;
                    Bitmap image = await Task.Run(() => GetImage(a.Poster, a.FurtherImage, tileList[i].Size));
                    tileList[i].TileImage = image;
                    i++;
                }
                i = 7;
                foreach (var a in releaseMovieList) {
                    if (i == 14) break;
                    tileList[i].Text = a.Title;
                    tileList[i].Tag = a;
                    Bitmap image = await Task.Run(() => GetImage(a.Poster, a.FurtherImage, tileList[i].Size));
                    tileList[i].TileImage = image;
                    i++;
                }
                i = 14;
                foreach (var a in newTvList) {
                    if (i == 21) break;
                    tileList[i].Text = a.Title;
                    tileList[i].Tag = a;
                    Bitmap image = await Task.Run(() => GetImage(a.Poster, a.FurtherImage, tileList[i].Size));
                    tileList[i].TileImage = image;
                    i++;
                }
            }
        }

        public static async void PopulateMoviesTiles(List<MetroTile> tileList) {
            using (var context = new MovieFoundEntities()) {
                var popularMovieList = context.MovieInfoes_PopularMovieInfo
                    .Select(c => c.MovieInfo)
                    //.OrderByDescending(c => c.ImdbRating);
                    .OrderByDescending(c => c.Id);

                var releaseMovieList = context.MovieInfoes_PopularMovieInfo
                    .Select(c => c.MovieInfo)
                    //.OrderByDescending(c => c.Released);
                    .OrderByDescending(c => c.Id);

                int i = 0;
                foreach (var a in popularMovieList) {
                    if (i == 14) break;
                    tileList[i].Text = a.Title;
                    tileList[i].Tag = a;
                    Bitmap image = await Task.Run(() => GetImage(a.Poster, a.FurtherImage, tileList[i].Size));
                    tileList[i].TileImage = image;
                    i++;
                }
                i = 14;
                foreach (var a in releaseMovieList) {
                    if (i == 21) break;
                    tileList[i].Text = a.Title;
                    tileList[i].Tag = a;
                    Bitmap image = await Task.Run(() => GetImage(a.Poster, a.FurtherImage, tileList[i].Size));
                    tileList[i].TileImage = image;
                    i++;
                }
            }
        }

        public static async void PopulateTVTiles(List<MetroTile> tileList) {
            using (var context = new MovieFoundEntities()) {
                var topTvList = context.MovieInfoes_TopTV
                    .Select(c => c.MovieInfo)
                    //.OrderByDescending(c => c.ImdbRating);
                    .OrderByDescending(c => c.Id);

                var newTvList = context.MovieInfoes_NewTV
                    .Select(c => c.MovieInfo)
                    //.OrderByDescending(c => c.Released);
                    .OrderByDescending(c => c.Id);

                int i = 0;
                foreach (var a in topTvList) {
                    if (i == 14) break;
                    tileList[i].Text = a.Title;
                    tileList[i].Tag = a;
                    Bitmap image = await Task.Run(() => GetImage(a.Poster, a.FurtherImage, tileList[i].Size));
                    tileList[i].TileImage = image;
                    i++;
                }
                i = 14;
                foreach (var a in newTvList) {
                    if (i == 21) break;
                    tileList[i].Text = a.Title;
                    tileList[i].Tag = a;
                    Bitmap image = await Task.Run(() => GetImage(a.Poster, a.FurtherImage, tileList[i].Size));
                    tileList[i].TileImage = image;
                    i++;
                }
            }
        }
    }
}
