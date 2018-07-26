var btcCurrentRate, btcDelayValue, btcLastestTime;
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
    bindLoadBTCDelayRate: function (start, end) {
        $.ajax({
            url: '/PricePrediction/GetBTCDelayRate',
            type: "POST",
            data: {
                Start: start,
                End :  end
            },
            success: function (data) {
                if (data.success) {
                    btcDelayValue = data.valueInString;
                }
                else {
                    btcDelayValue = null;
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
                        var series = this.series[0];
                        setInterval(function () {
                            PricePrediction.bindLoadBTCCurrentRate();
                            if (btcCurrentRate !== undefined && btcCurrentRate !== null) {
                                var x = parseFloat(btcCurrentRate.split(";")[2]) * 1000, // current time
                                    y = parseFloat(btcCurrentRate.split(";")[1]);
                                series.addPoint([x, y], true, true);
                                //var point = series.points[series.points.length - 1];
                                //var pointt = series.getPoint(point);
                                //pointt.setState('select');
                                //series.chart.tooltip.refresh(pointt);
                                //var i = series.data.length;
                                //var data;
                                //if (i > 0) {
                                //    data = series.data[i - 1];
                                //    data.setState('hover');
                                //    series.chart.tooltip.refresh(data);
                                //}


                                //// Find last not-null point in data
                                //let last = data.indexOf(null) - 1;
                                //last = (last === -2) ? data.length - 1 : last;
                                //const lastPoint = this.series[0].points[last];

                                //// Trigger the hover event 
                                //lastPoint.setState('hover');
                                //lastPoint.state = '';  // You need this to fix hover bug
                                //this.tooltip.refresh(lastPoint); // Show tooltip
                            }
                        }, 1000);
                        PricePrediction.bindLoadBTCDelayRate(btcLastestTime, (new Date()).getTime());
                        if (btcDelayValue !== undefined && btcDelayValue !== null)
                        {
                            var x = parseFloat(btcDelayValue.split(";")[1]) * 1000,
                                y = parseFloat(btcDelayValue.split(";")[0]);
                            series.addPoint([x, y], true, true);
                        }
                    }
                }
            },
            plotOptions: {
                area: {
                    marker: {
                        enabled: false,
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
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#808080'
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
                name: 'BTC/USD rate',
                fillOpacity: 0.5,
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

                    currentTime = (new Date()).getTime();
                    time = $("#PreviousBtcRate").val().split(";")[0].split(",");
                    rate = $("#PreviousBtcRate").val().split(";")[1].split(",");
                    count = time.length;

                    for (i = -count; i <= 0; i += 1) {
                        data.push({
                            x: parseFloat(time[count + i]) * 1000, // Convert to milisecond
                            y: parseFloat(rate[count + i])
                        });
                    }

                    btcLastestTime = parseFloat(time[count - 1]) * 1000;

                    for (i = 0; i <= 3600; i += 1) {
                        data.push({
                            x: currentTime + i * 1000,
                            y: 0
                        });
                    }
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
                        if (full.status === "ACTIVE")
                        {
                            return "<i class='fas fa-circle text-success fa-circle-active'></i> " + full.status;
                        }
                        else
                        {
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
            ],
        });
    }
}

$(document).ready(function () {
    PricePrediction.init();
});