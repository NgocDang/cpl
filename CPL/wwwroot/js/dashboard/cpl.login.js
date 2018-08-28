var LogIn = {
    init: function () {
        LogIn.bindDoLogin();
        LogIn.bindDoVerifyPIN();
        LogIn.bindResetTabIndex();
    },
    bindDoLogin: function () {
        $("body").on("click", "#btn-log-in", function () {
            var isFormValid = $("#form-log-in")[0].checkValidity();
            $("#form-log-in").addClass('was-validated');

            if (!checkValidReCaptchaV2()) {
                $("#login-error").html($('#captchaMessage').val());
                $("#login-error").show();
                return false;
            }

            if (isFormValid) {
                $("#login-error").hide();
                $.ajax({
                    url: "/Authentication/LogIn/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-log-in").attr("disabled", true);
                        $("#btn-log-in").html("<i class='fa fa-spinner fa-spin'></i> <i class='fas fa-sign-in-alt'></i> " + $("#btn-log-in").text());
                    },
                    data: {
                        Email: $("#Email").val(),
                        Password: $("#Password").val(),
                        ReturnUrl: $("#ReturnUrl").val(),
                    },
                    success: function (data) {
                        if (data.success) {
                            if (data.twofactor) {
                                $("#login").hide();
                                $("#two-factor").show();
                            } else if ($("#login-modal").length > 0) {
                                //Lottery.loadAjaxConfirmPurchaseTicket();
                                location.reload();
                            } else {
                                window.location.href = data.url;
                            }
                            
                        } else {
                            $("#login-error").html(data.message);
                            $("#login-error").show();
                        }
                    },
                    complete: function (data) {
                        $("#btn-log-in").attr("disabled", false);
                        $("#btn-log-in").html("<i class='fas fa-sign-in-alt'></i> " + $("#btn-log-in").text().trim());
                    }

                });
            }
            return false;
        });
        var checkValidReCaptchaV2 = function () {
            //var v = grecaptcha.getResponse();
            //if (v === '') {
            //    return false;
            //}
            //else {
                return true;
            //}
        }
    },
    bindDoVerifyPIN: function () {
        $("form-two-factor").validate();
        $("body").on("click", "#btn-two-factor", function () {
            var isFormValid = $("#form-two-factor")[0].checkValidity();
            $("#form-two-factor").addClass('was-validated');
            if (isFormValid) {
                $("#two-factor-error").hide();
                $.ajax({
                    url: "/Authentication/VerifyPIN/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-two-factor").addClass("disabled");
                    },
                    data: {
                        Email: $("#Email").val(),
                        PIN: $("#PIN").val().toString()
                    },
                    success: function (data) {
                        if (data.success) {
                            if ($("#login-modal").length > 0) {
                                Lottery.loadAjaxConfirmPurchaseTicket();
                            }
                            else {
                                window.location.replace(data.url);
                            }
                        } else {
                            $("#two-factor-error").html(data.message);
                            $("#two-factor-error").show();
                        }
                    },
                    complete: function (data) {
                        $("#btn-two-factor").removeClass("disabled");
                    }
                });
            }
            return false;
        });
    },
    bindResetTabIndex: function () {
        $("[tabindex]").each(function () {
            $(this).attr('tabindex', '0');
        });
    }
};


$(document).ready(function () {
    LogIn.init();
});