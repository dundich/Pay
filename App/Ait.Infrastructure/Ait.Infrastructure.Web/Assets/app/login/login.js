
; (function (angular, window, document, undefined) {
    'use strict';

    var LoginComp = function ($scope, $routeParams, $location, $localStorage, emitter, authSettings, authService, aitToast) {

        var self = this;

        this.message = '';
        this.isAuthExternal = true;

        var state = this.state = {
            userName: $routeParams.username || '',
            password: '',
            useRefreshTokens: true
        };

        this.login = function () {
            self.message = '';
            return authService.login(state).catch(function (err) {
                self.message = err;
            });
        };


        this.authExternalProvider = function (provider) {

            var redirectUri = window.location.origin + window.location.pathname + 'authcomplete.html';

            var externalProviderUrl = authSettings.apiServiceBaseUri
                + "api/Account/ExternalLogin?provider=" + provider
                + "&response_type=token&client_id=" + authSettings.clientId
                + "&redirect_uri=" + redirectUri;

            window.$windowScope = $scope;

            var oauthWindow = window.open(externalProviderUrl, "Authenticate Account", "location=0,status=0,width=600,height=750");
        };


        $scope.authCompletedCB = function (fragment) {

            $scope.$apply(function () {

                if (fragment.haslocalaccount == 'False') {

                    authService.logOut();

                    authService.externalAuthData = {
                        provider: fragment.provider,
                        userName: fragment.external_user_name,
                        userEmail: fragment.external_user_email,
                        externalAccessToken: fragment.external_access_token
                    };

                    $location.path('/associate');
                }
                else {
                    //Obtain access token and redirect to orders
                    var externalData = { provider: fragment.provider, externalAccessToken: fragment.external_access_token };

                    authService.obtainAccessToken(externalData)
                        .then(function (response) {
                            $location.path(authSettings.uriHome || '/');
                        },
                        function (err) {
                            $scope.message = err.error_description;
                        });
                }
            });
        }

        this.$onInit = function () {

        };

        this.$onDestroy = function () {
            $scope.authCompletedCB = null;
            window.$windowScope = null;
        };
    };


    LoginComp.$inject = ['$scope', '$routeParams', '$location', '$localStorage', 'aitEmitter', 'authSettings', 'authService', 'aitToast'];



    var Ctrl = function ($scope, $location, emitter, authSettings) {

        var cbk = emitter.on('event:login', function () {
            $location.path(authSettings.uriHome);
        });

        $scope.$on('$destroy', function () {
            if (cbk)
                cbk.off();
        });
    };

    Ctrl.$inject = ['$scope', '$location', 'aitEmitter', 'authSettings'];


    var tmpl = {
        controller: Ctrl,

        template: '\
<div class="row">\
    <div class="col s12 l6 offset-l3">\
        <h2 class="col s12 header">Войти</h2>\
        <login></login>\
    </div>\
</div>'
    };



    angular
      .module('login', ['ngRoute', 'ngSanitize', 'ngStorage', 'aitEmitter', 'authService', 'aitUI'])

      .config(['$routeProvider', function ($routeProvider) {
          $routeProvider
            .when('/login/:username', tmpl)
            .when('/login', tmpl);
      }])

      .component('login', {
          controller: LoginComp,
          template: '\
<form name="form" role="form">\
    <div class="">\
        <ait-field class="col s12" form="form" caption="Пользователь" ng-model="$ctrl.state.userName" required="true" ait-focus-on="::!$ctrl.state.userName">\
        </ait-field>\
        <ait-field class="col s12" ait-field-type="password" form="form" caption="Пароль" ng-model="$ctrl.state.password" required="true" ait-focus-on="::!!$ctrl.state.userName">\
        </ait-field>\
    </div>\
    <div class="row">\
        <p class="col s12">\
            <br\>\
            <button class="btn btn-large" type="submit"\
                    ng-disabled="form.$invalid"\
                    ng-class="{disabled:(form.$invalid)}"\
                    data-ng-click="$ctrl.login()">Войти</button>\
            <a style="float:right;" class="btn btn-large btn-flat" type="button" ng-href="#/signup/{{$ctrl.state.userName}}">Регистрация</a>&nbsp;\
        </p>\
    </div>\
    <div data-ng-hide="$ctrl.message == \'\'" class="alert alert-danger">\
        <ait-error-panel error="$ctrl.message"></ait-error-panel>\
    </div>\
    <div class="col s12">\
        <p>Войти через социальные сети</p>\
        <p>\
            <button ng-class="{disabled:!$ctrl.isAuthExternal}" class="btn-large btn-floating blue" type="button" ng-click="$ctrl.authExternalProvider(\'Facebook\')"><i class="fa fa-facebook"></i></button>&nbsp;\
            <button ng-class="{disabled:!$ctrl.isAuthExternal}" class="btn-large btn-floating red" type="button" ng-click="$ctrl.authExternalProvider(\'Google\')"><i class="fa fa-google-plus"></i></button>\
        </p>\
    </div>\
</form>\
'
      });


})(angular, window, document);