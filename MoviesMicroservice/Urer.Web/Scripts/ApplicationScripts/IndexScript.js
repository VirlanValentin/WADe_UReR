var iVM;

var IndexViewModel = function() {
  var self = this;

  self.Username = ko.observable().extend({ required: true });
  self.Password = ko.observable().extend({ required: true });
  self.Location = ko.observable(null);
  //validation:
  self.saveErrors = ko.validation.group([
    self.Username, self.Password]);
  self.Login = function () {
    if (iVM.saveErrors().length > 0) {
      iVM.saveErrors.showAllMessages()
    }
    else {
      //continue
      //TODO: post data and redirect to homepage
    }
  }
  self.SetPosition = function (position) {
    self.Location({
      latitude: position.coords.latitude,
      longitude: position.coords.longitude
    });
  }
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

  iVM = new IndexViewModel();
  ko.applyBindings(iVM);
  //get info about user location
  if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(iVM.SetPosition);
  } else {
    //iVM.Location("Geolocation is not supported by this browser.");
    iVM.Location(0);
  }

});