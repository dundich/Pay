; (function (angular, window, document, undefined) {
    'use strict';

    var Comp = function ($routeParams, $location, $localStorage, emitter, authService, aitToast) {

        var self = this;

        var state = this.state = {
            userName: '',
            claims: null,
            currentPassword: '',
            newPassword: ''
        };

        function load() {
            return angular.extend(state, authService.authentication);
        }


        this.$onInit = function () {
            load();

            $('.collapsible').collapsible({
                accordion: false // A setting that changes the collapsible behavior to expandable instead of the default accordion style
            });
        };

        this.$onDestroy = function () {

        };


        this.logout = function () {
            authService.logOut();
            load();
        };

        this.isClaimsLoading = true;

        this.loadClaims = function () {
            if (state.claims) return;

            this.getClaims().then(function (d) {
                self.isClaimsLoading = false;
                state.claims = d;
            }, function (e) {
                self.isClaimsLoading = false;
                aitToast(e, 'error');
            });
        };


        this.getClaims = function () {
            return authService.getClaims().then(function (d) {
                return d.data;
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
    <div class="col m8 offset-m2">\
        <home_></home_>\
    </div>\
</div>'
            });
      }])

      .component('home_', {
          controller: Comp,
          template: '\
\
    <h2 class="header row">Привет <strong>{{$ctrl.state.userName}}</strong>! <a ng-show="$ctrl.state.isAuth" href="" style="float:right;" ng-click="$ctrl.logout()" class="ng-binding">(выйти)</a></h2>\
    <p>\
        <span ng-hide="$ctrl.state.isAuth">\
            <a href="#/login" ng-click="$ctrl.logout()" class="ng-binding">Войти</a>\
                <span style="margin:3em;"> | </span> \
            <a href="#/login" ng-click="$ctrl.logout()" class="ng-binding">Регистрация</a>\
        </span>\
    </p>\
    <div ng-show="$ctrl.state.isAuth">\
    <ul class="collapsible" data-collapsible="accordion">\
        <li>\
            <div class="collapsible-header"><i class="fa fa-key" aria-hidden="true"></i> Сменить пароль</div>\
            <div class="collapsible-body">\
<form name="form" class="row" role="form"  ng-submit="$ctrl.signUp()" style="margin-top: 1em;">\
    <div class="col l1">&nbsp;</div>\
    <ait-field class="col l5 s12" ait-field-type="password" form="form" caption="Текущий пароль" ng-model="$ctrl.state.currentPassword" required="true">\
    </ait-field>\
    <ait-field class="col l5 s12" ait-field-type="password" form="form" caption="Новый пароль" ng-model="$ctrl.state.newPassword" required="true">\
    </ait-field>\
    <div class="col l1">&nbsp;</div>\
    <div class="offset-m1 col m10 s12" >\
            <br>\
            <br>\
            <button type="submit" class="btn"\
                ng-disabled="form.$invalid"\
                ng-class="{disabled:(form.$invalid)}">\
                Сменить пароль\
            </button>\
            <ait-loading style="display:block;float:right;" ng-if="$ctrl.isLoading"></ait-loading>\
    </div>\
    <div ng-if="$ctrl.message" class="col s12">\
        <br>\
        <p ng-class="{\'red-text\': !$ctrl.savedSuccessfully, \'blue-text\': $ctrl.savedSuccessfully}">\
          <span ng-bind-html="$ctrl.message"></span>\
        </p>\
     </div>\
</form>\
            </div>\
        </li>\
        <li ng-click="$ctrl.loadClaims()">\
            <div class="collapsible-header"><i class="fa fa-credit-card" aria-hidden="true"></i> Удостоверения</div>\
            <div class="collapsible-body">\
                <p>\
                    <ait-loading ng-if="$ctrl.isClaimsLoading"></ait-loading>\
                    <span class="row" ng-repeat="claim in $ctrl.state.claims">\
                        {{$index+1}}. <span ng-bind="claim.type"></span> &#8594; <strong ng-bind="claim.value"></strong>\
                    </span>\
                </p>\
            </div>\
        </li>\
    </ul>\
    </div>\
\
'
      });


})(angular, window, document);