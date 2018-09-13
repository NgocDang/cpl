var AdminGame = {
    init: function () {
        AdminGame.loadHoldingRevenuePercentage();
        AdminGame.loadHoldingTerminalPercentage();
    },
    loadHoldingRevenuePercentage: function () {
        $.ajax({
            url: '/Admin/GetDataRevenuePercentagePieChart',
            type: "POST",
            data: {},
            success: function (data) {
                if (data.success && (data.revenueLotteryGame > 0 || data.revenuePricePredictionGame > 0)) {
                    Highcharts.chart('holding-revenue-percentage-chart', {
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
                            pointFormat: '{series.name}: <b>{point.y:.1f} CPL</b>'
                        },
                        exporting: {
                            enabled: false
                        },
                        plotOptions: {
                            pie: {
                                allowPointSelect: false,
                                cursor: 'pointer',
                                dataLabels: {
                                    enabled: true,
                                    format: '<b>{point.percentage:.1f} %',
                                    distance: -50,
                                },
                                showInLegend: true
                            }
                        },
                        series: [{
                            name: $("#revenue").val(),
                            colorByPoint: true,
                            data: [{
                                name: $("#lottery").val(),
                                y: data.revenueLotteryGame,
                                sliced: false,
                                selected: false,
                                color: '#ff0000'
                            }, {
                                name: $("#pricePrediction").val(),
                                y: data.revenuePricePredictionGame,
                                color: '#0092cd'
                            }]
                        }]
                    });
                }
                else {
                    $("#holding-revenue-percentage-chart").addClass("d-none");
                    $("#holding-revenue-percentage-no-assets").removeClass("d-none");
                }
            }
        });
    },
    loadHoldingTerminalPercentage: function () {
        $.ajax({
            url: '/Admin/GetDataTeminalPercentagePieChart',
            type: "POST",
            data: {},
            success: function (data) {
                debugger;
                if (data.success && (data.pc > 0 || data.mobile > 0 || data.table > 0)) {
                    Highcharts.chart('holding-terminal-percentage-chart', {
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
                            pointFormat: '{series.name}: <b>{point.y:.1f}</b>'
                        },
                        exporting: {
                            enabled: false
                        },
                        plotOptions: {
                            pie: {
                                allowPointSelect: false,
                                cursor: 'pointer',
                                dataLabels: {
                                    enabled: true,
                                    format: '<b>{point.percentage:.1f} %',
                                    distance: -50,
                                },
                                showInLegend: true
                            }
                        },
                        series: [{
                            name: $("#terminal").val(),
                            colorByPoint: true,
                            data: [{
                                name: "PC",
                                y: data.pc,
                                sliced: false,
                                selected: false,
                                color: '#4267b2'
                            }, {
                                name: "Mobile",
                                y: data.mobile,
                                color: '#f7931a'
                            }, {
                                name: "Tablel",
                                y: data.table,
                                color: '#828384'
                            }]
                        }]
                    });
                }
                else {
                    $("#holding-terminal-percentage-chart").addClass("d-none");
                    $("#holding-terminal-percentage-no-assets").removeClass("d-none");
                }
            }
        });
    },
}
$(document).ready(function () {
    AdminGame.init();
});

