'use strict';

; (function (angular, window, document, undefined) {

    var Comp = function (service) {

        var state = this.state = {
            list: null,
            isLoading: true
        };

        function Load() {
            state.isLoading = true;
            return service.GetResearchList()
                .success(function (d) {
                    state.list = d;
                })
                .finally(function () {
                    state.isLoading = false;
                });
        };


        this.$onInit = function () {
            Load();
        };
    };


    Comp.$inject = ['researchService'];


    angular
        .module('researchList', ['ngRoute', 'researchService'])

        .config(['$routeProvider', function ($routeProvider) {
            $routeProvider.when('/research-list', {
                template: '<research-list/>'
            });
        }])

        .component('researchList', {
            controller: Comp,
            templateUrl: 'Assets/app/research-list/research-list.html'
        });


})(angular, window, document);