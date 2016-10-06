; (function (angular, window, undefined) {

    var Service = function (serviceFactory) {
        return serviceFactory({
            GetDoctor: 'api/PayDoctor/GetDoctor',
            GetDoctorList: 'api/PayDoctor/GetDoctorList',
            GetVisitDays: 'api/PayDoctor/GetVisitDays',
            GetSpecialityList: 'api/PayDoctor/GetSpecialityList',
            GetVisitSlots: 'api/PayDoctor/GetVisitSlots',
            CreateVisit: 'api/PayVisit/CreateDoctorVisit',
            GetVisit: 'api/PayDoctor/GetVisit',
        });
    };

    Service.$inject = ['aitServiceFactory'];

    angular
    	.module('doctorService', ['aitServiceFactory'])
    	.factory('doctorService', Service);

})(angular, window);