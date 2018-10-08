var TopAgencyAffiliateAdmin = {
    topAgencyAffiliateDataTable: null,
    tier2AffiliateDataTable: null,
    tier3AffiliateDataTable: null,
    init: function () {
        TopAgencyAffiliateAdmin.bindCopy();
        TopAgencyAffiliateAdmin.bindSwitchery();

        TopAgencyAffiliateAdmin.bindTopAgencyTab();
        TopAgencyAffiliateAdmin.bindTopAgencyTimeRangeChange();
        TopAgencyAffiliateAdmin.bindDoUpdateAgencyAffiliateRate();
        TopAgencyAffiliateAdmin.bindDoUpdateAgencyAffiliateSetting();
        TopAgencyAffiliateAdmin.bindConfirmPayment();
        TopAgencyAffiliateAdmin.bindDoPayment();

        TopAgencyAffiliateAdmin.bindTier2Tab();
        TopAgencyAffiliateAdmin.bindTier2TimeRangeChange();

        TopAgencyAffiliateAdmin.bindTier3Tab();
        TopAgencyAffiliateAdmin.bindTier3TimeRangeChange();
        
        

        var tab = $("#tab").val();
        if (tab === "")
            tab = "top-agency";
        $(".nav-tabs a[id='" + tab + "-nav-tab']").tab('show');
    },
    bindCopy: function () {
        if ($(".btn-copy").length > 0) {
            var clipboard = new ClipboardJS('.btn-copy');
            clipboard.on('success', function (e) {
                toastr.success($("#CopiedSuccessfully").val());
            });
        }
    },
    initAffiliateDataTable: function (tabPaneElement) {
        if (tabPaneElement.data().kindOfTier == 1)
            TopAgencyAffiliateAdmin.topAgencyAffiliateDataTable.on('responsive-display', function (e, datatable, row, showHide, update) {
                TopAgencyAffiliateAdmin.loadEditable(tabPaneElement);
            });
        else if (tabPaneElement.data().kindOfTier == 2)
            TopAgencyAffiliateAdmin.tier2AffiliateDataTable.on('responsive-display', function (e, datatable, row, showHide, update) {
                TopAgencyAffiliateAdmin.loadEditable(tabPaneElement);
            });
        else // (tabPaneElement.data().kindOfTier == 3)
            TopAgencyAffiliateAdmin.tier3AffiliateDataTable.on('responsive-display', function (e, datatable, row, showHide, update) {
                TopAgencyAffiliateAdmin.loadEditable(tabPaneElement);
            });

    },
    loadAffiliateDataTable: function (tabPaneElement) {
        return tabPaneElement.find(".dt-top-agency-affiliate").DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "ajax": {
                url: "/Admin/SearchTopAgencyAffiliate",
                type: 'POST',
                data: {
                    sysUserId: $("#SysUserId").val(),
                    kindOfTier: tabPaneElement.data().kindOfTier,
                    periodInDay: tabPaneElement.find("select.time-range").val()
                },
                complete: function (data) {
                    TopAgencyAffiliateAdmin.loadEditable(tabPaneElement);
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
                    "data": "TotalDirectIntroducedUsers",
                    "render": function (data, type, full, meta) {
                        return full.totalDirectIntroducedUsers;
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
    bindDoUpdateAgencyAffiliateSetting: function () {
        $("#form-top-agency-setting").on("click", ".switchery", function () {
            var _this = this;
            var _postData = {};
            var _data = [{ name: $(_this).prev().prop("name"), value: $(_this).prev().is(":checked") }];
            _data.forEach(function (element) {
                _postData[element['name']] = element['value'];
            });
            $.ajax({
                url: "/Admin/DoUpdatetopAgencySetting/",
                type: "POST",
                dataType: 'json',
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: { 'viewModel': _postData, 'agencyId': $("#AgencyId").val() },
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message, 'Success!');
                    } else {
                        toastr.error(data.message, 'Error!');
                    }
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                }
            });
        });
    },
    bindDoUpdateAgencyAffiliateRate: function () {
        $("#form-agency-affiliate-tier").on("click", "#btn-update", function () {
            var isFormValid = $("#form-agency-affiliate-tier")[0].checkValidity();
            $("#form-agency-affiliate-tier").addClass('was-validated');
            var _this = this;

            if (isFormValid) {
                var _postData = {};
                var _formData = $("#form-agency-affiliate-tier").serializeArray();
                _formData.forEach(function (element) {
                    _postData[element['name']] = parseInt(element['value']);
                });
                $.ajax({
                    url: "/Admin/DoUpdateRateOnAgencyAffiliate/",
                    type: "POST",
                    dataType: 'json',
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                        $(_this).html("<i class='fa fa-spinner fa-spin'> </i> " + $(_this).text());
                    },
                    data: {
                        'viewModel': _postData,
                         'agencyId': $("#AgencyId").val()
                    },
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message, 'Success!');
                        } else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $(_this).attr("disabled", false);
                        $(_this).html($(_this).text());
                        $("#form-agency-affiliate-tier").removeClass('was-validated');
                    }
                });
            }

            return false;
        });
    },
    bindConfirmPayment: function () {
        $("#form-agency-affiliate-tier").on("click", "#btn-payment", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/ConfirmPayment",
                type: "GET",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: {
                    sysUserId: $("#SysUserId").val()
                },
                success: function (data) {
                    $("#modal").html(data);
                    $("#view-payment").modal("show");
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                }
            });
            return false;
        });
    },
    bindDoPayment: function () {
        $("#modal").on("click", "#btn-confirm-payment", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/DoPayment",
                type: "POST",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: {
                    sysUserId : $("#SysUserId").val()
                },
                success: function (data) {
                    if (data.success) {
                        $("#view-payment").modal("hide");
                        toastr.success(data.message, "Success!");
                        $("#btn-payment").attr("disabled", true);
                    } else {
                        toastr.error(data.message, "Error!");
                    }
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                }
            });
            return false;
        });
    },
    bindTopAgencyTab: function () {
        $('a#top-agency-nav-tab').on('show.bs.tab', function (e) {
            if ($("#top-agency-nav .tab-detail").html().trim().length === 0) {
                TopAgencyAffiliateAdmin.loadStatistics($("#top-agency-nav"));
            }

            if ($("#top-agency-nav table tbody").length == 0) {
                TopAgencyAffiliateAdmin.topAgencyAffiliateDataTable = TopAgencyAffiliateAdmin.loadAffiliateDataTable($("#top-agency-nav"));
                TopAgencyAffiliateAdmin.initAffiliateDataTable($("#top-agency-nav"));
            }

        })
    },
    bindTier2Tab: function () {
        $('a#tier-2-nav-tab').on('show.bs.tab', function (e) {
            if ($("#tier-2-nav .tab-detail").html().trim().length === 0) {
                TopAgencyAffiliateAdmin.loadStatistics($("#tier-2-nav"));
            }

            if ($("#tier-2-nav table tbody").length == 0) {
                TopAgencyAffiliateAdmin.tier2AffiliateDataTable = TopAgencyAffiliateAdmin.loadAffiliateDataTable($("#tier-2-nav"));
                TopAgencyAffiliateAdmin.initAffiliateDataTable($("#tier-2-nav"));
            }
        })
    },
    bindTier3Tab: function () {
        $('a#tier-3-nav-tab').on('show.bs.tab', function (e) {
            if ($("#tier-3-nav .tab-detail").html().trim().length === 0) {
                TopAgencyAffiliateAdmin.loadStatistics($("#tier-3-nav"));
            }

            if ($("#tier-3-nav table tbody").length == 0) {
                TopAgencyAffiliateAdmin.tier3AffiliateDataTable = TopAgencyAffiliateAdmin.loadAffiliateDataTable($("#tier-3-nav"));
                TopAgencyAffiliateAdmin.initAffiliateDataTable($("#tier-3-nav"));
            }
        })
    },
    loadStatistics: function (tabPaneElement) {
        if (tabPaneElement.data().kindOfTier == 1) {
            $.ajax({
                url: "/Admin/GetTopAgencyStatistics/",
                type: "GET",
                beforeSend: function () {
                    tabPaneElement.find(".tab-detail").html("<div class='text-center py-5'><img src='/css/dashboard/plugins/img/loading.gif' class='img-fluid' /></div>");
                },
                data: {
                    sysUserId: $("#SysUserId").val(),
                    periodInDay: tabPaneElement.find("select.time-range").val(),
                },
                success: function (data) {
                    tabPaneElement.find(".tab-detail").html(data);
                    TopAgencyAffiliateAdmin.loadTopAgencyStatisticsChart(tabPaneElement);
                },
            });
        } else {
            $.ajax({
                url: "/Admin/GetNonTopAgencyStatistics/",
                type: "GET",
                beforeSend: function () {
                    tabPaneElement.find(".tab-detail").html("<div class='text-center py-5'><img src='/css/dashboard/plugins/img/loading.gif' class='img-fluid' /></div>");
                },
                data: {
                    sysUserId: $("#SysUserId").val(),
                    periodInDay: tabPaneElement.find("select.time-range").val(),
                    kindOfTier: tabPaneElement.data().kindOfTier
                },
                success: function (data) {
                    tabPaneElement.find(".tab-detail").html(data);
                    TopAgencyAffiliateAdmin.loadNonTopAgencyStatisticsChart(tabPaneElement);
                },
            });
        }
    },
    bindTopAgencyTimeRangeChange: function () {
        $("#top-agency-nav select.time-range").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                TopAgencyAffiliateAdmin.loadStatistics($("#top-agency-nav"));
                TopAgencyAffiliateAdmin.topAgencyAffiliateDataTable.destroy();
                TopAgencyAffiliateAdmin.topAgencyAffiliateDataTable = TopAgencyAffiliateAdmin.loadAffiliateDataTable($("#top-agency-nav"));
        });
    },
    bindTier2TimeRangeChange: function () {
        $("#tier-2-nav select.time-range").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                TopAgencyAffiliateAdmin.loadStatistics($("#tier-2-nav"));
                TopAgencyAffiliateAdmin.tier2AffiliateDataTable.destroy();
                TopAgencyAffiliateAdmin.tier2AffiliateDataTable = TopAgencyAffiliateAdmin.loadAffiliateDataTable($("#tier-2-nav"));
            });
    },
    bindTier3TimeRangeChange: function () {
        $("#tier-3-nav select.time-range").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                TopAgencyAffiliateAdmin.loadStatistics($("#tier-3-nav"));
                TopAgencyAffiliateAdmin.tier3AffiliateDataTable.destroy();
                TopAgencyAffiliateAdmin.tier3AffiliateDataTable = TopAgencyAffiliateAdmin.loadAffiliateDataTable($("#tier-3-nav"));
            });
    },
    loadTopAgencyStatisticsChart: function (tabPaneElement) {
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

        var totalAffiliateSale = { data: [], name: tabPaneElement.find(".total-affiliate-sale").val(), color: '#4267b2' };
        var directAffiliateSale = { data: [], name: tabPaneElement.find(".direct-affiliate-sale").val(), color: '#f7931a' };
        var totalIntroducedUsers = { data: [], name: tabPaneElement.find(".total-introduced-users").val(), color: '#828384' };
        var directIntroducedUsers = { data: [], name: tabPaneElement.find(".direct-introduced-users").val(), color: '#F69BF9' };

        var directAffiliateSaleChanges = JSON.parse(tabPaneElement.find(".direct-affiliate-sale-changes").val());
        if (directAffiliateSaleChanges.length !== 0) {
            $.each(directAffiliateSaleChanges, function (index, value) {
                now = moment(value.Date).valueOf();
                val = value.Value;
                directAffiliateSale.data.push([now, val]);
            });
        }
        else {
            now = moment().valueOf();
            val = 0;
            directAffiliateSale.data.push([now, val]);
        }
        directAffiliateSale.data.sort();

        var totalAffiliateSaleChanges = JSON.parse(tabPaneElement.find(".total-affiliate-sale-changes").val());
        if (totalAffiliateSaleChanges.length !== 0) {
            $.each(totalAffiliateSaleChanges, function (index, value) {
                now = moment(value.Date).valueOf();
                val = value.Value;
                totalAffiliateSale.data.push([now, val]);
            });
        }
        else {
            now = moment().valueOf();
            val = 0;
            totalAffiliateSale.data.push([now, val]);
        }
        totalAffiliateSale.data.sort();

        var totalIntroducedUsersChanges = JSON.parse(tabPaneElement.find(".total-introduced-users-changes").val());
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

        var directIntroducedUsersChanges = JSON.parse(tabPaneElement.find(".direct-introduced-users-changes").val());
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
        options.series.push(totalAffiliateSale, directAffiliateSale, totalIntroducedUsers, directIntroducedUsers);

        // Create the plot
        tabPaneElement.find(".statistic-chart").highcharts(options);
    },
    loadNonTopAgencyStatisticsChart: function (tabPaneElement) {
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

        var totalAffiliateSale = { data: [], name: tabPaneElement.find(".total-affiliate-sale").val(), color: '#4267b2' };

        var totalAffiliateSaleChanges = JSON.parse(tabPaneElement.find(".total-affiliate-sale-changes").val());
        if (totalAffiliateSaleChanges.length !== 0) {
            $.each(totalAffiliateSaleChanges, function (index, value) {
                now = moment(value.Date).valueOf();
                val = value.Value;
                totalAffiliateSale.data.push([now, val]);
            });
        }
        else {
            now = moment().valueOf();
            val = 0;
            totalAffiliateSale.data.push([now, val]);
        }
        totalAffiliateSale.data.sort();

        // Push the completed series
        options.series.push(totalAffiliateSale);

        // Create the plot
        tabPaneElement.find(".statistic-chart").highcharts(options);
    },
    loadEditable: function (jQueryElement) {
        $.fn.editable.defaults.clear = false;
        $.fn.editable.defaults.mode = 'popup';
        $.fn.editable.defaults.placement = 'top';
        $.fn.editable.defaults.type = 'number';
        $.fn.editable.defaults.step = '1.00';
        $.fn.editable.defaults.min = '0.00';
        $.fn.editable.defaults.max = '100.00';
        jQueryElement.find(".dt-top-agency-affiliate tr").each(function (index, element) {
            TopAgencyAffiliateAdmin.loadEditableOnRow(element);
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
                    url: '../DoUpdateRateOnStandardAffiliate',
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
    TopAgencyAffiliateAdmin.init();
});