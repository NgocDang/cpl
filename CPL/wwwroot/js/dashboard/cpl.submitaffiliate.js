var SubmitAffiliate = {
    init: function () {
        SubmitAffiliate.bindDoSubmit();
    },
    bindDoSubmit: function () {
        $("#btn-do-submit-affiliate").on("click", function () {
            var _this = this;
            var isFormValid = $('#form-submit-affiliate')[0].checkValidity();
            $("#form-submit-affiliate").addClass('was-validated');

            if (isFormValid) {
                $.ajax({
                    url: "/Profile/DoSubmitAffiliate/",
                    type: "POST",
                    processData: false,
                    contentType: false,
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                        $(_this).html("<i class='fa fa-spinner fa-spin'></i> " + $(_this).text());
                    },
                    data: {},
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message, 'Success!');
                            $("#form-submit-affiliate").hide();
                            $("#affiliate-response").html(data.message);
                            $("#affiliate-response").show();
                        } else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $(_this).attr("disabled", false);
                        $(_this).html($(_this).text());
                    }
                });
            }
            return false;
        });
    },
};

$(document).ready(function () {
    SubmitAffiliate.init();
});