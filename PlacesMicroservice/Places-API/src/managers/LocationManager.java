package managers;
import java.util.List;
import java.util.UUID;

import org.apache.jena.query.QueryExecution;
import org.apache.jena.query.QueryExecutionFactory;
import org.apache.jena.query.QuerySolution;
import org.apache.jena.query.ResultSet;
import org.apache.jena.query.ResultSetFormatter;
import org.apache.jena.update.UpdateExecutionFactory;
import org.apache.jena.update.UpdateFactory;
import org.apache.jena.update.UpdateProcessor;

import constants.Constants;
import models.Location;
import queries.QueryBuilderLocationFuseki;

public class LocationManager {
	
	public Location get(Location location) {

		String query = new QueryBuilderLocationFuseki().get(location);

		QueryExecution q = QueryExecutionFactory.sparqlService(constants.Constants.FUSEKI_PLACES_QUERY, query);
		ResultSet results = q.execSelect(); // get result-set
		// print results
		if (results.hasNext()) {
        	QuerySolution result = results.next();
        	String subject = result.getLiteral("id").getString();
			location.setId(subject);
			
        }
		//ResultSetFormatter.out(System.out, results); 
		return location;
	}
	public List<Location> getAll() {
		
		/*String query = new QueryBuilderLocationFuseki()
		
		QueryExecution q = QueryExecutionFactory.sparqlService(constants.Constants.FUSEKI_PLACES_QUERY, query);
        ResultSet results = q.execSelect(); // get result-set
        ResultSetFormatter.out(System.out, results); // print results	*/
		
		
		return null;
	}
	
	public String insert(Location location) {
		
		String id = UUID.randomUUID().toString();
		location.setId(id);
		
		String query = new  QueryBuilderLocationFuseki().insert(location);			
		
		UpdateProcessor upp = UpdateExecutionFactory.createRemote(UpdateFactory.create(query),
								Constants.FUSEKI_PLACES_UPDATE);
		upp.execute();
		return  id;
	}
}
