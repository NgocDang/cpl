var Register = {
    init: function () {
        Register.bindRegister();
    },
    bindRegister: function () {
        $("#btn-do-register").on("click", function () {
            var isFormValid = $("#form-register")[0].checkValidity();
            $("#form-register").addClass('was-validated');
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
                    url: "/Authentication/DoRegister/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-do-register").attr("disabled", true);
                        $("#btn-do-register").html("<i class='fa fa-spinner fa-spin'></i><i class='fas fa-user-plus'></i> " + $("#btn-do-register").text());
                    },
                    data: {
                        Email: $("#Email").val(),
                        Password: $("#Password").val(),
                        PasswordConfirm: $("#PasswordConfirm").val(),
                        IsIntroducedById: $("#IsIntroducedById").val(),
                        AgencyToken: $("#AgencyToken").val()
                    },  
                    success: function (data) {
                        if (data.success) {
                            if (data.activated) { // Account activation is disabled
                                toastr.success(data.message, 'Success!');
                                window.location.href = data.url;
                            } else {
                                $("#form-register").hide();
                                $("#register-message").html(data.message);
                                $("#register-message").show();
                                toastr.success(data.message, 'Success!');
                            }
                        } else {
                            if (data.name == "email")
                                $("#email-msg").show();
                            else
                                $("#register-message").html(data.message).show();

                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $("#btn-do-register").attr("disabled", false);
                        $("#btn-do-register").html("<i class='fas fa-user-plus'></i> "+ $("#btn-do-register").text());
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