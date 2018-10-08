var TopAgencyAffiliate = {
    topAgencyAffiliateDataTable: null,
    tier2AffiliateDataTable: null,
    tier3AffiliateDataTable: null,
    init: function () {
        TopAgencyAffiliate.bindCopy();

        TopAgencyAffiliate.bindTopAgencyTab();
        TopAgencyAffiliate.bindTopAgencyTimeRangeChange();

        TopAgencyAffiliate.bindTier2Tab();
        TopAgencyAffiliate.bindTier2TimeRangeChange();

        TopAgencyAffiliate.bindTier3Tab();
        TopAgencyAffiliate.bindTier3TimeRangeChange();

        var tab = $("#tab").val();
        if (tab === "")
            tab = "top-agency";
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
    loadAffiliateDataTable: function (tabPaneElement) {
        return tabPaneElement.find(".dt-top-agency-affiliate").DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "ajax": {
                url: "/Profile/SearchTopAgencyAffiliate",
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
                },
            ]
        });
    },
    bindTopAgencyTab: function () {
        $('a#top-agency-nav-tab').on('show.bs.tab', function (e) {
            if ($("#top-agency-nav .tab-detail").html().trim().length === 0) {
                TopAgencyAffiliate.loadStatistics($("#top-agency-nav"));
            }

            if ($("#top-agency-nav table tbody").length == 0) {
                TopAgencyAffiliate.topAgencyAffiliateDataTable = TopAgencyAffiliate.loadAffiliateDataTable($("#top-agency-nav"));
            }

        })
    },
    bindTier2Tab: function () {
        $('a#tier-2-nav-tab').on('show.bs.tab', function (e) {
            if ($("#tier-2-nav .tab-detail").html().trim().length === 0) {
                TopAgencyAffiliate.loadStatistics($("#tier-2-nav"));
            }

            if ($("#tier-2-nav table tbody").length == 0) {
                TopAgencyAffiliate.tier2AffiliateDataTable = TopAgencyAffiliate.loadAffiliateDataTable($("#tier-2-nav"));
            }
        })
    },
    bindTier3Tab: function () {
        $('a#tier-3-nav-tab').on('show.bs.tab', function (e) {
            if ($("#tier-3-nav .tab-detail").html().trim().length === 0) {
                TopAgencyAffiliate.loadStatistics($("#tier-3-nav"));
            }

            if ($("#tier-3-nav table tbody").length == 0) {
                TopAgencyAffiliate.tier3AffiliateDataTable = TopAgencyAffiliate.loadAffiliateDataTable($("#tier-3-nav"));
            }
        })
    },
    loadStatistics: function (tabPaneElement) {
        if (tabPaneElement.data().kindOfTier == 1) {
            $.ajax({
                url: "/Admin/GetTopAgencyStatistics/",
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
                    TopAgencyAffiliate.loadTopAgencyStatisticsChart(tabPaneElement);
                },
            });
        } else {
            $.ajax({
                url: "/Profile/GetNonTopAgencyStatistics/",
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
                    TopAgencyAffiliate.loadNonTopAgencyStatisticsChart(tabPaneElement);
                },
            });
        }
    },
    bindTopAgencyTimeRangeChange: function () {
        $("#top-agency-nav select.time-range").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                TopAgencyAffiliate.loadStatistics($("#top-agency-nav"));
                TopAgencyAffiliate.topAgencyAffiliateDataTable.destroy();
                TopAgencyAffiliate.topAgencyAffiliateDataTable = TopAgencyAffiliate.loadAffiliateDataTable($("#top-agency-nav"));
            });
    },
    bindTier2TimeRangeChange: function () {
        $("#tier-2-nav select.time-range").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                TopAgencyAffiliate.loadStatistics($("#tier-2-nav"));
                TopAgencyAffiliate.tier2AffiliateDataTable.destroy();
                TopAgencyAffiliate.tier2AffiliateDataTable = TopAgencyAffiliate.loadAffiliateDataTable($("#tier-2-nav"));
            });
    },
    bindTier3TimeRangeChange: function () {
        $("#tier-3-nav select.time-range").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                TopAgencyAffiliate.loadStatistics($("#tier-3-nav"));
                TopAgencyAffiliate.tier3AffiliateDataTable.destroy();
                TopAgencyAffiliate.tier3AffiliateDataTable = TopAgencyAffiliate.loadAffiliateDataTable($("#tier-3-nav"));
            });
    },
    loadTopAgencyStatisticsChart: function (tabPaneElement) {
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
    loadNonTopAgencyStatisticsChart: function (tabPaneElement) {
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
    TopAgencyAffiliate.init();
});