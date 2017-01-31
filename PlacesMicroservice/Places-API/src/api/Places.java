package api;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import javax.ws.rs.GET;
import javax.ws.rs.Path;
import javax.ws.rs.PathParam;
import javax.ws.rs.Produces;
import javax.ws.rs.core.Context;
import javax.ws.rs.core.Response;
import javax.ws.rs.core.UriInfo;

import org.json.simple.JSONObject;
import org.json.simple.parser.ParseException;

import managers.GooglePlacesAPIManager;
import managers.PlaceTypeManager;
import models.ErrorMessage;
import models.Place;
import models.PlaceType;

@Path("/places")
@SuppressWarnings("unchecked")
public class Places {

	@GET
	@Path("/{place_id}")
	@Produces("application/json")
	public Response getPlaceById(@PathParam("place_id") String place_id) {

		if (place_id == null) {
			ErrorMessage errorMessage = new ErrorMessage();
			errorMessage.setCode(400);
			errorMessage.setMessage(
					"Bad request. The request could not be understood by the server. Please make a good request.");
			return Response.status(errorMessage.getCode()).entity(new org.json.JSONObject(errorMessage).toString())
					.build();
		} else {

			Place place = new GooglePlacesAPIManager().getPlaceDetailsFuseki(place_id);
			if (place != null) {
				return Response.status(200).entity(place.toString()).build();
			} else {
				ErrorMessage errorMessage = new ErrorMessage();
				errorMessage.setCode(404);
				errorMessage.setMessage("Not Found. The server has not found anything matching the request URI.");
				return Response.status(errorMessage.getCode()).entity(new org.json.JSONObject(errorMessage).toString())
						.build();
			}

		}

	}

	@GET
	@Path("/types")
	@Produces("application/json")
	public Response getAllTypes() {
		List<PlaceType> types = new PlaceTypeManager().getAll();
		if (types == null) {
			ErrorMessage errorMessage = new ErrorMessage();
			errorMessage.setCode(500);
			errorMessage.setMessage("Internal Server Error. Something bad happened. Please contact the support team.");
			return Response.status(errorMessage.getCode()).entity(new org.json.JSONObject(errorMessage).toString())
					.build();
		}
		JSONObject jObject = new JSONObject();
		jObject.put("types", types);
		return Response.status(200).entity(jObject.toString()).build();
	}

	@GET
	@Produces("application/json")
	public Response getPlaces(
			/*
			 * @DefaultValue("-1") @PathParam("lat") double lat,
			 * 
			 * @DefaultValue("-1") @QueryParam("long") double lon,
			 * 
			 * @DefaultValue("-1") @QueryParam("radius") double rad,
			 * 
			 * @DefaultValue("") @QueryParam("type") String type
			 */ @Context UriInfo info) {

		String lat = info.getQueryParameters().getFirst("lat");
		String lon = info.getQueryParameters().getFirst("lon");

		if (lat == null || lon == null) {
			ErrorMessage errorMessage = new ErrorMessage();
			errorMessage.setCode(400);
			errorMessage.setMessage(
					"Bad request. The request could not be understood by the server. Please make a good request.");
			return Response.status(errorMessage.getCode()).entity(new org.json.JSONObject(errorMessage).toString())
					.build();
		} else {
			double latitude = Double.parseDouble(lat);
			double longitude = Double.parseDouble(lon);
			String type = info.getQueryParameters().getFirst("type");

			if (type == null) {
				type = "";
			}

			String rad = info.getQueryParameters().getFirst("radius");
			double radius = 5000;
			if (rad != null) {
				radius = Double.parseDouble(rad);
			}

			if (radius <= 100 || radius >= 50000) {
				ErrorMessage errorMessage = new ErrorMessage();
				errorMessage.setCode(400);
				errorMessage.setMessage(
						"Bad request. The request could not be understood by the server. Please make a good request.");
				return Response.status(errorMessage.getCode()).entity(new org.json.JSONObject(errorMessage).toString())
						.build();
			}

			String lim = info.getQueryParameters().getFirst("limit");
			int limit = 50;
			if (lim != null) {
				limit = Integer.parseInt(lim);
			}
			if (limit <= 0) {
				ErrorMessage errorMessage = new ErrorMessage();
				errorMessage.setCode(400);
				errorMessage.setMessage(
						"Bad request. The request could not be understood by the server. Please make a good request.");
				return Response.status(errorMessage.getCode()).entity(new org.json.JSONObject(errorMessage).toString())
						.build();
			}

			JSONObject jsonObject = new JSONObject();
			jsonObject.put("latitude", latitude);
			jsonObject.put("longitude", longitude);
			jsonObject.put("radius", radius);
			jsonObject.put("limit", limit);
			jsonObject.put("type", type);

			GooglePlacesAPIManager googlePlacesAPIManager = new GooglePlacesAPIManager();
			try {
				List<Place> places = googlePlacesAPIManager.getNearbyPlaces(jsonObject);

				JSONObject jObject = new JSONObject();
				jObject.put("places", places);
				return Response.status(200).entity(jObject.toString()).build();

			} catch (IOException e) {
				ErrorMessage errorMessage = new ErrorMessage();
				errorMessage.setCode(500);
				errorMessage
						.setMessage("Internal Server Error. Something bad happened. Please contact the support team.");
				return Response.status(errorMessage.getCode()).entity(new org.json.JSONObject(errorMessage).toString())
						.build();
			} catch (ParseException e) {
				ErrorMessage errorMessage = new ErrorMessage();
				errorMessage.setCode(500);
				errorMessage
						.setMessage("Internal Server Error. Something bad happened. Please contact the support team.");
				return Response.status(errorMessage.getCode()).entity(new org.json.JSONObject(errorMessage).toString())
						.build();
			}

		}

	}

	@GET
	@Path("/{place_id}/related")
	@Produces("application/json")
	public Response getRelatedPlaces(@PathParam("place_id") String place_id) {
		if (place_id == null) {
			ErrorMessage errorMessage = new ErrorMessage();
			errorMessage.setCode(400);
			errorMessage.setMessage(
					"Bad request. The request could not be understood by the server. Please make a good request.");
			return Response.status(errorMessage.getCode()).entity(new org.json.JSONObject(errorMessage).toString())
					.build();
		} else {

			Place place = new GooglePlacesAPIManager().getPlaceDetailsFuseki(place_id);
			if (place != null) {
				// ia toate place-urile care au tipurile ca place-ul cu id-ul
				// place_id
				List<Place> relatedPlaces;
				try {
					relatedPlaces = new GooglePlacesAPIManager().getRelatedPlaces(place);
					if (relatedPlaces != null) {

						JSONObject jObject = new JSONObject();
						jObject.put("related", relatedPlaces);
						return Response.status(200).entity(jObject.toString()).build();

					} else {
						ErrorMessage errorMessage = new ErrorMessage();
						errorMessage.setCode(500);
						errorMessage.setMessage("Internal Server Error. Something bad happened. Please contact the support team.");
						return Response.status(errorMessage.getCode()).entity(new org.json.JSONObject(errorMessage).toString()).build();
					}
				} catch (Exception e) {
					ErrorMessage errorMessage = new ErrorMessage();
					errorMessage.setCode(500);
					errorMessage.setMessage("Internal Server Error. Something bad happened. Please contact the support team.");
					return Response.status(errorMessage.getCode()).entity(new org.json.JSONObject(errorMessage).toString()).build();
				}
				

			} else {
				ErrorMessage errorMessage = new ErrorMessage();
				errorMessage.setCode(404);
				errorMessage.setMessage("Not Found. The server has not found anything matching the request URI.");
				return Response.status(errorMessage.getCode()).entity(new org.json.JSONObject(errorMessage).toString())
						.build();
			}

		}

	}
}
