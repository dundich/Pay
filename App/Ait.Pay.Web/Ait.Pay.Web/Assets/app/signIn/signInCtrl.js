; (function (angular, window, undefined) {

    angular.module('signIn', ['authService'])
        .controller('signInCtrl', ['$scope', '$routeParams', 'authService', function ($scope, $routeParams, service) {
            $scope.message = $routeParams.message;

            $scope.signIn = function () {
                $scope.showMessage = false;

                service.signIn($scope)
                .success(function (data) {
                    window.location = '#/todomanager';
                })
                .error(function (data, status, headers, config) {
                    $scope.message = data.error_description.replace(/["']{1}/gi, "");
                    $scope.showMessage = true;
                });
            };
        }]);

})(angular, window);