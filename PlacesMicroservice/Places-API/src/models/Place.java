package models;

import java.io.Serializable;
import java.util.List;

import org.json.JSONObject;

import constants.Constants;

public class Place implements Serializable {

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	private String name;
	private String url;
	private Location location;
	private String id;
	private List<PlaceType> types;
	private String icon;
	private String address;
	private String phone_number;
	private double rating;
	private String vicinity;
	private boolean open_now;
	private String schedule;
	private String website;

	public String getId() {
		return id;
	}

	public void setId(String id) {
		this.id = id;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public String getUrl() {
		return url;
	}

	public void setUrl(String url) {
		this.url = url;
	}

	public Location getLocation() {
		return location;
	}

	public void setLocation(Location location) {
		this.location = location;
	}

	public List<PlaceType> getTypes() {
		return types;
	}

	public void setTypes(List<PlaceType> types) {
		this.types = types;
	}

	public String getIcon() {
		return icon;
	}

	public void setIcon(String icon) {
		this.icon = icon;
	}

	public String getAddress() {
		return address;
	}

	public void setAddress(String address) {
		this.address = address;
	}

	public String getPhone_number() {
		return phone_number;
	}

	public void setPhone_number(String phone_number) {
		this.phone_number = phone_number;
	}

	public double getRating() {
		return rating;
	}

	public void setRating(double rating) {
		this.rating = rating;
	}

	public String getVicinity() {
		return vicinity;
	}

	public void setVicinity(String vicinity) {
		this.vicinity = vicinity;
	}

	public boolean isOpen_now() {
		return open_now;
	}

	public void setOpen_now(boolean open_now) {
		this.open_now = open_now;
	}

	public String getSchedule() {
		return schedule;
	}

	public void setSchedule(String schedule) {
		this.schedule = schedule;
	}

	public String getWebsite() {
		return website;
	}

	public void setWebsite(String website) {
		this.website = website;
	}

	@Override
	public String toString() {
		/*String result = "{ 'id' : \"" + this.id + "\", \n" + "'name' : \"" + this.name + "\", \n" + "'icon' : \""
				+ this.icon + "\", \n" + "'location' : " + this.location.toString() + ", \n" 
				+ "'isOpen' : " + this.isOpen_now() + ", \n";
		result += "'types' : [ \n";
		for (PlaceType placeType : this.types) {
			
		}
		result += "] \n";
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
		}*/
		return new JSONObject(this).toString();
	}

}
