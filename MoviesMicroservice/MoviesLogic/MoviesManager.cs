﻿using System;
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
            Fuseki = new FusekiConnector("http://localhost:3030/movies2/data");
        }

        public FusekiConnector Fuseki { get; set; }

        public ManagerUpdateDb ManagerUpdateDb { get; set; }

        public List<MovieModelResponse> Get(DateTime releaseDateDate, string genre)
        {
            //ManagerUpdateDb.AddGenres();

            Fuseki = new FusekiConnector("http://localhost:3030/movies2/data");

            //Create a Parameterized String
            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            queryString.Namespaces.AddNamespace("wd", new Uri("http://www.wikidata.org/entity/"));
            queryString.Namespaces.AddNamespace("wdt", new Uri("http://www.wikidata.org/prop/direct/"));

            //Set the SPARQL command
            //For more complex queries we can do this in multiple lines by using += on the
            queryString.CommandText = "SELECT * WHERE { ?s wdt:P1476 ?title. ?s wdt:P577 ?date. ?s wdt:P136 ?genre. ?genre rdfs:label @genre. " + "?s wdt:P345 ?imdb" +
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
    }
}
