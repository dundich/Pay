﻿
; (function (angular, window, document, undefined) {
    'use strict';

    var Comp = function ($routeParams, $location, $timeout, $sce, emitter, authService) {

        var self = this;

        this.message = '';
        this.savedSuccessfully = false;
        this.isLoading = false;

        var state = this.state = {
            userName: "",
            password: "",
            confirmPassword: ""
        };


        var startTimer = function () {
            var timer = $timeout(function () {
                $timeout.cancel(timer);
                $location.path(authService.uriLogin);
            }, 2000);
        }

        this.signUp = function () {

            self.message = '';
            self.isLoading = true;
            return authService.saveRegistration(state).then(function (response) {
                self.isLoading = false;
                self.savedSuccessfully = true;
                self.message = "Пользователь был успешно зарегистрирован, вы будете перенаправлены на страницу входа через 2-е секунды.";
                startTimer();
            },
            function (response) {
                self.isLoading = false;
                self.savedSuccessfully = false;
                var errors = [];
                for (var key in response.data.modelState) {
                    for (var i = 0; i < response.data.modelState[key].length; i++) {
                        errors.push(response.data.modelState[key][i]);
                    }
                }
                self.message = $sce.trustAsHtml("Не удалось зарегистрировать пользователя: <br/>" + errors.join('<br/>'));
            });
        };

        this.$onInit = function () {

        };


        this.$onDestroy = function () {

        };

    };


    Comp.$inject = ['$routeParams', '$location', '$timeout', '$sce', 'aitEmitter', 'authService'];


    angular
      .module('signup', ['ngRoute', 'ngSanitize', 'ngStorage', 'aitEmitter', 'authService', 'aitUI'])

      .config(['$routeProvider', function ($routeProvider) {
          $routeProvider
            .when('/signup', {
                template: '\
<div class="row">\
    <div class="col s4 offset-s4">\
        <h2 class="col s12 header">Регистрация</h2>\
        <signup></signup>\
    </div>\
</div>\
                '
            })
          ;
      }])

      .component('signup', {
          controller: Comp,
          template: '\
<form name="form" class="form-login" role="form"  ng-submit="$ctrl.signUp()">\
    <ait-field class="col s12" form="form" caption="Пользователь" ng-model="$ctrl.state.userName" required="true" ait-focus-on="true">\
    </ait-field>\
    <ait-field class="col s12" ait-field-type="password" form="form" caption="Пароль" ng-model="$ctrl.state.password" required="true">\
    </ait-field>\
    <ait-field class="col s12" ait-field-type="password" form="form" caption="Повторите пароль" ng-model="$ctrl.state.confirmPassword" required="true">\
    </ait-field>\
    <div class="col s12">\
        <p>\
            <br>\
            <button type="submit" class="btn"\
                ng-disabled="form.$invalid || $ctrl.isLoading"\
                ng-class="{disabled:(form.$invalid || $ctrl.isLoading)}">\
                Зарегистрировать\
            </button>\
            <ait-loading style="display:block;float:right;" ng-if="$ctrl.isLoading"></ait-loading>\
        </p>\
    </div>\
    <div ng-if="$ctrl.message" class="col s12">\
        <br>\
        <p ng-class="{\'red-text\': !$ctrl.savedSuccessfully, \'blue-text\': $ctrl.savedSuccessfully}">\
          <span ng-bind-html="$ctrl.message"></span>\
        </p>\
     </div>\
</form>\
'
      });


})(angular, window, document);