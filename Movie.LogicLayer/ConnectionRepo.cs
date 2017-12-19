using Movie.ConnectionLayer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movie.Models;
using TMDbLib.Objects.Search;

namespace Movie.LogicLayer {
    public partial class MyRepo {
        private readonly Connector _connector;
        private Bitmap _tomatoImage;

        public List<string> GetPopularList() {
            List<Popular> jsonList = new List<Popular>();
            List<string> Tmovies = new List<string>();
            jsonList = _popularAccess.GetAllPopulars();
            if (jsonList == null || jsonList.Count < 10) {
                //jsonList.Clear();
                if (jsonList == null) jsonList = new List<Popular>();
                var popSearch = _connector.GetPopular();
                Tmovies = GetJsonsList(popSearch.Results);
                foreach (var json in Tmovies) {
                    jsonList.Add(new Popular(json));
                }
                _popularAccess.AddPopulars(jsonList);
            }
            else {
                foreach (var json in jsonList) {
                    Tmovies.Add(json.Json);
                }
            }
            return Tmovies;
            
        }

        public List<string> GetBrowseList(string year,int order) {
            string error = string.Empty;
            var browseMovies = _connector.GetBrowse(Convert.ToInt32(year), order);
            if (browseMovies.TotalResults == 0) error = "No Result Found!!";
            return GetJsonsList(browseMovies.Results);
        }

        public MovieInfo GetMovieInfo(SearchMovie searchMovie) {
            return  _connector.GenerateInfo(_connector.GetJson(searchMovie));
        }

        public MovieInfo GetMovieInfo(string json) {
            return _connector.GenerateInfo(json);
        }

        public Bitmap GetImage(string url) {
            var image=_connector.GetImage(url);
            return image;
        }

        public Bitmap TomatoImage(int x,int y) {
            Rectangle rect=new Rectangle(x*64,y*64,64,64);
            return _tomatoImage.Clone(rect,_tomatoImage.PixelFormat);
        }

        public List<string> GetSearchInfo(string text,out string error) {
            error = string.Empty;
            if (string.IsNullOrEmpty(text)) {
                error = "Search Box is Empty!";
                return null;
            }
            var movie = _connector.SearchInfo(text);
            if (movie != null) {
                return GetJsonsList(movie.Results);
            }
            else {
                error= "No Info Found!";
                return null;
            }
        }

        public List<SearchMovie> FindInfo(string imdb) {
            var find = _connector.FindByImdb(imdb);
            return find.MovieResults;
        }

        public List<string> GetJsonsList(List<SearchMovie> searchMovies ) {
            List<string> jsonList = new List<string>();
            foreach (var movies in searchMovies) {
                if (jsonList.Count == 10) break;
                var temp = _connector.GetJson(movies);
                jsonList.Add(temp);
            }
            return jsonList;
        }
    }
}
