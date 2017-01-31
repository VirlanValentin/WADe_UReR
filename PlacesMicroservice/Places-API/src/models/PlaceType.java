package models;

import java.io.Serializable;

import org.json.JSONObject;

public class PlaceType implements Serializable {
	
   	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	private String name;
   	private String id;
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
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
