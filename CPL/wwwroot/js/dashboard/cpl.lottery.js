var Lottery = {
    init: function () {
        Lottery.bindPurchaseTicket();
        Lottery.bindConfirmPurchaseTicket();
        Lottery.bindCancelPurchaseTicket();
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
                            $(".ticket-price").html(new Intl.NumberFormat('vn-VN').format(data.ticketPrice) + " CPL");
                            $(".ticket-price").val(data.ticketPrice); // Set value to calculate in contronller
                            $(".total-of-tiket").html(new Intl.NumberFormat('vn-VN').format(data.totalTickets));
                            $(".total-of-tiket").val(data.totalTickets); // Set value to calculate in contronller
                            $(".total-price").html(new Intl.NumberFormat('vn-VN').format(data.totalPriceOfTickets) + " CPL");
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
        $('#btn-confirm-purchase-lottery-ticket').click(function () {
            Lottery.loadAjaxConfirmPurchaseTicket();
        })

    },
    loadAjaxConfirmPurchaseTicket() {
         $.ajax({
             url: "/Lottery/ConfirmPurchaseTicket/",
             type: "POST",
             data: {
                 TicketPrice: parseInt($(".ticket-price").val()),
                 TotalTickets: parseInt($(".total-of-tiket").val()),
                 TotalPriceOfTickets: parseInt($(".total-price").val()),
                 LotteryId: parseInt($("#Lottery_Id").val())
             },
             success: function (data) {
                 if (data.success === undefined) { // before log in
                     $("#modal").html(data);
                     $("#login-modal").modal("show");
                     $.getScript("https://www.google.com/recaptcha/api.js?hl=en");
                 }
                 else { // after log in
                     $("#login-modal").modal("hide");
                     if ($("#lottery-history").hasClass("d-none")) { // not login yet
                         $("#lottery-history").removeClass("d-none");
                         LotteryHistory.historyDatatable = LotteryHistory.loadLotteryHistoryTable();
                     }

                     Lottery.loadHeaderViewComponent(); // Reloader header view component after login
                     if (data.success) {
                         $("#div-confirm-lottery").hide();
                         $("#div-thankyou-lottery").show();
                         $("#span-txHashId").html("<a class='text-success' target='_blank' href = https://rinkeby.etherscan.io/tx/" + data.txHashId + "><u>" + data.txHashId + "</u></a>");
                         toastr.success(data.message, 'Success!');
                         LotteryHistory.historyDatatable.ajax.reload();
                     }
                     else {
                         toastr.error(data.message, 'Error!');
                     }
                 }
             },
             complete: function (data) {
             }
         });
    },
    loadHeaderViewComponent: function () {
        $.ajax({
            url: "/Home/LoadHeaderViewComponent/",
            type: "GET",
            processData: false,
            contentType: false,
            success: function (data) {
                $("#header-content").html(data);
            }
        });
    }
};

$(document).ready(function () {
    Lottery.init();
});