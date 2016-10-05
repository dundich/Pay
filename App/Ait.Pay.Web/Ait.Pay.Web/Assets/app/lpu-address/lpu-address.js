

; (function (angular, window, document, undefined) {
    'use strict';

    var Comp = function () {

    };


    var app = angular.module('lpuAddress', []);

    app.component('lpuAddress', {
        controller: Comp,
        bindings: {
            data: '<'
        },
        templateUrl: 'Assets/app/lpu-address/lpu-address.html'
    });

})(angular, window, document);