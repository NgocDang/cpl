var ResetPassword = {
    init: function () {
        ResetPassword.bindResetPasswordForm();
    },
    bindResetPasswordForm: function () {
        $("#btn-reset-password").on("click", function () {
            var isFormValid = $("#form-reset-password")[0].checkValidity();
            $("#form-reset-password").addClass('was-validated');

            var isPasswordValid = $("#Password").val() == $("#PasswordConfirm").val();
            if (!isPasswordValid) {
                $("#password-confirm-message").show();
            } else {
                $("#password-confirm-message").hide();
            }

            if (isFormValid && isPasswordValid) {
                $("#reset-password-error").hide();
                $.ajax({
                    url: "/Authentication/ResetPassword/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-reset-password").attr("disabled", true);
                        $("#btn-reset-password").html("<i class='fa fa-spinner fa-spin'></i> " + $("#btn-reset-password").text());
                    },
                    data: {
                        Password: $("#Password").val(),
                        PasswordConfirm: $("#PasswordConfirm").val(),
                        Id: $("#Id").val(),
                        Token: $("#Token").val()
                    },
                    success: function (data) {
                        if (data.success) {
                            $("#reset-password-message").html(data.message);
                            $("#reset-password-message").show();
                            $("#form-reset-password").hide();
                        } else {
                            $("#reset-password-message").html(data.message);
                            $("#reset-password-message").removeClass("text-muted").addClass("invalid-feedback").show();
                        }
                    },
                    complete: function (data) {
                        $("#btn-reset-password").attr("disabled", false);
                        $("#btn-reset-password").html($("#btn-reset-password").text());
                    }
                });
            }
            return false;
        });
    }
};


$(document).ready(function () {
    ResetPassword.init();
});