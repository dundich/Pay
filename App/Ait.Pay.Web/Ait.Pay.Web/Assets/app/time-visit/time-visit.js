; (function (angular, window, document, undefined) {

    function strToDate(input) {
        var parts = input.split('-');
        return new Date(parts[0], parts[1] - 1, parts[2]); // Note: months are 0-based
    }


    var Comp = function ($scope, $routeParams, $location, $route, aitEmitter, aitUtils) {

        var now = new Date();
        var dateToYYYYMMDD = aitUtils.dateToYYYYMMDD;

        var service = this.service;

        var state = this.state = {
            events: [],
            times: null,
            isLoading: true,
            isTimesLoading: false,

            slotId: $routeParams.slotId,

            selectEvent: null   //selectEvent.date
        };


        function getReq() {

            var dt = state.selectEvent
                    ? state.selectEvent.date
                    : now

            return angular.extend({}, $routeParams, {
                Date: dateToYYYYMMDD(dt)
            });
        }


        function Load() {
            state.isLoading = true;

            var rin = getReq();

            return service
                .GetVisitDays(rin)
                .success(function (d) {
                    state.events = d.map(function (e) {
                        e.date = strToDate(e.Value);
                        return e;
                    });
                })
                .error(function (e) {
                    aitEmitter.emit('toastError', e);
                })
                .finally(function () {
                    state.isLoading = false;
                });
        };


        this.$onInit = function () {
            $scope.$watch(function () { return state.selectEvent; }, this.onSelectEventChanged);
            return Load();
        };


        this.calendarOptions = {
            //defaultDate: "",
            minDate: new Date(now.getFullYear(), now.getMonth(), now.getDate()),
            maxDate: new Date([2020, 12, 31]),
            dayNamesLength: 1, // How to display weekdays (1 for "M", 2 for "Mo", 3 for "Mon"; 9 will show full day names; default is 1)
            multiEventDates: true, // Set the calendar to render multiple events in the same day or only one event, default is false
            maxEventsPerDay: 3, // Set how many events should the calendar display before showing the 'More Events' message, default is 3;
            eventClick: function (d) {
                state.selectEvent = d.event[0] || null;
            }
            //dateClick: $scope.dateClick
        };


        this.onSelectEventChanged = function (newValue, oldValue) {

            if (!newValue) {
                return state.times;
            }

            state.times = [];
            state.isTimesLoading = true;

            var rin = getReq();

            return service
                .GetVisitSlots(rin)
                .success(function (d) {
                    state.times = d;
                })
                .error(function (e) {
                    aitEmitter.emit('toastError', e);
                })
                .finally(function () {
                    state.isTimesLoading = false;
                });
        };


        this.clickOnSlot = function (slotId) {
            $route.updateParams({ slotId: slotId });
        };

    };


    Comp.$inject = ['$scope', '$routeParams', '$location', '$route', 'aitEmitter', 'aitUtils'];


    angular
      .module('timeVisit', ['ngRoute', 'ngSanitize', 'simple-calendar', 'aitEmitter', 'aitUtils'])

      .component('timeVisit', {
          controller: Comp,
          bindings: {
              service: '='
          },
          templateUrl: 'Assets/app/time-visit/time-visit.html'
      });


})(angular, window, document);