var AmountViewComponent = {
    init: function () {
        AmountViewComponent.loadSlider();
    },
    loadSlider: function () {
        $('.owl-carousel').owlCarousel({
            loop: true,
            margin: 10,
            auto: true,
            dots: false,
            autoplay: true,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 1
                },
                768: {
                    items: 2
                },
                1024: {
                    items: 1
                },
                1440: {
                    items: 2
                },
                2560: {
                    items: 3
                }
            }
        });
    }
}

$(document).ready(function () {
    AmountViewComponent.init();
});