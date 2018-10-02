var Home = {
    init: function () {
        Home.loadSliderBanner();
    },
    loadSliderBanner: function () {
        $("#slide .owl-carousel").owlCarousel({
            items: 1,
            loop: true,
            autoplay: true,
            dots: false
        });
    }
};


$(document).ready(function () {
    Home.init();
});