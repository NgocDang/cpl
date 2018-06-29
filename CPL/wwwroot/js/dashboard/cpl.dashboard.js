var Dashboard = {
    init: function () {
        $('#dt-history').DataTable();

        $('.owl-carousel').owlCarousel({
            loop: true,
            margin: 15,
            nav: false,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 1,
                    nav: false
                },
                600: {
                    items: 1,
                    nav: false
                },
                1000: {
                    items: 3,
                    nav: false,
                    loop: true
                }
            },
            autoplay: true,
            autoplayTimeout: 3000,
            autoplayHoverPause: true
        })

        $("#btn-depo-withdr").on("click", function () {
            $("#wallet-view").hide();
            $("#deposite-withdrawal-view").show();

            $.ajax({
                url: "/Dashboard/DepositeAndWithdrawal/",
                type: "GET",
                beforeSend: function () {
                    $("#btn-depo-withdr").attr("disabled", true);
                    $("#btn-depo-withdr").html("<i class='fa fa-spinner fa-spin'></i> " + $("#btn-depo-withdr").text());
                },
                data: {
                },
                success: function (data) {
                    $("#deposite-withdrawal-view").html(data);
                    Dashboard.bindCopy();
                    Dashboard.bindBtcOut();
                },
                complete: function (data) {
                    $("#btn-depo-withdr").attr("disabled", false);
                    $("#btn-depo-withdr").html($("#btn-depo-withdr").text());
                }
            });
        })

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
                            text: 'Holding Percentage'
                        },
                        tooltip: {
                            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
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
        })

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
                            text: 'Snow depth at Vikjafjellet, Norway'
                        },
                        subtitle: {
                            text: 'Irregular time data in Highcharts JS'
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
                                text: 'Snow depth (m)'
                            },
                        },
                        tooltip: {
                            headerFormat: '<b>{series.name}</b><br>',
                            pointFormat: '{point.x:%e. %b}: {point.y:.2f} m'
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

                    var asset = { data: [], name: 'Asset Changes', color: '#28b3fe' };
                    var invest = { data: [], name: 'Monthly Invests', color: '#a858fe' };
                    var prize = { data: [], name: 'Prizes', color: '#60e1e3' };

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
        })

        return false;
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
    },

    requestPieData: function () {
        $.ajax({
            url: '/Dashboard/GetDataPieChart',
            type: "POST",
            data: {},
            success: function (data) {
                if (data.success) {
                    var a = data.message;
                    Highcharts.chart('pie-chart', {
                        chart: {
                            plotBackgroundColor: null,
                            plotBorderWidth: null,
                            plotShadow: false,
                            type: 'pie'
                        },
                        title: {
                            text: 'Holding Percentage'
                        },
                        tooltip: {
                            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
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
                                color: '#f96332'
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

    requestLineData: function () {
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
                            text: 'Snow depth at Vikjafjellet, Norway'
                        },
                        subtitle: {
                            text: 'Irregular time data in Highcharts JS'
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
                                text: 'Snow depth (m)'
                            },
                        },
                        tooltip: {
                            headerFormat: '<b>{series.name}</b><br>',
                            pointFormat: '{point.x:%e. %b}: {point.y:.2f} m'
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

                    var asset = { data: [], name: 'Asset Changes', color: '#f96332' };
                    var invest = { data: [], name: 'Monthly Invests', color: '#0000fd' };
                    var prize = { data: [], name: 'Prizes', color: '#1eaf1e' };

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
                    new Highcharts.Chart("line-chart-card", options);
                }
            }
        });
    },
}

$(document).ready(function () {
    Dashboard.init();
});