(function () {
    "use strict";

    function ConfigurationViewerController($http, $timeout, navigationService) {
        const vm = this;

        vm.name = 'Configuration Viewer';
        vm.loading = true;
        vm.error = null;
        vm.data = [];
        vm.expandedSections = {};
        vm.search = '';

        $timeout(function () {
            navigationService.syncTree({
                tree: "configurationViewer",
                path: "-1"
            });
        });

        vm.isVisible = function (node) {
            if (vm.search) {
                return node.Path.includes(vm.search) || node.Value?.includes(vm.search);
            }

            let section = vm.expandedSections;
            for (const parent of node.Parents) {
                if (!section[parent]) {
                    return false;
                }

                section = section[parent];
            }

            return true;
        }

        vm.isOpen = function (node) {
            let section = vm.expandedSections;
            for (const parent of node.Parents) {
                if (!section[parent]) {
                    return false;
                }

                section = section[parent];
            }

            return section[node.Key];
        }

        vm.toggle = function (node) {
            let section = vm.expandedSections;
            for (const parent of node.Parents) {
                section[parent] = section[parent] || {};
                section = section[parent];
            }

            if (section[node.Key]) {
                delete section[node.Key];
            } else {
                section[node.Key] = {}
            }
        };

        $http({
            method: 'GET',
            url: '/umbraco/backoffice/umbracoconfigurationviewer/configurationviewer/config'
        }).then(function (response) {
            vm.loading = false;
            vm.data = response.data;
        }, function (error) {
            vm.loading = false;
            vm.error = error;
        });
    }

    angular.module("umbraco").controller("ConfigurationViewer.OverviewController", ConfigurationViewerController);
})();