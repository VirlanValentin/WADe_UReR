package queries;

import constants.Constants;

public class QueryBuilderPrefixes {
	public String addPrefixes() {
		String prefixes = "PREFIX " + Constants.FOAF_PREFIX + " " + Constants.FOAF_URI + "\n" +
				   "PREFIX " + Constants.DBO_PREFIX + " " + Constants.DBO_URI + "\n" +
				   "PREFIX " + Constants.GEO_PREFIX + " " + Constants.GEO_URI + "\n" +
				   "PREFIX " + Constants.RDF_PREFIX + " " + Constants.RDF_URI + "\n" + 
				   "PREFIX " + Constants.URER_PREFIX + " " + Constants.URER_URI + "\n" +
				   "PREFIX " + Constants.XSD_PREFIX + " " + Constants.XSD_URI + "\n";
		
		return prefixes;
	}
}
