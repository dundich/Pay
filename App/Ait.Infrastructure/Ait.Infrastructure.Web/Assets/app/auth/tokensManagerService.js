; (function (angular, window, undefined) {

    'use strict';

    var app = angular.module('tokensManagerService', ['authSettings']);

    app.factory('tokensManagerService', ['$http', 'authSettings', function ($http, authSettings) {

        var serviceBase = authSettings.apiServiceBaseUri;

        var tokenManagerServiceFactory = {};

        var _getRefreshTokens = function () {

            return $http.get(serviceBase + 'api/refreshtokens').then(function (results) {
                return results;
            });
        };

        var _deleteRefreshTokens = function (tokenid) {

            return $http.delete(serviceBase + 'api/refreshtokens/?tokenid=' + tokenid).then(function (results) {
                return results;
            });
        };

        tokenManagerServiceFactory.deleteRefreshTokens = _deleteRefreshTokens;
        tokenManagerServiceFactory.getRefreshTokens = _getRefreshTokens;

        return tokenManagerServiceFactory;

    }]);

})(angular, window);