//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MoviePortal.v1._1
{
    using System;
    using System.Collections.Generic;
    
    public partial class MovieInfo1
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Rated { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Awards { get; set; }
        public string Poster { get; set; }
        public string MetaScore { get; set; }
        public string ImdbRating { get; set; }
        public string ImdbVotes { get; set; }
        public string imdbId { get; set; }
        public string Type { get; set; }
        public string TomatoMeter { get; set; }
        public string TomatoImage { get; set; }
        public string TomatoRating { get; set; }
        public string TomatoUserMeter { get; set; }
        public string TomatoUserRating { get; set; }
        public string TomatoUserReviews { get; set; }
        public string TomatoReviews { get; set; }
        public string TomatoFresh { get; set; }
        public string TomatoRotten { get; set; }
        public string TomatoConsensus { get; set; }
        public string TomatoURL { get; set; }
        public string DVD { get; set; }
        public string BoxOffice { get; set; }
        public string Production { get; set; }
        public string Website { get; set; }
        public string Response { get; set; }
        public string TotalSeasons { get; set; }
        public string FurtherImage { get; set; }
        public string Status { get; set; }
        public string Network { get; set; }
        public string Tagline { get; set; }
        public string Budget { get; set; }
    
        public virtual MovieInfoes_NewTV MovieInfoes_NewTV { get; set; }
        public virtual MovieInfoes_PopularMovieInfo MovieInfoes_PopularMovieInfo { get; set; }
        public virtual MovieInfoes_ReleaseMovie MovieInfoes_ReleaseMovie { get; set; }
        public virtual MovieInfoes_TopTV MovieInfoes_TopTV { get; set; }
        public virtual MovieInfoes_WatchList MovieInfoes_WatchList { get; set; }
    }
}
