; (function (angular, window, undefined) {

    var app = angular.module('authSettings', []);

    app.constant('authSettings', {
        apiServiceBaseUri: '../Ait.Auth.Api/',
        clientId: 'ngAit',
        uriLogin: '/login',
        uriHome: '/userprofile'
    });

})(angular, window);