var ViewComponent = {
    getRateViewComponent: function () {
        $.ajax({
            url: "/ViewComponent/GetRateViewComponent/",
            type: "GET",
            processData: false,
            contentType: false,
            success: function (data) {
                $("#rate-component").html(data);
            }
        });
    },
    getExchangeViewComponent: function () {
        $.ajax({
            url: "/ViewComponent/GetExchangeViewComponent/",
            type: "GET",
            processData: false,
            contentType: false,
            success: function (data) {
                $("#exchange-content").html(data);
            }
        });
    },
    getDepositWithdrawViewComponent: function (tab) {
        $.ajax({
            url: "/ViewComponent/GetDepositWithdrawViewComponent/",
            type: "GET",
            processData: false,
            contentType: false,
            success: function (data) {
                $("#depositwithdraw-content").html(data);
                if (tab == "withdraw") {
                    $("#deposit-nav-tab").removeClass("active");
                    $("#deposit-nav").removeClass("active");
                    $("#withdraw-nav-tab").addClass("active");
                    $("#withdraw-nav").addClass("active");
                }
            }
        });
    }
};
