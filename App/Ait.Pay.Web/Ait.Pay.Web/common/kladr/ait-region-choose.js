; (function (angular, window, document, undefined) {
    'use strict';

    var Comp = function (newRegionModel) {

        var self = this;

        var choose = this.choose = newRegionModel();

        //this.regionId, function (sender) {

        choose.Load(this.regionId).then(function () {
            Object.defineProperty(
                self,
                "regionId",
                {
                    get: function () {
                        return choose.regionId;
                    },
                    set: function (newValue) {
                        choose.regionId = newValue;
                    },
                    configurable: true
                });
        });


        this.is_open_regions = false;

        this.openRegionsModal = function () {
            this.is_open_regions = !this.is_open_regions;
        };


        this.onRegionSelected = function (item) {
            this.is_open_regions = false;
            if (this.regionId != item.code) {
                this.regionId = item.code;
                this.onSelected({ item: item });
            }
        };
    };

    Comp.$inject = ['aitNewRegionModel'];


    //--------------------  REGISTER ----------------------
    var app = angular.module('aitRegionChoose', ['aitKladrModel', 'aitRegionList']);


    app.component('aitRegionChoose', {
        controller: Comp,
        bindings: {
            regionId: '=?',
            onSelected: "&"
        },
        templateUrl: 'Assets/app/common/kladr/ait-region-choose.html'
    });


})(angular, window, document);