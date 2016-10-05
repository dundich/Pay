'use strict';

; (function (angular, window, document, undefined) {

    var Comp = function ($routeParams, service, aitEmitter) {

        var state = this.state = {
            list: null,
            specialityId: $routeParams.specialityId,
            isLoading: true
        };

        function Load() {

            state.isLoading = true;

            return service
                .GetDoctorList({
                    SpecialityId: state.specialityId
                })
                .success(function (d) {
                    state.list = d;
                })
                .error(function (e) {
                    aitEmitter.emit('toastError', e);
                })
                .finally(function () {
                    state.isLoading = false;
                });

        };


        this.$onInit = function () {
            Load();
        };
    };


    Comp.$inject = ['$routeParams', 'doctorService', 'aitEmitter'];


    angular
      .module('doctorList', ['ngRoute', 'ngSanitize', 'doctorService', 'lpuAddress'])

      .config(['$routeProvider', function ($routeProvider) {
          $routeProvider
            .when('/doctor-list/:specialityId', {
                template: '<doctor-list></doctor-list>'
            });
      }])

      .component('doctorList', {
          controller: Comp,
          templateUrl: 'Assets/app/doctor-list/doctor-list.html'
      });


})(angular, window, document);