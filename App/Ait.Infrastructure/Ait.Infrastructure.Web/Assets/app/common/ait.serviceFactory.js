; (function (angular, window, undefined) {

    // ****** HOW USE *************
    //var Service = function (serviceFactory) {
    //	return serviceFactory({
    //		register: 'api/Account/Register'
    //	});
    //};

    //Service.$inject = ['aitServiceFactory'];

    //angular
    //	.module('registerService', ['aitServiceFactory'])
    //	.factory('registerService', Service);


    'use strict';

    var app = angular.module('aitServiceFactory', []);


    var ServiceFactory = function ($http, $q, $timeout) {

        var createCallFunc = function (method, addr) {
            return function (data, config) {
                return $http[method](addr, data || {}, config);
            };
        };

        var CreateService = function (config) {

            var serv = {};

            //fill services
            angular.forEach(config, function (addr, func) {
                var method = 'post';
                var vals = addr.split(" ");

                if (vals.length > 1) {
                    addr = vals[0];
                    method = vals[1].toLowerCase();
                }

                serv[func] = createCallFunc(method, addr);

            });

            return serv;
        };

        return CreateService;
    };


    ServiceFactory.$inject = ['$http', '$q', '$timeout'];


    app.factory('aitServiceFactory', ServiceFactory);


})(angular, window);