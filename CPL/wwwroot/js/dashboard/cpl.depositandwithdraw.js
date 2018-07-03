var DepositAndWithdraw = {
    init: function () {
        if ($(".btn-copy").length > 0) {
            var clipboard = new ClipboardJS('.btn-copy');
            clipboard.on('success', function (e) {
                toastr.success($("#CopiedText").val());
            });
        }

        $("#btn-withdraw-btc").on("click", function () {
            $("#panel-withdraw-btc").slideToggle("slow");
            $("#panel-withdraw-eth").slideUp("slow");
        });

        $("#btn-withdraw-eth").on("click", function () {
            $("#panel-withdraw-eth").slideToggle("slow");
            $("#panel-withdraw-btc").slideUp("slow");
        });

        $("#btn-max-btc").on("click", function () {
            $("#btc-amount").val($("#available-bct").text());
        });

        $("#btn-max-eth").on("click", function () {
            $("#eth-amount").val($("#available-eth").text());
        });

        $("#btc-withdraw").on("click", function () {
            $("#form-withdraw-btc").valid();
            $.ajax({
                url: "/DepositAndWithdraw/DoDepositWithdrawBTC/",
                type: "POST",
                data: {
                    BtcAmount: $("#btc-amount").val(),
                    BtcAddress: $("#address-btc").val(),
                },
                success: function (data) {
                    if (data.success) {
                        $("#error-address-btc").hide();
                        $("#btc-amount-error").hide();
                        toastr.success(data.message, 'Success!');
                    } else {
                        if (data.name === "btc-wallet") {
                            $("#btc-amount-error").hide();
                            $("#error-address-btc").show();
                            $("#error-address-btc").text(data.message);
                        }
                        if (data.name === "btc-amount") {
                            $("#error-address-btc").hide();
                            $("#btc-amount-error").show();
                            $("#btc-amount-error").text(data.message);
                        }
                    }
                },
                complete: function (data) {
                }
            });
            return false;
        });

        $("#eth-withdraw").on("click", function () {
            $("#form-withdraw-eth").valid();
            $.ajax({
                url: "/DepositAndWithdraw/DoDepositWithdrawETH/",
                type: "POST",
                data: {
                    EthAmount: $("#eth-amount").val(),
                    EthAddress: $("#eth-address").val(),
                },
                success: function (data) {
                    if (data.success) {
                        $("#error-address-eth").hide();
                        $("#error-amount-eth").hide();
                        toastr.success(data.message, 'Success!');
                    } else {
                        if (data.name === "eth-wallet") {
                            $("#error-amount-eth").hide();
                            $("#error-address-eth").show();
                            $("#error-address-eth").text(data.message);
                        }
                        if (data.name === "eth-amount") {
                            $("#error-address-eth").hide();
                            $("#error-amount-eth").show();
                            $("#error-amount-eth").text(data.message);
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