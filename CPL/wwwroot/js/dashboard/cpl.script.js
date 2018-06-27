// Global
$(function () {
    // Modal
    $("[data-confirm]").each(function () {
        var d = $(this),
            b = d.data("confirm");
        b = b.split("|");
        d.fireModal({
            title: b[0],
            body: b[1],
            buttons: [{
                text: "Yes",
                "class": "btn btn-link btn-neutral",
                handler: function () {
                    eval(d.data("confirm-yes"))
                }
            }, {
                text: "No",
                    "class": "btn btn-link btn-neutral",
                handler: function (a) {
                    $.destroyModal(a)
                }
            }]
        })
    });
});