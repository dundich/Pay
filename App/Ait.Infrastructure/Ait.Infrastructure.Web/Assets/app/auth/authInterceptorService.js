; (function (angular, window, document, undefined) {

    'use strict';

    var app = angular.module('authInterceptorService', ['ngStorage', 'authService', 'authSettings']);

    app.factory('authInterceptorService', ['$q', '$injector', '$location', '$localStorage', 'authSettings', function ($q, $injector, $location, localStorageService, authSettings) {

        var authInterceptorServiceFactory = {};

        var _request = function (config) {

            config.headers = config.headers || {};

            var authData = localStorageService.authorizationData;
            if (authData) {
                config.headers.Authorization = 'Bearer ' + authData.token;
            }

            return config;
        }

        var _responseError = function (rejection) {
            if (rejection.status === 401) {

                var authService = $injector.get('authService');
                var authData = localStorageService.authorizationData;

                if (authData && authData.useRefreshTokens) {
                    return authService
                        .refreshToken()
                        .then(function () {
                            //needed try again...    
                            return $q.reject(rejection);
                        }, function () {
                            authService.logOut();
                            $location.path(authSettings.uriLogin);
                            return $q.reject(rejection);
                        });
                }

                authService.logOut();
                $location.path(authSettings.uriLogin);
            }


            return $q.reject(rejection);
        }

        authInterceptorServiceFactory.request = _request;
        authInterceptorServiceFactory.responseError = _responseError;

        return authInterceptorServiceFactory;
    }]);

})(angular, window, document);