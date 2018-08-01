var Home = {
    init: function () {
        Home.loadSlider();
    },
    loadSlider: function () {
        $("#slider-banner .owl-carousel").owlCarousel({
            items: 2,
            loop: true,
            center: true,
            merge: true,
            margin: 25,
            autoplay: true,
            smartSpeed: 1000,
            responsive: {
            }, 
        });
        $("#slider-game .owl-carousel").owlCarousel({
            items: 4,
            loop: true,
            margin: 5,
            autoplay: true,
            smartSpeed: 1000,
            responsive: {
            },
        });
    }
}


$(document).ready(function () {
    Home.init();
});