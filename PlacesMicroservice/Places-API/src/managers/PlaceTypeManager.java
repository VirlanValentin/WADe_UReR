package managers;
import java.util.ArrayList;
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
import models.PlaceType;
import queries.QueryBuilderPlaceTypeFuseki;

public class PlaceTypeManager {
		
	public List<PlaceType> getAll() {
		
		String query = new QueryBuilderPlaceTypeFuseki().getAll();
		List<PlaceType> types = null; 
		
		QueryExecution q = QueryExecutionFactory.sparqlService(constants.Constants.FUSEKI_PLACES_QUERY, query);
        ResultSet results = q.execSelect(); // get result-set
        
        if (results.hasNext()) {
        	types = new ArrayList<>();
        	PlaceType placeType; 
        	while (results.hasNext()) {
            	QuerySolution result = results.next();
    			String id = result.getLiteral("id").getString();
    			String name = result.getLiteral("name").getString();
    			
    			placeType = new PlaceType();
    			placeType.setId(id);
    			placeType.setName(name);
    			types.add(placeType);
    			
            }
        }

		
		
		
		return types;
	}
	
	public PlaceType get(PlaceType placeType) {

		String query = new QueryBuilderPlaceTypeFuseki().get(placeType);

		QueryExecution q = QueryExecutionFactory.sparqlService(constants.Constants.FUSEKI_PLACES_QUERY, query);
		ResultSet results = q.execSelect(); // get result-set
		
		if (results.hasNext()) {
        	QuerySolution result = results.next();
			String subject = result.getLiteral("id").getString();
			placeType.setId(subject);
			
        }
		//ResultSetFormatter.out(System.out, results); // print results
		return placeType;
	}
	
	public String insert(PlaceType placeType) {
		String id = UUID.randomUUID().toString();
		placeType.setId(id);
		String query = new  QueryBuilderPlaceTypeFuseki().insert(placeType);
		
		
		
		UpdateProcessor upp = UpdateExecutionFactory.createRemote(UpdateFactory.create(query),
								Constants.FUSEKI_PLACES_UPDATE);
		upp.execute();
		return id ;
	}
}
