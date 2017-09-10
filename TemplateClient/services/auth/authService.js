/**
*
*  Serviço de Autenticação
*  ==============================================================
* 
**/

(function () {
    'use strict';

    angular.module('app')
           .factory('authService', authService);

    authService.$inject = ['$rootScope', '$http', '$q', 'appConfig', 'localStorageService'];

    function authService($rootScope, $http, $q, appConfig, localStorageService) {

        var serviceBase = appConfig.authUrl;
        var authServiceFactory = {};

        var _authentication = {
            isAuth: false,
            UserName: "",
            Name: "",
            Roles: undefined
        };

        var _confirmAccount = function (account) {

            return $http.post(serviceBase + '/user/activate', account);
        }

        var _signUp = function (registration) {

            console.log(registration);
            _logOut();
            return $http.post(serviceBase + '/user/signup', registration);

        };

        var _login = function (user) {

            //var data = "grant_type=password&username=" + user.UserName + "&password=" + user.Password;
            var data = "grant_type=password&username=" + user.UserName + "&password=" + user.Password + "&module=" + appConfig.idModulo;

            var deferred = $q.defer();

            $http.post(serviceBase + '/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

                var rolesObj = undefined;
                if (response.roles)
                    rolesObj = angular.fromJson(response.roles);

                _authentication.Roles = rolesObj;
                localStorageService.set('authorizationData', { token: response.access_token, userName: user.UserName, name: response.name, roles: rolesObj });

                _authentication.isAuth = true;
                _authentication.UserName = user.UserName;
                _authentication.Name = response.name;

                deferred.resolve(response);

            }).error(function (err, status) {
                _logOut();
                deferred.reject(err);
            });

            return deferred.promise;

        };

        var _logOut = function () {

            localStorageService.remove('authorizationData');
            $rootScope.userRoles = undefined;
            $rootScope.User = undefined;

            _authentication.isAuth = false;
            _authentication.UserName = "";
            _authentication.Name = "";
        };

        var _fillAuthData = function () {

            var authData = localStorageService.get('authorizationData');
            if (authData) {
                _authentication.isAuth = true;
                _authentication.UserName = authData.userName;
                _authentication.Name = authData.name;
                _authentication.Roles = authData.roles;
            }
        }

        authServiceFactory.signUp = _signUp;
        authServiceFactory.login = _login;
        authServiceFactory.logOut = _logOut;
        authServiceFactory.fillAuthData = _fillAuthData;
        authServiceFactory.authentication = _authentication;
        authServiceFactory.confirmAccount = _confirmAccount;

        return authServiceFactory;

    }
})();