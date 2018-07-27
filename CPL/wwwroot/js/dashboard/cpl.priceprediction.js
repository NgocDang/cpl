var btcCurrentRate, btcLastestTime;
var PricePrediction = {
    init: function () {
        PricePrediction.bindLoadPredictionResult();
        PricePrediction.loadBTCPriceChart();
        PricePrediction.bindLoadBTCCurrentRate();
        PricePrediction.loadHistoryDatatable();
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
    bindLoadBTCCurrentRate: function () {
        $.ajax({
            url: '/PricePrediction/GetBTCCurrentRate',
            type: "POST",
            data: {},
            success: function (data) {
                if (data.success) {
                    if ($("#btc-rate").val() < data.value) { // Up
                        $("#btc-rate").removeClass("text-danger");
                        $("#btc-rate").addClass("text-success");
                    }
                    else if ($("#btc-rate").val() > data.value) { //Down
                        $("#btc-rate").removeClass("text-success");
                        $("#btc-rate").addClass("text-danger");
                    }
                    $("#btc-rate").val(data.value);
                    $("#btc-rate").html(data.valueInString.split(";")[0]); // Assign data to get current rate

                    // Get data to show in to the chart
                    btcCurrentRate = data.valueInString;
                }
                else {
                    btcCurrentRate = null;
                }
            }
        });
    },
    setUserProgress: function (up, down) {
        $("#up-bar").css({ "width": up + "%" })
            .attr("aria-valuenow", up)
            .html("<i class='fas fa-arrow-up'></i>" + up + "%");
        $("#down-bar").css({ "width": down + "%" })
            .attr("aria-valuenow", down)
            .html("<i class='fas fa-arrow-down'></i>" + down + "%");
    },
    loadBTCPriceChart: function () {
        Highcharts.setOptions({
            global: {
                useUTC: false
            }
        });

        var btcPriceChart = Highcharts.chart('btc-price-chart', {
            chart: {
                type: 'area',
                zoomType: 'x',
                panning: true,
                panKey: 'shift',
                events: {
                    load: function () {
                        // set up the updating of the chart each second
                        var series = this.series[0], x, y;

                        // Load gap between real time
                        var previousX = x;
                        setInterval(function () {
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
                        }, 1000);

                        // Load current BTC-USDT rate in real time
                        setInterval(function () {
                            PricePrediction.bindLoadBTCCurrentRate();
                            if (btcCurrentRate !== undefined && btcCurrentRate !== null) {
                                x = parseFloat(btcCurrentRate.split(";")[2]) * 1000; // current time
                                y = parseFloat(btcCurrentRate.split(";")[1]);
                                series.addPoint([x, y], true, true);
                            }
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
                    turboThreshold: 50000
                }
            },
            title: {
                text: 'Live BTC/USD rate'
            },
            xAxis: {
                type: 'datetime',
                tickPixelInterval: 150
            },
            yAxis: {
                title: {
                    text: 'Price'
                },
                min: $("#LowestBtcRate").val(),
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#1EC481'
                }]
            },
            tooltip: {
                enable: false
            },
            legend: {
                enabled: false
            },
            exporting: {
                enabled: false
            },
            series: [{
                name: 'BTC/USDT rate',
                fillOpacity: 1,
                states: { hover: { enabled: false } },
                dataGrouping: { enabled: false },
                data: (function () {
                    // generate an array of random data
                    var data = [],
                        currentTime,
                        time,
                        rate,
                        count,
                        i;

                    currentTime = parseInt(((new Date()).getTime() / 1000).toFixed());
                    time = $("#PreviousBtcRate").val().split(";")[0];
                    rate = $("#PreviousBtcRate").val().split(";")[1].split(",");
                    count = rate.length;

                    for (i = -count; i <= 0; i += 1) {
                        data.push({
                            x: (parseFloat(time) + count + i) * 1000, // Convert to milisecond
                            y: parseFloat(rate[count + i])
                        });
                    }

                    btcLastestTime = (parseFloat(time) + count) * 1000;
                    //for (i = 0; i <= 3600; i += 1) {
                    //    data.push({
                    //       x: (currentTime + i) * 1000,
                    //       y: 0
                    //    });
                    //}
                    return data;
                }())
            }]
        });
    },
    loadHistoryDatatable: function () {
        if ($("#SysUserId").val() === undefined)
            return false;
        $('#dt-prediction-history').DataTable({
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
    }
};

$(document).ready(function () {
    PricePrediction.init();
});