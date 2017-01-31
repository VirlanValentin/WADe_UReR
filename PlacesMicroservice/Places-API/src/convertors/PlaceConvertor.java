package convertors;

import java.util.ArrayList;
import java.util.List;

import org.apache.jena.query.QuerySolution;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;

import models.Location;
import models.Place;
import models.PlaceType;

public class PlaceConvertor {
	public Place fromSparqlToPlace(QuerySolution result) {
		Place place = new Place();
		
		Double latitude = Double.parseDouble(result.getLiteral("lat").getString());
		Double longitude = Double.parseDouble(result.getLiteral("long").getString());
		String url = result.getResource("url").getURI();
		String type = result.getLiteral("type").getString();
		String name = result.getLiteral("name").getString();
		String id = result.getLiteral("id").getString();
		
		//place.setLatitude(latitude);
		//place.setLongitude(longitude);
		place.setName(name);
		//place.setType(type);
		place.setUrl(url);
		place.setId(id);
		
		return place;
	}
	
	public Place fromGoogleDetailsToPlace(JSONObject jsonObject) {
		Place place = new Place();
		
		JSONObject result = (JSONObject) jsonObject.get("result");
		String formatted_address; 
		String formatted_phone_number;
		if (result.get("formatted_address") != null) {
			formatted_address = result.get("formatted_address").toString();
			place.setAddress(formatted_address);
		}
		if (result.get("formatted_phone_number") != null) {
			formatted_phone_number = result.get("formatted_phone_number").toString();
			place.setPhone_number(formatted_phone_number);
		}
		JSONObject geometry = (JSONObject) result.get("geometry");
		JSONObject location = (JSONObject) geometry.get("location");
		double lat = Double.parseDouble(location.get("lat").toString());
		double lng = Double.parseDouble(location.get("lng").toString());
		Location loc = new Location();
		loc.setLatitude(lat);
		loc.setLongitude(lng);
		place.setLocation(loc);
		
		String icon = result.get("icon").toString();
		place.setIcon(icon);
		String name = result.get("name").toString();
		place.setName(name);
		String place_id = result.get("place_id").toString();
		place.setId(place_id);
		double rating;
		if (result.get("rating") != null) {
			rating =  Double.parseDouble(result.get("rating").toString());
			place.setRating(rating);
		}
		JSONArray types = (JSONArray)result.get("types");
		if (types.size() > 0) {
			List<PlaceType> typesList = new ArrayList<>();
			for (int index = 0; index < types.size(); index ++) {
				PlaceType type = new PlaceType();
				type.setName(types.get(index).toString());
				typesList.add(type);
			}
			place.setTypes(typesList);
		}
		String url;
		if (result.get("url") != null) {
			url = result.get("url").toString();
			place.setUrl(url);
		}
		
		String vicinity = result.get("vicinity").toString();
		place.setVicinity(vicinity);
		JSONObject opening_hours = (JSONObject)result.get("opening_hours");
		if (opening_hours != null) {
			boolean open_now;
			if (opening_hours.get("open_now") != null) {
				open_now = Boolean.parseBoolean(opening_hours.get("open_now").toString());
				place.setOpen_now(open_now);
			}
			
			JSONArray weekday_text = (JSONArray)opening_hours.get("weekday_text");
			if (weekday_text.size() > 0) {
				List<String> schedule = new ArrayList<>();
				for (int index = 0; index < weekday_text.size(); index ++) {
					schedule.add(weekday_text.get(index).toString());
				}
				place.setSchedule(schedule.toString());
			}
		}
		String website;
		if (result.get("website") != null) {
			website = result.get("website").toString();
			place.setWebsite(website);
		}
		
		return place;
	}
}
