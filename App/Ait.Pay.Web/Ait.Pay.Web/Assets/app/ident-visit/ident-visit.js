; (function (angular, window, document, undefined) {
    'use strict';

    var Comp = function (identService, $storage, notifier) {


        var self = this;


        var state = this.state = {

            sexs: [{ id: "M", value: "мужской" }, { id: "F", value: "женский" }],

            isLoading: false,

            error: null,

            copy: function () {
                return angular.extend({
                    FirstName: null,
                    LastName: null,
                    MiddleName: null,
                    Birthdate: null,
                    Sex: null,
                    Phone: null,
                    Email: null,
                    Kladr: null,
                    Address: null,
                    House: null,
                    Build: null,
                    Room: null,
                    DocSer: null,
                    DocNum: null,
                    DocDate: null,
                    DocIssuer: null
                }
                , self
                , {
                    state: null
                });
            },


            onSubmit: function () {

                state.isLoading = true;
                state.error = null;

                var p = this.copy();

                save();

                return identService
                    .Register(p)
                    .success(function (d) {
                        notifier.emit('patentIdentified', d, p);
                        //
                    })
                    .error(function (e) {
                        state.error = e;
                        notifier.emit('toastError', e);
                        state.isLoading = false;
                    });
            },

            onRegionSelected: function (item) {
                var $e = $('#street');
                $e.data('kladr-options').clear();
                $e.focus();
            }

        };


        function save() {
            var ctx = self;
            if (ctx.isLocalStorage) {
                $storage.patient = state.copy();
            }
        }


        function restore() {
            var ctx = self;
            if (ctx.isLocalStorage) {
                var p = $storage.patient;
                if (p) {
                    for (var prop in p) {
                        if (prop != 'state' && p.hasOwnProperty(prop)) {
                            var val = p[prop] || '';
                            if (val.length) {
                                ctx[prop] = val;
                            }
                        }
                    };

                    if (p.Sex) {
                        ctx.Sex = state.sexs[0];
                    }
                }
            }
        }


        var offKladr = notifier.on('kladrChanged', function (etype, kladr) {
            if (kladr) {
                self.Kladr = kladr.id;
            }
            else {
                self.Kladr = null;
            }
        });


        this.$onInit = function () {
            restore();
        };


        this.$onDestroy = function () {
            offKladr.off();
        }

    };

    Comp.$inject = ['identService', '$localStorage', /*'$sessionStorage',*/ 'aitEmitter'];


    angular
      .module('identVisit', ['ngRoute', 'ngSanitize', 'aitUI', 'aitKladrStreet', 'aitRegionChoose', 'identService', 'ngStorage', 'aitEmitter'])

      .component('identVisit', {
          controller: Comp,
          bindings: {
              isLocalStorage: '<'
          },
          templateUrl: 'Assets/app/ident-visit/ident-visit.html'
      });


})(angular, window, document);