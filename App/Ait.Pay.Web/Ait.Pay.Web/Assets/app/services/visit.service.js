; (function (angular, window, undefined) {

    var Service = function (serviceFactory) {
        return serviceFactory({            
            CreateVisit: 'api/PayVisit/CreateDoctorVisit',
            GetReport: 'api/PayVisit/GetReport',
        });
    };

    Service.$inject = ['aitServiceFactory'];

    angular
    	.module('visitService', ['aitServiceFactory'])
    	.factory('visitService', Service);

})(angular, window);