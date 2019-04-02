(function () {
    'use strict';

    angular
        .module('umbraco')
        .controller('BookingController', function ($scope, $http, assetsService) {
            $scope.title = 'Booking';
            $scope.bookings = [];

            assetsService
                .load([
                    "~/App_Plugins/Booking/lib/datepicker.js"
                ])
                .then(function () {
                    initialize();
                });
            assetsService.loadCss(
                "~/App_Plugins/Booking/lib/datepicker.css"
            );

            var url = '/umbraco/backoffice/api/Booking/';

            $scope.save = function () {
                console.log($scope.bookings);
                $http.post(url + 'save', $scope.bookings).then(function (response) {
                    alert('saved');
                });
            };

            function initialize() {
                $http.get(url + 'load').then(function (response) {
                    console.log(response);
                    $scope.bookings = response.data;
                    setTimeout(function () {
                        flatpickr(".booking--date", { dateFormat: "d-m-Y" });
                    }, 0);
                });
            }
        });

})();
