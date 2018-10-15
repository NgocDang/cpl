var StandardAffiliate = {
    tier1StandardAffiliateDataTable: null,
    tier2StandardAffiliateDataTable: null,
    tier3StandardAffiliateDataTable: null,
    init: function () {
        StandardAffiliate.bindCopy();

        StandardAffiliate.bindTier1Tab();
        StandardAffiliate.bindTier1TimeRangeChange();

        StandardAffiliate.bindTier2Tab();
        StandardAffiliate.bindTier2TimeRangeChange();

        StandardAffiliate.bindTier3Tab();
        StandardAffiliate.bindTier3TimeRangeChange();

        var tab = $("#tab").val();
        if (tab === "")
            tab = "tier-1";
        $(".nav-tabs a[id='" + tab + "-nav-tab']").tab('show');
    },
    bindCopy: function () {
        if ($(".btn-copy").length > 0) {
            var clipboard = new ClipboardJS('.btn-copy');
            clipboard.on('success', function (e) {
                toastr.success($("#CopiedSuccessfully").val());
            });
        }
    },
    loadStandardAffiliateIntroducedUsersDataTable: function (tabPaneElement) {
        return tabPaneElement.find(".dt-standard-affiliate").DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "searchDelay": 350,
            "ajax": {
                url: "/Profile/SearchStandardAffiliateIntroducedUsers",
                type: 'POST',
                data: {
                    sysUserId: $("#SysUserId").val(),
                    kindOfTier: tabPaneElement.data().kindOfTier,
                    periodInDay: tabPaneElement.find("select.time-range").val()
                },
                complete: function (data) {
                }
            },
            'order': [[0, 'asc']],
            "language": DTLang.getLang(),
            "columns": [
                {
                    "data": "KindOfTier",
                    "render": function (data, type, full, meta) {
                        return full.kindOfTier;
                    }
                },
                {
                    "data": "UsedCPL",
                    "render": function (data, type, full, meta) {
                        return full.usedCPL;
                    }
                },
                {
                    "data": "LostCPL",
                    "render": function (data, type, full, meta) {
                        return full.lostCPL;
                    }
                },
                {
                    "data": "AffiliateSale",
                    "render": function (data, type, full, meta) {
                        return full.affiliateSale;
                    }
                },
                {
                    "data": "TotalDirectIntroducedUsers",
                    "render": function (data, type, full, meta) {
                        return full.totalDirectIntroducedUsers;
                    }
                },
                {
                    "data": "AffiliateCreatedDate",
                    "render": function (data, type, full, meta) {
                        return full.affiliateCreatedDateInString;
                    }
                }
            ]
        });

    },
    bindTier1Tab: function () {
        $('a#tier-1-nav-tab').on('show.bs.tab', function (e) {
            if ($("#tier-1-nav .tab-detail").html().trim().length === 0) {
                StandardAffiliate.loadStatistics($("#tier-1-nav"));
            }
            if ($("#tier-1-nav table tbody").length == 0) {
                StandardAffiliate.tier1StandardAffiliateDataTable = StandardAffiliate.loadStandardAffiliateIntroducedUsersDataTable($("#tier-1-nav"));
            }
        });
    },
    bindTier2Tab: function () {
        $('a#tier-2-nav-tab').on('show.bs.tab', function (e) {
            if ($("#tier-2-nav .tab-detail").html().trim().length === 0) {
                StandardAffiliate.loadStatistics($("#tier-2-nav"));
            }
            if ($("#tier-2-nav table tbody").length == 0) {
                StandardAffiliate.tier2StandardAffiliateDataTable = StandardAffiliate.loadStandardAffiliateIntroducedUsersDataTable($("#tier-2-nav"));
            }
        });
    },
    bindTier3Tab: function () {
        $('a#tier-3-nav-tab').on('show.bs.tab', function (e) {
            if ($("#tier-3-nav .tab-detail").html().trim().length === 0) {
                StandardAffiliate.loadStatistics($("#tier-3-nav"));
            }
            if ($("#tier-3-nav table tbody").length == 0) {
                StandardAffiliate.tier3StandardAffiliateDataTable = StandardAffiliate.loadStandardAffiliateIntroducedUsersDataTable($("#tier-3-nav"));
            }
        });
    },
    loadStatistics: function (tabPaneElement) {
        if (tabPaneElement.data().kindOfTier == 1) {
            $.ajax({
                url: "/Profile/GetTier1StandardAffiliateStatistics/",
                type: "GET",
                beforeSend: function () {
                    tabPaneElement.find(".tab-detail").html("<div class='text-center py-5'><img src='/css/dashboard/plugins/img/loading.gif' class='img-fluid' /></div>");
                },
                data: {
                    sysUserId: $("#SysUserId").val(),
                    periodInDay: tabPaneElement.find("select.time-range").val(),
                },
                success: function (data) {
                    tabPaneElement.find(".tab-detail").html(data);
                    StandardAffiliate.loadTier1StatisticsChart(tabPaneElement);
                },
            });
        } else {
            $.ajax({
                url: "/Profile/GetNonTier1StandardAffiliateStatistics/",
                type: "GET",
                beforeSend: function () {
                    tabPaneElement.find(".tab-detail").html("<div class='text-center py-5'><img src='/css/dashboard/plugins/img/loading.gif' class='img-fluid' /></div>");
                },
                data: {
                    sysUserId: $("#SysUserId").val(),
                    periodInDay: tabPaneElement.find("select.time-range").val(),
                    kindOfTier: tabPaneElement.data().kindOfTier
                },
                success: function (data) {
                    tabPaneElement.find(".tab-detail").html(data);
                    StandardAffiliate.loadNonTier1StatisticsChart(tabPaneElement);
                },
            });
        }
    },
    bindTier1TimeRangeChange: function () {
        $("#tier-1-nav select.time-range").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                StandardAffiliate.loadStatistics($("#tier-1-nav"));
                StandardAffiliate.tier1StandardAffiliateDataTable.destroy();
                StandardAffiliate.tier1StandardAffiliateDataTable = StandardAffiliate.loadStandardAffiliateIntroducedUsersDataTable($("#tier-1-nav"));
            });
    },
    bindTier2TimeRangeChange: function () {
        $("#tier-2-nav select.time-range").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                StandardAffiliate.loadStatistics($("#tier-2-nav"));
                StandardAffiliate.tier2StandardAffiliateDataTable.destroy();
                StandardAffiliate.tier2StandardAffiliateDataTable = StandardAffiliate.loadStandardAffiliateIntroducedUsersDataTable($("#tier-2-nav"));
            });
    },
    bindTier3TimeRangeChange: function () {
        $("#tier-3-nav select.time-range").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                StandardAffiliate.loadStatistics($("#tier-3-nav"));
                StandardAffiliate.tier3StandardAffiliateDataTable.destroy();
                StandardAffiliate.tier3StandardAffiliateDataTable = StandardAffiliate.loadStandardAffiliateIntroducedUsersDataTable($("#tier-3-nav"));
            });
    },
    loadTier1StatisticsChart: function (tabPaneElement) {
        Highcharts.setOptions({
            global: {
                useUTC: false
            }
        });

        Highcharts.setOptions({
            lang: DTLang.getHighChartLang()
        });
        options = {
            chart: {
                type: 'spline'
            },
            title: {
                text: null
            },
            subtitle: {
                text: null
            },
            exporting: {
                enabled: false
            },
            xAxis: {
                type: 'datetime',
                dateTimeLabelFormats: { // don't display the dummy year
                    day: '%b/%e',
                    month: '%e. %b',
                    year: '%b'
                },

            },
            yAxis: {
                title: {
                    text: ''
                },
            },
            tooltip: {
                headerFormat: '<b>{series.name}</b><br>',
                pointFormat: '{point.x:%e. %b}: {point.y}'
            },

            plotOptions: {
                spline: {
                    marker: {
                        enabled: true
                    }
                }
            },

            series: []

        };

        var totalAffiliateSale = { data: [], name: tabPaneElement.find(".total-affiliate-sale").val(), color: '#4267b2' };
        var directAffiliateSale = { data: [], name: tabPaneElement.find(".direct-affiliate-sale").val(), color: '#f7931a' };
        var totalIntroducedUsers = { data: [], name: tabPaneElement.find(".total-introduced-users").val(), color: '#828384' };
        var directIntroducedUsers = { data: [], name: tabPaneElement.find(".direct-introduced-users").val(), color: '#F69BF9' };

        var directAffiliateSaleChanges = JSON.parse(tabPaneElement.find(".direct-affiliate-sale-changes").val());
        if (directAffiliateSaleChanges.length !== 0) {
            $.each(directAffiliateSaleChanges, function (index, value) {
                now = moment(value.Date).valueOf();
                val = value.Value;
                directAffiliateSale.data.push([now, val]);
            });
        }
        else {
            now = moment().valueOf();
            val = 0;
            directAffiliateSale.data.push([now, val]);
        }
        directAffiliateSale.data.sort();

        var totalAffiliateSaleChanges = JSON.parse(tabPaneElement.find(".total-affiliate-sale-changes").val());
        if (totalAffiliateSaleChanges.length !== 0) {
            $.each(totalAffiliateSaleChanges, function (index, value) {
                now = moment(value.Date).valueOf();
                val = value.Value;
                totalAffiliateSale.data.push([now, val]);
            });
        }
        else {
            now = moment().valueOf();
            val = 0;
            totalAffiliateSale.data.push([now, val]);
        }
        totalAffiliateSale.data.sort();

        var totalIntroducedUsersChanges = JSON.parse(tabPaneElement.find(".total-introduced-users-changes").val());
        if (totalIntroducedUsersChanges.length !== 0) {
            $.each(totalIntroducedUsersChanges, function (index, value) {
                now = moment(value.Date).valueOf();
                val = value.Value;
                totalIntroducedUsers.data.push([now, val]);
            });
        }
        else {
            now = moment().valueOf();
            val = 0;
            totalIntroducedUsers.data.push([now, val]);
        }
        totalIntroducedUsers.data.sort();

        var directIntroducedUsersChanges = JSON.parse(tabPaneElement.find(".direct-introduced-users-changes").val());
        if (directIntroducedUsersChanges.length != 0) {
            $.each(directIntroducedUsersChanges, function (index, value) {
                now = moment(value.Date).valueOf();
                val = value.Value;
                directIntroducedUsers.data.push([now, val]);
            });
        }
        else {
            now = moment().valueOf();
            val = 0;
            directIntroducedUsers.data.push([now, val]);
        }
        directIntroducedUsers.data.sort();

        // Push the completed series
        options.series.push(totalAffiliateSale, directAffiliateSale, totalIntroducedUsers, directIntroducedUsers);

        // Create the plot
        tabPaneElement.find(".statistic-chart").highcharts(options);
    },
    loadNonTier1StatisticsChart: function (tabPaneElement) {
        Highcharts.setOptions({
            global: {
                useUTC: false
            }
        });

        Highcharts.setOptions({
            lang: DTLang.getHighChartLang()
        });
        options = {
            chart: {
                type: 'spline'
            },
            title: {
                text: null
            },
            subtitle: {
                text: null
            },
            exporting: {
                enabled: false
            },
            xAxis: {
                type: 'datetime',
                dateTimeLabelFormats: { // don't display the dummy year
                    day: '%b/%e',
                    month: '%e. %b',
                    year: '%b'
                },

            },
            yAxis: {
                title: {
                    text: ''
                },
            },
            tooltip: {
                headerFormat: '<b>{series.name}</b><br>',
                pointFormat: '{point.x:%e. %b}: {point.y}'
            },

            plotOptions: {
                spline: {
                    marker: {
                        enabled: true
                    }
                }
            },

            series: []

        };

        var totalAffiliateSale = { data: [], name: tabPaneElement.find(".total-affiliate-sale").val(), color: '#4267b2' };

        var totalAffiliateSaleChanges = JSON.parse(tabPaneElement.find(".total-affiliate-sale-changes").val());
        if (totalAffiliateSaleChanges.length !== 0) {
            $.each(totalAffiliateSaleChanges, function (index, value) {
                now = moment(value.Date).valueOf();
                val = value.Value;
                totalAffiliateSale.data.push([now, val]);
            });
        }
        else {
            now = moment().valueOf();
            val = 0;
            totalAffiliateSale.data.push([now, val]);
        }
        totalAffiliateSale.data.sort();

        // Push the completed series
        options.series.push(totalAffiliateSale);

        // Create the plot
        tabPaneElement.find(".statistic-chart").highcharts(options);
    },
};

$(document).ready(function () {
    StandardAffiliate.init();
});