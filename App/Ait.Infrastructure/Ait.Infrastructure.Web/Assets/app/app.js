; (function (angular, window, undefined) {

    var app = angular.module('appAuth', [
        'ait',
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


        $rootScope.$on('$locationChangeSuccess', function (event) {
            //if ($http.defaults.headers.common.RefreshToken != null) {
            //    var params = "grant_type=refresh_token&refresh_token=" + $http.defaults.headers.common.RefreshToken;
            //    $http({
            //        url: '/Token',
            //        method: "POST",
            //        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            //        data: params
            //    })
            //    .success(function (data, status, headers, config) {
            //        $http.defaults.headers.common.Authorization = "Bearer " + data.access_token;
            //        $http.defaults.headers.common.RefreshToken = data.refresh_token;

            //        $cookieStore.put('_Token', data.access_token);
            //        $cookieStore.put('_RefreshToken', data.refresh_token);

            //        $http.get('/api/WS_Account/GetCurrentUserName')
            //            .success(function (data, status, headers, config) {
            //                if (data != "null") {
            //                    $rootScope.username = data.replace(/["']{1}/gi, "");//Remove any quotes from the username before pushing it out.
            //                    $rootScope.loggedIn = true;
            //                }
            //                else
            //                    $rootScope.loggedIn = false;
            //            });


            //    })
            //    .error(function (data, status, headers, config) {
            //        $rootScope.loggedIn = false;
            //    });
            //}
        });


    }]);

})(angular, window);