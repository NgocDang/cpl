﻿var PieChart = {
    init: function () {
    },
    loadPercentageAjax: function (container, emptyContainer, url) {
        $.ajax({
            url: url, 
            type: "GET",
            beforeSend: function () {
                container.html("<div class='text-center py-5'><img src='/css/dashboard/plugins/img/loading.gif' class='img-fluid' /></div>");
            },
            data: {},
            success: function (response) {
                if (response.success) {
                    PieChart.loadPercentage(response.data, container, emptyContainer, response.seriesName);
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
            var seriesArray = [];
            for (var i = 0; i < jsonObj.length; i++) {
                var point = { name: jsonObj[i].Label, y: jsonObj[i].Value, color: jsonObj[i].Color };
                seriesArray.push(point);
            }
            container.highcharts({
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
    PieChart.init();
});