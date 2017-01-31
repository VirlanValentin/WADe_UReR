package models;

import java.io.Serializable;

import org.json.JSONObject;

public class Location implements Serializable {

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	private double latitude;
	private double longitude;
	private String id;
	
	public double getLatitude() {
		return latitude;
	}
	public void setLatitude(double latitude) {
		this.latitude = latitude;
	}
	public double getLongitude() {
		return longitude;
	}
	public void setLongitude(double longitude) {
		this.longitude = longitude;
	}
	public String getId() {
		return id;
	}
	public void setId(String id) {
		this.id = id;
	}
	
	@Override
	public String toString() {
		return new JSONObject(this).toString();
	}
	
	
}
