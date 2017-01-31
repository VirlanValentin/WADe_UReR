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
            placesRestClient = new PlacesRestClient();
            moviesRestClient = new MoviesRestClient();
            userProfileRestClient = new UserProfileRestClient();
        }

        public UrerActionResult  GetLikedMovies(Guid userId)
        {
            var movieIds = userProfileRestClient.GetLikedMovies(userId).Data as IEnumerable<object>;
            var movies = GetMovies(movieIds);
            return new UrerActionResult(HttpStatusCode.Accepted, movies);
        }

        private List<object> GetMovies(IEnumerable<object> movieIds)
        {
            var movies = new List<object>();
            if (movieIds != null)
                movies.AddRange(movieIds.ToList()
                    .Select(movieId => new Guid(movieId.ToString()))
                    .Select(id => moviesRestClient.Get(id)));

            return movies;
        }

        private List<object> GetPlaces(IEnumerable<object> placesIds)
        {
            var places = new List<object>();
            if (placesIds != null)
                places.AddRange(placesIds.ToList() 
                    .Select(placeId => placeId)
                    .Select(id => placesRestClient.Get(id.ToString())));

            return places;
        }

        public UrerActionResult GetMoviePreferences(Guid userId)
        {
            var movieIds = userProfileRestClient.GetMoviePreferences(userId).Data as IEnumerable<object>;
            var movies = GetMovies(movieIds);
            return new UrerActionResult(HttpStatusCode.Accepted, movies);
        }

        public UrerActionResult GetPlacesPreferences(Guid userId)
        {
            var placesIds = userProfileRestClient.GetPlacesPreferences(userId).Data as List<object>;
            var places = GetPlaces(placesIds);

            return new UrerActionResult(HttpStatusCode.Accepted, places);
        }

        public UrerActionResult GetLikedPlaces(Guid userId)
        {
            var placesIds = userProfileRestClient.GetLikedPlaces(userId).Data as List<object>;
            var places = GetPlaces(placesIds);

            return new UrerActionResult(HttpStatusCode.Accepted, places);
        }
    }
}