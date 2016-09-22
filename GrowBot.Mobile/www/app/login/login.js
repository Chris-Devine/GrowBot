(function () {
    'use strict';

    angular
        .module('growbot.login')
        .controller('Login', Login);

    Login.$inject = ['dataservice', '$ionicPopup', '$state'];

    function Login(dataservice, $ionicPopup, $state) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'login';
        vm.login = login;
        vm.data = {};
        activate();

        function activate() { }

         function login(data) {
             
             dataservice.login(data.username, data.password)
                .then(function(authenticated) {
                    $state.go('tab.dash', {}, { reload: true });
                }, function(err) {
                    $ionicPopup.alert({
                        title: 'Login failed!',
                        template: 'Please check your credentials!'
                    });

                });
        };

    }
})();
