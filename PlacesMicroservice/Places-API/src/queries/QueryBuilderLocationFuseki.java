package queries;

import constants.Constants;
import models.Location;

public class QueryBuilderLocationFuseki extends QueryBuilderPrefixes {
	
	public String get(Location location) {
		String query = addPrefixes() + "\n" + 
					   "SELECT * \n" + 
					   "WHERE {  \n" + 
					   		"?subject " + Constants.RDF_TYPE + " " + Constants.URER_LOCATION + " . \n" +
					   		"?subject " + Constants.URER_HAS_LATITUDE + " \" " + location.getLatitude() + "\"^^xsd:decimal " + " . \n" + 
					   		"?subject " + Constants.URER_HAS_LONGITUDE + " \" " + location.getLongitude() + "\"^^xsd:decimal " + " . \n" + 
					   		"?subject " + Constants.URER_HAS_ID + " ?id . \n" + 
					   		"} \n";		
			
		return query;
	}
	
	public String insert(Location location) {
		
		String query = addPrefixes() + "\n" + 
					   "INSERT DATA { " +
					    "<" + Constants.URER_ONTOLOGY_URI + "/Location/" + location.getId() + "> " + Constants.RDF_TYPE + Constants.URER_LOCATION +" ; \n" +
					    Constants.URER_HAS_LATITUDE + location.getLatitude() + " ; \n" + 
					    Constants.URER_HAS_LONGITUDE + location.getLongitude() + " ; \n" +  
					    Constants.URER_HAS_ID +  ""+ "\"" +location.getId() + "\" " + " . \n" +
					   " }"; 
						
		return query;
	}
	
	
}
