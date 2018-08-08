var LogIn = {
    init: function () {
        LogIn.bindLoginForm();
        LogIn.bindVerify();
        LogIn.bindResetTabIndex();
    },
    bindLoginForm: function () {
        $("#form-log-in").validate();
        $("body").on("click", "#btn-log-in", function () {
            var isFormValid = $("#form-log-in").valid();
            if (!checkValidReCaptchaV2()) {
                $(".login-error").html($('#captchaMessage').val());
                $(".login-error").show();
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
                                $("div.card-login-page").removeClass("height-500");
                                $("div.card-login-page").addClass("height-200");
                                $("img#img-logo").css("margin-top", "-5%");
                                $("#login").hide();
                                $("#two-factor").show();
                            } else if ($("#login-modal").length > 0) {
                                Lottery.loadAjaxConfirmPurchaseTicket();
                            } else {
                                window.location.replace(data.url);
                            }
                            
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
                        $("#btn-log-in").html("<i class='fas fa-sign-in-alt'></i> " + $("#btn-log-in").text().trim());
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
    },
    bindVerify: function () {
        $("form-two-factor").validate();
        $("body").on("click", "#btn-two-factor", function () {
            var isFormValid = $("#form-two-factor").valid();
            $("#form-two-factor").addClass('was-validated');
            if (isFormValid) {
                $("#two-factor-error").hide();
                $(".two-factor-error").removeClass("border-danger");
                $.ajax({
                    url: "/Authentication/VerifyPIN/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-two-factor").addClass("disabled");
                    },
                    data: {
                        Email: $("#Email").val(),
                        PIN: $("#PIN").val()
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
                            $(".two-factor-error").addClass("danger");
                            $(".two-factor-error").html(data.message);
                            $(".two-factor-error").show();
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