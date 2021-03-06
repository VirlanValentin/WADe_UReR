﻿using System;
using System.Collections.Generic;
using VDS.RDF.Query;
using VDS.RDF.Storage;

namespace UsersDBUpdate
{
    public class PreferencesManager
    {
        public PreferencesManager()
        {
            Fuseki = new FusekiConnector("http://54.187.81.132:3030/users/data");
        }

        public FusekiConnector Fuseki { get; set; }

        public void AddMoviePreference(Guid id, Guid movieTypeId)
        {
            var userResourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + id;

            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));

            queryString.CommandText = "INSERT DATA { @resourceUser urer:hasMoviePreference @resourceMoviePreference }";

            queryString.SetUri("resourceUser", new Uri(userResourceId));
            queryString.SetLiteral("resourceMoviePreference", movieTypeId.ToString());

            //Query the collection, dump output
            Fuseki.Update(queryString.ToString());
        }

        public void AddPlacePreference(Guid id, Guid placeTypeId)
        {
            var userResourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + id;

            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));

            queryString.CommandText = "INSERT DATA { @resourceUser urer:hasPlacePreference @resourcePlacePreferece}";

            queryString.SetUri("resourceUser", new Uri(userResourceId));
            queryString.SetLiteral("resourcePlacePreferece", placeTypeId.ToString());

            //Query the collection, dump output
            Fuseki.Update(queryString.ToString());
        }

        public void DeleteMoviePreference(Guid id, Guid movieTypeId)
        {
            var userResourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + id;

            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));

            queryString.CommandText = "DELETE DATA { @resourceUser urer:hasMoviePreference @resourceMoviePreference }";

            queryString.SetUri("resourceUser", new Uri(userResourceId));
            queryString.SetLiteral("resourceMoviePreference", movieTypeId.ToString());

            //Query the collection, dump output
            Fuseki.Update(queryString.ToString());
        }

        public void DeletePlacePreference(Guid id, Guid placeTypeId)
        {
            var userResourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + id;

            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));

            queryString.CommandText = "DELETE DATA { @resourceUser urer:hasPlacePreference @resourcePlacePreference }";

            queryString.SetUri("resourceUser", new Uri(userResourceId));
            queryString.SetLiteral("resourcePlacePreference", placeTypeId.ToString());

            //Query the collection, dump output
            Fuseki.Update(queryString.ToString());
        }

        public List<string> GetMoviesPreferences(Guid id)
        {
            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));

            //Set the SPARQL command

            queryString.CommandText =
                "SELECT * WHERE { @user urer:hasMoviePreference ?movie}";

            var resourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + id;
            //Inject a Value for the parameter
            queryString.SetUri("user", new Uri(resourceId));

            //Query the collection, dump output
            var resultsFuseki = Fuseki.Query(queryString.ToString()) as SparqlResultSet;

            var movies = new List<string>();

            // ReSharper disable once PossibleNullReferenceException
            foreach (var result in resultsFuseki)
            {
                movies.Add(result.Value("movie").ToString());
            }

            return movies;
        }

        public List<string> GetPlacesPreferences(Guid id)
        {
            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));

            //Set the SPARQL command

            queryString.CommandText =
                "SELECT * WHERE { @user urer:hasPlacePreference ?place}";

            var resourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + id;
            //Inject a Value for the parameter
            queryString.SetUri("user", new Uri(resourceId));

            //Query the collection, dump output
            var resultsFuseki = Fuseki.Query(queryString.ToString()) as SparqlResultSet;

            var places = new List<string>();

            // ReSharper disable once PossibleNullReferenceException
            foreach (var result in resultsFuseki)
            {
                places.Add(result.Value("place").ToString());
            }

            return places;
        }

    }
}
