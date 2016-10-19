; (function (angular, window, undefined) {

    var app = angular.module('ait', [
        'ngRoute',
        'ngAnimate',

        'aitEmitter',
        'aitUI'
    ]);


    app.constant('ait', {
        events: {
            login: 'event:login',
            logout: 'event:logout',
            email: 'event:email',
            password_changed: 'event:password_changed'
        }
    });

})(angular, window);