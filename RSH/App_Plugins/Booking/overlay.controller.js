(function () {
    'use strict';

    angular
        .module('umbraco')
        .controller('BookingOverlayController', function ($scope) {

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
