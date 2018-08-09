var Dashboard = {
    init: function () {
        Dashboard.loadSlider();
        Dashboard.loadHoldingPercentage();
        Dashboard.loadBetStatistic();
    },
    loadBetStatistic: function () {
        $.ajax({
            url: '/Dashboard/GetDataLineChart',
            type: "POST",
            chartData: {},
            success: function (chartData) {
                if (chartData.success) {
                    var a = chartData.message;

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
                            title: {
                                text: 'Date'
                            }
                        },
                        yAxis: {
                            title: {
                                text: ''
                            },
                        },
                        tooltip: {
                            headerFormat: '<b>{series.name}</b><br>',
                            pointFormat: '{point.x:%e. %b}: {point.y:.2f} CPL'
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

                    var asset = { data: [], name: 'Asset Changes', color: '#4267b2' };
                    var invest = { data: [], name: 'Monthly Invests', color: '#f7931a' };
                    var prize = { data: [], name: 'Prizes', color: '#828384' };

                    $.each(JSON.parse(a).AssetChange, function (index, value) {
                        date = new Date(value.Date);
                        now_utc = Date.UTC(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate());
                        val = value.Amount;
                        asset.data.push([now_utc, val]);
                    });
                    asset.data.sort();

                    $.each(JSON.parse(a).MonthlyInvest, function (index, value) {
                        date = new Date(value.Date);
                        now_utc = Date.UTC(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate());
                        val = value.Amount;

                        invest.data.push([now_utc, val]);
                    });
                    invest.data.sort();

                    $.each(JSON.parse(a).BonusChange, function (index, value) {
                        date = new Date(value.Date);
                        now_utc = Date.UTC(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate());
                        val = value.Amount;

                        prize.data.push([now_utc, val]);
                    });
                    prize.data.sort();

                    // Push the completed series
                    options.series.push(asset, invest, prize);

                    // Create the plot
                    new Highcharts.Chart("bet-statistic-chart", options);
                }
            }
        });
    },
    loadHoldingPercentage: function () {
        $.ajax({
            url: '/Dashboard/GetDataPieChart',
            type: "POST",
            data: {},
            success: function (data) {
                if (data.success) {
                    var a = data.message;
                    Highcharts.chart('holding-percentage-chart', {
                        chart: {
                            plotBackgroundColor: null,
                            plotBorderWidth: null,
                            plotShadow: false,
                            type: 'pie'
                        },
                        title: {
                            text: null
                        },
                        tooltip: {
                            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                        },
                        exporting: {
                            enabled: false
                        },
                        plotOptions: {
                            pie: {
                                allowPointSelect: true,
                                cursor: 'pointer',
                                dataLabels: {
                                    enabled: false
                                },
                                showInLegend: true
                            }
                        },
                        series: [{
                            name: 'Balance',
                            colorByPoint: true,
                            data: [{
                                name: 'CPL',
                                y: JSON.parse(a).CPLPercentage,
                                sliced: true,
                                selected: true,
                                color: '#4267b2'
                            }, {
                                name: 'BTC',
                                y: JSON.parse(a).BTCPercentage,
                                color: '#f7931a'
                            }, {
                                name: 'ETH',
                                y: JSON.parse(a).ETHPercentage,
                                color: '#828384'
                            }]
                        }]
                    });
                }
            }
        });
    },
    loadSlider: function () {
        $('.owl-carousel').owlCarousel({
            loop: true,
            margin: 10,
            auto: true,
            dots: false,
            autoplay: true,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 1
                },
                768: {
                    items: 2
                },
                1024: {
                    items: 1
                },
                1440: {
                    items: 2
                },
                2560: {
                    items: 3
                }
            }
        });
    },
    bindCopy: function () {
        if ($(".btn-copy").length > 0) {
            var clipboard = new ClipboardJS('.btn-copy');
            clipboard.on('success', function (e) {
                toastr.success($("#CopiedText").val());
            });
        }
    },
    bindBtcOut: function () {
        $("#txt-btcOut").on("click", function () {
            $.ajax({
                url: "/Dashboard/WithdrawBTC/",
                type: "GET",
                beforeSend: function () {
                    //$("#txt-btcOut").attr("disabled", true);
                    //$("#txt-btcOut").html("<i class='fa fa-spinner fa-spin'></i> " + $("#txt-btcOut").text());
                },
                data: {
                },
                success: function (data) {
                    $("#btcOutView").html(data);
                },
                complete: function (data) {
                    $("#txt-btcOut").attr("disabled", false);
                    $("#txt-btcOut").html($("#txt-btcOut").text());
                }
            });
        })
    }
}

$(document).ready(function () {
    Dashboard.init();
});