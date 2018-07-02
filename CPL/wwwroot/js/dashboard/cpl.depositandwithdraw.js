var DepositAndWithdraw = {
    init: function () {
        if ($(".btn-copy").length > 0) {
            var clipboard = new ClipboardJS('.btn-copy');
            clipboard.on('success', function (e) {
                toastr.success($("#CopiedText").val());
            });
        }

        $("#txt-withdraw-btc").on("click", function () {
            $("#panel-withdraw-btc").slideToggle("slow");
        });

        $("#btn-max-btc").on("click", function () {
            $("#btc-amount").val($("#available-bct").text());
        });

        $("#btc-withdraw").on("click", function () {
            $("#form-withdraw-btc").valid();
            $.ajax({
                url: "/DepositAndWithdraw/DoDepositeWithdrawBTC/",
                type: "POST",
                data: {
                    BtcAmount: $("#btc-amount").val(),
                    BtcAddress: $("#btc-address").val(),
                },
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message, 'Success!');
                        $("#btc-address-error").hide();
                        $("#btc-amount-error").hide();
                    } else {
                        if (data.name === "btc-wallet") {
                            $("#btc-amount-error").hide();
                            $("#btc-address-error").show();
                            $("#btc-address-error").text(data.message);
                        }
                        if (data.name === "btc-amount") {
                            $("#btc-address-error").hide();
                            $("#btc-amount-error").show();
                            $("#btc-amount-error").text(data.message);
                        }
                    }
                },
                complete: function (data) {
                    //$("#btn-affiliate-update").attr("disabled", false);
                    //$("#btn-affiliate-update").html($("#btn-affiliate-update").text());
                }
            });
            return false;
        });
    }
}

$(document).ready(function () {
    DepositAndWithdraw.init();
});