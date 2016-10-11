

; (function (angular, window, document, undefined) {
    'use strict';

    var Comp = function (model) {

        var state = this.state = {
            model: model,
            list: null
        };

        function Group(list) {
            var hist = {};

            return list.map(function (a) {

                var group = a.name[0];

                if (!(group in hist)) {
                    hist[group] = 1
                    a.group = group;
                }

                return a;
            });
        };


        this.$onInit = function () {
            model.Load().then(function (r) {
                state.list = Group(r.data.result);
            });
        };
    };


    Comp.$inject = ['aitRegionListModel'];


    angular
        .module('aitRegionList', ['ngRoute', 'aitKladrModel'])

        //.config(['$routeProvider', function ($routeProvider) {
        //    $routeProvider.when('/region-list', {
        //        template: '<region-list/>'
        //    });
        //}])

        .component('aitRegionList', {
            bindings: {
                onSelected: '&'
            },
            controller: Comp,
            templateUrl: 'Assets/app/common/kladr/ait-region-list.html'
        });


})(angular, window, document);