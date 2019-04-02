(function () {
    'use strict';

    angular
        .module('umbraco')
        .controller('BookingController', function ($scope,assetsService) {
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

            $scope.title = 'Booking';

            $scope.bookings = [];

            $scope.bookings.push({
                Requested: '01.04.2019',
                From: '03.04.2019',
                To: '05.04.2019',
                Area: 'Stor sal',
                Telephone: '123456789',
                Email: 'test@example.com',
                Comment: 'Insert comment here',
                Wash: true,
                Approved: true,
                Payment: '01-04-2019'
            });

            $scope.bookings.push({
                Requested: '02.04.2019',
                From: '13.04.2019',
                To: '15.04.2019',
                Area: 'Liten sal',
                Telephone: '444 55 666',
                Email: 'test@example.com',
                Comment: 'Insert comment here',
                Wash: true,
                Approved: false,
                Payment: '02-04-2019'
            });

            $scope.bookings.push({
                Requested: '02.04.2019',
                From: '13.04.2019',
                To: '15.04.2019',
                Area: 'Liten sal',
                Telephone: '444 55 666',
                Email: 'test@example.com',
                Comment: 'Insert comment here',
                Wash: true,
                Approved: false,
                Payment: ''
            });

            $scope.save = function () {
                console.log($scope.bookings);
            };

            
            function initialize() {
                flatpickr(".booking--date", { dateFormat: "d-m-Y"});
            }
        });

})();
