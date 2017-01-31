package managers;
import java.util.List;
import java.util.UUID;

import org.apache.jena.query.QueryExecution;
import org.apache.jena.query.QueryExecutionFactory;
import org.apache.jena.query.ResultSet;
import org.apache.jena.query.ResultSetFormatter;
import org.apache.jena.update.UpdateExecutionFactory;
import org.apache.jena.update.UpdateFactory;
import org.apache.jena.update.UpdateProcessor;

import constants.Constants;
import models.Place;
import queries.QueryBuilderPlacesFuseki;

public class PlacesManager {
		
	public List<Place> getAll() {
		
		String query = new QueryBuilderPlacesFuseki().getAllPlaces();
		
		QueryExecution q = QueryExecutionFactory.sparqlService(constants.Constants.FUSEKI_PLACES_QUERY, query);
        ResultSet results = q.execSelect(); // get result-set
        ResultSetFormatter.out(System.out, results); // print results	
		
		
		return null;
	}
	
	public void insert(Place place) {
		String query = new  QueryBuilderPlacesFuseki().insert(place);
		
		String id = UUID.randomUUID().toString();
		
		UpdateProcessor upp = UpdateExecutionFactory.createRemote(UpdateFactory.create(String.format(query, id)),
								Constants.FUSEKI_PLACES_UPDATE);
		upp.execute();
	}
}
