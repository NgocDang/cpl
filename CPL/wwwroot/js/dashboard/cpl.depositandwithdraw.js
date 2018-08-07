var DepositAndWithdraw = {
    init: function () {
        DepositAndWithdraw.bindCopy();
        DepositAndWithdraw.bindWithdraw();
        DepositAndWithdraw.bindMax();
        DepositAndWithdraw.bindDoWithdraw();
        DepositAndWithdraw.bindReadQrCode();
    },
    bindCopy: function () {
        if ($(".btn-copy").length > 0) {
            var clipboard = new ClipboardJS('.btn-copy');
            clipboard.on('success', function (e) {
                toastr.success($("#CopiedText").val());
            });
        }
    },
    bindWithdraw: function () {
        $("#dipositwithdraw-content").on("click", ".btn-withdraw", function () {
            if ($(this).parents("section").find(".panel-withdraw:visible").length) {
                $(this).parents("section").find(".panel-withdraw").slideUp();
            } else {
                $(".panel-withdraw").slideUp();
                $(this).parents("section").find(".panel-withdraw").slideToggle();
            }
        });
    },
    bindMax: function () {
        $("#dipositwithdraw-content").on("click", ".btn-max", function () {
            if ($(this).parents("form").find("input.max-amount").length) {
                $(this).parents("form").find(".amount-value").val($(this).parents("form").find(".max-amount").val());
            }
        });
    },
    bindDoWithdraw: function () {
        $("#dipositwithdraw-content").on("click", ".btn-do-withdraw", function () {
            $(this).parents("form").valid();
            var _this = this;
            $.ajax({
                url: "/DepositAndWithdraw/DoDepositWithdraw/",
                type: "POST",
                data: {
                    Currency: $(_this).parents("form").find(".currency").val(),
                    Amount: $(_this).parents("form").find(".amount-value").val(),
                    Address: $(_this).parents("form").find(".address-value").val(),
                },
                success: function (data) {
                    if (data.success) {
                        $(_this).parents("form").find(".address-error").hide();
                        $(_this).parents("form").find(".amount-error").hide();
                        toastr.success(data.message, 'Success!');
                        DepositAndWithdraw.bindLoadViewComponent("withdraw");
                    } else {
                        if (data.name === "wallet") {
                            $(_this).parents("form").find(".amount-error").hide();
                            $(_this).parents("form").find(".address-error").show();
                            $(_this).parents("form").find(".address-error").html(data.message);
                        }
                        if (data.name === "amount") {
                            $(_this).parents("form").find(".address-error").hide();
                            $(_this).parents("form").find(".amount-error").html(data.message);
                            $(_this).parents("form").find(".amount-error").show();
                        }
                    }
                }
            });
            return false;
        });
    },
    bindReadQrCode: function () {
        $("#dipositwithdraw-content").on("click", ".btn-qrcode", function () {
            $(this).parents("form").find(".file-qrcode").click();
        });

        $("#dipositwithdraw-content").on("change", ".file-qrcode", function () {
            var _this = this;
            var image = $(_this).parents("form").find(".file-qrcode")[0].files[0];
            if (image.size > 0) {
                var formData = new FormData();
                formData.append('FormFile', image);
                $.ajax({
                    url: "/DepositAndWithdraw/DecodeQR/",
                    type: "POST",
                    processData: false,
                    contentType: false,
                    data: formData,
                    success: function (data) {
                        if (data.success) {
                            $(_this).parents("form").find(".address-value").val(data.address);
                            toastr.success(data.message, 'Success!');
                        } else {
                            $(_this).parents("form").find(".address-value").val("");
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                    }
                })
            };
            return false;
        });
    },
    bindLoadViewComponent: function (tab) {
        $.ajax({
            url: "/DepositAndWithdraw/LoadDepositWithdrawViewComponent/",
            type: "GET",
            processData: false,
            contentType: false,
            success: function (data) {
                $("#dipositwithdraw-content").html(data);
                if (tab == "withdraw") {
                    $("#deposit-nav-tab").removeClass("active");
                    $("#deposit-nav").removeClass("active");
                    $("#withdraw-nav-tab").addClass("active");
                    $("#withdraw-nav").addClass("active");
                }
            }
        });
    }
}

$(document).ready(function () {
    DepositAndWithdraw.init();
});