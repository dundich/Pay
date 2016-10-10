
; (function (angular, window, undefined) {
    'use strict';

    //---------------------  LIST MODEL ---------------------


    var ListFactory = function (service) {

        var __$promise = null;

        var ListModel = function () {
            this.list = null;
            this.isLoading = true;
        };


        ListModel.prototype.getById = function (id) {
            var self = this;
            return this.Load().then(function () {
                if (self.list == null) return null;
                if (!id) return null;
                //330000000000000
                id = id + "  ";
                id = id.slice(0, 2);
                id = parseInt(id, 10);
                if (isNaN(id)) return null;
                if (id < 10) {
                    id = '0' + id;
                }
                else {
                    id = '' + id;
                }

                return self.list.find(function (e) {
                    return e.code === id;
                });
            });
        };

        ListModel.prototype.Load = function (forced) {

            if (!forced && __$promise) {
                return __$promise;
            }

            this.isLoading = true;

            var self = this;

            __$promise = service.GetRegions()
                .success(function (d) {

                    self.list = d.result
                        .sort(function (a, b) {
                            return a.name.localeCompare(b.name);
                        })
                        .map(function (a) {
                            a.name = a.name + ' ' + a.socr + '.';
                            return a;
                        });
                })
                .finally(function () {
                    self.isLoading = false;
                });




            return __$promise;
        };

        return new ListModel(service);
    };

    ListFactory.$inject = ['aitKladrService'];




    //--------------------  CHOOSE MODEL -------------------

    var Model = function (regionListModel/*, regId, onInitialized */) {

        var _region = { data: null };
        var self = this;

        Object.defineProperty(
            this,
            "regionId",
            {
                get: function () { return _region.data ? (_region.data.code || "") : ""; },
                set: function (newValue) {
                    if (_region.data) {
                        if (newValue == _region.data.code) return;
                    }
                    self.Load(newValue);
                },
                configurable: true
            });

        Object.defineProperty(
            this,
            "isRegionLoading",
            {
                get: function () { return regionListModel.isLoading; }
            });

        Object.defineProperty(
            this,
            "region",
            {
                get: function () { return _region.data; },
                configurable: true
            });

        this.Load = function (regionId) {
            var self = this;
            return regionListModel.getById(regionId).then(function (d) {
                _region.data = d;
                return d;
            }, function (e) {
                _region.data = null;
                return e;
            });
        };


        ////initialized
        //if (regId) {
        //    this.Load(regId);
        //    //if (onInitialized) {
        //    //    p.then(onInitialized);
        //    //}
        //}

    };


    var ModelFactory = function (regionListModel) {
        return function (regionId/*, onInitialized*/) {
            return new Model(regionListModel/*, regionId, onInitialized*/);
        }
    };

    ModelFactory.$inject = ['aitRegionListModel'];


    //--------------------  FACTORY -------------------

    var app = angular.module('aitKladrModel', ['aitKladrService']);


    app.factory('aitRegionListModel', ListFactory);
    app.factory('aitNewRegionModel', ModelFactory);



})(angular, window);