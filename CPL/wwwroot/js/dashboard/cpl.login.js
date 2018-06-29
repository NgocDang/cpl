var LogIn = {
    init: function () {
        $("#form-log-in").validate();

        //{
        //    highlight: function (element) {
        //        $(element).closest('.form-group').removeClass('has-success').addClass('has-danger');
        //        $(element).closest('.form-check').removeClass('has-success').addClass('has-danger');
        //    },
        //    success: function (element) {
        //        $(element).closest('.form-group').removeClass('has-danger').addClass('has-success');
        //        $(element).closest('.form-check').removeClass('has-danger').addClass('has-success');
        //    },
        //    errorPlacement: function (error, element) {
        //        $(element).append(error);
        //    },
        //}

        $("#btn-log-in").on("click", function () {
            var isFormValid = $("#form-log-in").Isvalid();
            if (!checkValidReCaptchaV2()) {
                $(".login-error").html($('#captchaMessage').val());
                $(".login-error").show();
                return false;
            }

            if (true) {
                $("#login-error").hide();
                $.ajax({
                    url: "/Authentication/LogIn/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-log-in").attr("disabled", true);
                        $("#btn-log-in").html("<i class='fa fa-spinner fa-spin'></i> " + $("#btn-log-in").text());
                    },
                    data: {
                        Email: $("#Email").val(),
                        Password: $("#Password").val(),
                        ReturnUrl: $("#ReturnUrl").val()
                    },
                    success: function (data) {
                        if (data.success) {
                            //if (data.twofactor) {
                            //    $("#log-in").hide();
                            //    $("#two-factor").show();
                            //} else {
                                window.location.replace(data.url);
                            //}
                        } else {
                            //if (data.name == "mobile-verify") {
                            //    $("#mobile-verify-message").html(data.message);
                            //    $("#mobile-verify-message").removeClass("text-muted").addClass("invalid-feedback").show();
                            //    $("#Id").val(data.id);
                            //    $("#log-in").hide();
                            //    $("#mobile-verify").show();
                            //} else {
                            $(".login-error").html(data.message);
                            $(".login-error").show();
                            //}
                        }
                    },
                    complete: function (data) {
                        $("#btn-log-in").attr("disabled", false);
                        $("#btn-log-in").html($("#btn-log-in").text().trim());
                    }

                });
            }
            return false;
        });

        var checkValidReCaptchaV2 = function () {
            var v = grecaptcha.getResponse();
            if (v === '') {
                return false;
            }
            else {
                return true;
            }
        }

    }
};


$(document).ready(function () {
    LogIn.init();
});