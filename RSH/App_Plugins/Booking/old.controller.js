(function () {
    'use strict';
    angular
        .module('umbraco')
        .controller('OldBookingController', function ($scope, $http, assetsService) {
            $scope.title = 'Booking';
            $scope.bookings = [];
            $scope.ready = false;

            var url = '/umbraco/backoffice/api/Booking/';

            $http.get(url + 'LoadOld').then(function (response) {

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

            function isSortDirection(col, direction) {
            }

            function sort(field, allow, isSystem) {
            }

        });

})();
