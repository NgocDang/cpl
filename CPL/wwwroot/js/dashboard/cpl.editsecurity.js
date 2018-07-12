var EditSecurity = {
    init: function () {
        EditSecurity.bindSaveKYCButton();
        EditSecurity.bindTwoFactorAuthenticate();
        EditSecurity.bindTwoFactorAuthenticateEnable();
    },
    bindSaveKYCButton: function () {
        $("#form-edit-account").on("click", "#btn-save-account", function () {
            if ($("#KYCVerified").val() !== "") {
                $("#btn-save-account").attr("disabled", true);
            }
            else {
                $("#btn-save-account").attr("disabled", false);
                var fsFileUpload = $("#FrontSideImage").get(0);
                if (fsFileUpload !== undefined && fsFileUpload.files.length > 0) {
                    var fsFile = fsFileUpload.files[0];
                }
                var bsFileUpload = $("#BackSideImage").get(0);
                if (bsFileUpload !== undefined && bsFileUpload.files.length > 0) {
                    var bsFile = bsFileUpload.files[0];
                }

                var isFormValid = $("#form-edit-account").valid();

                if (isFormValid) {
                    var formData = new FormData();
                    formData.append('FrontSideImage', fsFile);
                    formData.append('BackSideImage', bsFile);
                    $.ajax({
                        url: "/Profile/UpdateKYC/",
                        type: "POST",
                        processData: false,
                        contentType: false,
                        beforeSend: function () {
                            $("#btn-save-account").attr("disabled", true);
                            $("#btn-save-account").html("<i class='fa fa-spinner fa-spin'></i> <i class='far fa-save'></i> " + $("#btn-save-account").text());
                        },
                        data: formData,
                        success: function (data) {
                            if (data.success) {
                                toastr.success(data.message, 'Success!');
                                if (fsFile !== undefined && bsFile !== undefined) {
                                    $("#kyc-verify").replaceWith("<div class='row mb-1 col-sm-12' id='kyc-verify'><p class='text-muted'>" + data.kycconfirm + "</p><span class='badge badge-info h-50'>" + data.kycverify + "</span></div>");
                                }
                                $("#KYCVerified").val(0);
                            } else {
                                toastr.error(data.message, 'Error!');
                            }
                        },
                        complete: function (data) {
                            $("#btn-save-account").attr("disabled", false);
                            $("#btn-save-account").html("<i class='far fa-save'></i> " + $("#btn-save-account").text());
                        }
                    });
                }
                return false;
            }
        });
    },
    bindTwoFactorAuthenticate: function () {
        if ($("#TwoFactorAuthenticationEnable").val().toLowerCase() == "false") {
            $("#form-two-factor-enable").show();
            $("#form-two-factor-disable").hide();
        } else {
            $("#form-two-factor-enable").hide();
            $("#form-two-factor-disable").show();
        }
    },
    bindTwoFactorAuthenticateEnable: function () {
        $("#two-factor-authenticator").on("click", "#btn-two-factor-disable", function () {
            $.ajax({
                url: "/Profile/UpdateTwoFactorAuthentication/",
                type: "POST",
                beforeSend: function () {
                    $("#btn-two-factor-disable").attr("disabled", true);
                    $("#btn-two-factor-disable").html("<i class='fa fa-spinner fa-spin'></i> " + $("#btn-two-factor-disable").text());
                },
                data: {
                    value: false
                },
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message, 'Success!');
                        $("#form-two-factor-disable").hide();
                        $("#form-two-factor-enable").show();
                    } else {
                        toastr.error(data.message, 'Error!');
                    }
                },
                complete: function (data) {
                    $("#btn-two-factor-disable").attr("disabled", false);
                    $("#btn-two-factor-disable").html($("#btn-two-factor-disable").text());
                }
            });
        });
        $("#form-two-factor-enable").validate();
        $("#two-factor-authenticator").on("click", "#btn-two-factor-enable", function () {
            var isFormValid = $("#form-two-factor-enable").valid();
            $("#form-two-factor-enable").addClass('was-validated');
            $("#PIN").remove("border-danger");
            if (isFormValid) {
                $.ajax({
                    url: "/Profile/UpdateTwoFactorAuthentication/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-two-factor-enable").attr("disabled", true);
                        $("#btn-two-factor-enable").html("<i class='fa fa-spinner fa-spin'></i> " + $("#btn-two-factor-enable").text());
                    },
                    data: {
                        value: true,
                        pin: $("#PIN").val()
                    },
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message, 'Success!');
                            $("#form-two-factor-disable").show();
                            $("#form-two-factor-enable").hide();
                        } else {
                            $("#PIN").addClass("border-danger");
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $("#btn-two-factor-enable").attr("disabled", false);
                        $("#btn-two-factor-enable").html($("#btn-two-factor-enable").text());
                    }
                });
            }
        });
    }
};

$(document).ready(function () {
    EditSecurity.init();
});