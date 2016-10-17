; (function (angular, window, document, undefined) {
    'use strict';

    var Comp = function ($routeParams, $location, $timeout, $sce, emitter, authService, aitToast) {

        var self = this;

        this.savedSuccessfully = false;
        this.message = "";

        var registerData = this.registerData = {
            userName: authService.externalAuthData.userName,
            userEmail: authService.externalAuthData.userEmail,
            provider: authService.externalAuthData.provider,
            externalAccessToken: authService.externalAuthData.externalAccessToken
        };

        this.registerExternal = function () {

            authService.registerExternal(registerData).then(function (response) {

                self.savedSuccessfully = true;
                self.message = "Пользователь был успешно зарегистрирован, вы будете перенаправлены на главную страницу в течение 2 секунд.";
                startTimer();

            }, function (response) {
                var errors = [];
                for (var key in response.modelState) {
                    errors.push(response.modelState[key]);
                }

                self.message = $sce.trustAsHtml("Не удалось зарегистрировать пользователя: <br/>" + errors.join('<br/>'));
            });
        };

        var startTimer = function () {
            var timer = $timeout(function () {
                $timeout.cancel(timer);
                $location.path('/');
            }, 2000);
        }

        this.$onInit = function () {
        };

        this.$onDestroy = function () {
        };
    };


    Comp.$inject = ['$routeParams', '$location', '$timeout', '$sce', 'aitEmitter', 'authService', 'aitToast'];


    angular
      .module('associate', ['ngRoute', 'ngSanitize', 'aitEmitter', 'authService', 'aitUI', 'aitServiceFactory'])

      .config(['$routeProvider', function ($routeProvider) {
          $routeProvider
            .when('/associate', {
                template: '\
<div class="row">\
    <div class="col m6 offset-m3">\
        <associate></associate>\
    </div>\
</div>'
            });
      }])

      .component('associate', {
          controller: Comp,
          template: '\
<div>\
    <h3 class="header">Связать ваш {{$ctrl.registerData.provider}} аккаунт - ({{$ctrl.registerData.userEmail}}) ?</h3>\
    <p class="text-success">Вы успешно аутентифицированы в <strong>{{$ctrl.registerData.provider}} </strong>. <br>\
    Пожалуйста, введите имя пользователя ниже для этого сайта и нажмите кнопку Зарегистрировать, чтобы войти.</p>\
    <div class="col l8 s12">\
        <br>\
        <form name="form" role="form">\
            <div class="row">\
                <ait-field class="col s12" form="form" caption="Пользователь" ng-model="$ctrl.registerData.userName" required="true" ait-focus-on="true">\
                </ait-field>\
                <div class="col s12">\
                    <br>\
                    <button class="btn btn-large" type="button" style="margin-top:10px;"\
                        ng-disabled="form.$invalid"\
                        ng-class="{disabled:(form.$invalid)}"\
                        data-ng-click="$ctrl.registerExternal()">Зарегистрировать</button>\
                </div>\
            </div>\
            <br>\
            <p ng-class="{\'red-text\': !$ctrl.savedSuccessfully, \'green-text\': $ctrl.savedSuccessfully}">\
                <span ng-bind-html="$ctrl.message"></span>\
            </p>\
        </form>\
    </div>\
</div>\
'
      });


})(angular, window, document);