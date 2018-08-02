var Home = {
    init: function () {
        Home.loadSliderBanner();
        Home.loadSliderGame();
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
    loadSliderGame: function () {
        $("#slider-game .owl-carousel").owlCarousel({
            items: 4,
            loop: true,
            margin: 5,
            merge: true,
            autoplay: true,
            nav: true,
            dots: false,
            smartSpeed: 1000,
            responsive: {
                320: {
                    items: 1,
                },
                768: {
                    items: 3,
                },
                1000: {
                    items: 4,
                }
            },
        });
    }
}


$(document).ready(function () {
    Home.init();
});