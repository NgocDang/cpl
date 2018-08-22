var Register = {
    init: function () {
        Register.bindDoRegister();
    },
    bindDoRegister: function () {
        $("#btn-register").on("click", function () {
            var isFormValid = $("#form-register").valid();
            var isPasswordValid = $("#Password").val() == $("#PasswordConfirm").val();
            if (!isPasswordValid) {
                $("#password-confirm-msg").show();
            } else {
                $("#password-confirm-msg").hide();
            }
            if (grecaptcha.getResponse() == '') {
                $("#register-msg").html($('#captchaMessage').val());
                $("#register-msg").show();
                return false;
            }

            if (isFormValid && isPasswordValid) {
                $("#register-msg").hide();
                $("#email-msg").hide();
                $.ajax({
                    url: "/Authentication/Register/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-register").attr("disabled", true);
                        $("#btn-register").html("<i class='fa fa-spinner fa-spin'></i><i class='fas fa-user-plus'></i> " + $("#btn-register").text());
                    },
                    data: {
                        Email: $("#Email").val(),
                        Password: $("#Password").val(),
                        PasswordConfirm: $("#PasswordConfirm").val(),
                    },  
                    success: function (data) {
                        if (data.success) {
                            if (data.activated) { // Account activation is disabled
                                toastr.success(data.message, 'Success!');
                                window.location.href = data.url;
                            } else {
                                $("#form-register").hide();
                                toastr.success(data.message, 'Success!');
                            }
                        } else {
                            if (data.name == "email")
                                $("#email-msg").html(data.message).show();
                            else
                                $("#register-message").html(data.message).show();

                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $("#btn-register").attr("disabled", false);
                        $("#btn-register").html("<i class='fas fa-user-plus'></i> "+ $("#btn-register").text());
                    }
                });
            }
            return false;
        });
    }
};


$(document).ready(function () {
    Register.init();
});