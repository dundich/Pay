; (function (angular, window, undefined) {


    'use strict';

    //*****************************  EventDispatcher  ***************************
    var EventDispatcher = function () {
        this._listeners = {};
    };


    EventDispatcher.prototype.on = function (type, listener) {
        if (!this._listeners[type]) {
            this._listeners[type] = [];
        }

        this._listeners[type].push(listener);

        return {
            type: type,
            listener: listener,
            emitter: this,
            off: function () {
                if (this.emitter) {
                    this.emitter.off(this.type, this.listener);
                    this.emitter = null;
                    this.listener = null;
                }
            }
        };
    };

    EventDispatcher.prototype.off = function (type, listener) {
        if (this._listeners[type]) {
            var index = this._listeners[type].indexOf(listener);

            if (index !== -1) {
                this._listeners[type].splice(index, 1);
            }
        }
    };

    //eventMan.notify('evName', 'param1', 20);
    EventDispatcher.prototype.notify = function (type, listener) {
        var listeners;

        if (typeof arguments[0] !== 'string') {
            console.warn('EventDispatcher', 'First params must be an event type (String)')
        } else {
            listeners = this._listeners[arguments[0]];

            for (var key in listeners) {
                //This could use .apply(arguments) instead, but there is currently a bug with it.
                listeners[key](arguments[0], arguments[1], arguments[2], arguments[3], arguments[4], arguments[5], arguments[6]);
            }
        }
    };


    EventDispatcher.prototype.emit = EventDispatcher.prototype.notify;


    var Notifications = function () {
        return new EventDispatcher();
    };

    var app = angular.module('aitEmitter', []);

    app.factory('aitEmitter', Notifications);


})(angular, window);