/**
*
*  Auth Interceptor
*  ==============================================================
* 
**/

(function () {
    'use strict';

    angular.module('app')
           .factory('authInterceptorService', authInterceptorService);

    authInterceptorService.$inject = ['$q', '$location', '$injector', 'localStorageService'];

    function authInterceptorService($q, $location, $injector, localStorageService) {

        var authInterceptorServiceFactory = {};

        var _request = function (config) {

            //TODO: passar as urls que não necessitam de autenticação para o localstorage em um array e definir as urls no config.js
            if (config.url.indexOf("buscacep") > -1)
                return config;

            config.headers = config.headers || {};

            var authData = localStorageService.get('authorizationData');
            if (authData) {
                config.headers.Authorization = 'Bearer ' + authData.token;
            }
            return config;
        }

        var _responseError = function (rejection) {
            if (rejection.status === 401) {
                var stateService = $injector.get('$state');
                stateService.go('login');
                //$location.path('/#/login');

            }
            return $q.reject(rejection);
        }

        authInterceptorServiceFactory.request = _request;
        authInterceptorServiceFactory.responseError = _responseError;

        return authInterceptorServiceFactory;
    }
})();