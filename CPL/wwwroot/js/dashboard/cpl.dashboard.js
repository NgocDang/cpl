var Dashboard = {
    init: function () {
        Dashboard.loadSlider();
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
    },
    bindCopy: function () {
        if ($(".btn-copy").length > 0) {
            var clipboard = new ClipboardJS('.btn-copy');
            clipboard.on('success', function (e) {
                toastr.success($("#CopiedText").val());
            });
        }
    },
    bindBtcOut: function () {
        $("#txt-btcOut").on("click", function () {
            $.ajax({
                url: "/Dashboard/WithdrawBTC/",
                type: "GET",
                beforeSend: function () {
                    //$("#txt-btcOut").attr("disabled", true);
                    //$("#txt-btcOut").html("<i class='fa fa-spinner fa-spin'></i> " + $("#txt-btcOut").text());
                },
                data: {
                },
                success: function (data) {
                    $("#btcOutView").html(data);
                },
                complete: function (data) {
                    $("#txt-btcOut").attr("disabled", false);
                    $("#txt-btcOut").html($("#txt-btcOut").text());
                }
            });
        })
    }
}

$(document).ready(function () {
    Dashboard.init();
});