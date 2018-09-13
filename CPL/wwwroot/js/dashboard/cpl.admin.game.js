var AdminGameManagement = {
    init: function () {
        AdminGameManagement.loadBetStatistic();
        AdminGameManagement.bindSelectTimeRange();
    },
    bindSelectTimeRange: function () {
        $("#Category").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                AdminGameManagement.loadBetStatistic(this.value);
                $("#GameSummaryStatistic").load("/ViewComponent/GetGameSummaryStatisticViewComponent?periodInDay=" + this.value);
            });
    },
    loadBetStatistic: function (period) {
        $.ajax({
            url: '/Admin/GetDataGameSummaryStatisticChart',
            type: "POST",
            data: {
                periodInDay: period
            },
            chartData: {},
            success: function (chartData) {
                if (chartData.success) {
                    var a = chartData.message;

                    Highcharts.setOptions({
                        global: {
                            useUTC: false
                        }
                    });
                    if (parseInt($("#LangId").val()) == 1) {
                        Highcharts.setOptions({
                            lang: {
                                months: [
                                    'January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'
                                ],
                                weekdays: [
                                    'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'
                                ],
                                shortMonths: [
                                    "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
                                ]
                            }
                        });
                    }
                    else if (parseInt($("#LangId").val()) == 2) {
                        Highcharts.setOptions({
                            lang: {
                                months: [
                                    '一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'
                                ],
                                weekdays: [
                                    '月曜日', '火曜日', '水曜日', '木曜日',
                                    '金曜日', '土曜日', '日曜日'
                                ],
                                shortMonths: [
                                    '一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'
                                ]
                            }
                        });
                    }
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
                        },
                        yAxis: {
                            title: {
                                text: ''
                            },
                        },
                        tooltip: {
                            headerFormat: '<b>{series.name}</b><br>',
                            pointFormat: '{point.x:%e. %b}: {point.y}'
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

                    var revenue = { data: [], name: parseInt($("#LangId").val()) == 1 ? 'Total revenue' : '総収入', color: '#4267b2' };
                    var sale = { data: [], name: parseInt($("#LangId").val()) == 1 ? 'Total sale' : 'トータルセール', color: '#f7931a' };
                    var pageView = { data: [], name: parseInt($("#LangId").val()) == 1 ? 'Page view' : 'ページビュー', color: '#828384' };
                    var totalPlayers = { data: [], name: parseInt($("#LangId").val()) == 1 ? 'Total players' : '総プレーヤー', color: '#F69BF9' };

                    if (JSON.parse(a).TotalSaleChanges.length != 0) {
                        $.each(JSON.parse(a).TotalSaleChanges, function (index, value) {
                            now = moment(value.Date).valueOf();
                            val = value.Value;
                            sale.data.push([now, val]);
                        });
                    }
                    else {
                        now = moment().valueOf();
                        val = 0;
                        sale.data.push([now, val]);
                    }
                    sale.data.sort();

                    if (JSON.parse(a).TotalRevenueChanges.length != 0) {
                        $.each(JSON.parse(a).TotalRevenueChanges, function (index, value) {
                            now = moment(value.Date).valueOf();
                            val = value.Value;
                            revenue.data.push([now, val]);
                        });
                    }
                    else {
                        now = moment().valueOf();
                        val = 0;
                        revenue.data.push([now, val]);
                    }
                    revenue.data.sort();

                    if (JSON.parse(a).PageViewChanges.length != 0) {
                        $.each(JSON.parse(a).PageViewChanges, function (index, value) {
                            now = moment(value.Date).valueOf();
                            val = value.Count;
                            pageView.data.push([now, val]);
                        });
                    }
                    else {
                        now = moment().valueOf();
                        val = 0;
                        pageView.data.push([now, val]);
                    }
                    pageView.data.sort();

                    if (JSON.parse(a).TotalPlayersChanges.length != 0) {
                        $.each(JSON.parse(a).TotalPlayersChanges, function (index, value) {
                            now = moment(value.Date).valueOf();
                            val = value.Value;
                            totalPlayers.data.push([now, val]);
                        });
                    }
                    else {
                        now = moment().valueOf();
                        val = 0;
                        totalPlayers.data.push([now, val]);
                    }
                    totalPlayers.data.sort();

                    // Push the completed series
                    options.series.push(revenue, sale, pageView, totalPlayers);

                    // Create the plot
                    new Highcharts.Chart("statistic-chart", options);
                }
            }
        });
    },
}

$(document).ready(function () {
    AdminGameManagement.init();
});