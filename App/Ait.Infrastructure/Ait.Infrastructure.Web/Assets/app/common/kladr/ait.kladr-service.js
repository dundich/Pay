; (function (angular, window, undefined) {
    'use strict';


    var app = angular.module('aitKladrService', ['aitKladr', 'aitServiceFactory']);

    var Service = function (serviceFactory, cfg, utils) {

        var url = utils.trimRight(cfg.url, '/') + '/region';

        return serviceFactory({
            GetRegions: url + ' get'
        });
    };

    Service.$inject = ['aitServiceFactory', 'aitKladrConfig', 'aitUtils'];

    app.factory('aitKladrService', Service);


})(angular, window);