(function () {
    'use strict';
    angular
        .module('umbraco')
        .controller('SettingsController', function ($scope, $http) {
            $scope.title = 'Settings';

            var url = '/umbraco/backoffice/api/Booking/';

            $scope.summaryEmails = [];
            $scope.summaryEmailsReady = false;
            $scope.newBookingEmails = [];
            $scope.newBookingEmailsReady = false;

            $http.get(url + 'GetSummaryEmails').then(function (response) {
                if (!Array.isArray(response.data) || response.data.length === 0) {
                    return;
                }
                console.log(response);
                for (var i = 0; i < response.data.length; i++) {
                    var data = {
                        name: response.data[i],
                        remove: 'click to remove'
                    };

                    $scope.summaryEmails.push(data);
                }

                $scope.summaryEmailsReady = true;
            });

            $http.get(url + 'GetNewBookingEmails').then(function (response) {
                if (!Array.isArray(response.data) || response.data.length === 0) {
                    return;
                }

                for (var i = 0; i < response.data.length; i++) {
                    var data = {
                        name: response.data[i],
                        remove: 'click to remove'
                    };

                    $scope.newBookingEmails.push(data);
                }

                $scope.newBookingEmailsReady = true;
            });

            $scope.options = {
                includeProperties: [
                    { alias: "remove", header: "Remove" }
                ]
            };

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
                console.log(item);
            }

            $scope.selectSummaryEmailItem = function (selectedItem, $index, $event) {
                if ($event.target.innerText === 'click to remove') {
                    $http.get(url + 'RemoveSummaryEmail?email=' + selectedItem.name);
                    $scope.summaryEmails = $scope.summaryEmails.filter(function (value, index, arr) { return value.name !== selectedItem.name; });
                }
            };

            $scope.selectNewBookingEmailItem = function (selectedItem, $index, $event) {
                if ($event.target.innerText === 'click to remove') {
                    $http.get(url + 'RemoveNewBookingEmail?email=' + selectedItem.name);
                    $scope.newBookingEmails = $scope.newBookingEmails.filter(function (value, index, arr) { return value.name !== selectedItem.name; });
                }
            };

            function isSortDirection(col, direction) {
            }

            function sort(field, allow, isSystem) {
            }

            $scope.AddSummaryEmail = function () {
                if (ValidateEmail($scope.SummaryEmail)) {
                    if (ListContainsEmailAlready($scope.summaryEmails, $scope.SummaryEmail)) {
                        alert('Eposten er allerede i listen');
                        return;
                    }

                    $http.get(url + 'AddSummaryEmail?email=' + $scope.SummaryEmail);
                    $scope.summaryEmails.push({
                        name: $scope.SummaryEmail,
                        remove: 'click to remove'
                    });
                    $scope.summaryEmailsReady = true;
                    $scope.SummaryEmail = '';
                } else {
                    alert('Invalid Email');
                }
            };

            $scope.AddNewBookingEmail = function () {
                if (ValidateEmail($scope.NewBookingEmail)) {
                    if (ListContainsEmailAlready($scope.newBookingEmails, $scope.NewBookingEmail)) {
                        alert('Eposten er allerede i listen');
                        return;
                    }

                    $http.get(url + 'AddNewBookingEmail?email=' + $scope.NewBookingEmail);
                    $scope.newBookingEmails.push({
                        name: $scope.NewBookingEmail,
                        remove: 'click to remove'
                    });
                    $scope.newBookingEmailsReady = true;
                    $scope.NewBookingEmail = '';
                } else {
                    alert('Invalid Email');
                }
            };

            function ValidateEmail(email) {
                return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
            }

            function ListContainsEmailAlready(list, email) {
                for (var i = 0; i < list.length; i++) {
                    if (list[i].name.toLowerCase() === email.toLowerCase()) return true;
                }
                return false;
            }
        });

})();
