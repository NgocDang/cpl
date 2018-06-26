var Register = {
    init: function () {
        $("#form-register").validate({
            highlight: function (element) {
                $(element).closest('.form-group').removeClass('has-success').addClass('has-danger');
                $(element).closest('.form-check').removeClass('has-success').addClass('has-danger');
            },
            success: function (element) {
                $(element).closest('.form-group').removeClass('has-danger').addClass('has-success');
                $(element).closest('.form-check').removeClass('has-danger').addClass('has-success');
            },
            errorPlacement: function (error, element) {
                $(element).append(error);
            },
        });

        $("#btn-register").on("click", function () {
            var isFormValid = $("#form-register")[0].checkValidity();
            $("#form-register").addClass('was-validated');
            var isPasswordValid = $("#Password").val() == $("#PasswordConfirm").val();
            if (!isPasswordValid) {
                $("#password-confirm-message").show();
            } else {
                $("#password-confirm-message").hide();
            }

            if (isFormValid && isPasswordValid) {
                $("#register-error").hide();
                $("#register-message").hide();
                $("#Email").removeClass("border-danger");
                $.ajax({
                    url: "/Authentication/Register/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-register").attr("disabled", true);
                        $("#btn-register").html("<i class='fa fa-spinner fa-spin'></i> " + $("#btn-register").text());
                    },
                    data: {
                        Email: $("#Email").val(),
                        Password: $("#Password").val(),
                        PasswordConfirm: $("#PasswordConfirm").val(),
                    },
                    success: function (data) {
                        debugger;
                        if (data.success) {
                            if (data.activated) { // Account activation is disabled
                                window.location.href = data.url;
                            } else {
                                $("#register-message").html(data.message);
                                $("#register-message").addClass("text-muted").removeClass("invalid-feedback").show();
                                $("#form-register").hide();
                                toastr.success(data.message, 'Success!');
                            }
                        } else {
                            if (data.name == "email") {
                                $("#Email").addClass("border-danger");
                            }
                            toastr.error(data.message, 'Error!');
                            $("#register-message").html(data.message);
                            $("#register-message").removeClass("text-muted").addClass("invalid-feedback").show();
                        }
                    },
                    complete: function (data) {
                        $("#btn-register").attr("disabled", false);
                        $("#btn-register").html($("#btn-register").text());
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