

; (function (angular, window, document, undefined) {

    'use strict';
    var Comp = function (service) {

        this.mode = 'time-visit';
        this.isLoading = true;
        this.data = null;

        var self = this;

        function Load() {
            self.isLoading = true;
            return service.GetResearch()
                .success(function (d) {
                    self.data = d;
                })
                .finally(function () {
                    self.isLoading = false;
                });
        };


        this.$onInit = function () {
            Load();
        };
    };


    Comp.$inject = ['researchService'];




    var app = angular
        .module('research', ['researchService', 'lpuAddress'])
        .config(['$routeProvider', function ($routeProvider) {
            $routeProvider.when('/research', {
                template: '<research/>'
            });
        }]);


    app.component('research', {
        controller: Comp,
        templateUrl: 'Assets/app/research/research.html'
    });



    app.directive('aitLevelsTree', [function () {

        function getLevel(item) {
            if (!item) return 0;
            var val = item.level || (item._extProps ? item._extProps.level : 0);
            return parseInt(val, 10) || 0;
        }

        return {
            scope: {
                cmdSelected: '=',
                items: '=',
                collapsed: '@'
            },
            restrict: 'E',
            template:
    "<div>\
  <div ng-repeat='item in items track by item.id' ng-if='item.__isShown' class=''>\
    <span ng-style='getStyle(item)'>\
     <div class='cursor-blinker'></div>\
     <span ng-click='toggle(item)' class='glyphicon glyphicon-{{item.__icon}} fbtn '></span>\
     <a ng-bind='item.value || item.name' href='' class=''></a>\
    </span>\
  </div>\
</div>",

            link: function ($scope, element, attrs, controller) {

                $scope.$watch("items", layoutDone);

                function doFoldUnfold(inv, collapse) {
                    if (!inv) return;

                    inv.isCollapsed = collapse;
                    inv.__icon = (collapse) ? 'plus' : 'minus';

                    var items = $scope.items || [];

                    var clickedLevel = inv.__level;
                    var index = inv.__index;
                    var length = items.length;

                    for (var i = index + 1; i < length; i++) {
                        var item = items[i];
                        var curLevel = item.__level;
                        if (curLevel <= clickedLevel) {
                            break;
                        }

                        var pindex = item.__pindex;

                        item.__isShown = !collapse && !items[pindex].isCollapsed;
                    }//for
                }

                function collapse(inv) {
                    if (inv.__icon === 'minus')
                        doFoldUnfold(inv, true);
                };

                function expand(inv) {
                    if (inv.__icon === 'plus')
                        doFoldUnfold(inv, false);
                };

                $scope.collapse = collapse;
                $scope.expand = expand;

                $scope.getStyle = function (inv) {
                    return inv.__style;
                };


                $scope.toggle = function (inv) {
                    switch (inv.__icon) {
                        case 'plus': expand(inv); break;
                        case 'minus': collapse(inv); break;
                    }
                };


                function layoutDone() { // collapse all = $scope.collapsed == "1"

                    var allCollapsed = $scope.collapsed;

                    if ($scope.items && $scope.items.length) {

                        var stack = [];
                        var prev = null;

                        $scope.items.forEach(function (item, key) {

                            item.__index = key;
                            item.__level = getLevel(item);
                            item.__pindex = -1;
                            //set icon
                            item.__icon = '';
                            item.__style = { "margin-left": (item.__level * 15) + "px" };
                            item.__isShown = true;


                            if (prev) {
                                if (prev.__level < item.__level) {

                                    stack.push(prev);

                                    item.__pindex = prev.__index;

                                    if (prev.isCollapsed || allCollapsed) {
                                        prev.isCollapsed = true;
                                        prev.__icon = 'plus';
                                        item.__isShown = false;
                                    }
                                    else {
                                        prev.isCollapsed = false;
                                        prev.__icon = 'minus';
                                        item.__isShown = prev.__isShown;
                                    }
                                }
                                else if (prev.__level == item.__level) {
                                    item.__pindex = prev.__pindex;
                                    item.__isShown = prev.__isShown;
                                }
                                else {

                                    if (stack.length) {
                                        var len = prev.__level - item.__level;
                                        stack.splice(stack.length - len, len);
                                    }

                                    if (stack.length) {
                                        var p = stack[stack.length - 1];
                                        item.__pindex = p.__index;
                                        item.__isShown = p.__isShown && !p.isCollapsed;
                                    }
                                }
                            }

                            prev = item;

                        });//forEach
                    }

                };

            } // link
        }
    }]);


})(angular, window, document);