var GameHistoryViewComponent = {
    init: function () {
        GameHistoryViewComponent.loadGameHistoryDataTable();
    },
    loadGameHistoryDataTable: function () {
        var sysUserId = $("#game-history-view-component #Id").val();
        $('#dt-history').DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "ajax": {
                url: "/History/SearchGameHistory",
                type: 'POST',
                data: {
                    sysUserId: $("#game-history-view-component #SysUserId").val()
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
                        return full.gameType;
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
                        else if (full.result == "REFUND")
                            return "<div class='badge badge-info'>Refund</div>";
                        else
                            return "";
                    }
                },
                {
                    "data": "Award",
                    "render": function (data, type, full, meta) {
                        if (full.result == "")
                            return "";
                        else 
                            return full.awardInString;
                    }
                },
                {
                    "data": "Balance",
                    "render": function (data, type, full, meta) {
                        if (full.result == "")
                            return "";
                        else
                            return full.balanceInString;
                    }
                },
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        if (full.gameType == "Lottery")
                            return "<a href='/History/Lottery?createdDate=" + full.createdDate + "&lotteryId=" + full.gameId + "&sysUserId=" + $("#SysUserId").val() + "' target='_blank'  data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-view'>" + $("#view").val() + "</a>";
                        else if (full.gameType == "Price Prediction")
                            return "<a href='/History/PricePrediction?pricePredictionId=" + full.gameId + "' target='_blank'  data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-view'>" + $("#view").val() + "</a>";
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