var Security = {
    init: function () {
        Security.bindDoEditEmail();
        Security.bindDoEditPassword();
        Security.bindLoadTwoFactorAuthentication();
        Security.bindDoEditTwoFactorAuthentication();
    },
    bindDoEditEmail: function () {
        $("#btn-do-edit-email").on("click", function () {
            var newEmail = $("#NewEmail").val();
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
                    url: "/Profile/DoEditEmail/",
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
                            $("#form-edit-email")[0].reset();
                            $("#CurrentEmail").val(newEmail);
                            $("#form-edit-email").removeClass('was-validated');
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
    bindDoEditPassword: function () {
        $("#btn-do-edit-password").on("click", function () {
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
                    url: "/Profile/DoEditPassword/",
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
                            $("#form-edit-password")[0].reset();
                            $("#form-edit-password").removeClass('was-validated');
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
    bindLoadTwoFactorAuthentication: function () {
        if ($("#TwoFactorAuthenticationEnable").val().toLowerCase() == "false") {
            $("#form-two-factor-enable").show();
            $("#form-two-factor-disable").hide();
        } else {
            $("#form-two-factor-enable").hide();
            $("#form-two-factor-disable").show();
        }
    },
    bindDoEditTwoFactorAuthentication: function () {
        $("#two-factor-authenticator").on("click", "#btn-do-disable-two-factor", function () {
            $.ajax({
                url: "/Profile/DoEditTwoFactorAuthentication/",
                type: "POST",
                beforeSend: function () {
                    $("#btn-do-disable-two-factor").attr("disabled", true);
                    $("#btn-do-disable-two-factor").html("<i class='fa fa-spinner fa-spin'></i> " + $("#btn-do-disable-two-factor").text());
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
                    $("#btn-do-disable-two-factor").attr("disabled", false);
                    $("#btn-do-disable-two-factor").html($("#btn-do-disable-two-factor").text());
                }
            });
        });
        $("#two-factor-authenticator").on("click", "#btn-do-enable-two-factor", function () {
            var isFormValid = $('#form-two-factor-enable')[0].checkValidity();
            $("#form-two-factor-enable").addClass('was-validated');
            if (isFormValid) {
                $.ajax({
                    url: "/Profile/DoEditTwoFactorAuthentication/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-do-enable-two-factor").attr("disabled", true);
                        $("#btn-do-enable-two-factor").html("<i class='fa fa-spinner fa-spin'></i> " + $("#btn-do-enable-two-factor").text());
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
                        $("#btn-do-enable-two-factor").attr("disabled", false);
                        $("#btn-do-enable-two-factor").html($("#btn-do-enable-two-factor").text());
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