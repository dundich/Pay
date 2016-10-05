; (function (angular, window, document, undefined) {

    angular

        .module('home', ['specialityList', 'researchList'])

        .config(['$routeProvider', function ($routeProvider) {
            $routeProvider.when('/home', {
                template: '<home/>'
            });
        }])

        .component('home', {
            controller: function () {
            },
            templateUrl: 'Assets/app/home/home.html'
        });

})(angular, window, document);