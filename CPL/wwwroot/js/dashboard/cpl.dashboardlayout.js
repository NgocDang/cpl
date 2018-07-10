var DashboardLayout = {
    init: function () {
        DashboardLayout.bindLogout();
        DashboardLayout.bindConfirmLogout();
        DashboardLayout.bindWinButton();
    },
    bindLogout: function () {
        $('.navbar-nav').on('click', '.logout-confirmation', function () {
            var _this = this;
            $.ajax({
                url: "/Authentication/GetConfirm/",
                type: "GET",
                success: function (data) {
                    $("#modal").html(data);
                    $("#logout").modal("show");
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                    $(_this).html($(_this).text());
                }
            });
        })
    },
    bindConfirmLogout: function () {
        $('#modal').on('click', '#btn-confirm-logout', function () {
            window.location.href = '/Authentication/Logout/';
        })
    },
    bindWinButton: function () {
        if ($("#NotificationStatus").val() == "True") {
            $("#winner-notification").show();

            $('.navbar-nav').on('click', '#btn-win', function () {
                if ($("#KYCStatus").val() == "True") {
                    window.location.replace('/Profile/EditSecurity');
                }
            })
        }
    }
};


$(document).ready(function () {
    DashboardLayout.init();
});