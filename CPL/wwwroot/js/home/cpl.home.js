var Home = {
    init: function () {
        Home.loadSlider();
        Home.loadDrawer();
        Home.bindContact();
    },
    bindContact: function () {
        $("#form-contact").on("click", ".btn-send-message", function (){
            var isFormValid = $("#form-contact").valid();
            if (isFormValid) {
                $.ajax({
                    url: "/Home/Contact/",
                    type: "POST",
                    beforeSend: function () {
                        $(".btn-send-message").attr("disabled", true);
                        $(".btn-send-message").html("<i class='fa fa-spinner fa-spin'></i> " + $(".btn-send-message").text());
                    },
                    data: {
                        Name: $("#Name").val(),
                        Email: $("#Email").val(),
                        Message: $("#Message").val()
                    },
                    success: function (data) {
                        if (data.success)
                            toastr.success(data.message);
                        else
                            toastr.error(data.message);
                    },
                    complete: function (data) {
                        $(".btn-send-message").attr("disabled", false);
                        $(".btn-send-message").html($(".btn-send-message").text());
                    }
                });
            }
        });
    },
    loadDrawer: function() {
        $('.drawer').drawer({
            class: {
                nav: 'drawer-nav',
                toggle: 'drawer-toggle',
                overlay: 'drawer-overlay',
                open: 'drawer-open',
                close: 'drawer-close',
                dropdown: 'drawer-dropdown'
            },
            iscroll: {
                mouseWheel: true,
                preventDefault: false
            },
            showOverlay: true
        });
    },
    loadSlider: function () {
        $('.owlSlider').addClass('owl-carousel')
            .addClass('owl-theme')
            .owlCarousel({
                loop: true,
                center: true,
                nav: true,
                navText: ['', ''],
                autoplay: true,
                smartSpeed: 1000,
                responsive: {
                    0: {
                        margin: 5,
                        items: 1
                    },
                    991: {
                        margin: 5,
                        items: 1
                    },
                    992: {
                        autoWidth: true,
                        margin: 30,
                        items: 1.25
                    }
                }
            });
    },
    screenWidth: function screenWidth() {
        var width = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
        if (width && width !== false) {
            return width;
        }
    },
}


$(document).ready(function () {
    Home.init();
});