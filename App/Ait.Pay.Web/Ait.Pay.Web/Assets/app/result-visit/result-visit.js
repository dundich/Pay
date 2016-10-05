; (function (angular, window, document, undefined) {

    var Comp = function () {
        this.$onInit = function () {

        };
    };


    angular
      .module('resultVisit', ['ngRoute', 'ngSanitize'])

      .component('resultVisit', {
          controller: Comp,
          bindings: {
          },
          templateUrl: 'Assets/app/result-visit/result-visit.html'
      });


})(angular, window, document);