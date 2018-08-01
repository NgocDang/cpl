var Layout = {
    init: function () {
        Layout.bindLogOut();
        Layout.bindConfirmLogOut();
        Layout.bindSwitchLanguage();
        //DashboardLayout.bindWinButton();
    },
    bindLogOut: function () {
        $('.header-navbar').on('click', '#logout-confirmation', function () {
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
            return false;
        });
    },
    bindConfirmLogOut: function () {
        $('#modal').on('click', '#btn-confirm-logout', function () {
            window.location.href = '/Authentication/LogOut/';
        });
    },
    bindWinButton: function () {
        if ($("#NotificationStatus").val().toLowerCase() == "true") {
            $("#winner-notification").show();

            $('.navbar-nav').on('click', '#btn-win', function () {
                if ($("#KYCStatus").val().toLowerCase() == "false") {
                    window.location.replace('/Profile/EditSecurity');
                }
            })
        }
    },
    bindSwitchLanguage: function () {
        $("body").on("click", ".lang-item", function () {
            var _this = this;
            $.ajax({
                url: "/Layout/SwitchLang/",
                type: "POST",
                beforeSend: function () {
                },
                data: {
                    id: $(_this).data().id,
                    url: window.location.href,
                },
                success: function (data) {
                    if (data.success) {
                        window.location.reload();
                    } else {
                        toastr.error(data.message, 'Error!');
                    }
                },
                complete: function (data) {
                }
            });
        });
    }
};


$(document).ready(function () {
    Layout.init();
});