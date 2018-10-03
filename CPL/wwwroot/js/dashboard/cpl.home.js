var Home = {
    init: function () {
        Home.bindLotteryPurchase();
        Home.bindPredictPricePrediction();
        Home.bindDoSubmitMessage();
    },
    bindDoSubmitMessage: function () {
        $("section#contact").on("click", "#btn-do-send", function () {
            var _this = this;
            var isFormValid = $('#form-message')[0].checkValidity();
            $("#form-message").addClass('was-validated');

            if (isFormValid) {
                $.ajax({
                    url: "/Home/DoSend",
                    type: "POST",
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                        $(_this).html("<i class='fa fa-spinner fa-spin'></i> " + $(_this).text());
                    },
                    data: {
                        Message: $("section#contact #Message").val(),
                        Name: $("section#contact #Name").val(),
                        PhoneNumber: $("section#contact #PhoneNumber").val(),
                        Email: $("section#contact #Email").val()
                    },
                    success: function (data) {
                        if (data.success) {
                            $("#form-message")[0].reset();
                            toastr.success(data.message, 'Success!');
                        }
                        else 
                            toastr.error(data.message, 'Error!');
                    },
                    complete: function (data) {
                        $(_this).attr("disabled", false);
                        $(_this).html($(_this).text());
                    }
                });
            }
            return false;
        });
    },
    bindLotteryPurchase: function () {
        $("#lottery-game").on("click", "#btn-lottery-purchase", function () {
            window.location.href = "/Lottery/Detail/" + $("#random-lottery-id").val() + "?lottery-category-id=" + $("#random-lottery-category-id").val() + "&lotteryTicketAmount=" + $("#lottery-ticket-amount").val();
        });
    },
    bindPredictPricePrediction: function () {
        $("#price-prediction-game").on("click", ".btn", function () {
            var _this = this;
            window.location.href = "/PricePrediction/Index?predictedTrend=" + $(_this).data().predictedTrend;
        });
    }
}


$(document).ready(function () {
    Home.init();
});