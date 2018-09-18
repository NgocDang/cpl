﻿var AdminGameManagement = {
    init: function () {
        // Summary Tab
        AdminGameManagement.bindSummaryTab();
        AdminGameManagement.bindSummaryTimeRangeChange();

        // Lottery Tab
        AdminGameManagement.bindLotteryTab();

        // Lottery Tab - Lottery Summary Tab
        AdminGameManagement.bindLotterySummaryTab();
        AdminGameManagement.bindLotterySummaryTimeRangeChange();

        // Lottery Tab - Lottery Category Tab
        AdminGameManagement.bindLotteryCategoryTabs();
        AdminGameManagement.bindLotteryCategoryTimeRangeChange();

        // Show tab base on URL tab parameter
        var tab = $("#tab").val();
        if (tab == "") 
            tab = "summary";
        $(".nav-tabs a[id='" + tab + "-nav-tab']").tab('show');
    },
    bindSummaryTab: function () {
        $('a#summary-nav-tab').on('show.bs.tab', function (e) {
            if ($("#summary-nav .tab-detail").html().trim().length == 0) {
                $.ajax({
                    url: "/Admin/GetSummaryStatistics/",
                    type: "GET",
                    beforeSend: function () {
                        $("#summary-nav .tab-detail").html("<div class='text-center py-5'><img src='/css/dashboard/plugins/img/loading.gif' class='img-fluid' /></div>");
                    },
                    data: {
                        periodInDay: $("#summary-nav").find("select.time-range").val()
                    },
                    success: function (data) {
                        $("#summary-nav .tab-detail").html(data);
                        AdminGameManagement.loadSummaryStatistics($("#summary-nav"))
                    },
                    complete: function (data) {
                    }
                });
            }

            if ($("#summary-nav .revenue-chart").html().trim().length == 0) {
                PieChart.loadPercentageAjax($("#summary-nav .revenue-chart"), $("#summary-nav .revenue-no-data"), "/Admin/GetSummaryRevenuePieChart/")
            }

            if ($("#summary-nav .device-category-chart").length > 0) {
                PieChart.loadPercentageAjax($("#summary-nav .device-category-chart"), $("#summary-nav .device-category-no-data"), "/Admin/GetSummaryDeviceCategoryPieChart/")
            }
        })
    },
    bindSummaryTimeRangeChange: function () {
        $("").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                $("#GameSummaryStatistic").load("/ViewComponent/GetGameSummaryStatisticViewComponent?periodInDay=" + this.value);
                AdminGameManagement.loadStatisticChart(this.value);
            });
    },
    loadSummaryStatistics: function (container) {
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

        var revenue = { data: [], name: container.find(".total-revenue").val(), color: '#4267b2' };
        var sale = { data: [], name: container.find(".total-sale").val(), color: '#f7931a' };
        var pageView = { data: [], name: container.find(".page-view").val(), color: '#828384' };
        var totalPlayers = { data: [], name: container.find(".total-players").val(), color: '#F69BF9' };

        var totalSaleChanges = JSON.parse(container.find(".total-sale-changes").val());
        if (totalSaleChanges.length != 0) {
            $.each(totalSaleChanges, function (index, value) {
                now = moment(value.Date).valueOf();
                val = value.Value;
                sale.data.push([now, val]);
            });
        }
        else {
            now = moment().valueOf();
            val = 0;
            sale.data.push([now, val]);
        }
        sale.data.sort();

        var totalRevenueChanges = JSON.parse(container.find(".total-revenue-changes").val());
        if (totalRevenueChanges.length != 0) {
            $.each(totalRevenueChanges, function (index, value) {
                now = moment(value.Date).valueOf();
                val = value.Value;
                revenue.data.push([now, val]);
            });
        }
        else {
            now = moment().valueOf();
            val = 0;
            revenue.data.push([now, val]);
        }
        revenue.data.sort();

        var pageViewChanges = JSON.parse(container.find(".page-view-changes").val());
        if (pageViewChanges.length != 0) {
            $.each(pageViewChanges, function (index, value) {
                now = moment(value.Date).valueOf();
                val = value.Count;
                pageView.data.push([now, val]);
            });
        }
        else {
            now = moment().valueOf();
            val = 0;
            pageView.data.push([now, val]);
        }
        pageView.data.sort();

        var totalPlayersChanges = JSON.parse(container.find(".total-players-changes").val());
        if (totalPlayersChanges.length != 0) {
            $.each(totalPlayersChanges, function (index, value) {
                now = moment(value.Date).valueOf();
                val = value.Value;
                totalPlayers.data.push([now, val]);
            });
        }
        else {
            now = moment().valueOf();
            val = 0;
            totalPlayers.data.push([now, val]);
        }
        totalPlayers.data.sort();

        // Push the completed series
        options.series.push(revenue, sale, pageView, totalPlayers);

        // Create the plot
        container.find(".statistic-chart").highcharts(options);
    },
    bindLotteryTab: function () {
        $('a#lottery-nav-tab').on('show.bs.tab', function (e) {
            if ($("#lottery-nav .revenue-chart").html().trim().length == 0) {
                alert('LOTTERY Revenue chart should be loaded!');
            }

            if ($("#lottery-nav .device-category-chart").html().trim().length == 0) {
                alert('LOTTERY Device category chart should be loaded!');
            }
        })
    },
    bindLotterySummaryTab: function () {
        $('a#lottery-summary-nav-tab').on('show.bs.tab', function (e) {
            if ($("#lottery-summary-nav .tab-detail").html().trim().length == 0) {
                alert('LOTTERY Summary statistics should be loaded!');
            }
        })
    },
    bindLotteryCategoryTabs: function () {
        $('a.lottery-category-nav-tab').on('show.bs.tab', function (e) {
            var _this = this;
            if ($("#lottery-category-nav-" + $(_this).data().id + " .tab-detail").html().trim().length == 0) {
                alert('LOTTERY Category ' + $(_this).data().id + ' statistic should be loaded!');
            }
        })
    }
}

$(document).ready(function () {
    AdminGameManagement.init();
});