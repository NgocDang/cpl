var ForgotPassword = {
    init: function () {
        ForgotPassword.bindForgotPasswordForm();
    },
    bindForgotPasswordForm: function () {
        $("#btn-forgot-password").on("click", function () {
            var isFormValid = $("#form-forgot-password")[0].checkValidity();
            $("#form-forgot-password").addClass('was-validated');

            if (isFormValid) {
                $("#forgot-password-error").hide();
                $.ajax({
                    url: "/Authentication/ForgotPassword/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-forgot-password").attr("disabled", true);
                        $("#btn-forgot-password").html("<i class='fa fa-spinner fa-spin'></i> " + $("#btn-forgot-password").text());
                    },
                    data: {
                        Email: $("#Email").val()
                    },
                    success: function (data) {
                        if (data.success) {
                            $("#forgot-password-message").html(data.message);
                            $("#forgot-password-message").show();
                            $("#form-forgot-password").hide();
                        } else {
                            $("#forgot-password-error").html(data.message);
                            $("#forgot-password-error").show();
                        }
                    },
                    complete: function (data) {
                        $("#btn-forgot-password").attr("disabled", false);
                        $("#btn-forgot-password").html($("#btn-forgot-password").text());
                    }
                });
            }
            return false;
        });
    }
};


$(document).ready(function () {
    ForgotPassword.init();
});