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
            $("#panel-withdraw-eth").slideUp("slow");
        });

        $("#txt-withdraw-eth").on("click", function () {
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
                url: "/DepositAndWithdraw/DoDepositeWithdrawBTC/",
                type: "POST",
                data: {
                    BtcAmount: $("#btc-amount").val(),
                    BtcAddress: $("#btc-address").val(),
                },
                success: function (data) {
                    if (data.success) {
                        $("#btc-address-error").hide();
                        $("#btc-amount-error").hide();
                        toastr.success(data.message, 'Success!');
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
                }
            });
            return false;
        });

        $("#eth-withdraw").on("click", function () {
            $("#form-withdraw-eth").valid();
            $.ajax({
                url: "/DepositAndWithdraw/DoDepositeWithdrawETH/",
                type: "POST",
                data: {
                    EthAmount: $("#eth-amount").val(),
                    EthAddress: $("#eth-address").val(),
                },
                success: function (data) {
                    if (data.success) {
                        $("#eth-address-error").hide();
                        $("#eth-amount-error").hide();
                        toastr.success(data.message, 'Success!');
                    } else {
                        if (data.name === "eth-wallet") {
                            $("#eth-amount-error").hide();
                            $("#eth-address-error").show();
                            $("#eth-address-error").text(data.message);
                        }
                        if (data.name === "eth-amount") {
                            $("#eth-address-error").hide();
                            $("#eth-amount-error").show();
                            $("#eth-amount-error").text(data.message);
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