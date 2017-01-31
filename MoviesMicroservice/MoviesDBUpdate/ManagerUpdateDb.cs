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
            Fuseki = new FusekiConnector("http://54.187.81.132:3030/movies/data");
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
                                ?genre rdfs:label @genre" + "@en. " +
                              "?s wdt:P345 ?imdb " +
                              "FILTER(year(?date) = @year && month(?date) = @month && langMatches(lang(?title)," +
                              "\"en\"" + "))" +
                              "} Limit 50"
            };

            queryString.SetLiteral("year", releaseDateDate.Year);
            queryString.SetLiteral("month", releaseDateDate.Month);

            queryString.SetLiteral("genre", genre);

            var results = Endpoint.QueryWithResultSet(queryString.ToString());
            foreach (SparqlResult result in results)
            {
                var newMovie = Helpers.Map(result);
                newMovie.GenreLabel = genre.ToLower();
                newMovie.Id = Guid.NewGuid();

                var genreExists =
                    moviesToAddInDatabase.Find(
                        movie =>
                            movie.GenreResource.Equals(newMovie.GenreResource) &&
                            movie.Resource.Equals(newMovie.Resource)) != null;
                //acelasi film(uri) cu acelasi gen

                if (!genreExists)
                    moviesToAddInDatabase.Add(newMovie);

                foreach (var movie in moviesToAddInDatabase)
                {
                    var moviesUpdateQuery = new SparqlParameterizedString();

                    moviesUpdateQuery.Namespaces.AddNamespace("wd", new Uri("http://www.wikidata.org/entity/"));
                    moviesUpdateQuery.Namespaces.AddNamespace("xsd", new Uri("http://www.w3.org/2001/XMLSchema#"));
                    moviesUpdateQuery.Namespaces.AddNamespace("wdt", new Uri("http://www.wikidata.org/prop/direct/"));
                    moviesUpdateQuery.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));

                    moviesUpdateQuery.CommandText =
                        "INSERT DATA {@resource urer:id @id; wdt:P31 wd:Q11424; wdt:P1476 @title; wdt:P577 @date^^xsd:dateTime; wdt:P136 @genreResource; wdt:P345 @imdbId }";


                    moviesUpdateQuery.SetLiteral("id", movie.Id.ToString());
                    moviesUpdateQuery.SetUri("resource", new Uri(movie.Resource));
                    moviesUpdateQuery.SetLiteral("date", movie.Date);
                    moviesUpdateQuery.SetLiteral("title", movie.Title);
                    moviesUpdateQuery.SetUri("genreResource", new Uri(movie.GenreResource));
                    moviesUpdateQuery.SetLiteral("imdbId", movie.ImdbIdentifier);

                    Fuseki.Update(moviesUpdateQuery.ToString());
                }
            }
        }

        public void AddGenres()
        {
            var genreNamesList = new List<string>
            {
                "science fiction animation",
                "science fiction action film",
                "live-action animated film",
                "christmas film",
                "short film",
                "ethnographic film",
                "action thriller",
                "superhero film",
                "musical comedy",
                "drama film",
                "science fiction anime",
                "fantasy film",
                "comedy film",
                "romantic thriller",
                "thriller film",
                "action comedy film",
                "horror film",
                "epic",
                "dance film",
                "action film",
                "adventure film",
                "war film",
                "historical film",
                "epic film"
            };


            var genreList = new List<GenreModel>();

            for (int i = 0; i < genreNamesList.Count; i++)
            {
                var queryForGenreTypesAtWikidata = new SparqlParameterizedString();

                queryForGenreTypesAtWikidata.Namespaces.AddNamespace("wd", new Uri("http://www.wikidata.org/entity/"));
                queryForGenreTypesAtWikidata.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
                queryForGenreTypesAtWikidata.Namespaces.AddNamespace("wdt", new Uri("http://www.wikidata.org/prop/direct/"));

                queryForGenreTypesAtWikidata.CommandText =
                    @"SELECT* WHERE { ?genre wdt:P31 wd:Q201658. ?genre rdfs:label ?label. ?genre rdfs:label @label" + "@en FILTER(langMatches(lang(?label), \"en\")) }";
                queryForGenreTypesAtWikidata.SetLiteral("label", genreNamesList[i]);

                var genresResults = Endpoint.QueryWithResultSet(queryForGenreTypesAtWikidata.ToString());

                foreach (var result in genresResults)
                {
                    var newGenre = new GenreModel
                    {
                        Resource = result.Value("genre").ToString(),
                        Label = Helpers.GetTitle(result.Value("label").ToString().ToLower()),
                        Id = Guid.NewGuid()
                    };

                    if (genreList.FirstOrDefault(x => x.Resource == newGenre.Resource) == null)
                    {
                        genreList.Add(newGenre);
                    }
                }
            }

            foreach (var genreModel in genreList)
            {
                var genreUpdateQuery = new SparqlParameterizedString();
                genreUpdateQuery.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
                genreUpdateQuery.Namespaces.AddNamespace("wdt", new Uri("http://www.wikidata.org/prop/direct/"));
                genreUpdateQuery.Namespaces.AddNamespace("wd", new Uri("http://www.wikidata.org/entity/"));
                genreUpdateQuery.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));


                genreUpdateQuery.CommandText = " INSERT DATA { @resource wdt:P31 wd:Q201658; rdfs:label @name; urer:id @id }";

                genreUpdateQuery.SetUri("resource", new Uri(genreModel.Resource));
                genreUpdateQuery.SetLiteral("name", genreModel.Label);
                genreUpdateQuery.SetLiteral("id", genreModel.Id.ToString());

                Fuseki.Update(genreUpdateQuery.ToString());
            }
        }

        public void UpdateDb(string genre)
        {
            var queryString = new SparqlParameterizedString
            {
                CommandText =
                    @"SELECT * WHERE { ?s wdt:P31 wd:Q11424.  ?s wdt:P1476 ?title. ?s wdt:P577 ?date. ?s wdt:P136 ?genre. ?genre rdfs:label @genre"
                    + "@en. " + "?s wdt:P345 ?imdb FILTER(langMatches(lang(?title)," +
                              "\"en\" ))} Limit 50"
            };

            queryString.SetLiteral("genre", genre);

            var results = Endpoint.QueryWithResultSet(queryString.ToString());
            var moviesToAddInDatabase = new List<MovieModel>();

            foreach (SparqlResult result in results)
            {
                var newMovie = Helpers.Map(result);
                newMovie.GenreLabel = genre.ToLower();
                newMovie.Id = Guid.NewGuid();

                var genreExists =
                    moviesToAddInDatabase.Find(
                        movie =>
                            movie.GenreResource.Equals(newMovie.GenreResource) &&
                            movie.Resource.Equals(newMovie.Resource)) != null;
                //acelasi film(uri) cu acelasi gen

                if (!genreExists)
                    moviesToAddInDatabase.Add(newMovie);
            }

            foreach (var movie in moviesToAddInDatabase)
            {
                var moviesUpdateQuery = new SparqlParameterizedString();

                moviesUpdateQuery.Namespaces.AddNamespace("wd", new Uri("http://www.wikidata.org/entity/"));
                moviesUpdateQuery.Namespaces.AddNamespace("xsd", new Uri("http://www.w3.org/2001/XMLSchema#"));
                moviesUpdateQuery.Namespaces.AddNamespace("wdt", new Uri("http://www.wikidata.org/prop/direct/"));
                moviesUpdateQuery.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));

                moviesUpdateQuery.CommandText =
                    "INSERT DATA {@resource urer:id @id; wdt:P31 wd:Q11424; wdt:P1476 @title; wdt:P577 @date^^xsd:dateTime; wdt:P136 @genreResource; wdt:P345 @imdbId }";

                moviesUpdateQuery.SetLiteral("id", movie.Id.ToString());
                moviesUpdateQuery.SetUri("resource", new Uri(movie.Resource));
                moviesUpdateQuery.SetLiteral("date", movie.Date);
                moviesUpdateQuery.SetLiteral("title", movie.Title);
                moviesUpdateQuery.SetUri("genreResource", new Uri(movie.GenreResource));
                moviesUpdateQuery.SetLiteral("imdbId", movie.ImdbIdentifier);

                Fuseki.Update(moviesUpdateQuery.ToString());
            }
        }
    }
}

