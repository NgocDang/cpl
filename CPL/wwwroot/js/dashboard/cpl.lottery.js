var Lottery = {
    init: function () {
        Lottery.bindPurchaseTicket();
        Lottery.bindConfirmPurchaseTicket();
        Lottery.bindColorPrize();
    },
    bindPurchaseTicket: function () {
        $('#form-purchase-lottery').on('click', '#btn-purchase-lottery', function () {
            var _this = this;
            var isFromValid = $('#form-purchase-lottery').valid();
            $("#number-of-ticket-error").addClass("text-danger");
            var _numberOfTickets = parseInt($("#number-of-ticket").val());
            if (_numberOfTickets < 0 || _numberOfTickets > 5000) {
                return false;
            }
            else {
                $.ajax({
                    url: "/Lottery/GetConfirmPurchaseTicket/",
                    type: "GET",
                    beforeSend: function () {
                        $("#btn-purchase-lottery").attr("disabled", true);
                        $("#btn-purchase-lottery").html("<i class='fa fa-spinner fa-spin'></i> <i class='fas fa-money-bill-alt'></i> " + $(_this).text().trim());
                    },
                    data: {
                        amount: _numberOfTickets
                    },
                    success: function (data) {
                        if (data.url == null) {
                            $("#modal").html(data);
                            $("#purchase-lottery-ticket").modal("show");
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
            };
        })
    },
    bindConfirmPurchaseTicket: function () {
        $('#modal').on('click', '#btn-confirm-purchase-lottery-ticket', function () {
            var _this = this;
            $.ajax({
                url: "/Lottery/ConfirmPurchaseTicket/",
                type: "POST",
                data: {
                    TicketPrice: parseInt($("#TicketPrice").val()),
                    TotalTickets: parseInt($("#TotalTickets").val()),
                    TotalPriceOfTickets: parseInt($("#TotalPriceOfTickets").val()),
                },
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message, 'Success!');
                    }
                    else {
                        toastr.error(data.message, 'Error!');
                    }
                },
                complete: function (data) {
                    $("#purchase-lottery-ticket").modal("hide");
                }
            });
        })
    },
    bindColorPrize: function () {
        $("#1st").addClass('bg-warning');
        $("#2nd").addClass('bg-primary');
        $("#3rd").addClass('bg-danger');
        $("#4th").addClass('bg-success');
    }
};


$(document).ready(function () {
    Lottery.init();
});