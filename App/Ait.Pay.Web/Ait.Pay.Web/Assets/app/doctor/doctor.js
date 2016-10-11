
; (function (angular, window, document, undefined) {
    'use strict';

    var Comp = function ($routeParams, $location, $localStorage, service, emitter) {

        this.service = service;

        var state = this.state = {
            tab: null, // time-visit|ident-visit|waiting-visit|result-visit
            doctor: null,
            isLoading: true,


            //in
            doctorId: $routeParams.doctorId || null,
            specialityId: $routeParams.specialityId || null,
            slotId: $routeParams.slotId || null,
            visitId: $routeParams.visitId || null,
            error: null,
            patient: null
        };

        function LoadDoctor(doctorId, specialityId) {
            state.isLoading = true;

            return service
                .GetDoctor({
                    DoctorId: doctorId,
                    SpecialityId: specialityId
                })
                .success(function (d) {
                    state.doctor = d;
                    state.specialityId = specialityId;
                    state.doctorId = doctorId;
                })
                .error(function (e) {
                    emitter.emit('toastError', e);
                })
                .finally(function () {
                    state.isLoading = false;
                });
        };


        this.$onInit = function () {

            if (state.visitId) {
                state.setTab('result-visit');
            }
            else {
                LoadDoctor(state.doctorId, state.specialityId).then(function () {
                    if (!state.setTab('ident-visit')) {
                        state.setTab('time-visit');
                    }
                });
            }

        };



        function CreateVisit(p) {

            if (!p || !p.Id) return;

            state.patient = p;
            state.tab = 'waiting-visit';

            $('html, body').animate({ scrollTop: 0 }, 500);

            return service
                .CreateVisit({
                    DoctorId: state.doctorId,
                    SpecialityId: state.specialityId,
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


        var off_rv = emitter.on('visitLoaded', function (t, v, d) {
            if (v.Doctor && v.Speciality) {
                if (state.doctorId != v.Doctor.Id || state.specialityId != v.Speciality.Id) {
                    LoadDoctor(v.Doctor.Id, v.Speciality.Id);
                }
            }
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


        this.$onDestroy = function () {
            off_pi.off();
            off_rv.off();
        };

    };


    Comp.$inject = ['$routeParams', '$location', '$localStorage', 'doctorService', 'aitEmitter'];


    angular
      .module('doctor', ['ngRoute', 'ngSanitize', 'ngStorage', 'doctorService', 'aitEmitter', 'timeVisit', 'identVisit', 'resultVisit'])

      .config(['$routeProvider', function ($routeProvider) {
          $routeProvider
            .when('/doctor/:doctorId/:specialityId', {
                template: '<doctor/>'
            })
            .when('/doctor/:doctorId/:specialityId/slot/:slotId', {
                template: '<doctor/>'
            })
            .when('/visit/:visitId', {
                template: '<doctor/>'
            })
          ;
      }])

      .component('doctor', {
          controller: Comp,
          templateUrl: 'Assets/app/doctor/doctor.html'
      });


})(angular, window, document);