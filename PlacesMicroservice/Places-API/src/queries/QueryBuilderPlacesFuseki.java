package queries;

import java.util.ArrayList;
import java.util.List;

import constants.Constants;
import managers.LocationManager;
import managers.PlaceTypeManager;
import models.Location;
import models.Place;
import models.PlaceType;

public class QueryBuilderPlacesFuseki extends QueryBuilderPrefixes {

	public String getAllPlaces() {
		String query = "SELECT * \n" + "WHERE { ?s ?p ?o } \n";

		return query;
	}

	/*
	 * Select * WHERE { ?place rdf:type urer:Place. ?place urer:id
	 * "ChIJu9059nz7ykARPPUmVNqAy3w". ?place foaf:name ?name. ?place
	 * foaf:depiction ?depiction. ?place urer:isOpen ?isOpen.
	 * 
	 * 
	 * OPTIONAL { ?place urer:url ?url }. OPTIONAL { ?place urer:website
	 * ?website }. OPTIONAL { ?place urer:vicinity ?vicinity }. OPTIONAL {
	 * ?place urer:rating ?rating }. OPTIONAL { ?place urer:address ?address }.
	 * OPTIONAL { ?place urer:phoneNumber ?phoneNumber }. OPTIONAL { ?place
	 * urer:schedule ?schedule }.
	 * 
	 * 
	 * 
	 * ?place urer:location ?locationID. ?location rdf:type urer:Location.
	 * ?location urer:id ?locationID. ?location urer:latitude ?lat. ?location
	 * urer:longitude ?lng.
	 * 
	 * ?place urer:hasType ?typeID. ?type rdf:type urer:PlaceType. ?type urer:id
	 * ?typeID. ?type foaf:name ?typeName.
	 * 
	 * }
	 */
	public String getPlaceById(String id) {
		String query = addPrefixes()  + "\n" +
					   "SELECT * \n WHERE { \n" +
					   " ?place " + Constants.RDF_TYPE + " " +  Constants.URER_PLACE + ". \n" + 
					   " ?place " + Constants.URER_HAS_ID + " \"" +  id + "\" . \n" +
					   " ?place " + Constants.FOAF_NAME + " ?name . \n" +
					   " ?place " + Constants.FOAF_DEPICTION + " ?depiction . \n" +
					   " ?place " + Constants.URER_IS_OPEN_NOW + " ?isOpen . \n" +
					   " OPTIONAL { ?place " + Constants.URER_HAS_WEBSITE + " ?website } . \n" +
					   " OPTIONAL { ?place " + Constants.URER_HAS_VICINITY + " ?vicinity } . \n" +
					   " OPTIONAL { ?place " + Constants.URER_HAS_RATING + " ?rating } . \n" +
					   " OPTIONAL { ?place " + Constants.URER_HAS_ADDRESS + " ?address } . \n" +
					   " OPTIONAL { ?place " + Constants.URER_HAS_PHONE_NUMBER + " ?phoneNumber } . \n" +
					   " OPTIONAL { ?place " + Constants.URER_HAS_SCHEDULE + " ?schedule } . \n" +
					   " ?place " + Constants.URER_HAS_LOCATION + " ?locationID . \n" +
					   " ?location " + Constants.RDF_TYPE +  Constants.URER_LOCATION + " . \n" +
					   " ?location " + Constants.URER_HAS_ID  + " ?locationID . \n" +
					   " ?location " + Constants.URER_HAS_LATITUDE  + " ?lat . \n" +
					   " ?location " + Constants.URER_HAS_LONGITUDE  + " ?lng . \n" +
					   " ?place " + Constants.URER_HAS_TYPE + " ?typeID . \n" +
					   " ?type " + Constants.RDF_TYPE +  Constants.URER_PLACETYPE + " . \n" +
					   " ?type " + Constants.URER_HAS_ID  + " ?typeID . \n" +
					   " ?type " + Constants.FOAF_NAME + " ?typeName . \n" +					   
					   " } ";
		
		
		
		return query;
	}

	public String insert(Place place) {
		LocationManager locationManager = new LocationManager();

		Location location = locationManager.get(place.getLocation());
		String location_url;
		if (location.getId() != null) {
			location_url = location.getId();
		} else {
			location_url = locationManager.insert(place.getLocation());
		}
		PlaceTypeManager placeTypeManager = new PlaceTypeManager();
		List<String> types_url = new ArrayList<>();
		for (PlaceType placeType : place.getTypes()) {
			PlaceType pT = placeTypeManager.get(placeType);
			if (pT.getId() != null) {
				types_url.add(pT.getId());
			} else {
				types_url.add(placeTypeManager.insert(placeType));
			}

		}

		String query = addPrefixes() + "INSERT DATA { " + " <" + place.getUrl() + "> " + Constants.RDF_TYPE
				+ Constants.URER_PLACE + " ; " + Constants.URER_HAS_ID + "\"" + place.getId() + "\" ; "
				+ Constants.FOAF_NAME + "\"" + place.getName() + "\" ; " + Constants.URER_HAS_LOCATION + " \""
				+ location_url + "\" ; " + Constants.URER_IS_OPEN_NOW + "" + place.isOpen_now() + " ; ";

		for (String placeType : types_url) {
			query += Constants.URER_HAS_TYPE + " \"" + placeType + "\" ; ";
		}
		if (place.getAddress() != null) {
			query += Constants.URER_HAS_ADDRESS + "\"" + place.getAddress() + "\" ; ";
		}
		if (place.getPhone_number() != null) {
			query += Constants.URER_HAS_PHONE_NUMBER + "\"" + place.getPhone_number() + "\" ; ";
		}
		if (place.getRating() > 0) {
			query += Constants.URER_HAS_RATING + "" + place.getRating() + " ; ";
		}
		if (place.getSchedule() != null) {
			query += Constants.URER_HAS_SCHEDULE + "\"" + place.getSchedule() + "\" ; ";
		}
		if (place.getVicinity() != null) {
			query += Constants.URER_HAS_VICINITY + "\"" + place.getVicinity() + "\" ; ";
		}
		if (place.getWebsite() != null) {
			query += Constants.URER_HAS_WEBSITE + "\"" + place.getWebsite() + "\" ; ";
		}

		query += Constants.FOAF_DEPICTION + "\"" + place.getIcon() + "\" . ";
		query += " }";

		return query;
	}

	public String getRelatedPlaces(Place place) {
		String query = addPrefixes()  + "\n" +
				   "SELECT DISTINCT ?id \n WHERE { \n" +
				   " ?place " + Constants.RDF_TYPE + " " +  Constants.URER_PLACE + ". \n" + 
				   " ?place " + Constants.URER_HAS_ID + " ?id  . \n" +
				   " ?place " + Constants.URER_HAS_TYPE + " ?typeID . \n" +
				   " ?type " + Constants.RDF_TYPE +  Constants.URER_PLACETYPE + " . \n" +
				   " ?type " + Constants.URER_HAS_ID  + " ?typeID . \n" +
				   " ?type " + Constants.FOAF_NAME + " ?typeName . \n" ;
		
		String filter = "FILTER(?typeName IN ( " ;
		for (int index = 0; index < place.getTypes().size(); index ++) {
			PlaceType placeType = place.getTypes().get(index);
			filter += " \"" + placeType.getName() + "\"";
			if (index < place.getTypes().size() - 1) {
				filter += " ,";
			}
		}
		filter += ") && ?id != \"" + place.getId() +  "\" ) \n";

	
		return query + filter +  " } ";
	}

}
