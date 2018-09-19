var PricePredictionViewComponent = {
    btcCurrentRate: null,
    bctDelayTime: null,
    realtimeInterval: null,
    charts: [],
    init: function () {
        PricePredictionViewComponent.bindLoadPredictionResult();
        PricePredictionViewComponent.loadBTCPriceChart();
        PricePredictionViewComponent.bindLoadBTCCurrentRate();
        PricePredictionViewComponent.bindBet();
        PricePredictionViewComponent.bindConfirmBet();
        PricePredictionViewComponent.bindBack();
        PricePredictionViewComponent.bindDoBet();
        PricePredictionViewComponent.bindCountDownTick();
    },
    bindLoadPredictionResult: function () {
        var progressConnection = new signalR.HubConnection("/predictedUserProgress");
        progressConnection
            .start()
            .catch(() => {
                console.log("Error while establishing connection");
            });
        progressConnection.on("predictedUserProgress", (up, down, pricePredictionId) => {
            if (up !== undefined && down !== undefined && pricePredictionId !== undefined)
                this.setUserProgress(up, down, pricePredictionId);
        });
    },
    bindLoadBTCCurrentRate: function () {
        $.ajax({
            url: '/PricePrediction/GetBTCCurrentRate',
            type: "POST",
            data: {},
            success: function (data) {
                if (data.success) {
                    $(".btc-rate").each(function (index, element) {
                        if ($(element).val() < data.value) { // Up
                            $(element).removeClass("text-danger");
                            $(element).addClass("text-success");
                        }
                        else if ($(element).val() > data.value) { //Down
                            $(element).removeClass("text-success");
                            $(element).addClass("text-danger");
                        }

                        $(element).val(data.value);
                        $(element).html(data.valueInString.split(";")[0]); // Assign data to get current rate
                    });
                    // Get data to show in to the chart
                    PricePredictionViewComponent.btcCurrentRate = data.valueInString;
                }
                else {
                    PricePredictionViewComponent.btcCurrentRate = null;
                }
            }
        });
    },
    setUserProgress: function (up, down, pricePredictionId) {
        // Reset up-bar setting
        $("#price-prediction-nav-" + pricePredictionId + " #up-bar").css({ "width": up + "%" })
            .attr("aria-valuenow", up)
        $("#price-prediction-nav-" + pricePredictionId + " #up-bar-value").html(up + "%");

        // Reset down-bar setting
        $("#price-prediction-nav-" + pricePredictionId + " #down-bar").css({ "width": down + "%" })
            .attr("aria-valuenow", down)
        $("#price-prediction-nav-" + pricePredictionId + " #down-bar-value").html(down + "%");
    },
    loadBTCPriceChart: function () {
        $(".tab-pane.active .btc-price-chart").each(function (index, element) {
            var openTime = moment(parseInt($(element).closest(".tab-pane").find("#OpenBettingTime").val()));
            var closeTime = moment(parseInt($(element).closest(".tab-pane").find("#CloseBettingTime").val()));
            var resultTime = moment(parseInt($(element).closest(".tab-pane").find("#ResultTime").val()));
            Highcharts.setOptions({
                global: {
                    useUTC: false
                },
                lang: DTLang.getHighChartLang()
            });

            $(element).highcharts({
                chart: {
                    type: 'area',
                    zoomType: 'x',
                    panning: true,
                    panKey: 'shift',
                    events: {
                        load: function () {
                            // set up the updating of the chart each second
                            PricePredictionViewComponent.charts.push($(element).highcharts());

                            if (PricePredictionViewComponent.realtimeInterval != null) {
                                clearInterval(PricePredictionViewComponent.realtimeInterval)
                            }

                            PricePredictionViewComponent.realtimeInterval = setInterval(function () {
                                PricePredictionViewComponent.bindLoadBTCCurrentRate();

                                for (var i = 0; i < PricePredictionViewComponent.charts.length; i++) {
                                    var series = PricePredictionViewComponent.charts[i].series[0], x, y,
                                        chart = PricePredictionViewComponent.charts[i],
                                        yAxis = chart.yAxis[0];

                                    // Load gap between real time and real time data
                                    var previousX = x;
                                    if (x !== undefined) {
                                        var currentTime = parseInt(((new Date()).getTime() / 1000).toFixed());
                                        previousX = x / 1000;
                                        var count = currentTime - previousX;
                                        if (count > 0) {
                                            for (var i = 0; i < count; i++) {
                                                series.addPoint([x + i * 1000, y], true, true);
                                            }
                                        }
                                    }

                                    if (PricePredictionViewComponent.btcCurrentRate !== undefined && PricePredictionViewComponent.btcCurrentRate !== null) {
                                        x = parseFloat(PricePredictionViewComponent.btcCurrentRate.split(";")[2]) * 1000; // current time from wcf
                                        y = parseFloat(PricePredictionViewComponent.btcCurrentRate.split(";")[1]);
                                        series.addPoint([x, y], true, true);
                                        yAxis.plotLinesAndBands[0].options.label.text = y.toString();
                                        yAxis.plotLinesAndBands[0].options.label.align = 'right';
                                        yAxis.plotLinesAndBands[0].options.label.x = -80;
                                        yAxis.plotLinesAndBands[0].options.value = y;
                                        yAxis.plotLinesAndBands[0].options.zIndex = 4;
                                        yAxis.update();
                                    }
                                }

                                console.log(PricePredictionViewComponent.btcCurrentRate);

                            }, 1000);

                        }
                    }
                },
                plotOptions: {
                    area: {
                        marker: {
                            enabled: false
                        }
                    },
                    series: {
                        shadow: false,
                        lineWidth: 0.01,
                        turboThreshold: 200000,
                        color: '#ffc111',
                    }
                },
                title: {
                    text: $("#btcPricePredictionChartTitle").val(),
                    align: 'left'
                },
                xAxis: {
                    type: 'datetime',
                    dateTimeLabelFormats: {
                        day: '%b/%e'
                    },
                    tickPixelInterval: 150,
                    plotLines: [{
                        label: {
                            text: $("#open").val() + ' (' + openTime.format("HH:mm") + ')',
                            //rotation: 0,
                            zIndex: 4
                        },
                        color: '#000', // Color value
                        value: (openTime.valueOf()), // Value of where the line will appear
                        zIndex: 4,
                        dashStyle: 'dash',
                        width: 1 // Width of the line   
                    },
                    {
                        label: {
                            text: $("#close").val() + ' (' + closeTime.format("HH:mm") + ')',
                            //rotation: 0,
                            //x: -90,
                            zIndex: 4
                        },
                        color: '#000', // Color value
                        value: (closeTime.valueOf()), // Value of where the line will appear
                        zIndex: 4,
                        dashStyle: 'dash',
                        width: 1 // Width of the line    
                    },
                    {
                        label: {
                            text: $("#result").val() + ' (' + resultTime.format("HH:mm") + ')',
                            //rotation: 0,
                            zIndex: 4
                        },
                        color: '#000', // Color value
                        value: (resultTime.valueOf()), // Value of where the line will appear
                        zIndex: 4,
                        dashStyle: 'dash',
                        width: 1 // Width of the line    
                    }]
                },
                yAxis: {
                    title: null,
                    plotLines: [{
                        label: {
                            text: "",
                        },
                        color: 'red', // Color value
                        value: 0, // Value of where the line will appear
                        width: 1 // Width of the line    
                    }],
                    min: $("#LowestBtcRate").val(),
                },
                tooltip: {
                    crosshairs: [true, true]
                },
                legend: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                series: [{
                    name: $("#btcPricePredictionSeriesName").val(),
                    fillOpacity: 1,
                    states: { hover: { enabled: false } },
                    dataGrouping: { enabled: false },
                    connectNulls: true,
                    zIndex: 1,
                    data: (function () {
                        // generate an array of random data
                        var data = [];
                        var currentTime = parseInt(((new Date()).getTime() / 1000).toFixed());
                        var btcPrices = JSON.parse($("#PreviousBtcRate").val());
                        for (i = 0; i < btcPrices.length; i++) {
                            data.push({
                                x: moment(btcPrices[i].Time * 1000).valueOf(), // Convert to milisecond
                                y: parseFloat(btcPrices[i].Price)
                            });
                        }

                        PricePredictionViewComponent.bctDelayTime = parseInt(((new Date()).getTime() / 1000).toFixed());
                        var resultTimeInSeconds = moment(parseInt($(".tab-pane.active").find("#ResultTime").val())).valueOf() / 1000;
                        for (i = currentTime; i <= resultTimeInSeconds + 3600; i++) { // After result time 2 hours
                            data.push({
                                x: moment(i * 1000).valueOf(),
                                y: null
                            });
                        }

                        // Fill Delay Time
                        //var count = currentTime - PricePredictionViewComponent.bctDelayTime;
                        //for (var i = 0; i < count; i++) {
                        //    data.push({
                        //        x: moment.utc((PricePredictionViewComponent.bctDelayTime + i) * 1000).valueOf(),
                        //        y: null
                        //    });
                        //}

                        return data;
                    }())
                }]
            });
        });
    },
    loadPricePredictionHistoryDatatable: function () {
        if ($("#SysUserId").val() === undefined)
            return false;

        return $('#dt-prediction-history').DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "ajax": {
                url: "/PricePrediction/SearchPricePredictionHistory",
                type: 'POST'
            },
            "language": DTLang.getLang(),
            "columns": [
                {
                    "data": "PurcharseTime",
                    "render": function (data, type, full, meta) {
                        return full.purcharseTimeInString;
                    }
                },
                {
                    "data": "Bet",
                    "className": "text-center",
                    "render": function (data, type, full, meta) {
                        return full.bet;
                    }
                },
                {
                    "data": "StartRate",
                    "render": function (data, type, full, meta) {
                        return full.startRateInString;
                    }
                },
                {
                    "data": "Amount",
                    "render": function (data, type, full, meta) {
                        return full.amountInString;
                    }
                },
                {
                    "data": "Bonus",
                    "render": function (data, type, full, meta) {
                        return full.bonusInString;
                    }
                },
                {
                    "data": "Status",
                    "render": function (data, type, full, meta) {
                        if (full.status === "ACTIVE") {
                            return "<i class='fas fa-circle text-success fa-circle-active'></i> " + full.status;
                        }
                        else {
                            return "<i class='fas fa-circle text-danger fa-circle-inactive'></i> " + full.status;
                        }
                    }
                },
                {
                    "data": "ResultRate",
                    "render": function (data, type, full, meta) {
                        return full.resultRateInString;
                    }
                },
                {
                    "data": "ResultTime",
                    "render": function (data, type, full, meta) {
                        return full.resultTimeInString;
                    }
                }
            ]
        });
    },
    bindBet: function () {
        $(".tab-pane").on("click", ".btn-bet", function () {
            var _this = this;
            var tabPane = $(_this).closest(".tab-pane");
            $(_this).closest(".tab-pane").find(".bet-amount").val($(_this).data().value);
            tabPane.find(".btn-bet").removeClass("btn-secondary");
            tabPane.find(".btn-bet").addClass("btn-gray");
            $(_this).removeClass("btn-gray");
            $(_this).addClass("btn-secondary");
        });

        $(".tab-pane").on("click", ".btn-up-down-group > .btn", function () {
            var _this = this;
            var tabPane = $(_this).closest(".tab-pane");
            tabPane.find(".btn-up-down-group > .btn").removeClass("active");
            $(_this).addClass("active");
            return false;
        });
    },
    bindConfirmBet: function () {
        $(".tab-pane").on("click", ".btn-confirm-bet", function () {
            var _this = this;
            var tabPane = $(_this).closest(".tab-pane");
            if (tabPane.find(".bet-amount").val() <= 0) {
                toastr.error("Incorrect amount!");
            } else {
                if (tabPane.find(".btn-up").hasClass("active") || tabPane.find(".btn-down").hasClass("active")) {
                    tabPane.find(".predicted-trend-confirm").html(tabPane.find(".btn-up").hasClass("active") ? "UP" : "DOWN");
                    if (tabPane.find(".btn-up").hasClass("active")) {
                        tabPane.find(".predicted-trend-confirm").removeClass("danger").addClass("success");
                        tabPane.find(".bet-amount-confirm").removeClass("danger").addClass("success");
                    } else {
                        tabPane.find(".predicted-trend-confirm").removeClass("success").addClass("danger");
                        tabPane.find(".bet-amount-confirm").removeClass("success").addClass("danger");
                    }
                    tabPane.find(".bet-amount-confirm").html(tabPane.find(".bet-amount").val());
                    tabPane.find(".bet").hide();
                    tabPane.find(".bet-confirm").show();
                } else {
                    toastr.error("Please select UP or DOWN");
                }
            }
        });
    },
    bindBack: function () {
        $(".tab-pane").on("click", ".btn-back", function () {
            var _this = this;
            var tabPane = $(_this).closest(".tab-pane");
            tabPane.find(".bet-confirm").hide();
            tabPane.find(".bet").show();
        });
    },
    bindDoBet: function () {
        $(".tab-pane").on("click", ".btn-confirm", function () {
            var _this = this;
            var tabPane = $(_this).closest(".tab-pane");
            $.ajax({
                url: '/PricePrediction/ConfirmPrediction',
                type: "POST",
                data: {
                    pricePredictionId: $(_this).data().id,
                    betAmount: tabPane.find(".bet-amount").val(),
                    predictedTrend: tabPane.find(".btn-up-down-group > .btn.active").data().value,
                },
                success: function (data) {
                    if (data.success) {
                        if (data.url == null) {
                            tabPane.find(".bet-confirm").hide();
                            tabPane.find(".bet").show();
                            toastr.success(data.message);
                            PricePredictionHistoryViewComponent.pricePredictionHistoryDataTable.ajax.reload();
                            $(".user-token-amount").map(function (index, element) {
                                $(element).text(data.token + " CPL");
                            });
                        } else
                            window.location.replace(data.url);
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        });
    },
    bindCountDownTick: function () {
        $(".tab-pane.active .countdown-clock").each(function (index, element) {
            var closeTime = moment(parseInt($(element).closest(".tab-pane").find("#CloseBettingTime").val()));
            var dateString = closeTime.format("HH:mm") == "00:00" ? "24:00" : closeTime.format("HH:mm");
            // Update the count down every 1 second
            var x = setInterval(function () {

                // Get todays date and time
                var now = new Date().getTime();

                // Find the distance between now and the count down date
                var distance = closeTime - now;

                // Time calculations for days, hours, minutes and seconds
                var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                var seconds = Math.floor((distance % (1000 * 60)) / 1000);

                // Display the result in the element with id="demo"
                $(element).closest(".tab-pane").find(".countdown-clock").html($("#close").val() + ": " + dateString + '  <i class="la la-clock-o clock-icon"></i>' + hours + ":" + minutes + ":" + seconds);
            });
        });
    },

};

$(document).ready(function () {
    PricePredictionViewComponent.init();
});