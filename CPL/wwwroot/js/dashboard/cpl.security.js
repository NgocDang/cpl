var Security = {
    init: function () {
        Security.bindSaveEmail();
        Security.bindSavePassword();
        Security.bindTwoFactorAuthenticate();
        Security.bindTwoFactorEnableDisable();
    },
    bindSaveEmail: function () {
        $("#btn-save-email").on("click", function () {
            var isFormValid = $('#form-edit-email')[0].checkValidity();
            $("#form-edit-email").addClass('was-validated');

            var isConfirmEmailValid = $("#NewEmail").val() == $("#NewEmailConfirm").val();
            if (!isConfirmEmailValid) {
                $("#new-email-confirm-msg").show();
            } else {
                $("#new-email-confirm-msg").hide();
            }

            if (isFormValid && isConfirmEmailValid) {
                $("#new-email-msg").hide();

                $.ajax({
                    url: "/Profile/UpdateEmail/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-save-email").attr("disabled", true);
                        $("#btn-save-email").html("<i class='fa fa-spinner fa-spin'></i> <i class='far fa-save'></i> " + $("#btn-save-email").text());
                    },
                    data: {
                        CurrentEmail: $("#CurrentEmail").val(),
                        NewEmail: $("#NewEmail").val(),
                        NewEmailConfirm: $("#NewEmailConfirm").val()
                    },
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message, 'Success!');
                        } else {
                            if (data.name == "new-email") 
                                $("#new-email-msg").show();
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $("#btn-save-email").attr("disabled", false);
                        $("#btn-save-email").html("<i class='far fa-save'></i> " + $("#btn-save-email").text());
                    }
                });
            }
            return false;
        });
    },
    bindSavePassword: function () {
        $("#btn-save-password").on("click", function () {
            var isFormValid = $('#form-edit-password')[0].checkValidity();
            $("#form-edit-password").addClass('was-validated');

            var isConfirmPasswordValid = $("#NewPassword").val() == $("#NewPasswordConfirm").val();
            if (!isConfirmPasswordValid) {
                $("#new-password-confirm-msg").show();
            } else {
                $("#new-password-confirm-msg").hide();
            }

            if (isFormValid && isConfirmPasswordValid) {
                $("#current-password-msg").hide();
                $.ajax({
                    url: "/Profile/UpdatePassword/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-save-password").attr("disabled", true);
                        $("#btn-save-password").html("<i class='fa fa-spinner fa-spin'></i> <i class='far fa-save'></i> " + $("#btn-save-password").text());
                    },
                    data: {
                        CurrentPassword: $("#CurrentPassword").val(),
                        NewPassword: $("#NewPassword").val(),
                        NewPasswordConfirm: $("#NewPasswordConfirm").val()
                    },
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message, 'Success!');
                        } else {
                            if (data.name == "current-password")
                                $("#current-password-msg").show();
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $("#btn-save-password").attr("disabled", false);
                        $("#btn-save-password").html("<i class='far fa-save'></i> " + $("#btn-save-password").text());
                    }
                });
            }
            return false;
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
    bindTwoFactorEnableDisable: function () {
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
        $("#two-factor-authenticator").on("click", "#btn-two-factor-enable", function () {
            var isFormValid = $('#form-two-factor-enable')[0].checkValidity();
            $("#form-two-factor-enable").addClass('was-validated');
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
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $("#btn-two-factor-enable").attr("disabled", false);
                        $("#btn-two-factor-enable").html($("#btn-two-factor-enable").text());
                    }
                });
            }
            return false;
        });
    }
};


$(document).ready(function () {
    Security.init();
});