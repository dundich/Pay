; (function (angular, window, undefined) {

    var app = angular.module('ait', [
        'ngRoute',
        'ngAnimate',

        'aitEmitter',
        'aitUI'
    ]);


    app.constant('ait', {
        events: {
            login: 'event:login'
        }
    });

})(angular, window);