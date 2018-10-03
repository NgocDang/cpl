var Home = {
    init: function () {
        Home.loadSliderBanner();
    },
    loadSliderBanner: function () {
        $("#slide .owl-carousel").owlCarousel({
            items: 1,
            loop: true,
            dots: false,
            autoplay: true,
            smartSpeed: 2500
        });
    }
};


$(document).ready(function () {
    Home.init();
});