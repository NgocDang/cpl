var Dashboard = {
    init: function () {
        $('section').on('click', '.btn-swap', function () {
            //Swap label
            if ($(this).parents("section").length > 0) {
                var section = $(this).parents("section");
                var label = section.find("label[for='from-amount']").text();
                section.find("label[for='from-amount']").text(section.find("label[for='to-amount']").text());
                section.find("label[for='to-amount']").text(label);

                //Swap value
                var value = section.find(".from-amount").val();
                section.find(".from-amount").val(section.find(".to-amount").val());
                section.find(".to-amount").val(value);

                //Swap max amount
                var value = section.find(".from-amount").siblings(".max-amount").val();
                section.find(".from-amount").siblings(".max-amount").val(section.find(".to-amount").siblings(".max-amount").val());
                section.find(".to-amount").siblings(".max-amount").val(value);
            }
        })

        $('section').on('click', '.btn-max', function () {
            //Swap label
            if ($(this).parents("section").length > 0) {
                var section = $(this).parents("section");
                //var label = section.find((".from-amount").val);
                section.find(".from-amount").val(section.find(".from-amount").siblings(".max-amount").val());
            }
        })

        return false;
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
    },
}

$(document).ready(function () {
    Dashboard.init();
});