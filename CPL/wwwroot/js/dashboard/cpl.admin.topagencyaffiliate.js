var TopAgencyAffiliate = {
    TopAgencyAffiliateDataTable: null,
    init: function () {
        TopAgencyAffiliate.TopAgencyAffiliateDataTable = TopAgencyAffiliate.loadTopAgencyAffiliateDataTable();
        TopAgencyAffiliate.bindSwitchery();
        TopAgencyAffiliate.bindDoUpdateAgencyAffiliateRate();
        TopAgencyAffiliate.bindDoUpdateAgencyAffiliateSetting();
        TopAgencyAffiliate.bindConfirmPayment();
        TopAgencyAffiliate.bindDoPayment();

        TopAgencyAffiliate.bindTopAgencyTab();
        TopAgencyAffiliate.bindTopAgencyTimeRangeChange();

        var tab = $("#tab").val();
        if (tab === "")
            tab = "top-agency";
        $(".nav-tabs a[id='" + tab + "-nav-tab']").tab('show');
    },
    loadTopAgencyAffiliateDataTable: function () {
        return $('#dt-top-agency-affiliate').DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "ajax": {
                url: "/Admin/SearchTopAgencyAffiliate",
                type: 'POST',
                data: {
                    sysUserId: $("#SysUserId").val()
                },
                complete: function (data) {
                    var table = TopAgencyAffiliate.TopAgencyAffiliateDataTable;
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
                    url: "/Admin/DoUpdateAgencyAffiliateRate/",
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
                TopAgencyAffiliate.loadTier1Statistics();
            }
        })
    },
    loadTier1Statistics: function () {
        $.ajax({
            url: "/Admin/GetTopAgencyStatistics/",
            type: "GET",
            beforeSend: function () {
                $("#top-agency-nav .tab-detail").html("<div class='text-center py-5'><img src='/css/dashboard/plugins/img/loading.gif' class='img-fluid' /></div>");
            },
            data: {
                sysUserId: $("#SysUserId").val(),
                periodInDay: $("#top-agency-nav select.time-range").val()
            },
            success: function (data) {
                $("#top-agency-nav .tab-detail").html(data);
                //TopAgencyAffiliate.loadTier1StatisticsChart($("#top-agency-nav"))
            },
        });
    },
    bindTopAgencyTimeRangeChange: function () {
        $("#top-agency-nav select.time-range").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                TopAgencyAffiliate.loadTier1Statistics();
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
};

$(document).ready(function () {
    TopAgencyAffiliate.init();
});