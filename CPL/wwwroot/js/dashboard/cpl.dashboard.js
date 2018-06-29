var Dashboard = {
    init: function () {
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
            autoplayTimeout: 1000,
            autoplayHoverPause: true
        })

        Highcharts.chart('line-chart', {
            chart: {
                type: 'spline'
            },
            title: {
                //text: 'Snow depth at Vikjafjellet, Norway'
            },
            subtitle: {
                //text: 'Irregular time data in Highcharts JS'
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
                min: 0
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

            colors: ['#6CF', '#39F', '#06C', '#036', '#000'],

            // Define the data points. All series have a dummy year
            // of 1970/71 in order to be compared on the same x axis. Note
            // that in JavaScript, months start at 0 for January, 1 for February etc.
            series: [{
                name: "Asset Changes",
                data: [
                    [Date.UTC(1970, 10, 25), 0],
                    [Date.UTC(1970, 11, 6), 0.25],
                    [Date.UTC(1970, 11, 20), 1.41],
                    [Date.UTC(1970, 11, 25), 1.64]
                ],
                color: '#4267b2'
            }, {
                name: "Monthly Deposits",
                data: [
                    [Date.UTC(1970, 10, 9), 0],
                    [Date.UTC(1970, 10, 15), 0.23],
                    [Date.UTC(1970, 10, 20), 0.25],
                    [Date.UTC(1970, 10, 25), 0.23],
                    [Date.UTC(1970, 10, 30), 0.39]
                ],
                color: '#0000fd'
            }, {
                name: "Bonus",
                data: [
                    [Date.UTC(1970, 9, 15), 0],
                    [Date.UTC(1970, 9, 31), 0.09],
                    [Date.UTC(1970, 10, 7), 0.17],
                    [Date.UTC(1970, 10, 10), 0.1],
                    [Date.UTC(1970, 11, 10), 0.1]
                ],
                color: '#1eaf1e'
            }]
        });

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
                    y: 61.41,
                    sliced: true,
                    selected: true,
                    color: '#4267b2'
                }, {
                    name: 'BTC',
                    y: 11.84,
                    color: '#f7931a'
                }, {
                    name: 'ETH',
                    y: 10.85,
                    color: '#828384'
                }]
            }]
        });

        $('#dt-history').DataTable({
            "pagingType": "full_numbers",
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            responsive: true,
            language: {
                search: "_INPUT_",
                searchPlaceholder: "Search records",
                oPaginate: {
                    sFirst: "<<",
                    sLast: ">>",
                    sNext: ">",
                    sPrevious: "<"
                },
            },
        });

        $("#btn-wallet").on("click", function () {
            $("#wallet-view").show();
            $("#deposite-withdrawal-view").hide();
            $("#pie-chart-card").show();
            $("#line-chart-card").hide();
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
}

$(document).ready(function () {
    Dashboard.init();
});