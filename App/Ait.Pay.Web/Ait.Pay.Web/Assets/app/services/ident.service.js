; (function (angular, window, undefined) {

    var Service = function (serviceFactory) {
        return serviceFactory({
            Register: 'api/PayIdent/Register'
        });
    };

    Service.$inject = ['aitServiceFactory'];

    angular
    	.module('identService', ['aitServiceFactory'])
    	.factory('identService', Service);

})(angular, window);