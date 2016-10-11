
; (function (angular, window, document, undefined) {
    'use strict';

    var Comp = function ($routeParams, $location, $localStorage, emitter, authService) {

        var self = this;

        this.message = '';

        var state = this.state = {
            userName: '',
            password: '',
            useRefreshTokens: false,
        };

        this.login = function () {

            self.message = '';

            authService.login(state).then(function (response) {
                console.log('EHF!');
            },
            function (err) {
                if (err.error_description) {
                    self.message = err.error_description.replace(/["']{1}/gi, "");
                }
                else {
                    self.message = 'Ошибка!';
                }
            });
        };

        this.$onInit = function () {

        };


        this.$onDestroy = function () {

        };

    };


    Comp.$inject = ['$routeParams', '$location', '$localStorage', 'aitEmitter', 'authService'];


    angular
      .module('login', ['ngRoute', 'ngSanitize', 'ngStorage', 'aitEmitter', 'authService', 'aitUI'])

      .config(['$routeProvider', function ($routeProvider) {
          $routeProvider
            .when('/login', {
                template: '<login/>'
            })
          ;
      }])

      .component('login', {
          controller: Comp,
          template: '\
<form role="form">\
    <div class="row">\
        <div class="col l2">\
            &nbsp;\
        </div>\
        <div class="col l4">\
            <h2 class="form-login-heading">Войти</h2>\
            <input type="text" class="form-control" placeholder="Username" data-ng-model="$ctrl.state.userName" required autofocus>\
            <input type="password" class="form-control" placeholder="Password" data-ng-model="$ctrl.state.password" required>\
            <p>\
                <button class="btn btn-md btn-info btn-block" type="submit" data-ng-click="$ctrl.login()">Войти</button>\
            </p>\
            <div data-ng-hide="$ctrl.message == \'\'" class="alert alert-danger">\
                <ait-error-panel error="$ctrl.message"></ait-error-panel>\
            </div>\
        </div>\
        <div class="col l4">\
            <h2 class="form-login-heading">&nbsp;</h2>\
            <p>Или вы можете войти в систему, используя один из социальных логинов ниже</p>\
            <button class="btn btn-large btn-facebook btn-block blue" type="button" data-ng-click="authExternalProvider(\'Facebook\')"><i class="fa fa-facebook"></i> | Connect with Facebook</button>\
            <br>\
            <button class="btn btn-large btn-google-plus btn-block red" type="button" data-ng-click="authExternalProvider(\'Google\')"><i class="fa fa-google-plus"></i> | Connect with Google+</button>\
        </div>\
        <div class="col l2">\
            &nbsp;\
        </div>\
    </div>\
</form>\
'
      });


})(angular, window, document);