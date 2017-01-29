using System;
using VDS.RDF.Query;

namespace Framework.Common
{
    public static class Helpers
    {

        public static string GetTitle(string title)
        {
            return title.Split('@')[0];
        }

        public static string GetDate(string date)
        {
            return date.Split('^')[0];
        }

        public static MovieModel Map(SparqlResult result)
        {
            return new  MovieModel
            {
                Resource = result.Value("s").ToString(),
                Title = GetTitle(result.Value("title").ToString()),
                Date = GetDate(result.Value("date").ToString()),
                GenreResource = result.Value("genre").ToString(),
                ImdbIdentifier = result.Value("imdb").ToString() // "http://www.imdb.com/title/" +
            };
        }

        public static MovieModelResponse MapResponse(SparqlResult result)
        {
            return new MovieModelResponse()
            {
                Id = new Guid(result.Value("id").ToString()),
                Resource = result.Value("s").ToString(),
                Title = GetTitle(result.Value("title").ToString()),
                Date = GetDate(result.Value("date").ToString()),
                GenreResource = result.Value("genre").ToString(),
                GenreLabel = result.Value("label").ToString(),
                ImdbIdentifier = result.Value("imdb").ToString(),
                ImdbLink = "http://www.imdb.com/title/" + result.Value("imdb").ToString() 
            };
        }

        public static GenreModel MapGenreModel(SparqlResult result)
        {
            return  new GenreModel()
            {
                Id = new Guid(result.Value("id").ToString()),
                Resource = result.Value("genre").ToString(),
                Label = result.Value("label").ToString()
            };

        }
    }
}
