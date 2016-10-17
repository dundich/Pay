
; (function (angular, window, document, undefined) {
    'use strict';

    var Comp = function ($routeParams, $location, $timeout, $sce, emitter, authService, authSettings) {

        var self = this;

        this.message = '';
        this.savedSuccessfully = false;
        this.isLoading = false;

        var state = this.state = {
            userName: $routeParams.username || "",
            password: "",
            confirmPassword: ""
        };


        var startTimer = function () {
            var timer = $timeout(function () {
                $timeout.cancel(timer);

                $location.path(authSettings.uriLogin + '/' + state.userName);
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


    Comp.$inject = ['$routeParams', '$location', '$timeout', '$sce', 'aitEmitter', 'authService', 'authSettings'];


    var tmpl = {
        template: 
'<div class="row">\
    <div class="col s4 offset-s4">\
        <h2 class="col s12 header">Регистрация</h2>\
        <signup></signup>\
    </div>\
</div>'
    };



    angular
      .module('signup', ['ngRoute', 'ngSanitize', 'ngStorage', 'aitEmitter', 'authService', 'aitUI'])

      .config(['$routeProvider', function ($routeProvider) {
          $routeProvider
            .when('/signup/:username', tmpl)
            .when('/signup', tmpl)
          ;
      }])

      .component('signup', {
          controller: Comp,
          template: '\
<form name="form" class="form-login" role="form"  ng-submit="$ctrl.signUp()">\
    <ait-field class="col s12" form="form" caption="Пользователь" ng-model="$ctrl.state.userName" required="true" ait-focus-on="::!$ctrl.state.userName">\
    </ait-field>\
    <ait-field class="col s12" ait-field-type="password" form="form" caption="Пароль" ng-model="$ctrl.state.password" required="true" ait-focus-on="::!!$ctrl.state.userName">\
    </ait-field>\
    <ait-field class="col s12" ait-field-type="password" form="form" caption="Повторите пароль" ng-model="$ctrl.state.confirmPassword" required="true">\
    </ait-field>\
    <div class="col s12">\
        <p>\
            <br>\
            <button type="submit" class="btn btn-large"\
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