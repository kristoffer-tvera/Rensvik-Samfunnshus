(function () {
    'use strict';

    angular
        .module('umbraco')
        .controller('OldBookingController', function ($scope, $http, assetsService) {
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

            $scope.save = function (Id, Requested, From, To, Area, Telephone, Email, Comment, Wash, Approved, Payment) {
                var booking = { Id, Requested, From, To, Area, Telephone, Email, Comment, Wash, Approved, Payment };
                $http.post(url + 'save', booking).then(function (response) {
                    $scope.new = {
                        From: '',
                        To: '',
                        Area: '',
                        Telephone: '',
                        Email: '',
                        Comment: '',
                        Wash: false,
                        Approved: false,
                        Payment: ''
                    };
                });
            };

            function initialize() {
                $http.get(url + 'LoadOld').then(function (response) {
                    $scope.bookings = response.data;
                    setTimeout(function () {
                        flatpickr('.booking--date',
                            {
                                dateFormat: 'Z',
                                altInput: true,
                                altFormat: 'd-m-Y'
                            });
                    }, 0);
                });
            }

        });

})();
