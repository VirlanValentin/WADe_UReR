var hVM;
var map;
var infowindow;

function AddFriend(id) {
  //TODO: do put to add friend
  //api / UserProfile / { id } / friends / { friendId }
  $.put($("#baseUrl").val() + '/' + hVM.UserId + '/friends/'+ id, function (data) {
    
  });
}
function AddEnemy(id) {
  //TODO: do put to add friend
  $.put($("#baseUrl").val() + '/' + hVM.UserId + '/enemies', { friendId: id }, function (data) {

  });
}
function Like(id) {
  //TODO: do put to add friend
  //{id}/likes/places/{placeId}
  $.post($("#baseUrl").val() + '/' + hVM.UserId + '/likes/places', { placeId: id }, function(data){
    console.log('i like this!');
  });
}
function MapElement(name, imageSrc, description, url, lat, long, friendStatus, id) {
  this.Image = imageSrc;
  this.Name = name;
  this.Description = description;
  this.Url = url;
  this.Lat = lat;
  this.Long = long;
  this.FriendStatus = friendStatus; //-1 = enemy, 0 = neutral, 1 = friend, > 1 = liked
  this.Id = id;
}

function CreateMarkerContent(elem) {
  var display = 'initial';
  if (elem.Image == null) {
    display = 'none';
  }
  var action = ''
  switch (elem.FriendStatus) {
    case 0:
      action = `<a href="#" onclick="AddFriend('${elem.Id}')" >Add friend</a>
  <a href="#" onclick="AddEnemy('${elem.Id}')" class ="pull-right">Add enemy</a>`;
      break;
    case -2: //neutral place
      action = `<a href="#" id="map_${elem.Id}" onclick="Like('${elem.Id}')">Like</a>`;
      break;
  }

  //the img will break, but it won't affect UI
  var HTMLstring = `<div style="width: 20vh" >
  <div style="display:flex">
    <img class="img-keep-aspect-ratio" style="height:10vh; display:${display};" src="${elem.Image}" />
    <h3 onclick="hVM.ElementDetails()" style="cursor: pointer">${elem.Name}</h3>
  </div>
  <p>${elem.Description}</p>
  <a href=${elem.Url}>${elem.Name}</a>
  <div class='col-xs-12'>
  ${action}
  </div>
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
  self.UserId = sessionStorage['userId'];
  //icons:
  self.FriendsList = null;
  self.EnemiesList = null;
  self.PlacesElementsIds = [];

  self.GetLikedPlaces = function () {
    $.get($("#baseUrl").val() + '/' + self.UserId + '/likes/places', function (data) {
      console.log('liked places');
      console.log(data);
      //TODO: add this to PlacesElementsIds
    });
  }
  self.IsPlaceLiked = function (placeId) {
    if (self.PlacesElementsIds.indexOf(placeId) > -1) {
      $("#" + map + '_' + placeId).hide();
    }
  }

  self.GetFriends = function () {
    $.get($("#baseUrl").val() + '/'+self.UserId + '/friends', function (data) {
      console.log(data);
      self.FriendsList = [];

      self.GetAllUsers();
      //todo:add id array to friendslist
    });
  }

  self.GetEnemies = function () {
    $.get($("#baseUrl").val() + '/' + self.UserId + '/enemies', function (data) {
      console.log(data);
      //todo:add id array to EnemiesList
      self.EnemiesList = [];
      self.GetAllUsers();
    });
  }

  self.GetAllUsers = function () {
    if ($.isArray(self.FriendsList) && $.isArray(self.EnemiesList)) {

      $.get($("#baseUrl").val(), function (data) {
        
        for (var i = 0; i < data.length; i++) {
          if (self.FriendsList.indexOf(data[i].Id) > -1) {
            var mapElem = new MapElement(data[i].Name, null, 'Your friend', data[i].Resource,
              data[i].Latitude,data[i].Longitude,1, data[i].Id);
            self.AddMarker('friend', mapElem);
            continue;
          }
          if (self.EnemiesList.indexOf(data[i].Id) > -1) {
            var mapElem = new MapElement(data[i].Name, null, 'Your enemy', data[i].Resource,
              data[i].Latitude, data[i].Longitude, -1,data[i].Id);
            self.AddMarker('enemy', mapElem);
            continue;
          }
          if (data[i].Id == sessionStorage['userId']) {
            continue;
          }
          var mapElem = new MapElement(data[i].Name, null, 'Someone', data[i].Resource,
              data[i].Latitude, data[i].Longitude, 0, data[i].Id);
          self.AddMarker('neutral', mapElem);
        }
        console.log(data);
      });
    }
  }

  self.Icons = {};
  self.GetIcons = function () {
    $("#icons img").each(function (index) {
      var name = $(this).data('name');
      var src = $(this).prop('src');
      self.Icons[name] = src;
    });
  }
  
  
  self.LandMarks = ko.observable();
  //TODO: get stuff
  self.GetInfo = function () {
    //for the moment, mock some data
    //self.AddMarker('friend', elem);
    $.get($("#baseUrlPlaces").val() + '?lat=' +self.Lat +'&lon=' + self.Long, function (data) {
      //console.log(data.places[0]);
      data.places.forEach(function (elem) {
        var url = typeof  elem.website === "undefined" ? null : elem.website;
        var mapElem = new MapElement(elem.name,elem.icon, '',
          url, elem.location.latitude, elem.location.longitude,-2, elem.id);
        self.AddMarker(null, mapElem, elem.icon);
      });
    });
  }




  self.AddMarker = function (feature, element, iconSrc) {
    //element is of type MapElement
    // direction to the right and in the Y direction down.
    var image = {
      url: iconSrc,
      // This marker is 20 pixels wide by 32 pixels high.
      scaledSize: new google.maps.Size(30, 30), // scaled size
      origin: new google.maps.Point(0,0), // origin
      anchor: new google.maps.Point(0, 0) // anchor


      //size: new google.maps.Size(20, 32),
      //// The origin for this image is (0, 0).
      //origin: new google.maps.Point(0, 0)
      //// The anchor for this image is the base of the flagpole at (0, 32).
      ////anchor: new google.maps.Point(0, 32)
    };
    var icon = feature == null ? image : self.Icons[feature];
    var marker = new google.maps.Marker({
      position: new google.maps.LatLng(element.Lat, element.Long),
      icon: icon,
      map: map,
      //content: content, //HTML content
      content: CreateMarkerContent(element)
    });
    marker.addListener('click', function () {
      self.SelectedElement(element);
      self.IsPlaceLiked(element.Id);

      infowindow.setContent(this.content);
      infowindow.open(map, this);
    });
  }
  self.ElementDetails = function () {
    $("#detailsLarge").show();
    $("#detailsSmall").show();
    if ($("#detailsSmall:visible").length != 0) {
      //this means that the details small screen is visible
      //so hide map 
      $('#map').hide();
    }
    $("#plus-button").hide();
    //TODO: get data for object
    $.get($("#baseUrlPlaces").val() + '/' + self.SelectedElement().Id, function (data) {
      console.log(data);
    });

  }
  self.HideElementDetails = function () {
    $('#map').show();
    $("#detailsSmall").hide();
    $("#plus-button").show();
    $("#detailsLarge").hide();
  }

  //filters
  self.SearchField = ko.observable();
  self.SeeFriends = ko.observable(true);
  self.SeeEnemies = ko.observable(true);
  self.SeeRestaurants = ko.observable(true);
  self.SeeMuseums = ko.observable(true);

  self.ShowElementDetails = ko.observable(false);
  self.SelectedElement = ko.observable();
  self.SelectedElementImages = ko.observableArray();
  self.SelectedElementSugestions = ko.observableArray();

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
  
  self.Logout = function () {
    sessionStorage.removeItem('userId');
    sessionStorage.removeItem('userName');
    sessionStorage.removeItem('userUrl');
    sessionStorage.removeItem('lat');
    sessionStorage.removeItem('long');
    window.location.href = $("#indexUrl").val();
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
  //extend jquery to add put and delete
  jQuery.each(["put", "delete"], function (i, method) {
    jQuery[method] = function (url, data, callback, type) {
      if (jQuery.isFunction(data)) {
        type = type || callback;
        callback = data;
        data = undefined;
      }

      return jQuery.ajax({
        url: url,
        type: method,
        dataType: type,
        data: data,
        success: callback
      });
    };
  });

  $("#accordion").accordion({
    collapsible: true
  });
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
  hVM.GetFriends();
  hVM.GetEnemies();
  hVM.GetLikedPlaces();

  if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(hVM.SetPosition);
  } else {
   console.log("Geolocation is not supported by this browser.");
  }
  
});