using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Common;
using MoviesDBUpdate;
using VDS.RDF.Query;
using VDS.RDF.Storage;

namespace MoviesLogic
{
    public class MoviesManager
    {
        public MoviesManager()
        {
            ManagerUpdateDb = new ManagerUpdateDb();
            Fuseki = new FusekiConnector("http://54.187.81.132:3030/movies/data");
        }

        public FusekiConnector Fuseki { get; set; }

        public ManagerUpdateDb ManagerUpdateDb { get; set; }

        public List<MovieModelResponse> Get(DateTime releaseDateDate, string genre)
        {
           // ManagerUpdateDb.AddGenres();
           
            //Create a Parameterized String
            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            queryString.Namespaces.AddNamespace("wd", new Uri("http://www.wikidata.org/entity/"));
            queryString.Namespaces.AddNamespace("wdt", new Uri("http://www.wikidata.org/prop/direct/"));
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));

            //Set the SPARQL command
            //For more complex queries we can do this in multiple lines by using += on the
            queryString.CommandText = "SELECT * WHERE { ?s wdt:P31 wd:Q11424. ?s wdt:P1476 ?title. ?s wdt:P577 ?date. ?s wdt:P136 ?genre." +
                                      "?genre rdfs:label ?label.  ?genre rdfs:label @genre. ?s urer:id ?id. " + "?s wdt:P345 ?imdb" +
                    " FILTER(year(?date) = @year && month(?date) = @month) }";

            //Inject a Value for the parameter
            queryString.SetLiteral("year", releaseDateDate.Year);
            queryString.SetLiteral("month", releaseDateDate.Month);

            queryString.SetLiteral("genre", genre);

            //Query the collection, dump output
            var resultsFuseki = Fuseki.Query(queryString.ToString()) as SparqlResultSet;

            if (resultsFuseki?.Results.Count == 0)
            {
                ManagerUpdateDb.UpdateDb(releaseDateDate, genre);
                resultsFuseki = Fuseki.Query(queryString.ToString()) as SparqlResultSet;
            }

            var movies = new List<MovieModelResponse>();

            if (resultsFuseki != null)
                foreach (var result in resultsFuseki)
                {
                    var newMovie = Helpers.MapResponse(result);

                    newMovie.GenreLabel = genre;

                    var existingMovie = movies.FirstOrDefault(x => x.Resource == newMovie.Resource);
                    if (existingMovie == null)
                    {
                        newMovie.GenreLabelsList.Add(newMovie.GenreLabel);
                        movies.Add(newMovie);
                    }
                    else
                    {
                        if (!existingMovie.GenreLabelsList.Contains(newMovie.GenreLabel.ToLower()))
                            existingMovie.GenreLabelsList.Add(newMovie.GenreLabel.ToLower());
                    }

                }

            return movies;
        }

        public List<MovieModelResponse> Get(string genre)
        {

            //Create a Parameterized String
            var queryString = new SparqlParameterizedString
            {
                CommandText = @"SELECT *
                            WHERE
                            {
                                ?s wdt:P31 wd:Q11424.
                                ?s wdt:P1476 ?title.
                                ?s wdt:P577 ?date.
                                ?s wdt:P136 ?genre.
                                ?s urer:id ?id.
                                ?genre rdfs:label ?label. 
                                ?genre rdfs:label @genre. " +
                                "?s wdt:P345 ?imdb} Limit 50"
            };

            queryString.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            queryString.Namespaces.AddNamespace("wd", new Uri("http://www.wikidata.org/entity/"));
            queryString.Namespaces.AddNamespace("wdt", new Uri("http://www.wikidata.org/prop/direct/"));
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));

            //Inject a Value for the parameter
            queryString.SetLiteral("genre", genre);

            var resultsFuseki = Fuseki.Query(queryString.ToString()) as SparqlResultSet;

            if (resultsFuseki?.Results.Count == 0)
            {
                ManagerUpdateDb.UpdateDb(genre);
                resultsFuseki = Fuseki.Query(queryString.ToString()) as SparqlResultSet;
            }

            var movies = new List<MovieModelResponse>();

            if (resultsFuseki != null)
                foreach (var result in resultsFuseki)
                {
                    var newMovie = Helpers.MapResponse(result);

                    newMovie.GenreLabel = genre;

                    var existingMovie = movies.FirstOrDefault(x => x.Resource == newMovie.Resource);
                    if (existingMovie == null)
                    {
                        newMovie.GenreLabelsList.Add(newMovie.GenreLabel);
                        movies.Add(newMovie);
                    }
                    else
                    {
                        if (!existingMovie.GenreLabelsList.Contains(newMovie.GenreLabel.ToLower()))
                            existingMovie.GenreLabelsList.Add(newMovie.GenreLabel.ToLower());
                    }

                }

            return movies;
        }

        public List<MovieModelResponse> GetRelated(Guid id)
        {
            //Get genre of the movie
            var queryStringForGenre = new SparqlParameterizedString
            {
                CommandText = @"SELECT ?label
                            WHERE
                            {
                                ?s wdt:P31 wd:Q11424.
                                ?s urer:id @id.
                                ?s wdt:P136 ?genre.
                                ?genre rdfs:label ?label }"
            };

            queryStringForGenre.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            queryStringForGenre.Namespaces.AddNamespace("wd", new Uri("http://www.wikidata.org/entity/"));
            queryStringForGenre.Namespaces.AddNamespace("wdt", new Uri("http://www.wikidata.org/prop/direct/"));
            queryStringForGenre.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));

            //Inject a Value for the parameter
            queryStringForGenre.SetLiteral("id", id.ToString());

            var resultsFusekiForMovie = Fuseki.Query(queryStringForGenre.ToString()) as SparqlResultSet;

            var genreLabel = resultsFusekiForMovie[0].Value("label").ToString();

            //Create a Parameterized String
            var queryStringForMovies = new SparqlParameterizedString
            {
                CommandText = @"SELECT *
                            WHERE
                            {
                                ?s wdt:P31 wd:Q11424.
                                ?s wdt:P1476 ?title.
                                ?s wdt:P577 ?date.
                                ?s wdt:P136 ?genre.
                                ?s urer:id ?id.
                                ?genre rdfs:label ?label. 
                                ?genre rdfs:label @genre. " +
                                "?s wdt:P345 ?imdb FILTER (?id != @id) } Limit 50"
            };

            queryStringForMovies.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            queryStringForMovies.Namespaces.AddNamespace("wd", new Uri("http://www.wikidata.org/entity/"));
            queryStringForMovies.Namespaces.AddNamespace("wdt", new Uri("http://www.wikidata.org/prop/direct/"));
            queryStringForMovies.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));

            //Inject a Value for the parameter
            queryStringForMovies.SetLiteral("genre", genreLabel);
            queryStringForMovies.SetLiteral("id", id.ToString());

            var resultsFuseki = Fuseki.Query(queryStringForMovies.ToString()) as SparqlResultSet;

            if (resultsFuseki?.Results.Count == 0)
            {
                
                ManagerUpdateDb.UpdateDb(genreLabel);
                resultsFuseki = Fuseki.Query(queryStringForMovies.ToString()) as SparqlResultSet;
            }

            var movies = new List<MovieModelResponse>();

            if (resultsFuseki != null)
                foreach (var result in resultsFuseki)
                {
                    var newMovie = Helpers.MapResponse(result);

                    newMovie.GenreLabel = genreLabel;

                    var existingMovie = movies.FirstOrDefault(x => x.Resource == newMovie.Resource);
                    if (existingMovie == null)
                    {
                        newMovie.GenreLabelsList.Add(newMovie.GenreLabel);
                        movies.Add(newMovie);
                    }
                    else
                    {
                        if (!existingMovie.GenreLabelsList.Contains(newMovie.GenreLabel.ToLower()))
                            existingMovie.GenreLabelsList.Add(newMovie.GenreLabel.ToLower());
                    }

                }

            return movies;
        }

        public MovieModelResponse GetById(Guid id)
        {
            var queryString = new SparqlParameterizedString
            {
                CommandText = @"SELECT *
                            WHERE
                            {
                                ?s wdt:P31 wd:Q11424.
                                ?s wdt:P1476 ?title.
                                ?s wdt:P577 ?date.
                                ?s wdt:P136 ?genre.
                                ?s urer:id ?id.
                                ?genre rdfs:label ?label. 
                                ?s urer:id @id. " +
                              "?s wdt:P345 ?imdb} Limit 50"
            };


            queryString.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            queryString.Namespaces.AddNamespace("wd", new Uri("http://www.wikidata.org/entity/"));
            queryString.Namespaces.AddNamespace("wdt", new Uri("http://www.wikidata.org/prop/direct/"));
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));


            //Inject a Value for the parameter
            queryString.SetLiteral("id", id.ToString());

            var resultsFuseki = Fuseki.Query(queryString.ToString()) as SparqlResultSet;

            // ReSharper disable once PossibleNullReferenceException
            return resultsFuseki?.Count == 0 ? null : Helpers.MapResponse(resultsFuseki[0]);

        }

        public List<GenreModel> GetGenres()
        {
            ManagerUpdateDb.AddGenres();
            var queryString = new SparqlParameterizedString();

            queryString.Namespaces.AddNamespace("wd", new Uri("http://www.wikidata.org/entity/"));
            queryString.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            queryString.Namespaces.AddNamespace("wdt", new Uri("http://www.wikidata.org/prop/direct/"));
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));

            queryString.CommandText =
                @"SELECT * WHERE { ?genre wdt:P31 wd:Q201658. ?genre rdfs:label ?label. ?genre urer:id ?id }";

            var resultsFuseki = Fuseki.Query(queryString.ToString()) as SparqlResultSet;

            var genres = new List<GenreModel>();

            foreach (var result in resultsFuseki)
            {
                var newGenre = Helpers.MapGenreModel(result);

                genres.Add(newGenre);
            }

            return genres;
        }

        public GenreModel GetGenreById(Guid id)
        {
            var queryString = new SparqlParameterizedString();

            queryString.Namespaces.AddNamespace("wd", new Uri("http://www.wikidata.org/entity/"));
            queryString.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            queryString.Namespaces.AddNamespace("wdt", new Uri("http://www.wikidata.org/prop/direct/"));
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));

            queryString.CommandText =
                @"SELECT * WHERE { ?genre wdt:P31 wd:Q201658. ?genre rdfs:label ?label. ?genre urer:id @id. ?genre urer:id ?id }";

            queryString.SetLiteral("id", id.ToString());

            var resultsFuseki = Fuseki.Query(queryString.ToString()) as SparqlResultSet;

            return resultsFuseki?.Count == 0 ? null : Helpers.MapGenreModel(resultsFuseki[0]);

        }
    }
}
