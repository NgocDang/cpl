var Home = {
    init: function () {
        Home.loadSliderGame();
    },
    loadSliderGame: function () {
        $("#lottery-game .owl-carousel").owlCarousel({
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