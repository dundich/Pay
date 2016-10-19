; (function (angular, window, undefined) {

    var Service = function (serviceFactory, authSettings) {
        var baseUri = authSettings.apiServiceBaseUri;
        return serviceFactory({
            sendEmail: baseUri + 'api/Email/Send'
        });
    };

    Service.$inject = ['aitServiceFactory', 'authSettings'];

    angular
    	.module('emailService', ['aitServiceFactory', 'authSettings'])
    	.factory('emailService', Service);

})(angular, window);