'use strict';

; (function (angular, window, document, undefined) {

    var Comp = function (service, aitEmitter) {

        var state = this.state = {
            list: null,
            isLoading: true
        };

        function Load() {
            state.isLoading = true;
            return service.GetSpecialityList()
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


    Comp.$inject = ['doctorService', 'aitEmitter'];


    angular
      .module('specialityList', ['ngRoute', 'doctorService'])

      .config(['$routeProvider', function ($routeProvider) {
          $routeProvider.when('/speciality', {
              template: '<speciality-list/>'
          });
      }])

      .component('specialityList', {
          controller: Comp,
          templateUrl: 'Assets/app/speciality-list/speciality-list.html'
      });


})(angular, window, document);