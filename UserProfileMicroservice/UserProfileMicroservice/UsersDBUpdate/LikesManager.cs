using System;
using System.Collections.Generic;
using Framework.Common;
using VDS.RDF.Query;
using VDS.RDF.Storage;

namespace UsersDBUpdate
{
    public class LikesManager
    {
        public LikesManager()
        {
            Fuseki = new FusekiConnector("http://localhost:3030/users/data");
        }

        public FusekiConnector Fuseki { get; set; }

        public void AddMovieLike(Guid id, Guid movieLikeID)
        {
            var userResourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + id;

            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));

            queryString.CommandText = "INSERT DATA { @resourceUser urer:likesMovie @resourceMovieLike }";

            queryString.SetUri("resourceUser", new Uri(userResourceId));
            queryString.SetLiteral("resourceMovieLike", movieLikeID.ToString());

            //Query the collection, dump output
            Fuseki.Update(queryString.ToString());
        }

        public void AddPlaceLike(Guid id, Guid placeLikeID)
        {
            var userResourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + id;

            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));

            queryString.CommandText = "INSERT DATA { @resourceUser urer:likesPlace @resourcePlaceLike }";

            queryString.SetUri("resourceUser", new Uri(userResourceId));
            queryString.SetLiteral("resourcePlaceLike", placeLikeID.ToString());

            //Query the collection, dump output
            Fuseki.Update(queryString.ToString());
        }

        public void DeleteMovieLike(Guid id, Guid movieLikeID)
        {
            var userResourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + id;

            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));

            queryString.CommandText = "DELETE DATA { @resourceUser urer:likesMovie @resourceMovieLike }";

            queryString.SetUri("resourceUser", new Uri(userResourceId));
            queryString.SetLiteral("resourceMovieLike", movieLikeID.ToString());

            //Query the collection, dump output
            Fuseki.Update(queryString.ToString());
        }

        public void DeletePlaceLike(Guid id, Guid placeLikeID)
        {
            var userResourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + id;

            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));

            queryString.CommandText = "DELETE DATA { @resourceUser urer:likesPlace @resourceMovieLike }";

            queryString.SetUri("resourceUser", new Uri(userResourceId));
            queryString.SetLiteral("resourceMovieLike", placeLikeID.ToString());

            //Query the collection, dump output
            Fuseki.Update(queryString.ToString());
        }

        public List<string> GetMoviesLikes(Guid id)
        {
            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));

            //Set the SPARQL command

            queryString.CommandText =
                "SELECT * WHERE { @user urer:likesMovie ?movie}";

            var resourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + id;
            //Inject a Value for the parameter
            queryString.SetUri("user", new Uri(resourceId));

            //Query the collection, dump output
            var resultsFuseki = Fuseki.Query(queryString.ToString()) as SparqlResultSet;

            var movies = new List<string>();

            foreach (var result in resultsFuseki)
            {
                movies.Add(result.Value("movie").ToString());
            }

            return movies;
        }

        public List<string> GetPlacesLikes(Guid id)
        {
            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));

            //Set the SPARQL command

            queryString.CommandText =
                "SELECT * WHERE { @user urer:likesPlace ?place}";

            var resourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + id;
            //Inject a Value for the parameter
            queryString.SetUri("user", new Uri(resourceId));

            //Query the collection, dump output
            var resultsFuseki = Fuseki.Query(queryString.ToString()) as SparqlResultSet;

            var places = new List<string>();

            foreach (var result in resultsFuseki)
            {
                places.Add(result.Value("place").ToString());
            }

            return places;
        }

    }
}
