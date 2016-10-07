

; (function (angular, window, document, undefined) {

    'use strict';
    var Comp = function ($routeParams, service) {

        this.service = service;


        var state = this.state = {
            tab: null, // time-visit|ident-visit|waiting-visit|result-visit
            research: null,
            isLoading: true,

            //in
            researchId: $routeParams.researchId || null,
            slotId: $routeParams.slotId || null,
            visitId: $routeParams.visitId || null,
            error: null,
            patient: null
        };

        var self = this;

        function Load() {
            state.isLoading = true;
            return service
                .GetResearch({
                    ResearchId: state.researchId
                })
                .success(function (d) {
                    state.research = d;
                })
                .finally(function () {
                    state.isLoading = false;
                });
        };


        this.$onInit = function () {


            Load().then(function () {
                if (!state.setTab('ident-visit')) {
                    state.setTab('time-visit');
                }
            });

        };


        this.state.canTab = function (t) {

            if (state.tab == 'waiting-visit')
                return false;

            switch (t) {
                case 'time-visit':
                    return state.research;
                case 'ident-visit':
                    return state.slotId;
                case 'result-visit':
                    return state.visitId;
            }
            return false;
        };


        this.state.setTab = function (t) {
            if (state.canTab(t)) {
                state.tab = t;
                return true;
            }
            return false;
        };


    };


    Comp.$inject = ['$routeParams', 'researchService'];


    var app = angular
        .module('research', ['ngRoute', 'researchService', 'lpuAddress', 'aitEmitter', 'timeVisit', 'identVisit', 'resultVisit'])
        .config(['$routeProvider', function ($routeProvider) {
            $routeProvider.when('/research/:researchId', {
                template: '<research/>'
            });
        }]);


    app.component('research', {
        controller: Comp,
        templateUrl: 'Assets/app/research/research.html'
    });

})(angular, window, document);