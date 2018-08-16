var LotteryHistory = {
    historyDatatable: null,
    init: function () {
        LotteryHistory.historyDatatable = LotteryHistory.loadLotteryHistoryTable();
    },
    loadLotteryHistoryTable: function () {
        if ($("#lottery-history-view-component").hasClass("d-none"))
            return false;
        var _this = this;
        return $("#dt-lottery-history").DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "ajax": {
                url: "/History/SearchLotteryHistory",
                type: 'POST',
                data: {
                    lotteryId: $("#lottery-history-view-component #LotteryId").val(),
                    createdDate: $("#lottery-history-view-component #CreatedDate").val(),
                    sysUserId: $("#lottery-history-view-component #SysUserId").val()
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
                    "data": "LotteryPhase",
                    "render": function (data, type, full, meta) {
                        return full.lotteryPhaseInString;
                    }
                },
                {
                    "data": "LotteryStartDate",
                    "render": function (data, type, full, meta) {
                        return full.lotteryStartDateInString;
                    }
                },
                {
                    "data": "TicketNumber",
                    "render": function (data, type, full, meta) {
                        return full.ticketNumber;
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
                        return full.awardInString;
                    }
                },
                {
                    "data": "UpdatedDateInString",
                    "render": function (data, type, full, meta) {
                        return full.updatedDateInString;
                    }
                },
            ],
        });
    },
};

$(document).ready(function () {
    LotteryHistory.init();
});