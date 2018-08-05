/// <reference path="cpl.login.js" />
/// <reference path="cpl.login.js" />
var Lottery = {
    historyDatatable: null,
    init: function () {
        Lottery.bindPurchaseTicket();
        Lottery.bindConfirmPurchaseTicket();
        Lottery.bindCancelPurchaseTicket();
        Lottery.historyDatatable = Lottery.loadLotteryHistoryTable();
    },
    bindPurchaseTicket: function () {
        $('#form-purchase-lottery').on('click', '#btn-purchase-lottery', function () {
            var _this = this;
            var isFromValid = $('#form-purchase-lottery').valid();
            if (isFromValid) {
                $.ajax({
                    url: "/Lottery/GetConfirmPurchaseTicket/",
                    type: "GET",
                    beforeSend: function () {
                        $("#btn-purchase-lottery").attr("disabled", true);
                        $("#btn-purchase-lottery").html("<i class='fa fa-spinner fa-spin'></i> <i class='fas fa-money-bill-alt'></i> " + $(_this).text().trim());
                    },
                    data: {
                        amount: parseInt($("#number-of-ticket").val())
                    },
                    success: function (data) {
                        if (data.url == null) {
                            $("#div-buy-lottery").hide();
                            $(".ticket-price").html(data.ticketPrice + " CPL");
                            $(".ticket-price").val(data.ticketPrice); // Set value to calculate in contronller
                            $(".total-of-tiket").html(data.totalTickets);
                            $(".total-of-tiket").val(data.totalTickets); // Set value to calculate in contronller
                            $(".total-price").html(data.totalPriceOfTickets + " CPL");
                            $(".total-price").val(data.totalPriceOfTickets); // Set value to calculate in contronller
                            $("#div-confirm-lottery").show();
                        }
                        else {
                            window.location.replace(data.url);
                        }
                    },
                    complete: function (data) {
                        $(_this).attr("disabled", false);
                        $(_this).html("<i class='fas fa-money-bill-alt'></i> " + $(_this).text().trim());
                    }
                });
            }
        })
    },
    bindCancelPurchaseTicket: function () {
        $('#btn-cancel-purchase-lottery-ticket').click(function () {
            $("#div-buy-lottery").show();
            $("#div-confirm-lottery").hide();
        });
    },
    bindConfirmPurchaseTicket: function () {
        $('#btn-confirm-purchase-lottery-ticket').click( function () {
            var _this = this;
            $.ajax({
                url: "/Lottery/ConfirmPurchaseTicket/",
                type: "POST",
                data: {
                    TicketPrice: parseInt($("#ticket-price").val()),
                    TotalTickets: parseInt($("#total-of-tiket").val()),
                    TotalPriceOfTickets: parseInt($("#total-price").val()),
                },
                success: function (data) {
                    $("#modal").html(data);
                    $("#login-modal").modal("show");
                    $.getScript("https://www.google.com/recaptcha/api.js?hl=en");
                    $.getScript("/js/dashboard/cpl.login.js");
                },
                complete: function (data) {
                    $("#purchase-lottery-ticket").modal("hide");
                }
            });
        })
    },
    loadLotteryHistoryTable: function () {
        if ($("#dt-lottery-history").length == 0)
            return false;

        return $("#dt-lottery-history").DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "ajax": {
                url: "/Lottery/SearchLotteryHistory",
                type: 'POST'
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
                            return "<div class='badge badge-warning'>KYC Pending</div>";
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
    }
};

$(document).ready(function () {
    Lottery.init();
});