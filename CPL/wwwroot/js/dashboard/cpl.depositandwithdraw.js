var DepositAndWithdraw = {
    init: function () {
        DepositAndWithdraw.bindCopy();
        DepositAndWithdraw.bindWithdraw();
        DepositAndWithdraw.bindMax();
        DepositAndWithdraw.bindDoWithdraw();

        $(".btn-qrcode").on("click", function () {
            $(".file-qrcode").click();
        });

        $(".file-qrcode").on("change", function () {
            $.ajax({
                url: "/DepositAndWithdraw/DecodeQR/",
                type: "POST",
                data: {
                    formFile: $(".file-qrcode").get(0)
                },
                success: function (data) {
                    }
                },
                complete: function (data) {
                }
            });
            return false;
        });

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
        $(".btn-withdraw").on("click", function () {
            if ($(this).parents("section").find(".panel-withdraw:visible").length) {
                $(this).parents("section").find(".panel-withdraw").slideUp("slow");
            } else {
                $(".panel-withdraw").slideUp("slow");
                $(this).parents("section").find(".panel-withdraw").slideToggle("slow");
            }
        });
    },
    bindMax: function () {
        $(".btn-max").on("click", function () {
            if ($(this).parents("form").find("input.max-amount").length) {
                $(this).parents("form").find(".amount-value").val($(this).parents("form").find(".max-amount").val());
            }
        });
    },
    bindDoWithdraw: function () {
        $(".btn-do-withdraw").on("click", function () {
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
                },
                complete: function (data) {
                }
            });
            return false;
        });
    }
}

$(document).ready(function () {
    DepositAndWithdraw.init();
});