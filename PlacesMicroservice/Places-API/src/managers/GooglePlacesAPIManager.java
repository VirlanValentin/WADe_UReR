package managers;

import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;

import org.apache.jena.query.QueryExecution;
import org.apache.jena.query.QueryExecutionFactory;
import org.apache.jena.query.QuerySolution;
import org.apache.jena.query.ResultSet;
import org.apache.jena.update.UpdateExecutionFactory;
import org.apache.jena.update.UpdateFactory;
import org.apache.jena.update.UpdateProcessor;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;

import constants.Constants;
import convertors.PlaceConvertor;
import models.Location;
import models.Place;
import models.PlaceType;
import queries.QueryBuilderPlacesFuseki;

public class GooglePlacesAPIManager {
	private List<String> placeIds;

	public GooglePlacesAPIManager() {
		this.placeIds = new ArrayList<>();
	}

	// https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=47.173908,27.552289&radius=10000&key=AIzaSyAb9IZMXXv7J-13jA0y7iyPQIK9MCH7zIU
	public List<Place> getNearbyPlaces(JSONObject data) throws IOException, ParseException {
		List<Place> list = new ArrayList<>();
				
		search(data, Constants.GOOGLE_PLACES_API_SEARCH_URL);
		if (!data.get("type").toString().equals("")) {
			search(data, Constants.GOOGLE_PLACES_API_RADAR_SEARCH_URL);
		}
		Place place;
		for (String place_id : placeIds) {
			place = getPlaceDetails(place_id);
			list.add(place);
			insert(place);
		}
		return list;
	}

	public Place getPlaceDetails(String place_id) throws IOException, ParseException {
		String url = Constants.GOOGLE_PLACES_API_DETAILS_URL + "key=" + Constants.API_KEY + "&placeid=" + place_id;

		URL obj = new URL(url);
		HttpURLConnection con = (HttpURLConnection) obj.openConnection();
		con.setRequestMethod("GET");

		int responseCode = con.getResponseCode();
		System.out.println("\nplace_id : " + place_id);
		System.out.println("Sending 'GET' request to URL : " + url);
		System.out.println("Response Code : " + responseCode);

		JSONParser jsonParser = new JSONParser();
		JSONObject jsonObject = (JSONObject) jsonParser.parse(new InputStreamReader(con.getInputStream(), "UTF-8"));
		System.out.println("\nResponse : " + jsonObject);

		
		return new PlaceConvertor().fromGoogleDetailsToPlace(jsonObject);
	}

	private void search(JSONObject data, String url) throws IOException, ParseException {
		url += "key=" + Constants.API_KEY;

		double latitude = Double.parseDouble(data.get("latitude").toString());
		double longitude = Double.parseDouble(data.get("longitude").toString());

		// &location=47.173908,27.552289
		String location = latitude + "," + longitude;
		url += "&location=" + location;

		// &radius=10000
		double radius = Double.parseDouble(data.get("radius").toString());
		url += "&radius=" + radius;

		// $type=restaurant
		String type = data.get("type").toString();
		if (!type.equals("")) {
			url += "&type=" + type;
		}
		// limit
		int limit = Integer.parseInt(data.get("limit").toString());

		URL obj = new URL(url);
		HttpURLConnection con = (HttpURLConnection) obj.openConnection();
		con.setRequestMethod("GET");

		int responseCode = con.getResponseCode();
		System.out.println("\nSending 'GET' request to URL : " + url);
		System.out.println("Response Code : " + responseCode);

		JSONParser jsonParser = new JSONParser();
		JSONObject jsonObject = (JSONObject) jsonParser.parse(new InputStreamReader(con.getInputStream(), "UTF-8"));

		JSONObject objResJSON;
		String place_id;
		// print result
		System.out.println(jsonObject.toString());

		JSONArray results = (JSONArray) jsonObject.get("results");
		for (Object objRes : results) {
			objResJSON = (JSONObject) objRes;
			place_id = objResJSON.get("place_id").toString();
			if (!placeIds.contains(place_id) && limit > placeIds.size()) {
				placeIds.add(place_id);
			}

		}

	}

	private void insert(Place place) {
		String query = new QueryBuilderPlacesFuseki().insert(place);

		UpdateProcessor upp = UpdateExecutionFactory.createRemote(UpdateFactory.create(query),
				Constants.FUSEKI_PLACES_UPDATE);
		upp.execute();
	}
	
	public Place getPlaceDetailsFuseki(String id) {
		String query = new QueryBuilderPlacesFuseki().getPlaceById(id);
		
		QueryExecution q = QueryExecutionFactory.sparqlService(constants.Constants.FUSEKI_PLACES_QUERY, query);
		ResultSet results = q.execSelect(); 
		// print results
		Place place = new Place();
		place.setId(id);
		
		List<PlaceType> types = new ArrayList<>();
		Location location = new Location();
		
		while (results.hasNext()) {
        	QuerySolution result = results.next();
			
        	String url = result.getResource("place").getURI();
			place.setUrl(url);
			
			String name = result.getLiteral("name").getString();
			place.setName(name);
			
			String icon = result.getLiteral("depiction").getString();
			place.setIcon(icon);
			
			boolean isOpen = result.getLiteral("isOpen").getBoolean();
			place.setOpen_now(isOpen);
			if (result.getLiteral("website") != null) {
				place.setWebsite(result.getLiteral("website").getString());
			}					
			
			if (result.getLiteral("vicinity") != null ) {
				place.setVicinity(result.getLiteral("vicinity").getString());
			}
			
			
			if (result.getLiteral("rating") != null) {
				place.setRating(result.getLiteral("rating").getDouble());
			}
			
			
			if (result.getLiteral("address") != null) {
				place.setAddress(result.getLiteral("address").getString());
			}
			
			if (result.getLiteral("phoneNumber") != null) {
				place.setPhone_number(result.getLiteral("phoneNumber").getString());
			}
			
			if (result.getLiteral("schedule") != null) {
				place.setSchedule(result.getLiteral("schedule").getString());
			}
			
			
			if (place.getLocation() == null) {
				double latitude = result.getLiteral("lat").getDouble();
				double longitude = result.getLiteral("lng").getDouble();
				String locationID = result.getLiteral("locationID").getString();
				
				location.setId(locationID);
				location.setLatitude(latitude);
				location.setLongitude(longitude);
				
				place.setLocation(location);
			}
			
			PlaceType placeType = new PlaceType();
			String typeID = result.getLiteral("typeID").getString();
			String typeName = result.getLiteral("typeName").getString();
			placeType.setId(typeID);
			placeType.setName(typeName);
			types.add(placeType);
        }
		place.setTypes(types);
		
		if (place.getLocation() == null) {
			place = null;
		}
			
		return place;
		
	}

	public List<Place> getRelatedPlaces(Place place) throws Exception {
		List<Place> relatedPlaces;
		String query = new  QueryBuilderPlacesFuseki().getRelatedPlaces(place);
		
		QueryExecution q = QueryExecutionFactory.sparqlService(constants.Constants.FUSEKI_PLACES_QUERY, query);
		ResultSet results = q.execSelect(); 
		relatedPlaces = new ArrayList<>();
			
		Place relatedPlace;
		while (results.hasNext()) {
			QuerySolution result = results.next();
			
        	String id = result.getLiteral("id").getString();
			relatedPlace = getPlaceDetailsFuseki(id);
			if (relatedPlace == null) {
				throw new Exception();
			} else {
				relatedPlaces.add(relatedPlace);
			}
		} 
		return relatedPlaces;
				
	}
	

}
