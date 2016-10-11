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
        template: '\
<div>\
    <a href="" ng-click="$ctrl.openRegionsModal()" type="button" class="region-btn btn waves-effect waves-light modal-trigger">\
        <span ng-if="$ctrl.choose.isRegionLoading"><ait-loading style="color:white"></ait-loading></span>\
        <span ng-bind="$ctrl.choose.region.name || \'?\'"></span>\
    </a>\
    <ait-dialog is-opened="$ctrl.is_open_regions">\
        <ait-dialog-body>\
            <h4 style="color: #009688;">Выберите регион</h4>\
            <ait-region-list on-selected="$ctrl.onRegionSelected(item)"></ait-region-list>\
        </ait-dialog-body>\
        <ait-dialog-footer>\
            <a href="" class="modal-action modal-close waves-effect waves-green btn-flat">Закрыть</a>\
        </ait-dialog-footer>\
    </ait-dialog>\
</div>\
'
      //  templateUrl: 'Assets/app/common/kladr/ait-region-choose.html'
    });


})(angular, window, document);