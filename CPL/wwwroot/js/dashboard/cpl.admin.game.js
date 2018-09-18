var AdminGameManagement = {
    init: function () {
        AdminGameManagement.bindNavbar();

        // Show tab base on URL tab parameter
        var tab = $("#tab").val();
        if (tab == "") 
            tab = "summary";
        $(".nav-tabs a[id='" + tab + "-nav-tab']").tab('show');


        
        //AdminGameManagement.loadStatisticChart();
        //AdminGameManagement.bindSelectTimeRange();
        //PieChartViewComponent.loadPercentage($("#sumary-revenue-chart").find("#ChartData").val(), $("#sumary-revenue-chart"), $("#sumary-revenue-no-data"), $("#sumary-revenue-chart").find("#SeriesName").val());
        //PieChartViewComponent.loadPercentage($("#device-category-chart").find("#ChartData").val(), $("#device-category-chart"), $("#device-category-no-data"), $("#device-category-chart").find("#SeriesName").val());
    },
    bindSelectTimeRange: function () {
        $("#Category").on("changed.bs.select",
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

        var revenue = { data: [], name: $("#TotalRevenue").val(), color: '#4267b2' };
        var sale = { data: [], name: $("#TotalSale").val(), color: '#f7931a' };
        var pageView = { data: [], name: $("#PageView").val(), color: '#828384' };
        var totalPlayers = { data: [], name: $("#TotalPlayers").val(), color: '#F69BF9' };

        if (JSON.parse(a).TotalSaleChanges.length != 0) {
            $.each(JSON.parse(a).TotalSaleChanges, function (index, value) {
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

        if (JSON.parse(a).TotalRevenueChanges.length != 0) {
            $.each(JSON.parse(a).TotalRevenueChanges, function (index, value) {
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

        if (JSON.parse(a).PageViewChanges.length != 0) {
            $.each(JSON.parse(a).PageViewChanges, function (index, value) {
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

        if (JSON.parse(a).TotalPlayersChanges.length != 0) {
            $.each(JSON.parse(a).TotalPlayersChanges, function (index, value) {
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
    bindNavbar: function () {
        $('a#summary-nav-tab').on('show.bs.tab', function (e) {
            if ($("#summary-nav .tab-detail").html().length == 0) {
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
                        AdminGameManagement.loadSummaryStatistics($("#summary-nav .tab-detail"))
                    },
                    complete: function (data) {
                    }
                });
            }

            if ($("#summary-nav .revenue-chart .loading").length > 0) {
                alert(2);
            }

            if ($("#summary-nav .device-category-chart .loading").length > 0) {
                alert(3);
            }
        })
    }
}

$(document).ready(function () {
    AdminGameManagement.init();
});