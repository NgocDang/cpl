var Setting = {
    init: function () {
        Setting.bindSwitchery();
        Setting.bindDoUpdateKYCVerificationActivated();
        Setting.bindDoUpdateStandardAffiliateRate();
        Setting.bindDoUpdateAgencyAffiliateRate();
        Setting.bindDoUpdateCookieExpirations();
        Setting.bindDoUpdateAccountActivationEnable();
    },
    bindSwitchery: function () {
        $.each($(".checkbox-switch"), function (index, element) {
            var switches = new Switchery(element, { size: 'small' });
        });
    },
    bindDoUpdateKYCVerificationActivated: function () {
        $("#affiliate-program").on("click", ".switchery", function () {
            var _this = this;
            var _data = JSON.stringify([{ name: $(_this).prev().prop("name"), value: $(_this).prev().is(":checked") }]);
            $.ajax({
                url: "/Admin/DoUpdateSetting/",
                type: "POST",
                dataType: 'json',
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: { 'data': _data },
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
    bindDoUpdateStandardAffiliateRate: function () {
        $("#form-standard-affiliate-tier").on("click", ".btn-update", function () {
            var isFormValid = $("#form-standard-affiliate-tier")[0].checkValidity();
            $("#form-standard-affiliate-tier").addClass('was-validated');
            var _this = this;

            if (isFormValid) {
                var _data = JSON.stringify($("#form-standard-affiliate-tier").serializeArray());
                $.ajax({
                    url: "/Admin/DoUpdateSetting/",
                    type: "POST",
                    dataType: 'json',
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                        $(_this).html("<i class='fa fa-spinner fa-spin'> </i> " + $(_this).text());
                    },
                    data: { 'data': _data },
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
                        $("#form-standard-affiliate-tier").removeClass('was-validated');
                    }
                });
            }

            return false;
        });
    },
    bindDoUpdateAgencyAffiliateRate: function () {
        $("#form-agency-affiliate-tier").on("click", ".btn-update", function () {
            var isFormValid = $("#form-agency-affiliate-tier")[0].checkValidity();
            $("#form-agency-affiliate-tier").addClass('was-validated');
            var _this = this;

            if (isFormValid) {
                var _data = JSON.stringify($("#form-agency-affiliate-tier").serializeArray());
                $.ajax({
                    url: "/Admin/DoUpdateSetting/",
                    type: "POST",
                    dataType: 'json',
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                        $(_this).html("<i class='fa fa-spinner fa-spin'> </i> " + $(_this).text());
                    },
                    data: { 'data': _data },
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
    bindDoUpdateCookieExpirations: function () {
        $("#form-cookie-expired").on("click", ".btn-update", function () {
            var isFormValid = $("#form-cookie-expired")[0].checkValidity();
            $("#form-cookie-expired").addClass('was-validated');
            var _this = this;

            if (isFormValid) {
                var _data = JSON.stringify($("#form-cookie-expired").serializeArray());
                $.ajax({
                    url: "/Admin/DoUpdateSetting/",
                    type: "POST",
                    dataType: 'json',
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                        $(_this).html("<i class='fa fa-spinner fa-spin'>  </i> " + $(_this).text());
                    },
                    data: { 'data': _data },
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
                        $("#form-cookie-expired").removeClass('was-validated');
                    }
                });
            }

            return false;
        });
    },
    bindDoUpdateAccountActivationEnable: function () {
        $("#authentication").on("click", ".switchery", function () {
            var _this = this;
            var _data = JSON.stringify([{ name: $(_this).prev().prop("name"), value: $(_this).prev().is(":checked") }]);
            $.ajax({
                url: "/Admin/DoUpdateSetting/",
                type: "POST",
                dataType: 'json',
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: { 'data': _data },
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
    }
}


$(document).ready(function () {
    Setting.init();
});