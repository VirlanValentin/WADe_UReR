var hVM;
var map;
var infowindow;

function AddFriend(id) {
  //TODO: do put to add friend
  //api / UserProfile / { id } / friends / { friendId }
  $.post($("#baseUrl").val() + '/' + hVM.UserId + '/friends/',{ id: id }, function (data) {
    
  });
}
function AddEnemy(id) {
  //TODO: do put to add friend
  $.post($("#baseUrl").val() + '/' + hVM.UserId + '/enemies', { id: id }, function (data) {
    console.log(data);
  });
}
function Like(id) {
  //TODO: do put to add friend
  //{id}/likes/places/{placeId}
  $.post($("#baseUrluserProfile").val() + '/' + hVM.UserId + '/likes/places', { id: id }, function (data) {
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

function ElementDetails(address, openNow, phone, website, type, rating) {
  this.Address = address;
  this.OpenNow = openNow;
  this.Phone = phone;
  this.WebSite = website;
  this.Type = type;
  this.Rating = rating;
}

function Movie(name, data, imdb) {
  this.Name = name;
  this.Data = data;
  this.ImdbUrl = imdb;
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
  self.LikedPlacesElementsIds = [];
  self.SelectedPlaceDetails = ko.observable();
  self.LocationTypes = ko.observableArray();


  self.GetLikedPlaces = function () {
    $.get($("#baseUrl").val() + '/' + self.UserId + '/likes/places', function (data) {
      console.log('liked places');
      self.LikedPlacesElementsIds = data.map(function (elem) {
        if (typeof elem.Data.id === "undefined")
          return ''; 
        return elem.Data.id;
      });
    });
  }
  self.IsPlaceLiked = function (placeId) {
    if (self.LikedPlacesElementsIds.indexOf(placeId) > -1) {
      setTimeout(function () { $("#" + 'map_' + placeId).hide() }, 500);
      //$("#" + 'map_' + placeId).hide();
    }
  }

  self.GetFriends = function () {
    $.get($("#baseUrl").val() + '/'+self.UserId + '/friends', function (data) {
      self.FriendsList = data.map(function (elem) {
        return elem.Id;
      });

      self.GetAllUsers();
    });
  }

  self.GetEnemies = function () {
    $.get($("#baseUrl").val() + '/' + self.UserId + '/enemies', function (data) {
      self.EnemiesList = data.map(function (elem) {
        return elem.Id;
      });
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
  self.PlacesMarkers = [];

  
  self.LandMarks = ko.observable();
  self.GetInfo = function (type) {
    //remove all markers
    if (self.PlacesMarkers) {
      for (i in self.PlacesMarkers) {
        self.PlacesMarkers[i].setMap(null);
      }
      self.PlacesMarkers.length = 0;
    }


    var typestring = ''
    if (type != undefined && type != 'none')
      typestring += '&type=' + type;
    //for the moment, mock some data
    //self.AddMarker('friend', elem);
    console.log('getting places...');
    $.get($("#baseUrlPlaces").val() + '?lat=' + self.Lat + '&lon=' + self.Long + typestring, function (data) {
      data.places.forEach(function (elem) {
        var url = typeof elem.website === "undefined" ? null : elem.website;
        var alreadyLiked = self.LikedPlacesElementsIds.indexOf(elem.Id) > -1 ? 2 : -2;
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
    self.PlacesMarkers.push(marker);
    marker.addListener('click', function () {
      self.SelectedElement(element);
      self.IsPlaceLiked(element.Id);

      infowindow.setContent(this.content);
      infowindow.open(map, this);
    });
  }
  self.Related = ko.observableArray();
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
      var types = "";
      var website = typeof data.website === undefined ? "" : data.website;
      var address = typeof data.address === undefined ? "" : data.address;
      var phone =  typeof data.phone === undefined ? "" : data.phone;
      var rating =  typeof data.rating === undefined ? "" : data.rating;
      var open_now = data.open_now === true ? 'Yes' : 'No';
      for (var index = 0; index < data.types.length ; index++) {
        types += data.types[index].name;
        if (index !== data.types.length) {
          types += ", ";
        }
      }
      var e = new ElementDetails(address, open_now, phone, website, types, rating);
      self.Related.removeAll();
      self.SelectedPlaceDetails(e);
      //TODO: get sugestions:
      $.get($("#baseUrlPlaces").val() + '/' + self.SelectedElement().Id + '/related', function (data) {
        console.log(data);
        for (var i = 0; i < data.related.length && i < 4; i++) {
          var me = new MapElement(data.related[i].name, data.related[i].icon, '', data.related[i].url, 0, 0, -3, data.related[i].id);
          self.Related.push(me);
        }
      });
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
  self.LocationType = ko.observable();
  self.LocationType.subscribe(function (newValue) {
    //console.log(newValue);
    self.GetInfo(newValue);
  }, this);

  self.GetPlaces = function () {
    $.get($("#baseUrlPlaces").val() + '/types', function (data) {
      self.LocationTypes(data.types.map(function (e) {
        return e.name;
      }));
      self.LocationTypes.push('none');
      self.LocationType('none');
    });
  }


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

  //MOVIES!
  self.MoviesGenres = ko.observableArray();
  self.SelectedGenre = ko.observable();
  self.SelectedGenre.subscribe(function (newValue) {
    //remove old movies: 
    self.MoviesToShow.removeAll();
    //get movies 
    $.get($("#baseUrlMovies").val() +'?releaseDate=01-01-2017'+ '&genre=' + newValue, function (data) {
      console.log(data);
      data.forEach(function (m) {
        var mov = new Movie(m.Title, m.Date, m.ImdbLink);
        self.MoviesToShow.push(mov);
      });
//movies to show
    });
    $("#moviesModal").modal('show');
  }, this);
  self.MoviesToShow = ko.observableArray();
  self.GetAllGenres = function () {
    $.get($("#baseUrlMovies").val() + '/genres', function (data) {
      console.log(data);
      self.MoviesGenres(data.map(function (mg) {
        return mg.Label;
      }));
      self.MoviesGenres.push('none');
    })
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
    collapsible: true,
    heightStyle: "content"
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
  hVM.GetPlaces();
  hVM.GetAllGenres();
  if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(hVM.SetPosition);
  } else {
   console.log("Geolocation is not supported by this browser.");
  }
  
});