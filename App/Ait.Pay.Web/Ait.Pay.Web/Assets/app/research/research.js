

; (function (angular, window, document, undefined) {

    'use strict';
    var Comp = function ($routeParams, $location, $localStorage, service, emitter) {

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

        function CreateVisit(p) {

            if (!p || !p.Id) return;

            state.patient = p;
            state.tab = 'waiting-visit';

            $('html, body').animate({ scrollTop: 0 }, 500);

            return service
                .CreateVisit({
                    ResearchId: state.researchId,
                    SlotId: state.slotId,
                    PatientId: p.Id
                })
                .success(function (visit) {

                    if (visit.Id) {
                        var visits = $localStorage.visits || {};
                        visits[visit.Id] = p.Id;
                        $localStorage.visits = visits;

                        $location.path("/visit/" + visit.Id);
                    }

                })
                .error(function (e) {
                    emitter.emit('toastError', e);
                    state.tab = 'error-visit';
                    state.error = e;
                })
                .finally(function () {
                    //state.tab =
                });
        }



        var off_pi = emitter.on('patentIdentified', function (t, p, d) {
            CreateVisit(p);
        });

        this.$onDestroy = function () {
            off_pi.off();
        };
    };


    Comp.$inject = ['$routeParams', '$location', '$localStorage', 'researchService', 'aitEmitter'];


    var app = angular
        .module('research', ['ngRoute', 'ngStorage', 'researchService', 'lpuAddress', 'aitEmitter', 'timeVisit', 'identVisit', 'resultVisit'])
        .config(['$routeProvider', function ($routeProvider) {
            $routeProvider
                .when('/research/:researchId', {
                    template: '<research/>'
                })
                .when('/research/:researchId/slot/:slotId', {
                    template: '<research/>'
                })
            ;
        }]);


    app.component('research', {
        controller: Comp,
        templateUrl: 'Assets/app/research/research.html'
    });

})(angular, window, document);