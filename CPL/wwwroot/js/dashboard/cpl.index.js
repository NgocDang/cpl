var Index = {
    init: function () {
        Highcharts.chart('chart', {
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
                ]
            }, {
                name: "Monthly Deposits",
                data: [
                    [Date.UTC(1970, 10, 9), 0],
                    [Date.UTC(1970, 10, 15), 0.23],
                    [Date.UTC(1970, 10, 20), 0.25],
                    [Date.UTC(1970, 10, 25), 0.23],
                    [Date.UTC(1970, 10, 30), 0.39]
                ]
            }, {
                name: "Bonus",
                data: [
                    [Date.UTC(1970, 9, 15), 0],
                    [Date.UTC(1970, 9, 31), 0.09],
                    [Date.UTC(1970, 10, 7), 0.17],
                    [Date.UTC(1970, 10, 10), 0.1],
                    [Date.UTC(1970, 11, 10), 0.1]
                ]
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
    }
}

$(document).ready(function () {
    Index.init();
});