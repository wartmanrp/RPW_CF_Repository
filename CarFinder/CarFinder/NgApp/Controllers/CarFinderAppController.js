var app = angular.module('CarFinderApp');
app.controller('CarFinderAppController', ['$scope', '$http', function ($scope, $http) {
    $scope.years = null;
    $scope.makes = null;
    $scope.models = null;
    $scope.trims = null;


    $scope.selectedYear = '';
    $scope.selectedMake = '';
    $scope.selectedModel = '';
    $scope.selectedTrim = null;
    $scope.carData = null;

    $scope.slideInterval = 3000;
    $scope.wrapSlides = false;
    $scope.isCollapsed = true;

    $scope.getYears = function () {
        //make request = this can be from a local service or from the Angular $http service
        $http.get('../api/years').then(function (response) {
            $scope.years = response.data;
            //assign result to a $scope variable
        });
        $scope.makes = null;
        $scope.models = null;
        $scope.trims = null;
        $scope.carData = null;

    };

    $scope.getMakes = function () {
        $scope.selectedMake = '';
        $scope.selectedModel = '';
        $scope.selectedTrim = null;
        var options = {params: { year: $scope.selectedYear} };
        $http.get('../api/makes', options).then(function (response) {
            $scope.makes = response.data;
            //assign result to a $scope variable
        });
        $scope.models = null;
        $scope.trims = null;
        $scope.carData = null;
    };
        
    $scope.getModels = function () {
        $scope.selectedModel = '';
        $scope.selectedTrim = null;
        var options = {
            params: {
                year: $scope.selectedYear,
                make: $scope.selectedMake
            }
        };
        $http.get('../api/models', options).then(function (response) {
            $scope.models = response.data;
            //assign result to a $scope variable
        });
        $scope.trims = null;
        $scope.carData = null;
    };

    $scope.getTrims = function () {
        $scope.selectedTrim = null;
        var options = {  params: {year: $scope.selectedYear, make: $scope.selectedMake, model: $scope.selectedModel} };
        $http.get('../api/trims', options).then(function (response) {
            if (response.data[0] === "" && response.data.length === 1) {
                $scope.getCar();
            } else {
                $scope.trims = response.data;
                $scope.getCar();
            }
        });
        $scope.carData = null;
    };

    $scope.getCar = function () {
        var options = {};
        if ($scope.selectedTrim === "") {
            options = { params: { year: $scope.selectedYear, make: $scope.selectedMake, model: $scope.selectedModel } };
        } else {
            options = { params: { year: $scope.selectedYear, make: $scope.selectedMake, model: $scope.selectedModel, trim: $scope.selectedTrim } };
        }
            $http.get('../api/cars', options).then(function (response) {
                $scope.carData = response.data;
            });
        };

        $scope.getYears();

    }]);