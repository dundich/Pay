; (function (angular, window, document, undefined) {

    'use strict';

    var app = angular.module('authService', ['ngStorage', 'authSettings', 'aitEmitter']);

    app.factory('authService', ['$http', '$q', '$localStorage', 'authSettings', 'aitEmitter', function ($http, $q, localStorageService, authSettings, emitter) {

        var serviceBase = authSettings.apiServiceBaseUri;

        var authServiceFactory = {};

        var _authentication = {
            isAuth: false,
            userName: "",
            useRefreshTokens: false
        };

        var _externalAuthData = {
            provider: "",
            userName: "",
            externalAccessToken: ""
        };

        var _saveRegistration = function (registration) {
            _logOut();
            return $http.post(serviceBase + 'api/account/register', registration);
        };

        var _login = function (loginData) {

            var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

            if (loginData.useRefreshTokens) {
                data = data + "&client_id=" + authSettings.clientId;
            }

            return $http
                .post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
                .then(function (d) {
                    var response = d.data;
                    if (loginData.useRefreshTokens) {
                        localStorageService.authorizationData = {
                            token: response.access_token,
                            userName: loginData.userName,
                            refreshToken: response.refresh_token,
                            useRefreshTokens: true
                        };
                    }
                    else {
                        localStorageService.authorizationData = {
                            token: response.access_token,
                            userName: loginData.userName,
                            refreshToken: "",
                            useRefreshTokens: false
                        };
                    }

                    _authentication.isAuth = true;
                    _authentication.userName = loginData.userName;
                    _authentication.useRefreshTokens = loginData.useRefreshTokens;

                    //-------------------
                    emitter.emit('event:login', response);

                    return response;
                }, function (err, status) {
                    _logOut();
                    return $q.reject(err.data);
                });
        };

        var _logOut = function () {
            delete localStorageService.authorizationData;
            _authentication.isAuth = false;
            _authentication.userName = "";
            _authentication.useRefreshTokens = false;

            //-------------------
            emitter.emit('event:logout');
        };

        var _fillAuthData = function () {

            var authData = localStorageService.authorizationData;
            if (authData) {
                _authentication.isAuth = true;
                _authentication.userName = authData.userName;
                _authentication.useRefreshTokens = authData.useRefreshTokens;
            }

        };

        var _refreshToken = function () {
            var authData = localStorageService.authorizationData;

            if (authData && authData.useRefreshTokens) {

                var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + authSettings.clientId;

                delete localStorageService.authorizationData;

                return $http
                    .post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
                    .then(function (d) {
                        var response = d.data;

                        localStorageService.authorizationData = {
                            token: response.access_token,
                            userName: response.userName,
                            refreshToken: response.refresh_token,
                            useRefreshTokens: true
                        };

                        return response;

                    }, function (err) {
                        _logOut();
                        return $q.reject(err.data);
                    });
            }

            return $q.when(false);
        };

        var _obtainAccessToken = function (externalData) {

            return $http
                .get(serviceBase + 'api/account/ObtainLocalAccessToken', {
                    params: {
                        provider: externalData.provider,
                        externalAccessToken: externalData.externalAccessToken
                    }
                })
                .then(function (d) {
                    var response = d.data;

                    localStorageService.authorizationData = {
                        token: response.access_token,
                        userName: response.userName,
                        refreshToken: "",
                        useRefreshTokens: false
                    };

                    _authentication.isAuth = true;
                    _authentication.userName = response.userName;
                    _authentication.useRefreshTokens = false;

                    return response;

                }, function (err) {
                    _logOut();
                    return $q.reject(err.data);
                });

        };

        var _registerExternal = function (registerExternalData) {

            return $http
                .post(serviceBase + 'api/account/registerexternal', registerExternalData)
                .then(function (response) {
                    var response = d.data;

                    localStorageService.authorizationData = {
                        token: response.access_token,
                        userName: response.userName,
                        refreshToken: "",
                        useRefreshTokens: false
                    };
                    //localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: "", useRefreshTokens: false });

                    _authentication.isAuth = true;
                    _authentication.userName = response.userName;
                    _authentication.useRefreshTokens = false;

                    return response;

                }, function (err) {
                    _logOut();
                    return $q.reject(err.data);
                });
        };


        var _getClaims = function () {
            return $http.get(serviceBase + 'api/account/claims').then(function (d) {
                return d.data;
            });
        };


        var _changePassword = function (loginData) {
            var data = {
                userName: loginData.userName,
                currentPassword: loginData.currentPassword,
                newPassword: loginData.newPassword
            };
            return $http.post(serviceBase + 'api/account/changePassword', data);
        };


        authServiceFactory.saveRegistration = _saveRegistration;
        authServiceFactory.login = _login;
        authServiceFactory.logOut = _logOut;
        authServiceFactory.fillAuthData = _fillAuthData;
        authServiceFactory.authentication = _authentication;
        authServiceFactory.refreshToken = _refreshToken;

        authServiceFactory.getClaims = _getClaims;

        authServiceFactory.obtainAccessToken = _obtainAccessToken;
        authServiceFactory.externalAuthData = _externalAuthData;
        authServiceFactory.registerExternal = _registerExternal;
        authServiceFactory.changePassword = _changePassword;

        _fillAuthData();

        return authServiceFactory;
    }]);

})(angular, window, document);