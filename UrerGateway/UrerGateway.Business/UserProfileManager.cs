using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UrerGateway.Business.Models;
using UrerGateway.Business.RestClients;

namespace UrerGateway.Business
{
    public class UserProfileManager
    {
        private readonly MoviesRestClient moviesRestClient;
        private readonly UserProfileRestClient userProfileRestClient;
        private PlacesRestClient placesRestClient;

        public UserProfileManager()
        {
            this.placesRestClient = new PlacesRestClient();
            this.moviesRestClient = new MoviesRestClient();
            this.userProfileRestClient = new UserProfileRestClient();
        }

        public UrerActionResult GetLikedMovies(Guid userId)
        {
            var movieIds = this.userProfileRestClient.GetLikedMovies(userId).Data as IEnumerable<object>;
            var movies = this.GetMovies(movieIds);
            return new UrerActionResult(HttpStatusCode.Accepted, movies);
        }

        private List<object> GetMovies(IEnumerable<object> movieIds)
        {
            var movies = new List<object>();
            if (movieIds != null)
                movies.AddRange(movieIds.ToList()
                    .Select(movieId => new Guid(movieId.ToString()))
                    .Select(id => this.moviesRestClient.Get(id)));

            return movies;
        }

        public UrerActionResult GetMoviePreferences(Guid userId)
        {
            var movieIds = this.userProfileRestClient.GetMoviePreferences(userId).Data as IEnumerable<object>;
            var movies = this.GetMovies(movieIds);
            return new UrerActionResult(HttpStatusCode.Accepted, movies);
        }

        public UrerActionResult GetPlacesPreferences(Guid userId)
        {
            var placesIds = this.userProfileRestClient.GetPlacesPreferences(userId).Data as List<Guid>;

            var places = new List<object>();

            // if (placesIds != null)
            //  places.AddRange(placesIds.Select(placeId => this.placesRestClient.Get(movieId)));

            return new UrerActionResult(HttpStatusCode.Accepted, places);
        }

        public UrerActionResult GetLikedPlaces(Guid userId)
        {
            var placesIds = this.userProfileRestClient.GetLikedPlaces(userId).Data as List<Guid>;

            var places = new List<object>();

            // if (placesIds != null)
            //places.AddRange(placesIds.Select(placeId => this.placesRestClient.Get(movieId)));

            return new UrerActionResult(HttpStatusCode.Accepted, places);
        }
    }
}