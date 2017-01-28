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
                GenreLabel = GetTitle(result.Value("label").ToString().ToLower()),
                ImdbLink = "http://www.imdb.com/title/" + result.Value("imdb").ToString()
            };
        }
    }
}
