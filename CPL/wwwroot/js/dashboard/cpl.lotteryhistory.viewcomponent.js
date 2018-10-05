var LotteryHistoryViewComponent = {
    lotteryHistoryDataTable: null,
    init: function () {
        // Not implement yet
    },
    resizeOnMobile: function () {
        if ($(window).width() < 767 && $("#lottery-history-view-component .card-header").length > 0) {
            $('#lottery-history-view-component').on('click', '#txt-collapse', function () {
                if ($('#lottery-history-view-component .card-header .ft-minus').length === 1 && $("#dt-lottery-history").find("tbody").length === 0) {
                    // Load 1 time when press on minus button in mobile version
                    LotteryHistoryViewComponent.lotteryHistoryDataTable = LotteryHistoryViewComponent.loadLotteryHistoryDataTable();
                }
            })
            $('#lottery-history-view-component .card-content').removeClass('show');
            $('#lottery-history-view-component .card-header i').removeClass('ft-minus');
            $('#lottery-history-view-component .card-header i').addClass('ft-plus');
        } else {
            $('#lottery-history-view-component .card-content').addClass('show');
            $('#lottery-history-view-component .card-header i').removeClass('ft-plus');
            $('#lottery-history-view-component .card-header i').addClass('ft-minus');
            LotteryHistoryViewComponent.lotteryHistoryDataTable = LotteryHistoryViewComponent.loadLotteryHistoryDataTable();
        }
    },
    loadLotteryHistoryDataTable: function () {
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
    LotteryHistoryViewComponent.init();
});

$(window).bind('load', function () {
    LotteryHistoryViewComponent.resizeOnMobile();
});