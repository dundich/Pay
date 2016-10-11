

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

        .component('aitRegionList', {
            bindings: {
                onSelected: '&'
            },
            controller: Comp,
            template:'\
<div class="row">\
    <div class="col s12">\
        <div class="section">\
            <span ng-if="$ctrl.state.model.isLoading"><ait-loading></ait-loading></span>\
            <div class="row reg-container" ng-if="$ctrl.state.list">\
                <div ng-repeat="item in ::$ctrl.state.list">\
                    <div ng-if="item.group" class="group-reg">{{item.group}}</div>\
                    <a href="" ng-click="$ctrl.onSelected({item:item})">{{::item.name}}</a>\
                </div>\
            </div>\
        </div>\
    </div>\
</div>'

                //'Assets/app/common/kladr/ait-region-list.html'
        });


})(angular, window, document);