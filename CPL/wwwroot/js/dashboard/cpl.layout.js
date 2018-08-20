var Layout = {
    init: function () {
        Layout.bindConfirmLogOut();
        Layout.bindDropdownSubMenu();
    },
    bindLogOut: function () {
        $('a#logout-confirmation').on('click', function () {
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
    bindSwitchLanguage: function () {
        $("a.lang-item").on("click", function () {
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
    },
    bindDropdownSubMenu: function () {
        $('.header-navbar').on("click", "#btn-dropdown-item", function (e) {
            $(this).next('ul').toggle();
            e.stopPropagation();
            e.preventDefault();
        });
    }
};


$(document).ready(function () {
    Layout.init();
});

window.onload = function () {
   
};