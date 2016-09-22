(function() {
    'use strict';

    var LoginSvcs = function ($q) {

        var loginUser = function(name, pw) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            if (name == 'user' && pw == 'secret') {
                deferred.resolve('Welcome ' + name + '!');
            } else {
                deferred.reject('Wrong credentials.');
            }
            promise.success = function(fn) {
                promise.then(fn);
                return promise;
            }
            promise.error = function(fn) {
                promise.then(null, fn);
                return promise;
            }
            return promise;
        }

        return {
            loginUser: loginUser
        }
    };

    var app = angular.module("growbot");
    app.service("LoginSvcs", LoginSvcs);

}());