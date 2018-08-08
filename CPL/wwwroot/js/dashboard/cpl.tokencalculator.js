var Calculator = {
    init: function () {
        Calculator.bindCalculator();
    },
    bindCalculator: function () {
        $('#token-calculator').on('change paste keyup input', '#btc-amount', function () {
            if (parseFloat($('#btc-amount').val()) > 0) {
                if (parseFloat($('#eth-amount').val()) > 0) {
                    $("#token-amount").val(parseFloat($('#btc-amount').val()) * $(".btc-token-rate").val() + parseFloat($('#eth-amount').val()) * $(".eth-token-rate").val());
                }
                else {
                    $("#token-amount").val(parseFloat($('#btc-amount').val()) * $(".btc-token-rate").val());
                }
            }
            else {
                if (parseFloat($('#eth-amount').val()) > 0) {
                    $("#token-amount").val(parseFloat($('#eth-amount').val()) * $(".eth-token-rate").val());
                }
                else
                    $("#token-amount").val(null);
            }
        });
        $('#token-calculator').on('change paste keyup input', '#eth-amount', function () {
            if (parseFloat($('#eth-amount').val()) > 0) {
                if (parseFloat($('#btc-amount').val()) > 0) {
                    $("#token-amount").val(parseFloat($('#eth-amount').val()) * $(".eth-token-rate").val() + parseFloat($('#btc-amount').val()) * $(".btc-token-rate").val());
                }
                else {
                    $("#token-amount").val(parseFloat($('#eth-amount').val()) * $(".eth-token-rate").val());
                }
            }
            else {
                if (parseFloat($('#btc-amount').val()) > 0) {
                    $("#token-amount").val(parseFloat($('#btc-amount').val()) * $(".btc-token-rate").val());
                }
                else
                    $("#token-amount").val(null);
            }
        });
    }
}

$(document).ready(function () {
    Calculator.init();
});