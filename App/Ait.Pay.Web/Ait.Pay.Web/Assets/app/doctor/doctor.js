'use strict';

; (function (angular, window, document, undefined) {

    var Comp = function ($routeParams, service, emitter) {

        var state = this.state = {
            tab: null, //       time-visit|ident-visit|waiting-visit|result-visit
            doctor: null,
            isLoading: true,

            //in
            doctorId: $routeParams.doctorId || null,
            specialityId: $routeParams.specialityId || null,
            slotId: $routeParams.slotId || null,
            error: null,
            patient: null
        };

        function Load() {
            state.isLoading = true;

            return service
                .GetDoctor({
                    DoctorId: state.doctorId,
                    SpecialityId: state.specialityId
                })
                .success(function (d) {
                    state.doctor = d;
                    state.tab = 'time-visit';
                })
                .error(function (e) {
                    emitter.emit('toastError', e);                    
                })
                .finally(function () {
                    state.isLoading = false;
                });
        };


        this.$onInit = function () {
            Load().then(function () {

                if (state.canTab('ident-visit')) {
                    state.tab = 'ident-visit';
                }
                else {
                    state.tab = 'time-visit';
                }

            });
        };



        function CreateVisit(p) {

            if (!p || !p.Id) return;

            state.patient = p;
            state.tab = 'waiting-visit';

            return service
                .CreateVisit({
                    DoctorId: state.doctorId,
                    SpecialityId: state.specialityId,
                    SlotId: state.slotId,
                    PatientId: p.Id
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


        this.state.canTab = function (t) {

            if (state.tab == 'waiting-visit')
                return false;

            switch (t) {
                case 'time-visit':
                    return state.doctor;
                case 'ident-visit':
                    return state.slotId;
                case 'result-visit':
                    return false;
            }
            return false;
        };


        this.state.setTab = function (t) {
            if (state.canTab(t)) {
                state.tab = t;
            }
        };


        this.$onDestroy = function () {
            off_pi.off();
        };


    };


    Comp.$inject = ['$routeParams', 'doctorService', 'aitEmitter'];


    angular
      .module('doctor', ['ngRoute', 'ngSanitize', 'doctorService', 'aitEmitter', 'timeVisit', 'identVisit', 'resultVisit'])

      .config(['$routeProvider', function ($routeProvider) {
          $routeProvider
            .when('/doctor/:doctorId/:specialityId', {
                template: '<doctor/>'
            })
            .when('/doctor/:doctorId/:specialityId/slot/:slotId', {
                template: '<doctor/>'
            })
          //.when('/doctor/:doctorId/:specialityId/slot/:slotId/patient/:patientId', {
          //    template: '<doctor/>'
          //})
          ;

      }])

      .component('doctor', {
          controller: Comp,
          templateUrl: 'Assets/app/doctor/doctor.html'
      });


})(angular, window, document);