(function () {
    'use strict';

    angular
        .module('umbraco')
        .controller('BookingOverlayController', function ($scope) {

            $scope.UpdateReserved = function () {
                if ($scope.model.booking.Reserved === true) {
                    $scope.model.booking.ReservedDisplay = '✔';
                } else {
                    $scope.model.booking.ReservedDisplay = '❌';
                }
            };

            $scope.UpdateConfirmed = function () {
                if ($scope.model.booking.Confirmed === true) {
                    $scope.model.booking.ConfirmedDisplay = '✔';
                } else {
                    $scope.model.booking.ConfirmedDisplay = '❌';
                }
            };

        });

})();
