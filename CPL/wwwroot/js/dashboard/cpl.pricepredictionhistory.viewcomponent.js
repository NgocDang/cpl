var PricePredictionHistoryViewComponent = {
    pricePredictionHistoryDataTable: null,
    init: function () {
        // Not implement yet
    },
    resizeOnMobile: function () {
        if ($(window).width() < 767 && $("#price-prediction-history-view-component .card-header").length > 0) {
            $('#price-prediction-history-view-component').on('click', '#txt-collapse', function () {
                if ($('#price-prediction-history-view-component .card-header .ft-minus').length === 1 && $("#dt-price-prediction-history").find("tbody").length === 0) {
                    // Load 1 time when press on minus button in mobile version
                    PricePredictionHistoryViewComponent.pricePredictionHistoryDataTable = PricePredictionHistoryViewComponent.loadPricePredictionHistoryDataTable();
                }
            })
            $('#price-prediction-history-view-component .card-content').removeClass('show');
            $('#price-prediction-history-view-component .card-header i').removeClass('ft-minus');
            $('#price-prediction-history-view-component .card-header i').addClass('ft-plus');
        } else {
            $('#price-prediction-history-view-component .card-content').addClass('show');
            $('#price-prediction-history-view-component .card-header i').removeClass('ft-plus');
            $('#price-prediction-history-view-component .card-header i').addClass('ft-minus');
            PricePredictionHistoryViewComponent.pricePredictionHistoryDataTable = PricePredictionHistoryViewComponent.loadPricePredictionHistoryDataTable();
        }
    },
    loadPricePredictionHistoryDataTable: function () {
        if ($("#price-prediction-history-view-component").hasClass("d-none"))
            return false;
        var _this = this;
        return $("#dt-price-prediction-history").DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "ajax": {
                url: "/History/SearchPricePredictionHistory",
                type: 'POST',
                data: {
                    sysUserId: $("#price-prediction-history-view-component #SysUserId").val()
                }
            },
            "language": DTLang.getLang(),
            "columns": [
                {
                    "data": "Bet",
                    "className": "text-center",
                    "render": function (data, type, full, meta) {
                        return full.bet;
                    }
                },
                {
                    "data": "StartRate",
                    "render": function (data, type, full, meta) {
                        return full.toBeComparedPriceInString;
                    }
                },
                {
                    "data": "ResultRate",
                    "render": function (data, type, full, meta) {
                        return full.resultPriceInString;
                    }
                },
                {
                    "data": "Amount",
                    "render": function (data, type, full, meta) {
                        return full.amountInString;
                    }
                },
                {
                    "data": "Status",
                    "render": function (data, type, full, meta) {
                        if (full.status === "ACTIVE") {
                            return "<div class='badge badge-success'>" + full.status + "</div>";
                        }
                        else {
                            return "<div class='badge badge-secondary'>" + full.status + "</div>";
                        }
                    }
                },
                {
                    "data": "Result",
                    "render": function (data, type, full, meta) {
                        if (full.result == "Win")
                            return "<div class='badge badge-success'>Win</div>";
                        else if (full.result == "Lose")
                            return "<div class='badge badge-danger'>Lose</div>";
                        else if (full.result == "KYC Pending")
                            return "<div class='badge badge-info'>KYC Pending</div>";
                        else
                            return "";
                    }
                },
                {
                    "data": "Award",
                    "render": function (data, type, full, meta) {
                        return full.bonusInString;
                    }
                },
                {
                    "data": "ResultTime",
                    "render": function (data, type, full, meta) {
                        return full.resultTimeInString;
                    }
                },
                {
                    "data": "PurcharseTime",
                    "render": function (data, type, full, meta) {
                        return full.purcharseTimeInString;
                    }
                },
            ],
        });
    },
};

$(document).ready(function () {
    PricePredictionHistoryViewComponent.init();
});

$(window).bind('load', function () {
    PricePredictionHistoryViewComponent.resizeOnMobile();
});