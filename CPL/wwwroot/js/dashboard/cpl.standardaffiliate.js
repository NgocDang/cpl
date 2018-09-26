var StandardAffiliate = {
    init: function () {
        StandardAffiliate.bindCopy();
    },
    bindCopy: function () {
        if ($(".btn-copy").length > 0) {
            var clipboard = new ClipboardJS('.btn-copy');
            clipboard.on('success', function (e) {
                toastr.success($("#CopiedSuccessfully").val());
            });
        }
    }
};

$(document).ready(function () {
    StandardAffiliate.init();
});