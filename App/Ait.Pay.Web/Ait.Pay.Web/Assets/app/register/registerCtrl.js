; (function (angular, window, undefined) {



    var Comp = function (service) {

        this.showAlert = false;
        this.showSuccess = false;

        this.$onInit = function () {

        };

        var self = this;

        this.register = function () {

            var params = {
                Email: self.username,
                Password: self.password1,
                ConfirmPassword: self.password2
            };

            return service.register(params)
                .success(function (data, status, headers, config) {
                    self.successMessage = "Регистрация завершена. Пожалуйста, проверьте свою электронную почту инструкции по активации учетной записи.";
                    self.showErrorMessage = false;
                    self.showSuccessMessage = true;
                })
                .error(function (data, status, headers, config) {
                    if (angular.isArray(data))
                        self.errorMessages = data;
                    else
                        self.errorMessages = new Array(data.replace(/["']{1}/gi, ""));

                    self.showSuccessMessage = false;
                    self.showErrorMessage = true;
                });
        };


    };

    Comp.$inject = ['registerService'];

    angular
        .module('register', ['ngRoute', 'ngSanitize', 'registerService'])

        .config(['$routeProvider', function ($routeProvider) {
            $routeProvider.when('/register', {
                template: '<register/>'
            });
        }])

        .component('register', {
            controller: Comp,
            templateUrl: 'Assets/app/register/register.html'
        });

    //angular
    //    .module('register', ['registerService'])
    //    .controller('registerCtrl', ['$scope', 'registerService', function ($scope, service) {
    //        $scope.register = function () {
    //            var params = {
    //                Email: $scope.username,
    //                Password: $scope.password1,
    //                ConfirmPassword: $scope.password2
    //            };

    //            service.register(params)
    //                .success(function (data, status, headers, config) {
    //                    $scope.successMessage = "Регистрация завершена. Пожалуйста, проверьте свою электронную почту инструкции по активации учетной записи.";
    //                    $scope.showErrorMessage = false;
    //                    $scope.showSuccessMessage = true;
    //                })
    //                .error(function (data, status, headers, config) {
    //                    if (angular.isArray(data))
    //                        $scope.errorMessages = data;
    //                    else
    //                        $scope.errorMessages = new Array(data.replace(/["']{1}/gi, ""));

    //                    $scope.showSuccessMessage = false;
    //                    $scope.showErrorMessage = true;
    //                });
    //        };

    //        $scope.showAlert = false;
    //        $scope.showSuccess = false;
    //    }]);

})(angular, window);



