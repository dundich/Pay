; (function (angular, window, document, undefined) {
    'use strict';


    var app = angular.module('userprofile', ['ngRoute', 'ngSanitize', 'ngStorage', 'aitEmitter', 'authService', 'aitUI', 'aitServiceFactory']);

    app.config(['$routeProvider', function ($routeProvider) {
        $routeProvider
          .when('/userprofile', {
              template: '\
<div class="row">\
    <div class="col m8 offset-m2">\
        <userprofile></userprofile>\
        <a href="api/email/send/text">email</a>\
    </div>\
</div>'
          });
    }]);


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


        this.loadClaims = function (forced) {
            if (this.isClaimsLoading) return;
            if (!forced && state.claims) return;

            this.isClaimsLoading = true;
            this.getClaims().then(function (d) {
                self.isClaimsLoading = false;
                state.claims = d;
            }, function (e) {
                self.isClaimsLoading = false;
                aitToast(e, 'error');
            });
        };

        this.getClaims = function () {
            return authService.getClaims();
        };
    };


    Comp.$inject = ['$routeParams', '$location', '$localStorage', 'aitEmitter', 'authService', 'aitToast'];


    app.component('userprofile', {
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
                <changepassword></changepassword>\
            </div>\
        </li>\
        <li ng-click="$ctrl.loadClaims()">\
            <div class="collapsible-header"><i class="fa fa-credit-card" aria-hidden="true"></i> Удостоверения</div>\
            <div class="collapsible-body">\
                <p>\
                   <ait-loading ng-if="$ctrl.isClaimsLoading"></ait-loading>\
                   <a href="" ng-click="$ctrl.loadClaims(true)" style="float:right;"><i class="fa fa-refresh" aria-hidden="true"></i></a>\
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





    var ChangePasswordComp = function (emitter, authService, aitToast) {

        var self = this;

        var state = this.state = {
            userName: '',
            currentPassword: '',
            newPassword: ''
        };

        this.isLoading = false;

        function load() {
            return angular.extend(state, authService.authentication);
        }


        this.$onInit = function () {
            load();

            $('.collapsible').collapsible({
                accordion: false // A setting that changes the collapsible behavior to expandable instead of the default accordion style
            });
        };

        this.reset = function () {
            state.currentPassword = '';
            state.newPassword = '';
        };

        this.changePassword = function () {
            this.isLoading = true;
            return authService
                .changePassword({
                    userName: state.userName,
                    currentPassword: state.currentPassword,
                    newPassword: state.newPassword
                })
                .then(function (d) {
                    aitToast('Пароль успешно изменен!', 'ok');
                    emitter.emit('event:password_changed', d);
                    self.reset();
                    self.isLoading = false;
                }, function (e) {
                    aitToast(e, 'error');
                    self.isLoading = false;
                });
        };

    };

    ChangePasswordComp.$inject = ['aitEmitter', 'authService', 'aitToast'];




    app.component('changepassword', {
        controller: ChangePasswordComp,
        template: '\
<form name="form" class="row" role="form"  ng-submit="$ctrl.changePassword()" style="margin-top: 1em;">\
    <div class="col l1">&nbsp;</div>\
    <ait-field class="col l5 s12" ait-field-type="password" form="form" caption="Текущий пароль" ng-model="$ctrl.state.currentPassword">\
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
                Сохранить\
            </button>\
            <ait-loading style="display:block;float:right;" ng-if="$ctrl.isLoading"></ait-loading>\
    </div>\
    <div ng-if="$ctrl.message" class="col s12">\
        <br>\
        <p ng-class="{\'red-text\': !$ctrl.savedSuccessfully, \'blue-text\': $ctrl.savedSuccessfully}">\
          <span ng-bind-html="$ctrl.message"></span>\
        </p>\
     </div>\
</form>'
    });


})(angular, window, document);