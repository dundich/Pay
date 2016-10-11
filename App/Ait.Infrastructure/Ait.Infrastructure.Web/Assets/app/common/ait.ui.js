/// <reference path="tmpls/ait-field-item.html" />
/// <reference path="tmpls/ait-field-item.html" />
'use strict';

; (function (angular, window, document, undefined) {


    if (!Array.prototype.find) {
        Array.prototype.find = function (predicate) {
            'use strict';
            if (this == null) {
                throw new TypeError('Array.prototype.find called on null or undefined');
            }
            if (typeof predicate !== 'function') {
                throw new TypeError('predicate must be a function');
            }
            var list = Object(this);
            var length = list.length >>> 0;
            var thisArg = arguments[1];
            var value;

            for (var i = 0; i < length; i++) {
                value = list[i];
                if (predicate.call(thisArg, value, i, list)) {
                    return value;
                }
            }
            return undefined;
        };
    }

    var NEXT_ID = 1;

    var aitUtils = {

        Re: {
            fio: /^([\ ]*[ёа-яЁА-Я]+[ёа-яЁА-Я\ ]*[ёа-яЁА-Я\-ёа-яЁА-Я]?[ёа-яЁА-Я\ ]*[ёа-яЁА-Я]+[\ ]*)$/,
            phone: /((\+?\d)\s?)?\(?(\d\d\d)\)?\s?(\d\d\d)(\s|-)?(\d\d)(\s|-)?(\d\d)/,
            birthdate: /^(0?[1-9]|[12][0-9]|3[01])[\.](0?[1-9]|1[012])[\.][12]\d{3}/,
            date: /^(0?[1-9]|[12][0-9]|3[01])[\.](0?[1-9]|1[012])[\.][12]\d{3}/,
            date_us: /^[12]\d{3}[\-](0?[1-9]|1[012])[\-](0?[1-9]|[12][0-9]|3[01])/,
            email: /^(([^<>()\[\]\.,;:\s@\"]+(\.[^<>()\[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i
        },


        trimLeft: function (s, charlist) {
            if (charlist === undefined)
                charlist = "\s";

            return s.replace(new RegExp("^[" + charlist + "]+"), "");
        },

        trimRight: function (s, charlist) {
            if (charlist === undefined)
                charlist = "\s";

            return s.replace(new RegExp("[" + charlist + "]+$"), "");
        },

        trim: function (s, charlist) {
            s = this.trimLeft(s, charlist);
            return this.trimRight(s, charlist);
        },


        getUniqueId: function () {
            return NEXT_ID++;
        },

        dateToYYYYMMDD: function (date) {
            if (!date) return null;
            var yyyy = date.getFullYear().toString();
            var mm = (date.getMonth() + 1).toString(); // getMonth() is zero-based
            var dd = date.getDate().toString();
            return yyyy + "-" + (mm[1] ? mm : "0" + mm[0]) + "-" + (dd[1] ? dd : "0" + dd[0]);
        },

        strToDate: function StrToDate(input) {
            var parts;
            //RU
            if (aitUtils.Re.date.test(input)) {
                parts = input.split('.');
                return new Date(parts[2], parts[1] - 1, parts[0]);
            }
            //Eng
            parts = input.split('-');
            return new Date(parts[0], parts[1] - 1, parts[2]); // Note: months are 0-based
        }
    };


    angular.module('aitUtils', [])
        .factory('aitUtils', function () { return aitUtils; });

    var app = angular.module('aitUI', ['aitUtils', 'aitFieldTmpls']);

    app.component('aitLoading', {
        bindings: {},
        transclude: true,
        controller: function () { },
        template: '<i class="fa fa-spinner fa-pulse fa-3x fa-fw" ></i> <ng-transclude></ng-transclude>'
    });

    app.component('aitErrorPanel', {
        bindings: { error: '<' },
        transclude: true,
        controller: function () { },
        template: '<div class="red darken-1" ng-class="{active:$ctrl.error}"><i class="fa fa-exclamation-circle" aria-hidden="true"></i> \
            <ng-transclude></ng-transclude> \
            <span ng-bind="$ctrl.error.ExceptionMessage || $ctrl.error"></span>\
        </div>'
    });


    app.directive('aitFade', ['$parse', function ($parse) {
        return {
            restrict: 'A',
            scope: false,
            link: function (scope, element, attrs) {
                var funcWatch = function () { return $parse(attrs.aitFade)(scope) };
                scope.$watch(funcWatch, function (newValue, oldValue) {
                    if (newValue) {
                        element.removeClass('on').addClass('off');
                    } else {
                        element.removeClass('off').addClass('on');
                    }
                });
            }
        };
    }]);


    app.directive('aitFocusOn', ['$timeout', function ($timeout) {

        //<input type="text" ait-focus-on="data.value" ait-focus-on-name="focusMe"  />
        //  app.controller('MyCtrl', function($scope, aitFocus) {
        //      aitFocus('focusMe');
        //});

        return {
            scope: false,
            restrict: 'A',
            link: function (scope, element, attrs) {

                function setFocus() {
                    $timeout(function () {
                        var inputs = element.find('input');
                        if (inputs.length) {
                            //inputs[0].focus();
                            inputs.select();
                        }
                        else {
                            element[0].focus();
                        }
                        scope.trigger = false;
                    }, 100);
                }

                scope.$watch(attrs.aitFocusOn, function (value) {
                    if (value === true) {
                        setFocus();
                    }
                });

                var aname = attrs.aitFocusOnName;// || element[0].id || element[0].name;
                if (aname) {
                    scope.$on('aitFocusOn', function (e, name) {
                        if (name === aname) {
                            setFocus();
                        }
                    });
                }
            }
        };

    }]);

    app.factory('aitFocus', function ($rootScope, $timeout) {
        return function (name) {
            $timeout(function () {
                $rootScope.$broadcast('aitFocusOn', name);
            });
        }
    });


    app.directive('aitDialog', function () {

        return {
            restrict: 'E',

            link: function ($scope, element, attrs, controllers) {

                var $el = element.find('.ait-modal');

                var opts = {
                    ready: function () {
                        $scope.$apply(function () {
                            $scope.isOpened = true;
                        });
                    },

                    complete: function () {
                        $scope.$apply(function () {
                            $scope.isOpened = false
                        });
                    }
                };


                $scope.$watch('isOpened', function (newval, oldVal) {
                    if (newval) {
                        $el.openModal(opts);
                    }
                    else {
                        $el.closeModal(opts);
                    }
                });

            },

            scope: {
                'isOpened': '='
            },

            transclude: {
                'title': '?aitDialogTitle',
                'body': 'aitDialogBody',
                'footer': '?aitDialogFooter'
            },
            template: '\
 <div class="ait-modal modal modal-fixed-footer">\
    <div class="modal-content">\
      <div ng-transclude="body">Some body....</div>\
    </div>\
    <div class="modal-footer">\
      <div ng-transclude="footer"></div>\
    </div>\
  </div>\
'
        };
    });




    app.directive('aitValid', function () {

        var Re = aitUtils.Re;

        return {
            require: 'ngModel',
            link: function (scope, elm, attrs, ctrl) {
                ctrl.$parsers.unshift(function (val) {

                    var mask = attrs.aitValid;

                    if (!val) {
                        ctrl.$setValidity('re', true);
                        return val;
                    }

                    var re = Re[mask] || new RegExp(mask);

                    var isRe = re.test(val);

                    if (isRe) {
                        switch (mask) {
                            case 'birthdate':
                                isRe = (new Date().getTime() >= aitUtils.strToDate(val).getTime());
                                break;
                        }
                    }

                    ctrl.$setValidity('re', isRe);

                    return isRe ? val : undefined;
                });
            }
        };
    });


    app.directive('aitMask', function () {

        return {
            restrict: 'A',

            link: function ($scope, element, attrs, controllers) {
                var sMask = attrs.aitMask;
                if (!sMask) return;

                var $e = $(element);

                if (sMask && $e.inputmask) {
                    switch (sMask) {

                        case "date":
                        case "birthdate":
                            $e.inputmask("dd.mm.yyyy", { "clearIncomplete": true, placeholder: "ДД.ММ.ГГГГ", showMaskOnHover: false });
                            break;

                        case "phone":
                            $e.inputmask("+7 (999) 999-99-99", { "clearIncomplete": true, placeholder: "+7 (XXX) XXX-XX-XX", showMaskOnHover: false });
                            break;

                        case "fio":
                            var sReq = { regex: "^([\ ]*[ёа-яЁА-Я]+[ёа-яЁА-Я\ ]*[ёа-яЁА-Я\-ёа-яЁА-Я]?[ёа-яЁА-Я\ ]*[ёа-яЁА-Я]+[\ ]*)$" };
                            $e.inputmask('Regex', sReq);
                            break;

                        default:
                            var opts = { placeholder: "", showMaskOnHover: false };
                            if (attrs.aitMaskPlaceholder) {
                                var aitMaskPlaceholder = aitUtils.trimLeft(attrs.aitMaskPlaceholder, '{');
                                opts.placeholder = aitUtils.trimLeft(aitMaskPlaceholder, '}');
                            }
                            $e.inputmask(sMask, opts);
                            break;
                    }
                }
            },

            scope: false
        };

    });


    app.directive('aitTranscludeReplace', ['$log', function ($log) {
        return {
            terminal: true,
            restrict: 'EA',

            link: function ($scope, $element, $attr, ctrl, transclude) {
                if (!transclude) {
                    $log.error('orphan',
                               'Illegal use of ngTranscludeReplace directive in the template! ' +
                               'No parent directive that requires a transclusion found. ');
                    return;
                }
                transclude(function (clone) {
                    if (clone.length) {
                        $element.replaceWith(clone);
                    }
                    else {
                        $element.remove();
                    }
                });
            }
        };
    }]);



    function aitField(aitUtils, $element) {

        this.type = '';
        this.valid = this.aitFieldValid;


        var changeHandler = function () {

            var $leb = $element.find('label.caption');
            var val = $(this).val();

            if (val && val.length) {
                $leb.addClass("active");
            }
        };


        this.init = function () {
            if (this.bindModel) {
                $element.find('label.caption').addClass("active");
            }
        };


        this.$onInit = function () {

            if (!this.name)
                this.name = 'id_' + this.getUniqueId();

            var ft = this.aitFieldType || '';
            var checkChange = true;

            switch (ft) {
                case 'fio':
                    this.valid = ft;
                    this.type = 'fio';
                    break;

                case 'date':
                case 'birthdate':
                    this.valid = ft;
                    this.type = 'date';
                    break;

                case 'sex':
                case 'radio':
                    this.type = 'radio';
                    checkChange = false;
                    break;

                case 'email':
                case 'phone':
                    this.valid = ft;
                    this.type = ft;
                    break;

                case 'region':
                    this.type = 'region';
                    checkChange = false;
                    break;

                case 'street':
                    this.type = 'street';
                    break;

                default:
                    this.type = 'text';
                    break;
            }

            if (checkChange) {
                $element.on('input', 'input', changeHandler);
            }

            this.checkChange = true;

        }

        this.getTemplate = function () {
            return "Assets/app/common/tmpls/ait-field-" + this.type + ".html";
        };

        this.getUniqueId = aitUtils.getUniqueId;


        this.$onDestroy = function () {
            $element.off('input', 'input', changeHandler);
        };


    };

    aitField.$inject = ['aitUtils', '$element'];


    app.component('aitField', {

        restrict: "E",

        controller: aitField,

        bindings: {
            bindModel: '=ngModel',
            parentBind: '=?',
            itemsSource: '<?',
            form: '<',
            autocomplete: '@',
            required: '<?',
            caption: '@?',
            name: '@?',
            placeholder: '@?',
            aitFieldTag: '@?',
            aitFieldMask: '@?',
            aitFieldValid: '@?',
            aitFieldType: '@?', //fio, date, region, text, birthdate, sex, email...            
            aitFieldItemTemplate: '@?'
        },

        template: '<ng-include src="$ctrl.getTemplate()"/>'

    });


    app.directive('aitFieldItem', ['$compile', function ($compile) {
        return {
            restrict: 'E',
            scope: {
                template: '@?',
                item: '<'
            },
            template: '<ng-include src="template || \'Assets/app/common/tmpls/ait-field-item.html\'"/>'
            ,
            link: function (scope, element) {
            }
        };
    }]);


})(angular, window, document);