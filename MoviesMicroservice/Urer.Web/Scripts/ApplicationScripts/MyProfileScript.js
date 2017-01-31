var pVM;

function User(username, userId, qrLink) {
  this.UserName = username;
  this.UserId = userId;
  this.QRLink = qrLink;
}

function Element(name, picture, url) {
  this.Name = name;
  this.Picture = picture;
  this.Url = url;
}

var MyProfileViewModel = function (user) {
  var self = this;

  self.User = ko.observable(user);

  self.Friends = ko.observableArray();
  self.Enemies = ko.observableArray();
  self.Likes = ko.observableArray();

  //notifications
  self.AlertFriends = ko.observable(true);
  self.AlertEnemies = ko.observable(true);
  self.Places = ko.observable(true);
  self.Movies = ko.observable(true);
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

  ko.validation.init({
    registerExtenders: true,
    messagesOnModified: true,
    insertMessages: true,
    parseInputAttributes: true,
    messageTemplate: 'customMessageTemplate',
    errorClass: 'input-has-error'
  });
  ko.validation.registerExtenders();

  //TODO: get user and then applyBindings:
  var user = new User('Codrin', 10, 'qrSrc');
  pVM = new MyProfileViewModel(user);

  ko.applyBindings(pVM);
  //get info about user location
 

});
