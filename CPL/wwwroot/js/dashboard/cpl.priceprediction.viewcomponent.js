var btcCurrentRate, btcLastestTime, bctDelayTime;
var PricePredictionViewComponent = {
    init: function () {
        PricePredictionViewComponent.bindLoadPredictionResult();
        //PricePredictionViewComponent.loadBTCPriceChart();
        //PricePredictionViewComponent.bindLoadBTCCurrentRate();
        PricePredictionViewComponent.bindBet();
        PricePredictionViewComponent.bindConfirmBet();
        PricePredictionViewComponent.bindBack();
        PricePredictionViewComponent.bindDoBet();
    },
    bindLoadPredictionResult: function () {
        var progressConnection = new signalR.HubConnection("/predictedUserProgress");
        progressConnection
            .start()
            .catch(() => {
                console.log("Error while establishing connection");
            });

        progressConnection.on("predictedUserProgress", (up, down) => {
            if (up !== undefined && down !== undefined)
                this.setUserProgress(up, down);
        });
    },
    //bindLoadBTCCurrentRate: function () {
    //    $.ajax({
    //        url: '/PricePrediction/GetBTCCurrentRate',
    //        type: "POST",
    //        data: {},
    //        success: function (data) {
    //            if (data.success) {
    //                if ($("#btc-rate").val() < data.value) { // Up
    //                    $("#btc-rate").removeClass("text-danger");
    //                    $("#btc-rate").addClass("text-success");
    //                }
    //                    else if ($("#btc-rate").val() > data.value){ //Down
    //                    $("#btc-rate").removeClass("text-success");
    //                    $("#btc-rate").addClass("text-danger");
    //                }
    //                $("#btc-rate").val(data.value);
    //                $("#btc-rate").html(data.valueInString.split(";")[0]); // Assign data to get current rate

    //                // Get data to show in to the chart
    //                btcCurrentRate = data.valueInString;
    //            }
    //            else {
    //                btcCurrentRate = null;
    //            }
    //        }
    //    });
    //},
    setUserProgress: function (up, down) {
        // Reset up-bar setting
        $("#up-bar").css({ "width": up + "%" })
            .attr("aria-valuenow", up)
        $("#up-bar-value").html(up + "%");

        // Reset down-bar setting
        $("#down-bar").css({ "width": down + "%" })
            .attr("aria-valuenow", down)
        $("#down-bar-value").html(down + "%");
    },
    //loadBTCPriceChart: function () {
    //    Highcharts.setOptions({
    //        global: {
    //            useUTC: false
    //        }
    //    });

    //    var btcPriceChart = Highcharts.chart('btc-price-chart', {
    //        chart: {
    //            type: 'area',
    //            zoomType: 'x',
    //            panning: true,
    //            panKey: 'shift',
    //            events: {
    //                load: function () {
    //                    // set up the updating of the chart each second
    //                    var series = this.series[0], x, y;

    //                    //// auto set hover
    //                    //setInterval(function () {
    //                    //    var d = new Date();

    //                    //    var tzOffset = d.getTimezoneOffset();

    //                    //    series.redraw;
    //                    //    var lastPoint = series.getPoint(series.points[series.points.length - 1]);

    //                    //    lastPoint.setState('hover');
    //                    //    //lastPoint.state = '';  // need this to fix hover bug
    //                    //    series.chart.tooltip.refresh(lastPoint); // Show tooltip
    //                    //}, 1000);

    //                    // Load gap between real time
    //                    var previousX = x;
    //                    setInterval(function () {
    //                        if (x !== undefined) {
    //                            var currentTime = parseInt(((new Date()).getTime() / 1000).toFixed());
    //                            previousX = x / 1000;
    //                            var count = currentTime - previousX;
    //                            if (count > 0) {
    //                                for (var i = 0; i < count; i++) {
    //                                    series.addPoint([x + i * 1000, y], true, true);
    //                                }
    //                            }
    //                        }
    //                    }, 1000);

    //                    // Load current BTC-USDT rate in real time
    //                    setInterval(function () {
    //                        PricePredictionViewComponent.bindLoadBTCCurrentRate();
    //                        if (btcCurrentRate !== undefined && btcCurrentRate !== null) {
    //                            x = parseFloat(btcCurrentRate.split(";")[2]) * 1000; // current time from wcf
    //                            //x = (new Date()).getTime(); // current time
    //                            y = parseFloat(btcCurrentRate.split(";")[1]);
    //                            series.addPoint([x, y], true, true);
    //                        }
    //                    }, 1000);
    //                }
    //            }
    //        },
    //        plotOptions: {
    //            area: {
    //                marker: {
    //                    enabled: false
    //                }
    //            },
    //            series: {
    //                shadow: false,
    //                lineWidth: 0.01,
    //                turboThreshold: 50000
    //            }
    //        },
    //        title: {
    //            text: 'Live BTC/USDT rate'
    //        },
    //        xAxis: {
    //            type: 'datetime',
    //            tickPixelInterval: 150
    //        },
    //        yAxis: {
    //            title: {
    //                text: 'Price'
    //            },
    //            min: $("#LowestBtcRate").val(),
    //            plotLines: [{
    //                value: 0,
    //                width: 1,
    //                color: '#1EC481'
    //            }]
    //        },
    //        tooltip: {
    //            crosshairs: [true, true]
    //        },
    //        legend: {
    //            enabled: false
    //        },
    //        exporting: {
    //            enabled: false
    //        },
    //        series: [{
    //            name: 'BTC/USDT rate',
    //            fillOpacity: 1,
    //            states: { hover: { enabled: false } },
    //            dataGrouping: { enabled: false },
    //            connectNulls: true, 
    //            data: (function () {
    //                // generate an array of random data
    //                var data = [],
    //                    currentTime,
    //                    time,
    //                    rate,
    //                    count,
    //                    i;

    //                currentTime = parseInt(((new Date()).getTime() / 1000).toFixed());
    //                time = $("#PreviousBtcRate").val().split(";")[0];
    //                rate = $("#PreviousBtcRate").val().split(";")[1].split(",");
    //                count = rate.length;

    //                for (i = -count; i <= 0; i += 1) {
    //                    data.push({
    //                        x: (parseFloat(time) + count + i) * 1000, // Convert to milisecond
    //                        y: parseFloat(rate[count + i])
    //                    });
    //                }
    //                bctDelayTime = parseInt(((new Date()).getTime() / 1000).toFixed());
    //                for (i = 0; i <= 3600; i += 1) {
    //                    data.push({
    //                        x: (currentTime + i) * 1000,
    //                        y: null
    //                    });
    //                }

    //                // Fill Delay Time
    //                var count = currentTime - bctDelayTime;
    //                for (var i = 0; i < count; i++) {
    //                    data.push({
    //                        x: (bctDelayTime + i) * 1000,
    //                        y: null
    //                    });
    //                }

    //                return data;
    //            }())
    //        }]
    //    });
    //},
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
            $(_this).closest(".tab-pane").find(".bet-amount").val($(_this).data().value);
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
            debugger;
            $.ajax({
                url: '/PricePrediction/ConfirmPrediction',
                type: "POST",
                data: {
                    pricePredictionId: $(_this).data().id,
                    betAmount: tabPane.find(".bet-amount").val(),
                    predictedTrend: tabPane.find(".btn-up-down-group > .btn.active").data().value
                },
                success: function (data) {
                    if (data.success) {
                        if (data.url == null) {
                            tabPane.find(".bet-confirm").hide();
                            tabPane.find(".bet").show();
                            toastr.success(data.message);
                            PricePredictionHistoryViewComponent.pricePredictionHistoryDataTable.ajax.reload();
                        } else
                            window.location.replace(data.url);
                    } else {
                        toastr.error(a);
                    }
                }
            });
        });
    },
};

$(document).ready(function () {
    PricePredictionViewComponent.init();
});