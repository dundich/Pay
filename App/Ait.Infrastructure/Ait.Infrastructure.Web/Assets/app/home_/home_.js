; (function (angular, window, document, undefined) {
    'use strict';

    var Comp = function ($routeParams, $location, $localStorage, emitter, authService, aitToast) {

        var self = this;

        var state = this.state = {
            userName: ''
        };

        function load() {
            return angular.extend(state, authService.authentication);
        }


        this.$onInit = function () {
            load();
        };

        this.$onDestroy = function () {

        };


        this.logout = function () {
            authService.logOut();
            load();
        };

        this.getClaims = function () {
            authService.getClaims().then(function (d) {
                aitToast(d);
            });
        };
    };


    Comp.$inject = ['$routeParams', '$location', '$localStorage', 'aitEmitter', 'authService', 'aitToast'];


    angular
      .module('home_', ['ngRoute', 'ngSanitize', 'ngStorage', 'aitEmitter', 'authService', 'aitUI', 'aitServiceFactory'])

      .config(['$routeProvider', function ($routeProvider) {
          $routeProvider
            .when('/home_', {
                template: '\
<div class="row">\
    <div class="col s6 offset-s3">\
        <h2 class="col s12 header">Hello</h2>\
        <home_></home_>\
    </div>\
</div>'
            });
      }])

      .component('home_', {
          controller: Comp,
          template: '\
<div>USER:{{$ctrl.state.userName}}</div>\
<div><button class="btn" type="button" ng-click="$ctrl.getClaims()">Claims</button>&nbsp;\</div>\
<div>\
    <ul class="list-inline">\
        <li>\
            <a href="#/home_">Home</a>\
        </li>\
        <li>\
            <a href="#/login">Login</a>\
        </li>\
        <li>\
            <a href="" ng-click="$ctrl.logout()" class="ng-binding">Sign Out</a>\
        </li>\
        <li ng-hide="loggedIn" class="">\
            <a href="#/signup">sign up</a>\
        </li>\
    </ul>\
</div>\
'
      });


})(angular, window, document);