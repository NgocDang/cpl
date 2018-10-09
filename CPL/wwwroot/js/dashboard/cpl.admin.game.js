var AdminGameManagement = {
    init: function () {
        // Summary Tab
        //AdminGameManagement.bindSummaryTab();
        //AdminGameManagement.bindSummaryTimeRangeChange();

        //// Lottery Tab
        //AdminGameManagement.bindLotteryTab();
        //AdminGameManagement.bindAddLotteryCategory();
        //AdminGameManagement.bindDoAddLotteryCategory();

        //// Lottery Tab - Lottery Summary Tab
        //AdminGameManagement.bindLotterySummaryTab();
        //AdminGameManagement.bindLotterySummaryTimeRangeChange();

        //// Lottery Tab - Lottery Category Tab
        //AdminGameManagement.bindLotteryCategoryTabs();
        //AdminGameManagement.bindLotteryCategoryTimeRangeChange();

        //// Price Prediction Tab - Price Prediction Summary Tab
        //AdminGameManagement.bindAddPricePredictionCategory();
        //AdminGameManagement.bindDoAddPricePredictionCategory();

        AdminGameManagement.bindPricePredictionTab();
        AdminGameManagement.bindPricePredictionSummaryTab();
        //AdminGameManagement.bindPricePredictionSummaryTimeRangeChange();

        ////  Price Prediction Tab -  Price Prediction Category Tab
        //AdminGameManagement.bindPricePredictionCategoryTabs();
        //AdminGameManagement.bindPricePredictionCategoryTimeRangeChange();

        // Show tab base on URL tab parameter
        var tab = $("#tab").val();
        if (tab === "") 
            tab = "summary";
        $(".nav-tabs a[id='" + tab + "-nav-tab']").tab('show');
    },
    bindAddPricePredictionCategory: function () {
        $("#price-prediction-nav").on("click", "#btn-add-price-prediction-category", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/AddPricePredictionCategory/",
                type: "GET",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                    $(_this).html("<i class='fa fa-spinner fa-spin'></i> " + $(_this).text());
                },
                success: function (data) {
                    $("#modal").html(data);
                    $("#edit-price-prediction-category").modal("show");
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                    $(_this).html($(_this).text());
                }
            });
        });
    },
    bindDoAddPricePredictionCategory: function () {
        $("#modal").on("click", "#edit-price-prediction-category .btn-do-add", function () {
            var isFormValid = $("#form-edit-price-prediction-category")[0].checkValidity();
            $("#form-edit-price-prediction-category").addClass('was-validated');
            var _this = this;
            if (isFormValid) {
                var _postData = {};
                $(_this).closest("#form-edit-price-prediction-category").find("div.tab-pane").each(function (i, e) {
                    _postData['PricePredictionCategoryDetailAdminViewModels[' + i + '].LangId'] = $(this).find("#lang-id").val();
                    _postData['PricePredictionCategoryDetailAdminViewModels[' + i + '].Name'] = $(this).find("#name").val();
                    _postData['PricePredictionCategoryDetailAdminViewModels[' + i + '].Description'] = $(this).find("#description").val();
                });
                $.ajax({
                    url: "/Admin/DoAddPricePredictionCategory",
                    type: "POST",
                    data: _postData,
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                        $(_this).html("<i class='fa fa-spinner fa-spin'></i> " + $(_this).text() + " <i class='la la-plus font-size-15px'></i>");
                    },
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message, 'Success!');
                            $("#edit-price-prediction-category").modal("hide");
                        }
                        else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $(_this).attr("disabled", false);
                        $(_this).html($(_this).text() + " <i class='la la-plus font-size-15px'></i>");
                    }
                });
            };
            return false;
        });
    },
    bindPricePredictionTab: function () {
        $('a#price-prediction-nav-tab').on('show.bs.tab', function (e) {
            $("a#price-prediction-summary-nav-tab").tab("show");

            if ($("#price-prediction-nav .revenue-chart").html().trim().length === 0) {
                PieChart.loadPercentageAjax($("#price-prediction-nav .revenue-chart"), $("#price-prediction-nav .revenue-no-data"), "/Admin/GetPricePredictionRevenuePieChart/")
            }

        });

        $('a#price-prediction-nav-tab').on('show.bs.tab', function (e) {
            if ($("#price-prediction-nav .device-category-chart").html().trim().length === 0) {
                PieChart.loadPercentageAjax($("#price-prediction-nav .device-category-chart"), $("#price-prediction-nav .device-category-no-data"), "/Admin/GetPricePreicitonDeviceCategoryPieChart/")
           }
        });
    },
    bindPricePredictionSummaryTab: function () {
        $("a#price-prediction-summary-nav-tab").on('show.bs.tab', function (e) {
            if ($("#price-prediction-summary-nav .tab-detail").html().trim().length === 0) {
                AdminGameManagement.loadPricePredictionSummaryStatistics();
            }

            if ($("#price-prediction-summary-nav .purchased-price-prediction-summary-history table tbody").length == 0) {
                AdminGameManagement.loadPricePredictionHistoryDataTable("#price-prediction-summary-nav", null);
            }

        });
    },
    loadPricePredictionSummaryStatistics: function () {
        $.ajax({
            url: "/Admin/GetPricePredictionSummaryStatistics/",
            type: "GET",
            beforeSend: function () {
                $("#price-prediction-summary-nav .tab-detail").html("<div class='text-center py-5'><img src='/css/dashboard/plugins/img/loading.gif' class='img-fluid' /></div>");
            },
            data: {
                periodInDay: $("#price-prediction-summary-nav select.time-range").val()
            },
            success: function (data) {
                $("#price-prediction-summary-nav .tab-detail").html(data);
                AdminGameManagement.loadPricePredictionSummaryStatisticsChart($("#price-prediction-summary-nav"))
            },
        });
    },
    loadPricePredictionSummaryStatisticsChart: function (container) {
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
    bindPricePredictionSummaryTimeRangeChange: function () {
        $("#price-prediction-summary-nav select.time-range").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                AdminGameManagement.loadPricePredictionSummaryStatistics();
            });
    },
    bindPricePredictionCategoryTabs: function () {
        $('a.price-prediction-category-nav-tab').on('show.bs.tab', function (e) {
            var _this = this;
            if ($("#price-prediction-category-nav-" + $(_this).data().pricePredictionCategoryId + " .tab-detail").html().trim().length === 0) {
                AdminGameManagement.loadPricePredictionCategoryStatistics("#price-prediction-category-nav-" + $(_this).data().pricePredictionCategoryId, $(_this).data().pricePredictionCategoryId);
            }

            if ($("#price-prediction-category-nav-" + $(_this).data().pricePredictionCategoryId + " .purchased-price-prediction-category-history table tbody").length == 0) {
                AdminGameManagement.loadPricePredictionHistoryDataTable("#price-prediction-category-nav-" + $(_this).data().pricePredictionCategoryId, $(_this).data().pricePredictionCategoryId);
            }
        });
    },
    loadPricePredictionCategoryStatistics: function (container, pricePredictionCategoryId) {
        $.ajax({
            url: "/Admin/GetPricePredictionCategoryStatistics/",
            type: "GET",
            beforeSend: function () {
                $(container + " .tab-detail").html("<div class='text-center py-5'><img src='/css/dashboard/plugins/img/loading.gif' class='img-fluid' /></div>");
            },
            data: {
                periodInDay: $(container + " select.time-range").val(),
                pricePredictionCategoryId: pricePredictionCategoryId
            },
            success: function (data) {
                $(container + " .tab-detail").html(data);
                AdminGameManagement.loadPricePredictionCategoryStatisticsChart($(container))
            },
        });
    },
    loadPricePredictionCategoryStatisticsChart: function (container) {
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
    bindPricePredictionCategoryTimeRangeChange: function () {
        $("#price-prediction-nav select.time-range[data-price-prediction-category-id]").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                var _this = this;
                AdminGameManagement.loadPricePredictionCategoryStatistics("#price-prediction-category-nav-" + $(_this).data().pricePredictionCategoryId, $(_this).data().pricePredictionCategoryId);
            });
    },
    loadPricePredictionHistoryDataTable: function (parentElement, pricePredictionCategoryId) {
        return $(parentElement + " .dt-purchased-price-prediction-history").DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "ajax": {
                url: "/Admin/SearchPurchasedPricePredictionHistory",
                type: 'POST',
                data: {
                    pricePredictionCategoryId: pricePredictionCategoryId
                }
            },
            "language": DTLang.getLang(),
            "columns": [
                {
                    "data": "Email",
                    "render": function (data, type, full, meta) {
                        return full.email;
                    }
                },
                {
                    "data": "StatusInString",
                    "render": function (data, type, full, meta) {
                        if (full.status == 1) {
                            return "<p class='text-sm-center'><span class='badge badge-success'>" + $("#active").val() + "</span></p>";
                        }
                        if (full.status == 2) {
                            return "<p class='text-sm-center'><span class='badge badge-secondary'>" + $("#completed").val() + "</span></p>";
                        }
                        else {
                            return "";
                        }
                    }
                },
                {
                    "data": "NumberOfPrediction",
                    "render": function (data, type, full, meta) {
                        return full.numberOfPrediction;
                    }
                },
                {
                    "data": "TotalPurchasePrice",
                    "render": function (data, type, full, meta) {
                        return full.totalPurchasePrice;
                    }
                },
                {
                    "data": "Title",
                    "render": function (data, type, full, meta) {
                        return full.title;
                    }
                },
                {
                    "data": "PurchaseDateTime",
                    "render": function (data, type, full, meta) {
                        return full.purchaseDateTimeInString;
                    }
                },
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        var html = "<a style='margin:2px' href='/Admin/User/" + full.sysUserId + "' target='_blank' class='btn btn-sm btn-outline-secondary btn-view'>" + $("#view").val() + "</a>";
                        return html;
                    },
                    "orderable": false
                }
            ],
        });
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
            $("a#lottery-summary-nav-tab").tab("show");

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
                        $(_this).html("<i class='fa fa-spinner fa-spin'></i> " + $(_this).text() + " <i class='la la-plus font-size-15px'></i>");
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
                        $(_this).html($(_this).text() + " <i class='la la-plus font-size-15px'></i>");
                    }
                });
            };
            return false;
        });
    },
    bindLotterySummaryTab: function () {
        $("a#lottery-summary-nav-tab").on('show.bs.tab', function (e) {
            if ($("#lottery-summary-nav .tab-detail").html().trim().length === 0) {
                AdminGameManagement.loadLotterySummaryStatistics();
            }

            if ($("#lottery-summary-nav .purchased-lottery-summary-history table tbody").length == 0) {
                AdminGameManagement.loadLotteryHistoryDataTable("#lottery-summary-nav", null);
            }
            
        });
    },
    loadLotterySummaryStatistics: function () {
        $.ajax({
            url: "/Admin/GetLotterySummaryStatistics/",
            type: "GET",
            beforeSend: function () {
                $("#lottery-summary-nav .tab-detail").html("<div class='text-center py-5'><img src='/css/dashboard/plugins/img/loading.gif' class='img-fluid' /></div>");
            },
            data: {
                periodInDay: $("#lottery-summary-nav select.time-range").val()
            },
            success: function (data) {
                $("#lottery-summary-nav .tab-detail").html(data);
                AdminGameManagement.loadLotterySummaryStatisticsChart($("#lottery-summary-nav"))
            },
        });
    },
    loadLotterySummaryStatisticsChart: function (container) {
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
    bindLotterySummaryTimeRangeChange: function () {
        $("#lottery-summary-nav select.time-range").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                AdminGameManagement.loadLotterySummaryStatistics();
            });
    },
    bindLotteryCategoryTabs: function () {
        $('a.lottery-category-nav-tab').on('show.bs.tab', function (e) {
            var _this = this;
            if ($("#lottery-category-nav-" + $(_this).data().lotteryCategoryId + " .tab-detail").html().trim().length === 0) {
                AdminGameManagement.loadLotteryCategoryStatistics("#lottery-category-nav-" + $(_this).data().lotteryCategoryId, $(_this).data().lotteryCategoryId);
            }

            if ($("#lottery-category-nav-" + $(_this).data().lotteryCategoryId + " .purchased-lottery-category-history table tbody").length == 0) {
                AdminGameManagement.loadLotteryHistoryDataTable("#lottery-category-nav-" + $(_this).data().lotteryCategoryId, $(_this).data().lotteryCategoryId);
            }
        });
    },
    loadLotteryCategoryStatistics: function (container, lotteryCategoryId) {
        $.ajax({
            url: "/Admin/GetLotteryCategoryStatistics/",
            type: "GET",
            beforeSend: function () {
                $(container + " .tab-detail").html("<div class='text-center py-5'><img src='/css/dashboard/plugins/img/loading.gif' class='img-fluid' /></div>");
            },
            data: {
                periodInDay: $(container + " select.time-range").val(),
                lotteryCategoryId: lotteryCategoryId
            },
            success: function (data) {
                $(container + " .tab-detail").html(data);
                AdminGameManagement.loadLotteryCategoryStatisticsChart($(container))
            },
        });
    },
    loadLotteryCategoryStatisticsChart: function (container) {
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
    bindLotteryCategoryTimeRangeChange: function () {
        $("#lottery-nav select.time-range[data-lottery-category-id]").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                var _this = this;
                AdminGameManagement.loadLotteryCategoryStatistics("#lottery-category-nav-" + $(_this).data().lotteryCategoryId, $(_this).data().lotteryCategoryId);
            });
    },
    loadLotteryHistoryDataTable: function (parentElement, lotteryCategoryId) {
        return $(parentElement + " .dt-purchased-lottery-history").DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "ajax": {
                url: "/Admin/SearchPurchasedLotteryHistory",
                type: 'POST',
                data: {
                    lotteryCategoryId: lotteryCategoryId
                }
            },
            "language": DTLang.getLang(),
            "columns": [
                {
                    "data": "Email",
                    "render": function (data, type, full, meta) {
                        return full.email;
                    }
                },
                {
                    "data": "StatusInString",
                    "render": function (data, type, full, meta) {
                        if (full.status == 1) {
                            return "<p class='text-sm-center'><span class='badge badge-info'>" + $("#pending").val() + "</span></p>";
                        }
                        else if (full.status == 2) {
                            return "<p class='text-sm-center'><span class='badge badge-success'>" + $("#active").val() + "</span></p>";
                        }
                        else if (full.status == 3) {
                            return "<p class='text-sm-center'><span class='badge badge-secondary'>" + $("#completed").val() + "</span></p>";
                        }
                        else if (full.status == 4) {
                            return "<p class='text-sm-center'><span class='badge badge-warning'>" + $("#deactivated").val() + "</span></p>";
                        }
                        else {
                            return "";
                        }
                    }
                },
                {
                    "data": "NumberOfTicketInString",
                    "render": function (data, type, full, meta) {
                        return full.numberOfTicket;
                    }
                },
                {
                    "data": "TotalPurchasePrice",
                    "render": function (data, type, full, meta) {
                        return full.totalPurchasePrice;
                    }
                },
                {
                    "data": "Title",
                    "render": function (data, type, full, meta) {
                        return full.title;
                    }
                },
                {
                    "data": "PurchaseDateTimeInString",
                    "render": function (data, type, full, meta) {
                        return full.purchaseDateTimeInString;
                    }
                },
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        var html = "<a style='margin:2px' href='/Admin/User/" + full.sysUserId + "' target='_blank' class='btn btn-sm btn-outline-secondary btn-view'>" + $("#view").val() + "</a>";
                        return html;
                    },
                    "orderable": false
                }
            ],
        });
    },
}

$(document).ready(function () {
    AdminGameManagement.init();
});