var PricePrediction = {
    init: function () {
        PricePrediction.bindNavbar();
        PricePrediction.binLoadFirstPricePrediction();
    },
    binLoadFirstPricePrediction: function () {
        if ($("#price-prediction-nav-" + $("#FirstGame").val()).html().trim().length == 0) {
            $.ajax({
                url: '/PricePrediction/PricePredictionViewComponent',
                type: "POST",
                data: {
                    pricePredictionId: $("#FirstGame").val(),
                },
                success: function (data) {
                    $("#price-prediction-nav-" + $("#FirstGame").val()).html(data);
                }
            });
        } else {
        }
    },
    bindNavbar: function () {
        $('a[data-toggle="tab"]').on('show.bs.tab', function (e) {
            var _this = this;
            if ($("#price-prediction-nav-" + $(_this).data().id).html().trim().length == 0) {
                $.ajax({
                    url: '/PricePrediction/PricePredictionViewComponent',
                    type: "POST",
                    data: {
                        pricePredictionId: $(_this).data().id,
                    },
                    success: function (data) {
                        $("#price-prediction-nav-" + $(_this).data().id).html(data);
                    }
                });
            } else {
            }
        })
    }
};

$(document).ready(function () {
    PricePrediction.init();
});