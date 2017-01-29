var hVM;
var map;
var infowindow;

function MapElement(name, imageSrc, description, url, lat, long) {
  this.Image = imageSrc;
  this.Name = name;
  this.Description = description;
  this.Url = url;
  this.Lat = lat;
  this.Long = long;
}

function CreateMarkerContent(elem) {
  var display = 'initial';
  if (elem.Image == null) {
    display = 'none';
  }

  //the img will break, but it won't affect UI
  var HTMLstring = `<div style="width: 20vh" >
  <div style="display:flex">
    <img class="img-keep-aspect-ratio" style="height:10vh; display:${display};" src="${elem.Image}" />
    <h3>${elem.Name}</h3>
  </div>
  <p>${elem.Description}</p>
  <a href=${elem.Url}>${elem.Url}</a>
</div>
`;
  return HTMLstring;
}

var HomePageModel = function () {
  var self = this;
  self.Lat = sessionStorage['lat'];
  self.Long = sessionStorage['long'];
  self.SetPosition = function (position) {
      self.Lat =  position.coords.latitude,
      self.Long =  position.coords.longitude
  }
  //icons:
  self.Icons = {};
  self.GetIcons = function () {
    $("#icons img").each(function (index) {
      var name = $(this).data('name');
      var src = $(this).prop('src');
      self.Icons[name] = src;
    });
    console.log(self.Icons);
  }
    
  self.LandMarks = ko.observable();
  //TODO: get stuff
  self.GetInfo = function () {
    //for the moment, mock some data
    self.AddMarker('friend', 'this is a friend');
  }

  self.AddMarker = function (feature, element) {
    //element is of type MapElement
    var marker = new google.maps.Marker({
      position: new google.maps.LatLng(element.Lat, element.Long),
      icon: self.Icons[feature], //friend, enemy, ...
      map: map,
      //content: content, //HTML content
      content: CreateMarkerContent(element)
    });
    marker.addListener('click', function () {
      infowindow.setContent(this.content);
      infowindow.open(map, this);
    });
  }

  self.SearchField = ko.observable();


  self.ShowMenuForMobile = function () {
    //show left menu:
    //make map occupy col-xs-10
    $("#map").addClass('col-xs-7');
    $("#leftOptions").removeClass('hidden-xs');
    //hide plus button:
    $("#plus-button").hide();
  }

  self.HideMenuForMobile = function () {
    //hide left menu:
    //make map occupy col-xs-12 - again
    $("#map").removeClass('col-xs-7');
    $("#leftOptions").addClass('hidden-xs');
    //hide plus button:
    $("#plus-button").show();
  }
};

//show map:
function myMap() {
  var mapOptions = {
    center: new google.maps.LatLng(sessionStorage['lat'], sessionStorage['long']),
    zoom: 15,
    mapTypeId: 'terrain'
  }
  map = new google.maps.Map(document.getElementById("map"), mapOptions);
  infowindow = new google.maps.InfoWindow();
  setTimeout(function () { hVM.GetInfo() }, 500);
}

$(function () {
  ko.validation.init({
    registerExtenders: true,
    messagesOnModified: true,
    insertMessages: true,
    parseInputAttributes: true,
    messageTemplate: 'customMessageTemplate',
    errorClass: 'input-has-error'
  });
  ko.validation.registerExtenders();
  hVM = new HomePageModel();
  ko.applyBindings(hVM);
  hVM.GetIcons();

  if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(hVM.SetPosition);
  } else {
   console.log("Geolocation is not supported by this browser.");
  }
  //google.maps.event.addListener(marker, 'click', function () {
    
  //});
  
});