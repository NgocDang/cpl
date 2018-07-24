var PricePrediction = {
    init: function () {
        PricePrediction.bindLoadPredictionResult();
        PricePrediction.loadBTCPriceChart();
        PricePrediction.bindLoadBTCCurrentRate();
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
        setInterval(function () {
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
                        else if ($("#btc-rate").val() > data.value){ //Down
                            $("#btc-rate").removeClass("text-success");
                            $("#btc-rate").addClass("text-danger");
                        }
                        $("#btc-rate").val(data.value);
                        $("#btc-rate").html(data.valueInString);
                    }
                }
            });

        }, 1000);
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

        Highcharts.chart('btc-price-chart', {
            chart: {
                type: 'spline',
                animation: Highcharts.svg, // don't animate in old IE
                marginRight: 10,
                events: {
                    load: function () {

                        // set up the updating of the chart each second
                        var series = this.series[0];
                        setInterval(function () {
                            var x = (new Date()).getTime(), // current time
                                y = Math.random();
                            series.addPoint([x, y], true, true);
                        }, 1000);
                    }
                }
            },
            plotOptions: {
                area: {
                    color: 'rgba(24,90,169,.75)',
                    fillColor: 'rgba(24,90,169,.25)',
                    marker: {
                        enabled: false,
                        symbol: 'circle'
                    }
                },
                series: {
                    shadow: false
                }
            },
            title: {
                text: 'Live random data'
            },
            xAxis: {
                type: 'datetime',
                tickPixelInterval: 150
            },
            yAxis: {
                title: {
                    text: 'Value'
                },
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#808080'
                }]
            },
            tooltip: {
                formatter: function () {
                    return '<b>' + this.series.name + '</b><br/>' +
                        Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x) + '<br/>' +
                        Highcharts.numberFormat(this.y, 2);
                }
            },
            legend: {
                enabled: false
            },
            exporting: {
                enabled: false
            },
            series: [{
                name: 'Random data',
                lineColor: Highcharts.getOptions().colors[1],
                color: Highcharts.getOptions().colors[2],
                fillOpacity: 0.5,
                fillColor: 'rgba(24,90,169,.25)',
                data: (function () {
                    // generate an array of random data
                    var data = [],
                        time = (new Date()).getTime(),
                        i;

                    for (i = -19; i <= 0; i += 1) {
                        data.push({
                            x: time + i * 1000,
                            y: Math.random()
                        });
                    }
                    return data;
                }())
            }]
        });
    }
}

$(document).ready(function () {
    PricePrediction.init();
});