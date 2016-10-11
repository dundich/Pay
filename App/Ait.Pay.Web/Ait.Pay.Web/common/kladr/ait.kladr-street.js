
; (function (angular, window, undefined) {
    'use strict';


    var app = angular.module('aitKladrStreet', ['aitKladr', 'aitUI', 'aitEmitter']);

    app.directive('aitKladrStreet', ['$parse', '$timeout', 'aitKladrConfig', 'aitUtils', 'aitEmitter', function ($parse, $timeout, cfg, utils, notifier) {

        if (typeof jQuery === 'undefined') { throw new Error('kladr\'s JavaScript requires jQuery') }

        var $ = jQuery;

        $.kladr.url = utils.trimRight(cfg.url, '/') + "/kladr/";

        function link($scope, element, attrs, ngModel) {

            var
                regAttr = attrs.aitKladrStreetRegion,
                parseKladrFn = null,
                kladrAttr = attrs.aitKladrStreet,
                regionId = 50,
                $street = $(element);

            if (kladrAttr) {
                parseKladrFn = $parse(kladrAttr);
            }


            if (regAttr) {
                var parseFn = $parse(regAttr);
                regionId = parseFn($scope);

                var unwatch = $scope.$watch(regAttr, function (n, o) {
                    if (n != o) {
                        var opts = $street.data('kladr-options');
                        //обновляем регион
                        opts.parentId = n;
                        //opts.clear();
                    }
                });
            }

            // Подключение автодополнения улиц
            $street.kladr({
                token: '',
                key: '',
                type: $.kladr.type.street,
                parentType: $.kladr.type.region,
                parentId: regionId,
                select: function (obj) {
                    $scope.$evalAsync(AddressUpdate);
                }
            });


            function AddressUpdate() {

                if (parseKladrFn) {
                    var obj = $street.kladr('current');

                    if (obj && $street.val() == obj.name) {
                        var address = '(код КЛАДР = "' + obj.id + '")';
                    }
                    else {
                        obj = null;
                    }

                    var v = obj ? obj.name : element.val();
                    ngModel.$setViewValue(v, false);
                    parseKladrFn.assign($scope, obj);

                    notifier.emit('kladrChanged', obj, attrs.aitTag);
                }
            }
        };


        return {
            restrict: 'A',

            require: '?ngModel',

            scope: false,

            link: link,

            controller: ['$scope', function ($scope) {
            }]
        }
    }]);

})(angular, window);