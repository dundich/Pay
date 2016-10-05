; (function ($, angular, window, undefined) {
    'use strict';

    var cfg = {
        url: $.cfg.kladr
    };

    var app = angular.module('aitKladr', []);

    app.constant('aitKladrConfig', cfg);

})(jQuery, angular, window);
