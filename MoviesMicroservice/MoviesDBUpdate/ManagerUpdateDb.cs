using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Common;
using VDS.RDF.Query;
using VDS.RDF.Storage;

namespace MoviesDBUpdate
{
    public class ManagerUpdateDb
    {
        public ManagerUpdateDb()
        {
            EndpointUri = new Uri("https://query.wikidata.org/sparql");
            Endpoint = new SparqlRemoteEndpoint(EndpointUri, "https://query.wikidata.org");
            Fuseki = new FusekiConnector("http://localhost:3030/movies/data");
        }

        public FusekiConnector Fuseki { get; set; }

        public SparqlRemoteEndpoint Endpoint { get; set; }

        public Uri EndpointUri { get; set; }

        public void UpdateDb(DateTime releaseDateDate, string genre)
        {

            var moviesToAddInDatabase = new List<MovieModel>();
            var queryString = new SparqlParameterizedString
            {
                CommandText = @"SELECT *
                            WHERE
                            {
                                ?s wdt:P31 wd:Q11424.
                                ?s wdt:P1476 ?title.
                                ?s wdt:P577 ?date.
                                ?s wdt:P136 ?genre.
                                ?genre rdfs:label ?label.
                                ?s wdt:P345 ?imdb
                                FILTER(year(?date) = @year && month(?date) = @month && day(?date) <= @upperDate && day(?date) >= lowerDate && langMatches(lang(?title)," +
                              "\"en\"" + ") && langMatches(lang(?label), " + "\"en\"" + "))" +
                              "} Limit 55"
            };

            queryString.SetLiteral("year", releaseDateDate.Year);
            queryString.SetLiteral("month", releaseDateDate.Month);
            queryString.SetLiteral("upperDate", releaseDateDate.Day + 7);////PROBLEM cu ziua cu + si cu -ll
            queryString.SetLiteral("lowerDate", releaseDateDate.Day - 7);

            var results = Endpoint.QueryWithResultSet(queryString.ToString());
            foreach (SparqlResult result in results) 
            {

                var newMovie = Helpers.Map(result);

                var genreExists =
                      moviesToAddInDatabase.Find(
                          movie =>
                              movie.GenreResource.Equals(newMovie.GenreResource) && movie.Resource.Equals(newMovie.Resource)) != null;
                //acelasi film(uri) cu acelasi gen

                if (!genreExists)
                    moviesToAddInDatabase.Add(newMovie);

                foreach (var movie in moviesToAddInDatabase)
                {
                    var moviesUpdateQuery = new SparqlParameterizedString();


                    moviesUpdateQuery.Namespaces.AddNamespace("wd", new Uri("http://www.wikidata.org/entity/"));
                    moviesUpdateQuery.Namespaces.AddNamespace("xsd", new Uri("http://www.w3.org/2001/XMLSchema#"));
                    moviesUpdateQuery.Namespaces.AddNamespace("wdt", new Uri("http://www.wikidata.org/prop/direct/"));

                    moviesUpdateQuery.CommandText = "INSERT DATA { < @resource> wdt:P1476 \"@title\" ; wdt:P577 \"@date\"^^xsd:dateTime; wdt:P136 <@genreResource>; wdt:P345 <@imdbLink> }";

                    moviesUpdateQuery.SetLiteral("resource", movie.Resource);
                    moviesUpdateQuery.SetLiteral("date", movie.Date);
                    moviesUpdateQuery.SetLiteral("title", movie.Title);
                    moviesUpdateQuery.SetLiteral("genreResource", movie.GenreResource);
                    moviesUpdateQuery.SetLiteral("imdbLink", movie.ImdbLink);

                    Fuseki.Update(moviesUpdateQuery.ToString());
                }
            }
        }

        public void AddGenres()
        {
            var queryForGenreTypesAtWikidata = new SparqlParameterizedString();
            queryForGenreTypesAtWikidata.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));

            queryForGenreTypesAtWikidata.CommandText = " SELECT* WHERE { ?genre wdt:P31 wd:Q201658. ?genre rdfs:label ?label FILTER(langMatches(lang(?label), \"en\")) }";

            var genresResults = Endpoint.QueryWithResultSet(queryForGenreTypesAtWikidata.ToString());

            var genreList = new List<GenreModel>();

            foreach (var result in genresResults)
            {
                var newGenre = new GenreModel
                {
                    Resource = result.Value("genre").ToString(),
                    Label = Helpers.GetTitle(result.Value("label").ToString().ToLower())
                };

                if (genreList.FirstOrDefault(x => x.Resource == newGenre.Resource) == null)
                {
                    genreList.Add(newGenre);
                }
            }

            foreach (var genreModel in genreList)
            {
                var moviesUpdateQuery2 = new SparqlParameterizedString();
                moviesUpdateQuery2.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
                moviesUpdateQuery2.CommandText = " INSERT DATA { <@resource>" + " rdfs:label " + "\"@date\"" + " }";

                moviesUpdateQuery2.SetLiteral("resource", genreModel.Resource);
                moviesUpdateQuery2.SetLiteral("date", genreModel.Label);

                Fuseki.Update(moviesUpdateQuery2.ToString());
            }
        }

        public void UpdateDb(string genre)
        {
            var queryString = new SparqlParameterizedString
            {
                CommandText = @"SELECT * WHERE
                            {
                                ?s wdt:P31 wd:Q11424.
                                ?s wdt:P1476 ?title.
                                ?s wdt:P577 ?date.
                                ?s wdt:P136 ?genre.
                                ?genre rdfs:label @genre" + "@en." + "?s wdt:P345 ?imdb} Limit 50"
            };

            queryString.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            queryString.Namespaces.AddNamespace("wdt", new Uri("http://www.wikidata.org/prop/direct/"));


            var results = Endpoint.QueryWithResultSet(queryString.ToString());
        }
    }
}
