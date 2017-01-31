package queries;

import constants.Constants;
import models.PlaceType;

public class QueryBuilderPlaceTypeFuseki extends QueryBuilderPrefixes {

	public String get(PlaceType placeType) {
		String query = addPrefixes() + "\n" + "SELECT * \n" + "WHERE { \n" + "?subject " + Constants.RDF_TYPE + " "
				+ Constants.URER_PLACETYPE + " . \n" + "?subject " + Constants.FOAF_NAME + " ?name . \n" + "?subject "
				+ Constants.URER_HAS_ID + " ?id . \n" + " FILTER (?name = \"" + placeType.getName() + "\"). \n"
				+ "} \n";

		return query;
	}

	public String insert(PlaceType placeType) {

		String query = addPrefixes() + "\n" + "INSERT DATA { " + "<" + Constants.URER_ONTOLOGY_URI + "/PlaceType/"
				+ placeType.getId() + ">" + " " + Constants.RDF_TYPE + Constants.URER_PLACETYPE + " ; "
				+ Constants.FOAF_NAME + "\"" + placeType.getName() + "\" ; " + Constants.URER_HAS_ID + "" + "\""
				+ placeType.getId() + "\"" + " . " + " }";

		return query;
	}

	/*
	 * SELECT DISTINCT * WHERE {
	 * 
	 * ?type rdf:type urer:PlaceType. ?type foaf:name ?name. ?type urer:id ?id
	 * 
	 * }
	 * 
	 */
	public String getAll() {
		String query = addPrefixes() + "\n" + 
					"SELECT DISTINCT \n * WHERE { \n "  + 
				    "?type " + Constants.RDF_TYPE + " " + Constants.URER_PLACETYPE + ".\n" +  
				    "?type " + Constants.FOAF_NAME + " ?name .\n" + 
				    "?type " + Constants.URER_HAS_ID + " ?id .\n" + 
					" }";

		return query;
	}

}
