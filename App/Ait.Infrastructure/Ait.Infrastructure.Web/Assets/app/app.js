; (function (angular, window, undefined) {

    var app = angular.module('appAuth', [
        'ngRoute',
        'ngAnimate',
        'angular-loading-bar',
        'authInterceptorService',
        'login',
        'signup',
        'home_',
        'associate'
    ]);

    app.run(['$rootScope', '$http', 'aitEmitter', function ($rootScope, $http, aitEmitter) {

    }]);

    app.config(['$httpProvider', function ($httpProvider) {
        //Enable cross domain calls
        $httpProvider.defaults.useXDomain = true;

        //Remove the header used to identify ajax call  that would prevent CORS from working
        delete $httpProvider.defaults.headers.common['X-Requested-With'];

        //AOuth
        $httpProvider.interceptors.push('authInterceptorService');

    }]);

})(angular, window);