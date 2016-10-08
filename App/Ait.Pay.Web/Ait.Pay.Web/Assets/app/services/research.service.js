; (function (angular, window, undefined) {

    var Service = function (serviceFactory) {
        return serviceFactory({
            GetResearchList: 'api/PayResearch/GetResearchList',
            GetResearch: 'api/PayResearch/GetResearchLocation',
            GetVisitDays: 'api/PayResearch/GetVisitDays',
            GetVisitSlots: 'api/PayResearch/GetVisitSlots',
            CreateVisit: 'api/PayVisit/CreateResearchVisit'
        });
    };

    Service.$inject = ['aitServiceFactory'];

    angular
    	.module('researchService', ['aitServiceFactory'])
    	.factory('researchService', Service);

})(angular, window);