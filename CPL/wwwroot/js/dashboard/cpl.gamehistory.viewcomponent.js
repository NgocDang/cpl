var GameHistoryViewComponent = {
    init: function () {
        GameHistoryViewComponent.loadHistoryDatatable();
    },
    loadHistoryDatatable: function () {
        var sysUserId = $("#game-history-view-component #Id").val();
        $('#dt-history').DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "ajax": {
                url: "/History/SearchGameHistory",
                type: 'POST',
                data: {
                    userId: $("#Id").val()
                }
            },
            "language": DTLang.getLang(),
            "columns": [
                {
                    "data": "CreatedDate",
                    "render": function (data, type, full, meta) {
                        return full.createdDateInString;
                    }
                },
                {
                    "data": "CreatedTimeInString",
                    "render": function (data, type, full, meta) {
                        return full.createdTimeInString;
                    }
                },
                {
                    "data": "GameType",
                    "render": function (data, type, full, meta) {
                        if (full.gameType == "LOTTERY")
                            return "Lottery";
                        else if (full.gameType == "PRICE_PREDICTION")
                            return "Price Prediction"
                    }
                },
                {
                    "data": "Amount",
                    "render": function (data, type, full, meta) {
                        return "<i class='cpl-token-grey-sm'></i> " + full.amountInString;
                    }
                },
                {
                    "data": "Result",
                    "render": function (data, type, full, meta) {
                        if (full.result == "WIN")
                            return "<div class='badge badge-success'>Win</div>";
                        else if (full.result == "LOSE")
                            return "<div class='badge badge-danger'>Lose</div>";
                        else if (full.result == "KYC_PENDING")
                            return "<div class='badge badge-info'>KYC Pending</div>";
                        else
                            return "";
                    }
                },
                {
                    "data": "Award",
                    "render": function (data, type, full, meta) {
                        return full.awardInString;
                    }
                },
                {
                    "data": "Balance",
                    "render": function (data, type, full, meta) {
                        return full.balanceInString;
                    }
                },
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        if (full.gameType == "LOTTERY")
                            return "<a style='line-height:12px;' href='/History/Lottery?createdDate=" + full.createdDate + "&lotteryId=" + full.gameId + "&sysUserId=" + sysUserId + "' target='_blank'  data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-view'>View</a>";
                        else if (full.gameType == "PRICE_PREDICTION")
                            return "<a style='line-height:12px;' href='/History/PricePrediction?id=" + full.gameId + "' target='_blank'  data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-view'>View</a>";
                    },
                    "orderable": false
                }
            ],
        });
    },
}

$(document).ready(function () {
    GameHistoryViewComponent.init();
});