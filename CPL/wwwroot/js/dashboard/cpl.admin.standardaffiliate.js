var StandardAffiliate = {
    StandardAffiliateDataTable: null,
    init: function () {
        StandardAffiliate.bindSwitchery();
        StandardAffiliate.bindTier1Tab();
        StandardAffiliate.bindTier1TimeRangeChange();

        var tab = $("#tab").val();
        if (tab === "")
            tab = "tier-1";
        $(".nav-tabs a[id='" + tab + "-nav-tab']").tab('show');
    },
    initStandardAffiliateIntroducedUsersDataTable: function (parentElement) {
        StandardAffiliate.StandardAffiliateDataTable.on('responsive-display', function (e, datatable, row, showHide, update) {
            StandardAffiliate.loadEditable(parentElement.id);
        });
    },
    loadStandardAffiliateIntroducedUsersDataTable: function (parentElement) {
        return $(".dt-standard-affiliate").DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "ajax": {
                url: "/Admin/SearchStandardAffiliateIntroducedUsers",
                type: 'POST',
                data: {
                    sysUserId: $("#SysUserId").val(),
                    kindOfTier: StandardAffiliate.StandardAffiliateDataTable == null ? parentElement.data().kindOfTier : $("#" + StandardAffiliate.StandardAffiliateDataTable.data().node().id).closest(".tab-pane").data().kindOfTier,
                    periodInDay: StandardAffiliate.StandardAffiliateDataTable == null ? parentElement.find("select.time-range").val() : $("#" + StandardAffiliate.StandardAffiliateDataTable.data().node().id).closest(".tab-pane").find("select.time-range").val()
                },
                complete: function (data) {
                    StandardAffiliate.loadEditable("#" + parentElement.id);
                }
            },
            'order': [[0, 'asc']],
            "language": DTLang.getLang(),
            "columns": [
                {
                    "data": "KindOfTier",
                    "render": function (data, type, full, meta) {
                        return full.kindOfTier;
                    }
                },
                {
                    "data": "UsedCPL",
                    "render": function (data, type, full, meta) {
                        return full.usedCPL;
                    }
                },
                {
                    "data": "LostCPL",
                    "render": function (data, type, full, meta) {
                        return full.lostCPL;
                    }
                },
                {
                    "data": "AffiliateSale",
                    "render": function (data, type, full, meta) {
                        return full.affiliateSale;
                    }
                },
                {
                    "data": "TotalIntroducedUsers",
                    "render": function (data, type, full, meta) {
                        return full.totalIntroducedUsers;
                    }
                },
                {
                    "data": "AffiliateCreatedDate",
                    "render": function (data, type, full, meta) {
                        return full.affiliateCreatedDateInString;
                    }
                },
                {
                    "data": "Tier1DirectRate",
                    "render": function (data, type, full, meta) {
                        if (full.isLocked === false)
                            return '<a class="editable editable-unlocked" id="' + full.id + '" data-value="' + full.tier1DirectRate + '" data-name="Tier1DirectRate" data-pk=' + full.affiliateId + ' href="#">' + full.tier1DirectRate + '</a>';
                        else
                            return '<a class="editable editable-locked" id="' + full.id + '" data-value="' + full.tier1DirectRate + '" data-name="Tier1DirectRate" data-pk=' + full.affiliateId + '>' + full.tier1DirectRate + '</a>';

                    },
                    "orderable": false
                },
                {
                    "data": "Tier2SaleToTier1Rate",
                    "render": function (data, type, full, meta) {
                        if (full.isLocked === false)
                            return '<a class="editable editable-unlocked" id="' + full.id + '" data-value="' + full.tier2SaleToTier1Rate + '" data-name="Tier2SaleToTier1Rate" data-pk=' + full.affiliateId + ' href="#">' + full.tier2SaleToTier1Rate + '</a>';
                        else
                            return '<a class="editable editable-locked" id="' + full.id + '" data-value="' + full.tier2SaleToTier1Rate + '" data-name="Tier2SaleToTier1Rate" data-pk=' + full.affiliateId + '>' + full.tier2SaleToTier1Rate + '</a>';
                    },
                    "orderable": false
                },
                {
                    "data": "Tier3SaleToTier1Rate",
                    "render": function (data, type, full, meta) {
                        if (full.isLocked === false)
                            return '<a class="editable editable-unlocked" id="' + full.id + '" data-value="' + full.tier3SaleToTier1Rate + '" data-name="Tier3SaleToTier1Rate" data-pk=' + full.affiliateId + ' href="#">' + full.tier3SaleToTier1Rate + '</a>';
                        else
                            return '<a class="editable editable-locked" id="' + full.id + '" data-value="' + full.tier3SaleToTier1Rate + '" data-name="Tier3SaleToTier1Rate" data-pk=' + full.affiliateId + '>' + full.tier3SaleToTier1Rate + '</a>';
                    },
                    "orderable": false
                },
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        var html = "<a style='margin:2px' href='/Admin/User/" + full.id + "' target='_blank'  data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary'>" + $("#View").val() + "</a>";
                        return html;

                    },
                    "orderable": false
                }
            ]
        });

    },
    bindSwitchery: function () {
        $.each($(".checkbox-switch"), function (index, element) {
            var switches = new Switchery(element, { size: 'small' });
        });
    },
    bindTier1Tab: function () {
        $('a#tier-1-nav-tab').on('show.bs.tab', function (e) {
            if ($("#tier-1-nav .tab-detail").html().trim().length === 0) {
                StandardAffiliate.loadTier1Statistics();
            }
            if ($("#tier-1-nav table tbody").length == 0) {
                StandardAffiliate.StandardAffiliateDataTable = StandardAffiliate.loadStandardAffiliateIntroducedUsersDataTable($("#tier-1-nav"));
                StandardAffiliate.initStandardAffiliateIntroducedUsersDataTable("#tier-1-nav");
            }
        });
    },
    loadTier1Statistics: function () {
        $.ajax({
            url: "/Admin/GetStandardAffiliateStatistics/",
            type: "GET",
            beforeSend: function () {
                $("#tier-1-nav .tab-detail").html("<div class='text-center py-5'><img src='/css/dashboard/plugins/img/loading.gif' class='img-fluid' /></div>");
            },
            data: {
                sysUserId: $("#SysUserId").val(),
                periodInDay: $("#tier-1-nav select.time-range").val()
            },
            success: function (data) {
                $("#tier-1-nav .tab-detail").html(data);
                //TopAgencyAffiliate.loadTier1StatisticsChart($("#tier-1-nav"))
            },
        });
    },
    bindTier1TimeRangeChange: function () {
        $("#tier-1-nav select.time-range").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                StandardAffiliate.loadTier1Statistics();
                StandardAffiliate.StandardAffiliateDataTable.ajax.reload();
                StandardAffiliate.initStandardAffiliateIntroducedUsersDataTable("#tier-1-nav");
            });
    },
    loadTier1StatisticsChart: function (container) {
        Highcharts.setOptions({
            global: {
                useUTC: false
            }
        });

        Highcharts.setOptions({
            lang: DTLang.getHighChartLang()
        });
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
                    day: '%b/%e',
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

        var totalSale = { data: [], name: container.find(".total-sale").val(), color: '#4267b2' };
        var directSale = { data: [], name: container.find(".direct-sale").val(), color: '#f7931a' };
        var totalIntroducedUsers = { data: [], name: container.find(".total-introduced-users").val(), color: '#828384' };
        var directIntroducedUsers = { data: [], name: container.find(".direct-introduced-users").val(), color: '#F69BF9' };

        var directSaleChanges = JSON.parse(container.find(".direct-sale-changes").val());
        if (directSaleChanges.length !== 0) {
            $.each(directSaleChanges, function (index, value) {
                now = moment(value.Date).valueOf();
                val = value.Value;
                directSale.data.push([now, val]);
            });
        }
        else {
            now = moment().valueOf();
            val = 0;
            directSale.data.push([now, val]);
        }
        directSale.data.sort();

        var totalSaleChanges = JSON.parse(container.find(".total-sale-changes").val());
        if (totalSaleChanges.length !== 0) {
            $.each(totalSaleChanges, function (index, value) {
                now = moment(value.Date).valueOf();
                val = value.Value;
                totalSale.data.push([now, val]);
            });
        }
        else {
            now = moment().valueOf();
            val = 0;
            totalSale.data.push([now, val]);
        }
        totalSale.data.sort();

        var totalIntroducedUsersChanges = JSON.parse(container.find(".total-introduced-users-changes").val());
        if (totalIntroducedUsersChanges.length !== 0) {
            $.each(totalIntroducedUsersChanges, function (index, value) {
                now = moment(value.Date).valueOf();
                val = value.Value;
                totalIntroducedUsers.data.push([now, val]);
            });
        }
        else {
            now = moment().valueOf();
            val = 0;
            totalIntroducedUsers.data.push([now, val]);
        }
        totalIntroducedUsers.data.sort();

        var directIntroducedUsersChanges = JSON.parse(container.find(".direct-introduced-users-changes").val());
        if (directIntroducedUsersChanges.length != 0) {
            $.each(directIntroducedUsersChanges, function (index, value) {
                now = moment(value.Date).valueOf();
                val = value.Value;
                directIntroducedUsers.data.push([now, val]);
            });
        }
        else {
            now = moment().valueOf();
            val = 0;
            directIntroducedUsers.data.push([now, val]);
        }
        directIntroducedUsers.data.sort();

        // Push the completed series
        options.series.push(totalSale, directSale, totalIntroducedUsers, directIntroducedUsers);

        // Create the plot
        container.find(".statistic-chart").highcharts(options);
    },
    loadEditable: function (parentElement) {
        $.fn.editable.defaults.clear = false;
        $.fn.editable.defaults.mode = 'popup';
        $.fn.editable.defaults.placement = 'top';
        $.fn.editable.defaults.type = 'number';
        $.fn.editable.defaults.step = '1.00';
        $.fn.editable.defaults.min = '0.00';
        $.fn.editable.defaults.max = '100.00';
        $(parentElement + " .dt-standard-affiliate tr").each(function (index, element) {
            StandardAffiliate.loadEditableOnRow(element);
        });
    },
    loadEditableOnRow: function (element) {
        $(element).find('a.editable').editable({
            url: function (params) {
                return $.ajax({
                    cache: false,
                    async: true,
                    type: 'POST',
                    data: { affiliateId: params.pk, value: params.value, name: params.name },
                    url: '../DoUpdateStandardAffiliateRate',
                    success: function (data) {
                        if (data.success)
                            toastr.success(data.message, 'Success!');
                        else
                            toastr.error(data.message, 'Error!');
                    },
                    error: function (data) {
                        toastr.error(data.message, 'Error!');
                    }
                });
            },
        });
        $(element).find('a.editable-locked').editable('toggleDisabled');
    },
};

$(document).ready(function () {
    StandardAffiliate.init();
});