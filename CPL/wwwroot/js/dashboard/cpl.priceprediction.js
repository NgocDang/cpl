var PricePrediction = {
    init: function () {
        PricePrediction.bindNavbar();
    },
    bindNavbar: function () {
        $('a[data-toggle="tab"]').on('show.bs.tab', function (e) {
            var _this = this;
            if ($("#price-prediction-nav-" + $(_this).data().id).html().trim().length == 0) {
                $.ajax({
                    url: '/ViewComponent/GetPricePredictionViewComponent',
                    type: "POST",
                    data: {
                        id: $(_this).data().id,
                    },
                    success: function (data) {
                        $("#price-prediction-nav-" + $(_this).data().id).html(data);
                    }
                });
            } 
        })
    }
};

$(document).ready(function () {
    PricePrediction.init();
});