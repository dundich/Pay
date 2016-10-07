; (function (angular, window, document, undefined) {

    var Comp = function ($routeParams, $timeout, $location, service, emitter) {

        var state = this.state = {
            isLoading: true,
            visitId: $routeParams.visitId || null,
            error: null,
            data: null
        };

        function hideProgress() {            
            state.isLoading = false;
        }


        function Load(visitId) {

            state.isLoading = true;
            state.error = null;

            return service
                .GetReport({
                    VisitId: visitId
                })
                .success(function (data) {

                    state.data = data;

                    var repUri = data.report.view;
                    var rhost = $('#content-report');

                    rhost.empty();

                    $('<p><strong><i>Рекомендуем Вам <a href=' + repUri + ' target="_blank">распечатать</a> этот документ... </strong></i></p>').appendTo(rhost);

                    $('<iframe>', {
                        src: repUri,
                        id: 'reportFrame',
                        width: '100%',
                        height: '600px',
                        frameborder: 0,
                        scrolling: 'no'
                    }).appendTo(rhost);

                    $timeout(hideProgress, 2000);                    

                    emitter.emit('resultVisitLoaded', data);
                })
                .error(function (e) {
                    state.error = e;
                    state.isLoading = false;
                    hideProgress();
                });
        };
        

        this.$onInit = function () {
            Load(state.visitId);
        };

    };


    Comp.$inject = ['$routeParams', '$timeout', '$location', 'visitService', 'aitEmitter'];

    angular
      .module('resultVisit', ['aitUI', 'visitService'])

      .component('resultVisit', {
          controller: Comp,
          bindings: {
          },
          templateUrl: 'Assets/app/result-visit/result-visit.html'
      });


})(angular, window, document);