(function () {
    'use strict';

    angular
        .module('growbot')
        .factory('dataservice', dataservice);

    dataservice.$inject = ['$q', '$http'];

    function dataservice($q, $http) {
        var service = {
            login: login
        };

        return service;

        function login(name, pw) {

            var deferred = $q.defer();

            var data = "grant_type=password&username=" + name + "&password=" + pw;
            $http.post('http://192.168.10.49:49617' + '/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
                .success(function (response) {
                    deferred.resolve(response);
                }).error(function (err, status) {
                    deferred.reject(err);
                });

            return deferred.promise;
        }
    }
})();