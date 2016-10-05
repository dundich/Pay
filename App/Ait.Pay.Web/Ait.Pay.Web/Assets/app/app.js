; (function (angular, window, undefined) {

    var app = angular.module('app', [
        'ngRoute',
        'ngAnimate',

        'aitEmitter',
        'authService',

        'aitUI',

        'home',
        'signIn',

        'register',
        'specialityList',

        'researchList',
        'research',

        'doctorList',
        'doctor',

        'todoManager'
    ]);

    app.config(['$provide', '$routeProvider', '$httpProvider', function ($provide, $routeProvider, $httpProvider) {

        //================================================
        // Ignore Template Request errors if a page that was requested was not found or unauthorized.  The GET operation could still show up in the browser debugger, but it shouldn't show a $compile:tpload error.
        //================================================
        $provide.decorator('$templateRequest', ['$delegate', function ($delegate) {
            var mySilentProvider = function (tpl, ignoreRequestError) {
                return $delegate(tpl, true);
            }
            return mySilentProvider;
        }]);

        //================================================
        // Add an interceptor for AJAX errors
        //================================================
        $httpProvider.interceptors.push(['$q', '$location', function ($q, $location) {
            return {
                'responseError': function (response) {
                    if (response.status === 401)
                        $location.url('/signin');
                    return $q.reject(response);
                }
            };
        }]);


        //================================================
        // Routes
        //================================================

        $routeProvider.when('/signin/:message?', {
            templateUrl: 'App/SignIn',
            controller: 'signInCtrl'
        });

        $routeProvider.when('/todomanager', {
            templateUrl: 'App/TodoManager',
            controller: 'todoManagerCtrl'
        });

        $routeProvider.otherwise({
            redirectTo: '/home'
        });
    }]);

    //GLOBAL FUNCTIONS - pretty much a root/global controller.
    //Get username on each page
    //Get updated token on page change.
    //Logout available on each page.
    app.run(['$rootScope', '$http', 'authService', 'aitEmitter', function ($rootScope, $http, authService, aitEmitter) {

        $rootScope.logout = function () {
            return authService.logout().finally(function () {
                $rootScope.username = '';
                $rootScope.loggedIn = false;
                window.location = '#/signin';
            });
        };

        $rootScope.$on('$locationChangeSuccess', function (event) {

            if ($http.defaults.headers.common.RefreshToken != null) {

                authService.refresh().success(function () {
                    authService.getCurrentUserName()
                        .success(function (data) {
                            if (data != "null") {
                                $rootScope.username = data.replace(/["']{1}/gi, "");//Remove any quotes from the username before pushing it out.
                                $rootScope.loggedIn = true;
                            }
                            else {
                                $rootScope.logout();
                            }
                        });
                })
                .error(function (data, status, headers, config) {
                    $rootScope.logout();
                });
            }

        });

        aitEmitter.on('toastError', function (t, error) {
            var e = error.ExceptionMessage || error;
            //e = 'Ошибка!\r\n' + e;
            Materialize.toast(e, 4000, 'red error');
        });


    }]);

})(angular, window);