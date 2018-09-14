var PurchasedLotteryHistoryViewComponent = {
    lotteryHistoryDataTable: null,
    init: function () {
        PurchasedLotteryHistoryViewComponent.loadLotteryHistoryDataTable()
    },
    loadLotteryHistoryDataTable: function () {
        return $("#dt-purchased-lottery-history").DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "ajax": {
                url: "/Admin/SearchPurchasedLotteryHistory",
                type: 'POST',
                data: {
                    lotteryCategoryId: $("#lottery-category-Id").val(), // To be assigned later
                }
            },
            "language": DTLang.getLang(),
            "columns": [
                {
                    "data": "UserName",
                    "render": function (data, type, full, meta) {
                        return full.userName;
                    }
                },
                {
                    "data": "StatusInString",
                    "render": function (data, type, full, meta) {
                        if (full.status == 1) {
                            return "<p class='text-sm-center'><span class='badge badge-info'>" + $("#pending").val() + "</span></p>";
                        }
                        else if (full.status == 2) {
                            return "<p class='text-sm-center'><span class='badge badge-success'>" + $("#active").val() + "</span></p>";
                        }
                        else if (full.status == 3) {
                            return "<p class='text-sm-center'><span class='badge badge-secondary'>" + $("#completed").val() + "</span></p>";
                        }
                        else if (full.status == 4) {
                            return "<p class='text-sm-center'><span class='badge badge-warning'>" + $("#deactivated").val() + "</span></p>";
                        }
                        else {
                            return "";
                        }
                    }
                },
                {
                    "data": "NumberOfTicket",
                    "render": function (data, type, full, meta) {
                        return full.numberOfTicket;
                    }
                },
                {
                    "data": "NumberOfTicketInString",
                    "render": function (data, type, full, meta) {
                        return full.totalPurchasePrice;
                    }
                },
                {
                    "data": "Title",
                    "render": function (data, type, full, meta) {
                        return full.title;
                    }
                },
                {
                    "data": "PurchaseDateTimeInString",
                    "render": function (data, type, full, meta) {
                        return full.purchaseDateTimeInString;
                    }
                },
                {
                    "data": "Details",
                    "render": function (data, type, full, meta) {
                        var html = "<a style='line-height:12px;margin:2px' href='/Admin/User/" + full.sysUserId + "' target='_blank' class='btn btn-sm btn-outline-secondary btn-view'>" + $("#view").val() + "</a>";
                        return html;
                    },
                    "orderable": false
                }
            ],
        });
    },
};

$(document).ready(function () {
    PurchasedLotteryHistoryViewComponent.init();
});
