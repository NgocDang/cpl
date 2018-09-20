var Lottery = {
    init: function () {
        Lottery.bindSpinTicketNumber();
        Lottery.bindPurchaseTicket();
        Lottery.bindDoPurchaseTicket();
        Lottery.bindCancel();
    },
    bindSpinTicketNumber: function () {
        $('#form-purchase-lottery').on('change paste keyup input', '#number-of-ticket', function () {
            var isFormValid = $('#form-purchase-lottery')[0].checkValidity();
            $("#form-purchase-lottery").addClass('was-validated');
            if (isFormValid) {
                $("#total-amount").html(new Intl.NumberFormat('ja-JP').format($("#number-of-ticket").data().unitPrice * $("#number-of-ticket").val()));
            } else 
                $("#total-amount").html(0);
        });
    },
    bindPurchaseTicket: function () {
        $('#form-purchase-lottery').on('click', '#btn-purchase-lottery', function () {
            var _this = this;
            var isFormValid = $('#form-purchase-lottery')[0].checkValidity();
            $("#form-purchase-lottery").addClass('was-validated');

            if (isFormValid) {
                $.ajax({
                    url: "/Lottery/GetConfirmPurchaseTicket/",
                    type: "GET",
                    beforeSend: function () {
                        $("#btn-purchase-lottery").attr("disabled", true);
                        $("#btn-purchase-lottery").html("<i class='fa fa-spinner fa-spin'></i> <i class='fas fa-money-bill-alt'></i> " + $(_this).text().trim());
                    },
                    data: {
                        amount: parseInt($("#number-of-ticket").val()),
                        lotteryId: parseInt($("#div-confirm-lottery #Id").val())
                    },
                    success: function (data) {
                        if (data.url == null) {
                            $("#div-buy-lottery").toggleClass("d-none");
                            $(".ticket-price").html(new Intl.NumberFormat('vn-VN').format(data.ticketPrice) + " CPL");
                            $(".ticket-price").val(data.ticketPrice); // Set value to calculate in contronller
                            $(".total-of-tiket").html(new Intl.NumberFormat('vn-VN').format(data.totalTickets));
                            $(".total-of-tiket").val(data.totalTickets); // Set value to calculate in contronller
                            $(".total-price").html(new Intl.NumberFormat('vn-VN').format(data.totalPriceOfTickets) + " CPL");
                            $(".total-price").val(data.totalPriceOfTickets); // Set value to calculate in contronller
                            $("#div-confirm-lottery").toggleClass("d-none");
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
    bindCancel: function () {
        $('#btn-cancel-purchase-lottery-ticket').click(function () {
            $("#div-buy-lottery").toggleClass("d-none");
            $("#div-confirm-lottery").toggleClass("d-none");
        });
    },
    bindDoPurchaseTicket: function () {
        $('#btn-confirm-purchase-lottery-ticket').click(function () {
            Lottery.loadAjaxConfirmPurchaseTicket();
        });

    },
    loadAjaxConfirmPurchaseTicket: function () {
         $.ajax({
             url: "/Lottery/ConfirmPurchaseTicket/",
             type: "POST",
             beforeSend: function () {
                 $("#btn-confirm-purchase-lottery-ticket").attr("disabled", true);
                 $("#btn-confirm-purchase-lottery-ticket").html("<i class='fa fa-spinner fa-spin'></i> " + $("#btn-confirm-purchase-lottery-ticket").text());
             },
             data: {
                 TicketPrice: parseInt($(".ticket-price").val()),
                 TotalTickets: parseInt($(".total-of-tiket").val()),
                 TotalPriceOfTickets: parseInt($(".total-price").val()),
                 LotteryId: parseInt($("#div-confirm-lottery #Id").val())
             },
             success: function (data) {
                 if (data.success === undefined) { // before log in
                     $("#modal").html(data);
                     $("#login-modal").modal("show");
                     $.getScript("https://www.google.com/recaptcha/api.js?hl=en");
                 }
                 else { // after log in
                     $("#login-modal").modal("hide");
                     if ($("#lottery-history-view-component").hasClass("d-none")) { // not login yet
                         $("#lottery-history-view-component").removeClass("d-none");
                         LotteryHistoryViewComponent.lotteryHistoryDataTable = LotteryHistoryViewComponent.loadLotteryHistoryDataTable();
                     }

                     if (data.success) {
                         $("#div-confirm-lottery").toggleClass("d-none");
                         $("#div-thankyou-lottery").toggleClass("d-none");
                         //$("#p-txHashId").html("<a class='text-success' target='_blank' href='" + data.tx + "'><small><u>" + data.tx + "</u></small></a>");
                         $("#hint-thankyou-lottery").html("<small class='text-success'>" + data.hintThankyou + "</small>");
                         toastr.success(data.message, 'Success!');
                         $(".user-token-amount").map(function (index, element) {
                             $(element).text(data.token + " CPL");
                         });
                         ViewComponent.getRateViewComponent();
                         LotteryHistoryViewComponent.lotteryHistoryDataTable.ajax.reload();
                     }
                     else {
                         toastr.error(data.message, 'Error!');
                         $("#btn-confirm-purchase-lottery-ticket").attr("disabled", false);
                         $("#btn-confirm-purchase-lottery-ticket").html($("#btn-confirm-purchase-lottery-ticket").text().trim() + ' <i class="fa fa-arrow-right"></i>');
                     }
                 }
             },
             complete: function (data) {
             }
         });
    }
};

$(document).ready(function () {
    Lottery.init();
});