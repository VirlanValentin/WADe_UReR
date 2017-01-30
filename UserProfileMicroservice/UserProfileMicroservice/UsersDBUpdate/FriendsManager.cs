using System;
using System.Collections.Generic;
using Framework.Common;
using VDS.RDF.Query;
using VDS.RDF.Storage;

namespace UsersDBUpdate
{
    public class FriendsManager
    {
        public FriendsManager()
        {
            Fuseki = new FusekiConnector("http://54.187.81.132:3030/users/data");
        }

        public FusekiConnector Fuseki { get; set; }


        public void AddFriend(Guid id, Guid friendId)
        {
            var userResourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + id;
            var friendResourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + friendId;

            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("foaf", new Uri("http://xmlns.com/foaf/0.1/"));
            queryString.Namespaces.AddNamespace("rel", new Uri("http://purl.org/vocab/relationship/"));


            queryString.CommandText = "INSERT DATA { @resourceUser rel:friendOf @resourceFriend  }";


            queryString.SetUri("resourceUser", new Uri(userResourceId));
            queryString.SetUri("resourceFriend", new Uri(friendResourceId));

            //Query the collection, dump output
            Fuseki.Update(queryString.ToString());
        }

        public List<UserModellResponse> GetFriends(Guid id)
        {
            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));
            queryString.Namespaces.AddNamespace("foaf", new Uri("http://xmlns.com/foaf/0.1/"));
            queryString.Namespaces.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
            queryString.Namespaces.AddNamespace("rel", new Uri("http://purl.org/vocab/relationship/"));

            //Set the SPARQL command
            
            queryString.CommandText =
                "SELECT * WHERE {  @user rel:friendOf ?s. ?s rdf:type foaf:Person. ?s rdfs:email ?email. ?s foaf:firstName ?name. ?s urer:id ?id. }";

            var resourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + id;
            //Inject a Value for the parameter
            queryString.SetUri("user", new Uri(resourceId));

            //Query the collection, dump output
            var resultsFuseki = Fuseki.Query(queryString.ToString()) as SparqlResultSet;

            // ReSharper disable once PossibleNullReferenceException
            var friends = new List<UserModellResponse>();

            // ReSharper disable once PossibleNullReferenceException
            foreach (var result in resultsFuseki)
            {
                var friend = Helper.MapResponse(result);

                friends.Add(friend);
            }

            return friends;
        }


        public void RemoveFriend(Guid id, Guid friendId)
        {
            var userResourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + id;
            var friendResourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + friendId;

            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));
            queryString.Namespaces.AddNamespace("rel", new Uri("http://purl.org/vocab/relationship/"));


            queryString.CommandText = "Delete DATA { @resourceUser rel:friendOf @resourceFriend  }";

            queryString.SetUri("resourceUser", new Uri(userResourceId));
            queryString.SetUri("resourceFriend", new Uri(friendResourceId));

            //Query the collection, dump output
            Fuseki.Update(queryString.ToString());
        }

    }
}
