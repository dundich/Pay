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

                var s = d.data.map(function (e, i) {
                    return '<li>' + i + '. ' + e.type + " " + e.value + '</li>';
                }).join('<br>');
               
                aitToast($('<ul>' + s + '</ul>'), 7000, 'info');
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
        <home_></home_>\
    </div>\
</div>'
            });
      }])

      .component('home_', {
          controller: Comp,
          template: '\
<h2 class="col s12 header">Привет <strong>{{$ctrl.state.userName}}</strong>! <a ng-show="$ctrl.state.isAuth" href="" style="margin-left:3em;" ng-click="$ctrl.logout()" class="ng-binding">(выйти)</a></h2>\
<p>\
    <span ng-hide="$ctrl.state.isAuth">\
        <a href="#/login" ng-click="$ctrl.logout()" class="ng-binding">Войти</a>\
          <span style="margin:3em;"> | </span> \
        <a href="#/login" ng-click="$ctrl.logout()" class="ng-binding">Регистрация</a>\
    </span>\
    <span ng-show="$ctrl.state.isAuth">\
        <button class="btn" type="button" ng-click="$ctrl.getClaims()">Claims</button>&nbsp; \
    </span>\
</p>\
'
      });


})(angular, window, document);