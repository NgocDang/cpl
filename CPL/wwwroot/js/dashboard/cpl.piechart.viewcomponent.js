var PieChartViewComponent = {
    init: function () {
        PieChartViewComponent.loadPercentage($("#ChartData"), $("#pie-chart"));
    },
    //loadPercentageAjax: function (element, emptyElement, url) {
    //    $.ajax({
    //        url: url, //GetDeviceCategoryData
    //        type: "POST",
    //        data: {},
    //        success: function (data) {
    //            if (data.success) {
    //                PieChartViewComponent.loadPercentage(data);
    //            }
    //        }
    //    });
    //},
    loadPercentage: function (data, element) {
        options = {
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
                pointFormat: '{series.name}: <b>{point.y}</b>'
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
                    showInLegend: true,
                    colorByPoint: true,
                }
            },
            series: [{
                data: []
            }]
        };
        for (var chartData in data) {
            var point = { name: chartData.Label, y: chartData.Value, color: chartData.Color };
            options.series.data.push(point);
        }
        // Create the plot
        new Highcharts.Chart(element, options);
    }
}

$(document).ready(function () {
    PieChartViewComponent.init();
});