; (function (angular, window, undefined) {

    var app = angular.module('authSettings', []);

    app.constant('authSettings', {
        apiServiceBaseUri: '../Ait.Auth.Api/',
        clientId: 'ngAuthApp',
        uriLogin: '/login'
    });

})(angular, window);