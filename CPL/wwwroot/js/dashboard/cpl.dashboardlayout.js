var DashboardLayout = {
    init: function () {
        DashboardLayout.bindLogout();
        DashboardLayout.bindConfirmLogout();
    },
    bindLogout: function () {
        $('.navbar-nav').on('click', '.logout-confirmation', function () {
            var _this = this;
            $.ajax({
                url: "/Authentication/GetConfirm/",
                type: "GET",
                success: function (data) {
                    $("#modal").html(data);
                    $("#exchange").modal("show");
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
    }
};


$(document).ready(function () {
    DashboardLayout.init();
});