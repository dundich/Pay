
; (function (angular, window, document, undefined) {
    'use strict';

    var Comp = function ($scope, $routeParams, $location, $localStorage, emitter, authService, emailService, aitToast) {

        var self = this;

        this.error = null;

        var state = this.state = {
            message: ''
        };

        this.send = function () {
            var serv = emailService
                .sendEmail(state)
                .then(function () {
                    //-------------------
                    emitter.emit('event:email', self.state);
                    aitToast('Ваше сообщение было отправлено', 'ok');
                });

                serv.catch(function (err) {
                    self.error = err;
                });
        };

        this.$onInit = function () {

        };

        this.$onDestroy = function () {

        };
    };


    Comp.$inject = ['$scope', '$routeParams', '$location', '$localStorage', 'aitEmitter', 'authService', 'emailService', 'aitToast'];


    var Ctrl = function ($scope, $location, emitter) {

        var cbk = emitter.on('event:email', function () {

        });

        $scope.$on('$destroy', function () {
            if (cbk)
                cbk.off();
        });
    };

    Ctrl.$inject = ['$scope', '$location', 'aitEmitter'];


    var tmpl = {
        controller: Ctrl,

        template: '\
<div class="row">\
    <div class="col s12 l6 offset-l3">\
        <h2 class="col s12 header">Отправить сообщение</h2>\
        <email></email>\
    </div>\
</div>'
    };



    angular
      .module('email', ['ngRoute', 'ngSanitize', 'ngStorage', 'aitEmitter', 'authService', 'aitUI', 'emailService'])

      .config(['$routeProvider', function ($routeProvider) {
          $routeProvider
            .when('/email/:username', tmpl)
            .when('/email', tmpl);
      }])

      .component('email', {
          controller: Comp,
          template: '\
<form name="form"  ng-submit="$ctrl.send()">\
    <div class="">\
        <ait-field class="col s12" form="form" caption="Сообщение" ng-model="$ctrl.state.message" required="true" ait-focus-on="true">\
        </ait-field>\
    </div>\
    <div class="row">\
        <p class="col s12">\
            <br\>\
            <button class="btn btn-large" type="submit" ng-disabled="form.$invalid" ng-class="{disabled:(form.$invalid)}">Отправить</button>\
        </p>\
    </div>\
    {{$ctrl.error}}\
    <div ng-if="$ctrl.error">\
        <ait-error-panel error="$ctrl.error"></ait-error-panel>\
    </div>\
</form>\
'
      });


})(angular, window, document);