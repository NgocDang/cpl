var Home = {
    init: function () {
        Home.bindLotteryPurchase();
        Home.bindPredictPricePrediction();
    },
    bindLotteryPurchase: function () {
        $("#lottery-game").on("click", "#lottery-purchase", function () {
            window.location.href = "/Lottery/Detail/" + $("#lottery-id").val() + "?lottery-category-id=" + $("#lottery-category-id").val() + "&lotteryTicketAmount=" + $("#lottery-ticket-amount").val();
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