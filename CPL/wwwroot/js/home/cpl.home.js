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

        $('.owlSlider').on('initialized.owl.carousel resized.owl.carousel', function (event) {
            if ($('.owlSlider').length) {
                var screenWidth = Home.screenWidth();
                var slider_items = $('.owlSlider').find('a');
                $.each(slider_items, function (i, v) {
                    var slider_item = $(this).data('medium');
                    if (screenWidth > 767) {
                        if (screenWidth <= 1024) {
                            slider_item = $(this).data('large');
                        } else {
                            slider_item = $(this).data('origin');
                        }
                    }
                    $(this).attr('style', 'background-image:url(' + slider_item + ')');
                    $(this).html('<img src="' + slider_item + '">');
                });
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