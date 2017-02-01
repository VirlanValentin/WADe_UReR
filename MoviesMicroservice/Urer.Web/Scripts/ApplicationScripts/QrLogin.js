function callMyFunction(msg) {
  var indexOfSep = msg.indexOf('_');
  var name = msg.substring(0, indexOfSep);
  var pass = msg.substring(indexOfSep+1);
  sessionStorage.setItem('pass', pass);
  $.post($("#baseUrl").val() + "/Login", {
    Name: name,
    Email: '',
    Password: pass,
    Latitude: lat,
    Longitude: long,
  }, function (data) {
    //data.Latitude = self.Location().latitude;
    //data.Longitude = self.Location().longitude;

    sessionStorage.setItem('userId', data.Id);
    sessionStorage.setItem('userName', data.Name);
    sessionStorage.setItem('userUrl', data.Resource);

    window.location.href = $("#indexUrl").val();
  });
}

function setPosition(position) {
  lat = position.coords.latitude;
  long = position.coords.longitude;

  sessionStorage.setItem('lat', position.coords.latitude);
  sessionStorage.setItem('long', position.coords.longitude);
}

var lat;
var long;

$(function () {
  load();
  setimg();


  if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(setPosition);
  } else {
    //iVM.Location("Geolocation is not supported by this browser.");
    lat = 0;
    long = 0;
  }
});