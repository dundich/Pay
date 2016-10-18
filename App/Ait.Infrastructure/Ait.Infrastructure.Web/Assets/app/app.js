; (function (angular, window, undefined) {

    var app = angular.module('appAuth', [
        'ngRoute',
        'ngAnimate',
        'angular-loading-bar',
        'authInterceptorService',
        'authSettings',
        'login',
        'signup',
        'userprofile',
        'associate'
    ]);


    app.config(['$routeProvider', '$httpProvider', 'authSettings', function ($routeProvider, $httpProvider, authSettings) {
        //Enable cross domain calls
        $httpProvider.defaults.useXDomain = true;

        //Remove the header used to identify ajax call  that would prevent CORS from working
        delete $httpProvider.defaults.headers.common['X-Requested-With'];

        //AOuth
        $httpProvider.interceptors.push('authInterceptorService');

        $routeProvider.otherwise({
            redirectTo: function () { return authSettings.uriHome; }
        });

    }]);


    app.run(['$rootScope', '$http', 'aitEmitter', function ($rootScope, $http, aitEmitter) {

    }]);

})(angular, window);