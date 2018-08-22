var ResetPassword = {
    init: function () {
        ResetPassword.bindDoResetPassword();
    },
    bindDoResetPassword: function () {
        $("#btn-do-reset-password").on("click", function () {
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
                    url: "/Authentication/DoResetPassword/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-do-reset-password").attr("disabled", true);
                        $("#btn-do-reset-password").html("<i class='fa fa-spinner fa-spin'></i> " + $("#btn-do-reset-password").text());
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
                        $("#btn-do-reset-password").attr("disabled", false);
                        $("#btn-do-reset-password").html($("#btn-do-reset-password").text());
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