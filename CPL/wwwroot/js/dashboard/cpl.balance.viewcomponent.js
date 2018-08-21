var BalanceViewComponent = {
    init: function () {
        BalanceViewComponent.loadSlider();
    },
    loadSlider: function () {
        $('.owl-carousel').owlCarousel({
            loop: true,
            margin: 10,
            auto: true,
            dots: false,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 1,
                    autoplay: true
                },
                768: {
                    items: 1,
                    autoplay: true
                },
                1024: {
                    items: 1,
                    autoplay: true
                },
                1440: {
                    items: 2
                },
                2560: {
                    items: 2
                }
            }
        });
    }
}

$(document).ready(function () {
    BalanceViewComponent.init();
});