using System;
using System.Collections.Generic;
using Framework.Common;
using VDS.RDF.Query;
using VDS.RDF.Storage;

namespace UsersDBUpdate
{
    public class EnemiesManager
    {
        public EnemiesManager()
        {
            Fuseki = new FusekiConnector("http://localhost:3030/users/data");
        }

        public FusekiConnector Fuseki { get; set; }


        public void AddEnemy(Guid id, Guid enemyId)
        {
            var userResourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + id;
            var enemyResourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + enemyId;

            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("foaf", new Uri("http://xmlns.com/foaf/0.1/"));
            queryString.Namespaces.AddNamespace("rel", new Uri("http://purl.org/vocab/relationship/"));


            queryString.CommandText = "INSERT DATA { @resourceUser rel:enemyOf @resourceEnemy  }";


            queryString.SetUri("resourceUser", new Uri(userResourceId));
            queryString.SetUri("resourceEnemy", new Uri(enemyResourceId));

            //Query the collection, dump output
            Fuseki.Update(queryString.ToString());
        }

        public List<UserModellResponse> GetEnemies(Guid id)
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
                "SELECT * WHERE {  @user rel:enemyOf ?s. ?s rdf:type foaf:Person. ?s rdfs:email ?email. ?s foaf:firstName ?name. ?s urer:id ?id. }";

            var resourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + id;
            //Inject a Value for the parameter
            queryString.SetUri("user", new Uri(resourceId));

            //Query the collection, dump output
            var resultsFuseki = Fuseki.Query(queryString.ToString()) as SparqlResultSet;

            // ReSharper disable once PossibleNullReferenceException
            var enemies = new List<UserModellResponse>();

            // ReSharper disable once PossibleNullReferenceException
            foreach (var result in resultsFuseki)
            {
                var friend = Helper.MapResponse(result);

                enemies.Add(friend);
            }

            return enemies;
        }

        public void RemoveEnemy(Guid id, Guid enemyId)
        {
            var userResourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + id;
            var enemyResourceId = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + enemyId;

            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));
            queryString.Namespaces.AddNamespace("rel", new Uri("http://purl.org/vocab/relationship/"));


            queryString.CommandText = "Delete DATA { @resourceUser rel:enemyOf @resourceEnemy  }";

            queryString.SetUri("resourceUser", new Uri(userResourceId));
            queryString.SetUri("resourceEnemy", new Uri(enemyResourceId));

            //Query the collection, dump output
            Fuseki.Update(queryString.ToString());
        }
    }
}
