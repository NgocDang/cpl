var HowToPlay = {
    init: function () {
        HowToPlay.bindViewDesktopVersion();
        HowToPlay.bindViewMobileVersion();
    },
    bindViewDesktopVersion: function () {
        $("#how-to-play").on("click", ".btn-desktop-version", function () {
            $("#how-to-play-desktop-version").show();
            $("#how-to-play-mobile-version").hide();
            $(".btn.btn-mobile-version").removeClass("active");
            $(".btn.btn-desktop-version").addClass("active");
        });
    },
    bindViewMobileVersion: function () {
        $("#how-to-play").on("click", ".btn-mobile-version", function () {
            $("#how-to-play-desktop-version").hide();
            $("#how-to-play-mobile-version").show();
            $(".btn.btn-desktop-version").removeClass("active");
            $(".btn.btn-mobile-version").addClass("active");
        });
    }
};


$(document).ready(function () {
    HowToPlay.init();
});