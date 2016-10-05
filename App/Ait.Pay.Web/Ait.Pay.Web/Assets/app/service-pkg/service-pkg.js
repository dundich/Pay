; (function (angular, window, document, undefined) {

    var Comp = function () {
        this.$onInit = function () {

        };
    };


    angular
      .module('servicePkg', ['ngRoute', 'ngSanitize'])

      .component('servicePkg', {
          controller: Comp,
          bindings: {
              packages: '='
          },
          templateUrl: 'Assets/app/service-pkg/service-pkg.html'
      });


})(angular, window, document);