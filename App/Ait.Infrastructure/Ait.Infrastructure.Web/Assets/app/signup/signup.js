
; (function (angular, window, document, undefined) {
    'use strict';

    var Comp = function ($routeParams, $location, $timeout, emitter, authService) {

        var self = this;

        this.message = '';
        this.savedSuccessfully = false;

        var state = this.state = {
            userName: "",
            password: "",
            confirmPassword: ""
        };


        var startTimer = function () {
            var timer = $timeout(function () {
                $timeout.cancel(timer);
                $location.path('/login');
            }, 2000);
        }

        this.signUp = function () {

            self.message = '';

            authService.saveRegistration(state).then(function (response) {
                self.savedSuccessfully = true;
                self.message = "Пользователь был успешно зарегистрирован, вы будете перенаправлены на страницу входа через 2-е секунды.";
                startTimer();
            },
            function (response) {
                var errors = [];
                for (var key in response.data.modelState) {
                    for (var i = 0; i < response.data.modelState[key].length; i++) {
                        errors.push(response.data.modelState[key][i]);
                    }
                }
                self.message = "Не удалось зарегистрировать пользователя из-за:\r\n" + errors.join(' ');
            });
        };

        this.$onInit = function () {

        };


        this.$onDestroy = function () {

        };

    };


    Comp.$inject = ['$routeParams', '$location', '$timeout', 'aitEmitter', 'authService'];


    angular
      .module('signup', ['ngRoute', 'ngSanitize', 'ngStorage', 'aitEmitter', 'authService', 'aitUI'])

      .config(['$routeProvider', function ($routeProvider) {
          $routeProvider
            .when('/signup', {
                template: '<signup/>'
            })
          ;
      }])

      .component('signup', {
          controller: Comp,
          template: '\
<form class="form-login" role="form">\
    <h2 class="form-login-heading">Регистрация</h2>\
    <input type="text" class="form-control" placeholder="Username" data-ng-model="$ctrl.state.userName" required autofocus>\
    <input type="password" class="form-control" placeholder="Password" data-ng-model="$ctrl.state.password" required>\
    <input type="password" class="form-control" placeholder="Confirm Password" data-ng-model="$ctrl.state.confirmPassword" required>\
    <button class="btn btn-lg btn-info btn-block" type="submit" data-ng-click="$ctrl.signUp()">Зарегистрировать</button>\
    <div ng-if="$ctrl.message">\
      {{$ctrl.message}}\
     </div>\
  </form>\
'
      });


})(angular, window, document);