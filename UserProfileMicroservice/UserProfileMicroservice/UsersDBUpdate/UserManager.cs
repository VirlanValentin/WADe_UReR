using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Framework.Common;
using VDS.RDF.Query;
using VDS.RDF.Storage;

namespace UsersDBUpdate
{
    public class UserManager
    {
        public UserManager()
        {

            Fuseki = new FusekiConnector("http://localhost:3030/users/data");
        }

        public FusekiConnector Fuseki { get; set; }

        public UserModellResponse AddUser(UserLoginModel user)
        {
            if (CheckUserExists(user.Email))
            {
                return null;
            }

            var id = Guid.NewGuid();
            var password = MD5.Create(user.Password);

            var newUser = new UserModellResponse()
            {
                Id = id,
                Name = user.Name,
                Email = user.Email,
                Resource = "http://www.semanticweb.org/geo/ontologies/2017/0/urer/" + id
            };


            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));
            queryString.Namespaces.AddNamespace("foaf", new Uri("http://xmlns.com/foaf/0.1/"));
            queryString.Namespaces.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));

            //Set the SPARQL command
            queryString.CommandText = "INSERT DATA { @resource rdf:type foaf:Person ; urer:hasPassword @password foaf:firstName @name ; rdfs:email @email ; urer:id @id }";


            queryString.SetUri("resource", new Uri(newUser.Resource));
            queryString.SetLiteral("name", newUser.Name);
            queryString.SetLiteral("password", password.ToString());
            queryString.SetLiteral("email", newUser.Email);
            queryString.SetLiteral("id", newUser.Id.ToString());

            Fuseki.Update(queryString.ToString());


            return newUser;
        }

        public List<UserModellResponse> GetAll()
        {
            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));
            queryString.Namespaces.AddNamespace("foaf", new Uri("http://xmlns.com/foaf/0.1/"));
            queryString.Namespaces.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));

            //Set the SPARQL command
            queryString.CommandText = "SELECT * WHERE {?s rdf:type foaf:Person. ?s rdfs:email ?email. ?s foaf:firstName ?name. ?s urer:id ?id}";

            //Query the collection, dump output
            var resultsFuseki = Fuseki.Query(queryString.ToString()) as SparqlResultSet;

            var users = new List<UserModellResponse>();

            // ReSharper disable once PossibleNullReferenceException
            foreach (var result in resultsFuseki)
            {
                var newUser = Helper.MapResponse(result);

                users.Add(newUser);
            }


            return users;
        }

        public UserModellResponse Get(Guid id)
        {
            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));
            queryString.Namespaces.AddNamespace("foaf", new Uri("http://xmlns.com/foaf/0.1/"));
            queryString.Namespaces.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));

            //Set the SPARQL command
            
            queryString.CommandText =
                "SELECT * WHERE {?s rdf:type foaf:Person. ?s rdfs:email ?email. ?s foaf:firstName ?name. ?s urer:id ?id. ?s urer:id @id}";

            //Inject a Value for the parameter
            queryString.SetLiteral("id", id.ToString());

            //Query the collection, dump output
            var resultsFuseki = Fuseki.Query(queryString.ToString()) as SparqlResultSet;

            // ReSharper disable once PossibleNullReferenceException
            if (resultsFuseki.Count == 0)
            {
                return null;
            }

            return Helper.MapResponse(resultsFuseki[0]);
        }

        public bool CheckUserExists(string email)
        {

            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));
            queryString.Namespaces.AddNamespace("foaf", new Uri("http://xmlns.com/foaf/0.1/"));
            queryString.Namespaces.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));

            //Set the SPARQL command
            queryString.CommandText = "SELECT (str(count(?s)) AS ?total) WHERE {?s rdf:type foaf:Person. ?s rdfs:email @email .}";

            //Inject a Value for the parameter
            queryString.SetLiteral("email", email);

            //Query the collection, dump output
            var resultsFuseki = Fuseki.Query(queryString.ToString()) as SparqlResultSet;

            // ReSharper disable once PossibleNullReferenceException
            var result = Convert.ToInt32(resultsFuseki[0].Value("total").ToString());

            return result > 0;
        }

        public bool CheckUserExistsById(Guid id)
        {

            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));
            queryString.Namespaces.AddNamespace("foaf", new Uri("http://xmlns.com/foaf/0.1/"));
            queryString.Namespaces.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));

            //Set the SPARQL command
            queryString.CommandText = "SELECT (str(count(?s)) AS ?total) WHERE {?s rdf:type foaf:Person. ?s urer:id @id.}";

            //Inject a Value for the parameter
            queryString.SetLiteral("id", id.ToString());

            //Query the collection, dump output
            var resultsFuseki = Fuseki.Query(queryString.ToString()) as SparqlResultSet;

            // ReSharper disable once PossibleNullReferenceException
            var result = Convert.ToInt32(resultsFuseki[0].Value("total").ToString());

            return result > 0;
        }

        public string CheckCredentials(UserLoginModel user)
        {
            var queryString = new SparqlParameterizedString();

            //Add a namespace declaration
            queryString.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            queryString.Namespaces.AddNamespace("urer", new Uri("http://www.semanticweb.org/geo/ontologies/2017/0/urer#"));
            queryString.Namespaces.AddNamespace("foaf", new Uri("http://xmlns.com/foaf/0.1/"));
            queryString.Namespaces.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));

            //Set the SPARQL command

            var password = MD5.Create(user.Password).ToString();

            queryString.CommandText =
                "SELECT * WHERE {?s rdf:type foaf:Person. ?s urer:id ?id. ?s foaf:firstName @name. ?s urer:hasPassword @password}";

            //Inject a Value for the parameter
            queryString.SetLiteral("password", password);
            queryString.SetLiteral("name", user.Email);

            //Query the collection, dump output
            var resultsFuseki = Fuseki.Query(queryString.ToString()) as SparqlResultSet;

            // ReSharper disable once PossibleNullReferenceException
            if (resultsFuseki.Count != 0)
            {
                return resultsFuseki[0].Value("id").ToString();
            }
            return null;
        }
    }
}
