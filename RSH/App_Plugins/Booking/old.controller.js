(function () {
    'use strict';
    angular
        .module('umbraco')
        .controller('OldBookingController', function ($scope, $http, assetsService) {
            $scope.title = 'Booking';
            $scope.bookings = [];
            $scope.ready = false;

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

            function initialize() {
                $http.get(url + 'LoadOld').then(function (response) {
                    $scope.bookings = response.data;
                    $scope.ready = true;
                });
            }

            $scope.options = {
                includeProperties: [
                    { alias: "Norsk", header: "Norsk" },
                    { alias: "Nynorsk", header: "Nynorsk" },
                    { alias: "Engelsk", header: "Engelsk" },
                    { alias: "Others", header: "Andre språk" },
                    { alias: "LastUpdated", header: "Sist oppdatert" }
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
            }

            function selectItem(selectedItem, $index, $event) {
                $scope.overlay = {
                    view: "/App_Plugins/Booking/overlay.html",
                    title: "Booking",
                    show: true,
                    hideSubmitButton: true,
                    submit: function (model) {
                        // do submit magic here
                        $scope.overlay.show = false;
                        $scope.overlay = null;
                    },
                    close: function (oldModel) {
                        // do close magic here
                        $scope.overlay.show = false;
                        $scope.overlay = null;
                    },
                    location: selectedItem.id
                };
            }

            function isSortDirection(col, direction) {
            }

            function sort(field, allow, isSystem) {
            }

        });

})();
