﻿; (function (angular, window, undefined) {

    var Service = function (serviceFactory) {
        return serviceFactory({
            GetResearchList: 'api/PayResearch/GetResearchList',
            GetResearch: 'api/PayResearch/GetResearchLocation'
        });
    };

    Service.$inject = ['aitServiceFactory'];

    angular
    	.module('researchService', ['aitServiceFactory'])
    	.factory('researchService', Service);

})(angular, window);