var AdminGameManagement = {
    init: function () {
        // Summary Tab
        AdminGameManagement.bindSummaryTab();
        AdminGameManagement.bindSummaryTimeRangeChange();

        // Lottery Tab
        AdminGameManagement.bindLotteryTab();
        AdminGameManagement.bindAddLotteryCategory();
        AdminGameManagement.bindDoAddLotteryCategory();

        // Lottery Tab - Lottery Summary Tab
        AdminGameManagement.bindLotterySummaryTab();
        AdminGameManagement.bindLotterySummaryTimeRangeChange();

        // Lottery Tab - Lottery Category Tab
        AdminGameManagement.bindLotteryCategoryTabs();
        AdminGameManagement.bindLotteryCategoryTimeRangeChange();

        // Show tab base on URL tab parameter
        var tab = $("#tab").val();
        if (tab === "") 
            tab = "summary";
        $(".nav-tabs a[id='" + tab + "-nav-tab']").tab('show');
    },
    bindSummaryTab: function () {
        $('a#summary-nav-tab').on('show.bs.tab', function (e) {
            if ($("#summary-nav .tab-detail").html().trim().length === 0) {
                AdminGameManagement.loadSummaryStatistics();
            }

            if ($("#summary-nav .revenue-chart").html().trim().length === 0) {
                PieChart.loadPercentageAjax($("#summary-nav .revenue-chart"), $("#summary-nav .revenue-no-data"), "/Admin/GetSummaryRevenuePieChart/")
            }

            if ($("#summary-nav .device-category-chart").html().trim().length === 0) {
                PieChart.loadPercentageAjax($("#summary-nav .device-category-chart"), $("#summary-nav .device-category-no-data"), "/Admin/GetSummaryDeviceCategoryPieChart/")
            }
        })
    },
    loadSummaryStatistics: function () {
        $.ajax({
            url: "/Admin/GetSummaryStatistics/",
            type: "GET",
            beforeSend: function () {
                $("#summary-nav .tab-detail").html("<div class='text-center py-5'><img src='/css/dashboard/plugins/img/loading.gif' class='img-fluid' /></div>");
            },
            data: {
                periodInDay: $("#summary-nav select.time-range").val()
            },
            success: function (data) {
                $("#summary-nav .tab-detail").html(data);
                AdminGameManagement.loadSummaryStatisticsChart($("#summary-nav"))
            },
        });
    },
    bindSummaryTimeRangeChange: function () {
        $("#summary-nav select.time-range").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                AdminGameManagement.loadSummaryStatistics();
            });
    },
    loadSummaryStatisticsChart: function (container) {
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

        var revenue = { data: [], name: container.find(".total-revenue").val(), color: '#4267b2' };
        var sale = { data: [], name: container.find(".total-sale").val(), color: '#f7931a' };
        var pageView = { data: [], name: container.find(".page-view").val(), color: '#828384' };
        var totalPlayers = { data: [], name: container.find(".total-players").val(), color: '#F69BF9' };

        var totalSaleChanges = JSON.parse(container.find(".total-sale-changes").val());
        if (totalSaleChanges.length !== 0) {
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
        if (totalRevenueChanges.length !== 0) {
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
        if (pageViewChanges.length !== 0) {
            $.each(pageViewChanges, function (index, value) {
                now = moment(value.Date).valueOf();
                val = value.Value;
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
            if ($("#lottery-nav .revenue-chart").html().trim().length === 0) {
                PieChart.loadPercentageAjax($("#lottery-nav .revenue-chart"), $("#lottery-nav .revenue-no-data"), "/Admin/GetLotteryRevenuePieChart/")
            }

        });

        $('a#lottery-nav-tab').on('show.bs.tab', function (e) {
            if ($("#lottery-nav .device-category-chart").html().trim().length === 0) {
                PieChart.loadPercentageAjax($("#lottery-nav .device-category-chart"), $("#lottery-nav .device-category-no-data"), "/Admin/GetLotteryDeviceCategoryPieChart/")
            }

        });
    },
    bindAddLotteryCategory: function () {
        $("#lottery-nav").on("click", "#btn-add-lottery-category", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/AddLotteryCategory/",
                type: "GET",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                    $(_this).html("<i class='fa fa-spinner fa-spin'></i> " + $(_this).text());
                },
                success: function (data) {
                    $("#modal").html(data);
                    $("#edit-lottery-category").modal("show");
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                    $(_this).html($(_this).text());
                }
            });
        });
    },
    bindDoAddLotteryCategory: function () {
        $("#modal").on("click", "#edit-lottery-category .btn-do-add", function () {
            var isFormValid = $("#form-edit-lottery-category")[0].checkValidity();
            $("#form-edit-lottery-category").addClass('was-validated');
            var _this = this;

            if (isFormValid) {
                $.ajax({
                    url: "/Admin/DoAddLotteryCategory",
                    type: "POST",
                    data: $("#form-edit-lottery-category").serialize(),
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                    },
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message, 'Success!');
                            $("#edit-lottery-category").modal("hide");
                        }
                        else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $(_this).attr("disabled", false);
                    }
                });
            };
            return false;
        });
    },
    bindLotterySummaryTab: function () {
        $('a#lottery-summary-nav-tab').on('show.bs.tab', function (e) {
            if ($("#lottery-summary-nav .tab-detail").html().trim().length === 0) {
                alert('LOTTERY Summary statistics should be loaded!');
            }
        });
    },
    bindLotterySummaryTimeRangeChange: function () {
        $("#lottery-summary-nav select.time-range").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                alert('Time range change - LOTTERY Summary statistics should be reloaded!');
            });
    },
    bindLotteryCategoryTabs: function () {
        $('a.lottery-category-nav-tab').on('show.bs.tab', function (e) {
            var _this = this;
            if ($("#lottery-category-nav-" + $(_this).data().lotteryCategoryId + " .tab-detail").html().trim().length === 0) {
                alert('LOTTERY Category ' + $(_this).data().lotteryCategoryId + ' statistics should be loaded!');
            }
        });
    },
    bindLotteryCategoryTimeRangeChange: function () {
        $("#lottery-nav select.time-range[data-lottery-category-id]").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                var _this = this;
                alert('Time range change - LOTTERY Category ' + $(_this).data().lotteryCategoryId + ' statistics should be reloaded!');
            });
    }
}

$(document).ready(function () {
    AdminGameManagement.init();
});