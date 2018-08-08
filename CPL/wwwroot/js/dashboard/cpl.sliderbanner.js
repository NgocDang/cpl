var SliderBanner = {
    init: function () {
        SliderBanner.loadSliderBanner();
    },
    loadSliderBanner: function () {
        $("#slider-banner .owl-carousel").owlCarousel({
            items: 2,
            loop: true,
            center: true,
            merge: true,
            margin: 20,
            autoplay: true,
            smartSpeed: 1000,
            responsive: {
                320: {
                    items: 1,
                    dots: false,
                },
                768: {
                    items: 2,
                    center: true,
                }
            }, 
        });
        
    },
}

$(document).ready(function () {
    SliderBanner.init();
});