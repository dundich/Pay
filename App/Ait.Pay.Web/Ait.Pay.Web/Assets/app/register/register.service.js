; (function (angular, window, undefined) {

    var Service = function (serviceFactory) {
        return serviceFactory({
            register: 'api/Account/Register'
        });
    };

    Service.$inject = ['aitServiceFactory'];

    angular
    	.module('registerService', ['aitServiceFactory'])
    	.factory('registerService', Service);

})(angular, window);