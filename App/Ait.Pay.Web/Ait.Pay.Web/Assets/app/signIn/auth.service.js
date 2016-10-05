; (function (angular, window, undefined) {

    var Service = function (serviceFactory, $http, $cookies, $cookieStore) {


        //If a token exists in the cookie, load it after the app is loaded, so that the application can maintain the authenticated state.
        $http.defaults.headers.common.Authorization = 'Bearer ' + $cookieStore.get('_Token');
        $http.defaults.headers.common.RefreshToken = $cookieStore.get('_RefreshToken');

        var srv = serviceFactory({
            signIn: 'Token',
            logout: 'api/Account/Logout',
            refresh: 'Token',
            getCurrentUserName: 'api/WS_Account/GetCurrentUserName get'
        });


        var signIn = srv.signIn;

        srv.signIn = function (data) {
            var params = "grant_type=password&username=" + data.username + "&password=" + data.password;
            var cfg = { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } };

            return signIn(params, cfg).success(function (result) {
                $http.defaults.headers.common.Authorization = "Bearer " + result.access_token;
                $http.defaults.headers.common.RefreshToken = result.refresh_token;
                $cookieStore.put('_Token', result.access_token);
            });
        };

        var logout = srv.logout;

        srv.logout = function () {

            return logout().finally(function () {

                $http.defaults.headers.common.Authorization = null;
                $http.defaults.headers.common.RefreshToken = null;
                $cookieStore.remove('_Token');
                $cookieStore.remove('_RefreshToken');
            });

        };


        var refresh = srv.refresh;

        srv.refresh = function (data) {
            var params = "grant_type=refresh_token&refresh_token=" + $http.defaults.headers.common.RefreshToken;
            var cfg = { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } };

            return refresh(params, cfg)
                .success(function (result) {
                    $http.defaults.headers.common.Authorization = "Bearer " + result.access_token;
                    $http.defaults.headers.common.RefreshToken = result.refresh_token;
                    $cookieStore.put('_Token', result.access_token);
                    $cookieStore.put('_RefreshToken', result.refresh_token);
                });
        };

        return srv;
    };

    Service.$inject = ['aitServiceFactory', '$http', '$cookies', '$cookieStore'];

    angular
    	.module('authService', ['ngCookies', 'aitServiceFactory'])
    	.factory('authService', Service);


})(angular, window);