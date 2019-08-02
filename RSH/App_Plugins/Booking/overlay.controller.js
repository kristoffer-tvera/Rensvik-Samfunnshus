(function () {
    'use strict';

    angular
        .module('umbraco')
        .controller('BookingOverlayController', function ($scope, $http, assetsService) {
            $scope.ready = false;
            assetsService
                .load([
                    "~/App_Plugins/Booking/lib/datepicker.js"
                ])
                .then(function () {
                    $scope.ready = true;
                    setTimeout(function () {
                        flatpickr('[type="date"]',
                            {
                                dateFormat: 'Z',
                                altInput: true,
                                altFormat: 'd-m-Y'
                            });
                    }, 0);
                });
            assetsService.loadCss(
                "~/App_Plugins/Booking/lib/datepicker.css"
            );

            $scope.UpdateApproved = function () {
                if ($scope.model.booking.Approved === true) {
                    $scope.model.booking.ApprovedDisplay = '✔';
                } else {
                    $scope.model.booking.ApprovedDisplay = '❌';
                }
            };

            $scope.UpdateWash = function () {
                if ($scope.model.booking.Wash === true) {
                    $scope.model.booking.WashDisplay = '✔';
                } else {
                    $scope.model.booking.WashDisplay = '❌';
                }
            };

        });

})();
