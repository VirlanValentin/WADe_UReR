﻿var iVM;

var IndexViewModel = function() {
  var self = this;
 
  self.Username = ko.observable().extend({ required: true });
  self.Password = ko.observable().extend({ required: true });
  self.Location = ko.observable(null);
  self.Email = ko.observable().extend({ required: true })
    .extend({ email: true })
  .extend({
    validation: {
      async: true,
      validator: function (val, params, callback) {
        //TODO: check if email is already in use
        callback(true);
      },
      message: 'Email aready taken'
    }
  });
  //validation:
  self.RegisterErrors = ko.validation.group([
    self.Username, self.Password, self.Email]);
  self.LoginErrors = ko.validation.group([
    self.Username, self.Password]);

  self.Login = function () {
    if (iVM.LoginErrors().length > 0) {
      iVM.LoginErrors.showAllMessages()
    }
    else {
      //continue
      //TODO: post data and redirect to homepage
      self.GoToHome();

    }
  }

  self.Register = function () {
    if (iVM.RegisterErrors().length > 0) {
      iVM.RegisterErrors.showAllMessages()
    }
    else {
      //continue
      //TODO: post data and redirect to homepage
      self.GoToHome();
    }

  }
  self.SetPosition = function (position) {
    self.Location({
      latitude: position.coords.latitude,
      longitude: position.coords.longitude
    });
    sessionStorage.setItem('lat',position.coords.latitude );
    sessionStorage.setItem('long', position.coords.longitude);
  }

  self.GoToHome = function () {
    window.location.href = $("#indexUrl").val();
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