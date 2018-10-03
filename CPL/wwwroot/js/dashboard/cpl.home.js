var Home = {
    init: function () {
        Home.loadSliderBanner();
        Home.bindLotteryPurchase();
        Home.bindPredictPricePrediction();
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
    },

    loadSliderBanner: function () {
        $("#slide .owl-carousel").owlCarousel({
            items: 1,
            loop: true,
            dots: false,
            autoplay: true,
            smartSpeed: 2500
        });
    }
};


$(document).ready(function () {
    Home.init();
});