var Exchange = {
    init: function () {
        Exchange.bindSwap();
        Exchange.bindMax();
        Exchange.bindInputChange();
        Exchange.bindNext();
        Exchange.bindConfirmExchange();
    },
    bindNext: function () {
        $('.section-exchange').on('click', '.btn-next', function () {
            var _this = this;
            if (parseFloat($(_this).parents(".section-exchange").find(".from-amount").val()) > 0 && parseFloat($(_this).parents(".section-exchange").find(".from-amount").val()) <= parseFloat($(_this).parents(".section-exchange").find(".from-amount").siblings(".max-amount").val())) {
                $(_this).parents(".section-exchange").find("#from-amount-error").hide();
                $.ajax({
                    url: "/Exchange/GetConfirm/",
                    type: "GET",
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                        $(_this).html("<i class='fa fa-spinner fa-spin'></i> " + $(_this).text());
                    },
                    data: {
                        FromCurrency: $(_this).parents(".section-exchange").find(".from-currency").val(),
                        FromAmount: $(_this).parents(".section-exchange").find(".from-amount").val(),
                        ToCurrency: $(_this).parents(".section-exchange").find(".to-currency").val(),
                        ToAmount: $(_this).parents(".section-exchange").find(".to-amount").val(),
                    },
                    success: function (data) {
                        $("#modal").html(data);
                        $("#exchange").modal("show");
                    },
                    complete: function (data) {
                        $(_this).attr("disabled", false);
                        $(_this).html($(_this).text() + "<i class='la la-angle-right'></i>");
                    }
                });
            }
            else
                $(_this).parents(".section-exchange").find("#from-amount-error").show();
        })
    },
    bindConfirmExchange: function () {
        $('#modal').on('click', '#btn-confirm-exchange', function () {
            var _this = this;
            $.ajax({
                url: "/Exchange/Confirm/",
                type: "POST",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                    $(_this).html("<i class='fa fa-spinner fa-spin'></i> " + $(_this).text());
                },
                data: {
                    FromCurrency: $(_this).parents("#modal").find("#FromCurrency").val(),
                    FromAmount: $(_this).parents("#modal").find("#FromAmount").val(),
                    ToCurrency: $(_this).parents("#modal").find("#ToCurrency").val(),
                    ToAmount: $(_this).parents("#modal").find("#ToAmount").val(),
                },
                success: function (data) {
                    if (data.success) {
                        $("#exchange").modal("hide");
                        //alert("Success!");
                        toastr.success(data.message, "Success!");
                        Exchange.loadViewComponent();
                    } else {
                        //alert("Error!");
                        toastr.error(data.message, "Error!");
                    }
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                    $(_this).html($(_this).text());
                }
            });
        })
    },
    bindSwap: function () {
        $('.section-exchange').on('click', '.btn-swap', function () {
            //Swap label
            if ($(this).parents(".section-exchange").length > 0) {
                var section = $(this).parents(".section-exchange");
                var label = section.find("#from-amount").text();
                section.find("#from-amount").text(section.find("#to-amount").text());
                section.find("#to-amount").text(label);

                //Swap value
                var value = section.find(".from-amount").val();
                section.find(".from-amount").val(section.find(".to-amount").val());
                section.find(".to-amount").val(value);

                //Swap max amount
                var value = section.find(".from-amount").siblings(".max-amount").val();
                section.find(".from-amount").siblings(".max-amount").val(section.find(".to-amount").siblings(".max-amount").val());
                section.find(".to-amount").siblings(".max-amount").val(value);

                //Swap currency
                var currency = section.find(".from-amount").siblings(".from-currency").val();
                section.find(".from-amount").siblings(".from-currency").val(section.find(".to-amount").siblings(".to-currency").val());
                section.find(".to-amount").siblings(".to-currency").val(currency);
            }
        });
    },
    bindMax: function () {
        $('.section-exchange').on('click', '.btn-max', function () {
            //Swap label
            if ($(this).parents(".section-exchange").length > 0) {
                var section = $(this).parents(".section-exchange");
                //var label = section.find((".from-amount").val);
                section.find(".from-amount").val(section.find(".from-amount").siblings(".max-amount").val()).trigger('change');
            }
        });
    },
    bindInputChange: function(){
        $('.section-exchange').on('change paste keyup input', '.from-amount', function () {
            if ($(this).parents(".section-exchange").length > 0) {
                var section = $(this).parents(".section-exchange");
                if (parseFloat(section.find(".from-amount").val()) > 0) {
                    var fromCurrency = section.find(".from-amount").siblings(".from-currency").val();
                    section.find(".invalid-amount").hide()
                    if (fromCurrency == "BTC") {
                        section.find(".to-amount").val(parseFloat(section.find(".from-amount").val()) * $(".btc-token-rate").val());
                    }
                    else if (fromCurrency == "ETH") {
                        var btcAmount = parseFloat(section.find(".from-amount").val()) * $(".eth-btc-rate").val();
                        section.find(".to-amount").val(btcAmount * $(".btc-token-rate").val());
                    }
                    else {
                        var toCurrency = section.find(".to-amount").siblings(".to-currency").val()
                        if (toCurrency == "BTC") {
                            section.find(".to-amount").val(parseFloat(section.find(".from-amount").val()) / $(".btc-token-rate").val());
                        }
                        else if (toCurrency == "ETH") {
                            var btcAmount = parseFloat(section.find(".from-amount").val()) / $(".btc-token-rate").val();
                            section.find(".to-amount").val(parseFloat(btcAmount / $(".eth-btc-rate").val()));
                        }
                    }
                }
                else {
                    $(this).parents(".section-exchange").find(".invalid-amount").show();
                    $(this).parents(".section-exchange").find(".to-amount").val(null);
                }
            }
        });
    },
    loadViewComponent: function () {
        $.ajax({
            url: "/Exchange/LoadExchangeViewComponent/",
            type: "GET",
            processData: false,
            contentType: false,
            success: function (data) {
                $("#exchange-content").html(data);
            }
        });
    }
}

$(document).ready(function () {
    Exchange.init();
});