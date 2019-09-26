(function () {
    'use strict';
    angular
        .module('umbraco')
        .controller('BookingController', function ($scope, $http) {
            $scope.title = 'Booking';
            $scope.bookings = [];

            var url = '/umbraco/backoffice/api/Booking/';

            $http.get(url + 'Load').then(function (response) {

                for (var i = 0; i < response.data.length; i++) {
                    var data = response.data[i];

                    if (data.Approved === true) {
                        data.ApprovedDisplay = '✔';
                    } else {
                        data.ApprovedDisplay = '❌';
                    }

                    try {
                        data.From = new Date(data.From).toISOString().substring(0, 10);
                    } catch (e) {
                        //ignored
                    }

                    try {
                        data.To = new Date(data.To).toISOString().substring(0, 10);
                    } catch (e) {
                        //ignored
                    }

                    $scope.bookings.push(data);
                }

                $scope.ready = true;
            });

            $scope.options = {
                includeProperties: [
                    { alias: "Telephone", header: "Telefon" },
                    { alias: "Area", header: "Område" },
                    { alias: "From", header: "Fra" },
                    { alias: "To", header: "Til" },
                    { alias: "ApprovedDisplay", header: "Godkjent" }
                ]
            };

            $scope.selectItem = selectItem;
            $scope.clickItem = clickItem;
            $scope.selectAll = selectAll;
            $scope.isSelectedAll = isSelectedAll;
            $scope.isSortDirection = isSortDirection;
            $scope.sort = sort;
            $scope.allowSelectAll = false;

            function selectAll($event) {
                alert("select all");
            }

            function isSelectedAll() {
            }

            function clickItem(item) {
                OpenOverlay(item);
            }

            function selectItem(selectedItem, $index, $event) {
                OpenOverlay(selectedItem);
            }

            function OpenOverlay(item) {
                $scope.overlay = {
                    view: "/App_Plugins/Booking/overlay.html",
                    title: "Booking",
                    show: true,
                    hideSubmitButton: false,
                    submit: function (model) {

                        $http.post(url + 'Save', model.booking).then(function (response) {

                            for (var i = 0; i < $scope.bookings.length; i++) {
                                if ($scope.bookings[i].Id === model.booking.Id) {
                                    $scope.bookings[i] = model.booking;
                                    break;
                                }
                            }
                        });

                        $scope.overlay.show = false;
                        $scope.overlay = null;
                    },
                    close: function (oldModel) {
                        $scope.overlay.show = false;
                        $scope.overlay = null;
                    },
                    booking: angular.copy(item)
                };
            }

            $scope.newItem = function () {
                $scope.overlay = {
                    view: "/App_Plugins/Booking/overlay.html",
                    title: "Booking",
                    show: true,
                    hideSubmitButton: false,
                    submit: function (model) {

                        $http.post(url + 'New', model.booking).then(function (response) {
                            $scope.bookings.push(model.booking);
                        });

                        $scope.overlay.show = false;
                        $scope.overlay = null;
                    },
                    close: function (oldModel) {
                        // do close magic here
                        $scope.overlay.show = false;
                        $scope.overlay = null;
                    },
                    booking: {
                        name: '',
                        Area: '',
                        Telefon: '',
                        Comment: '',
                        Approved: false,
                        ApprovedDisplay: '❌',
                    }
                };
            };

            function isSortDirection(col, direction) {
            }

            function sort(field, allow, isSystem) {
            }

        });

})();
