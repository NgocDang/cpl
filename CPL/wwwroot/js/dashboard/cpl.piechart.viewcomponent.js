var PieChartViewComponent = {
    init: function () {
    },
    loadPercentageAjax: function (container, emptyContainer, url, seriesName) {
        $.ajax({
            url: url, //GetDeviceCategoryData
            type: "POST",
            data: {},
            success: function (data) {
                if (data.success) {
                    PieChartViewComponent.loadPercentage(data, container, emptyContainer, seriesName);
                }
            }
        });
    },
    loadPercentage: function (data, container, emptyContainer, seriesName) {
        var jsonObj = JSON.parse(data);
        var isEmpty = true;

        for (var i = 0; i < jsonObj.length; i++)
        {
            if (jsonObj[i].Value != 0) {
                isEmpty = false;
                break;
            }
        }
        if (isEmpty) {
            container.addClass("d-none");
            emptyContainer.removeClass("d-none");
        } else {
            var seriesArray = Array();
            for (var i = 0; i < jsonObj.length; i++) {
                var point = { name: jsonObj[i].Label, y: jsonObj[i].Value, color: jsonObj[i].Color };
                seriesArray.push(point);
            }
            Highcharts.chart(container.attr('id'),
                {
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
                            showInLegend: true,
                            colorByPoint: true,
                        }
                    },
                    series: [{
                        name: seriesName,
                        data: seriesArray
                    }]
                });
        }
    }
}
$(document).ready(function () {
    PieChartViewComponent.init();
});