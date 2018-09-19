var TopAgencyAffiliate = {
    TopAgencyAffiliateDataTable: null,
    init: function () {
        TopAgencyAffiliate.bindSwitchery();
        TopAgencyAffiliate.bindDoUpdateAgencyAffiliateRate();
        TopAgencyAffiliate.bindDoUpdateTopAgencySetting();
        TopAgencyAffiliate.bindViewPayment();
    },
    bindSwitchery: function () {
        $.each($(".checkbox-switch"), function (index, element) {
            var switches = new Switchery(element, { size: 'small' });
        });
    },
    bindDoUpdateTopAgencySetting: function () {
        $("#form-top-agency-setting").on("click", ".switchery", function () {
            var _this = this;
            var _postData = {};
            var _data = [{ name: $(_this).prev().prop("name"), value: $(_this).prev().is(":checked") }];
            debugger;
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
                debugger;
                $.ajax({
                    url: "/Admin/DoUpdateCommisionTopAgencyAffiliateRate/",
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
    bindViewPayment: function () {
        $("#form-agency-affiliate-tier").on("click", "#btn-payment", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/ViewPayment",
                type: "GET",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: {
                    sysUserId: $("#SysUserId").val()
                },
                success: function (data) {
                    $("#modal").html(data);
                    $("#view-lottery").modal("show");
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                }
            });
            return false;
        });
    },
    bindDoPayment: function () {
        $("#modal").on("click", "#btn-confirm", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/ViewPayment",
                type: "GET",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: {
                    //id: $("#SysUserId").val();
                    //id: $("#SysUserId").val();
                    //id: $("#SysUserId").val();
                },
                success: function (data) {
                    $("#modal").html(data);
                    $("#view-lottery").modal("show");
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                }
            });
            return false;
        });
    },

};

$(document).ready(function () {
    TopAgencyAffiliate.init();
});