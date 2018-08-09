var Contact = {
    init: function () {
        Contact.bindSend();
    },
    bindSend: function () {
        $("#btn-send").on("click", function () {
            var _this = this;
            var isFormValid = $("#form-contact")[0].checkValidity();
            $("#form-contact").addClass('was-validated');

            var isCategoryValid = $("#Category").val() != "";
            if (isCategoryValid)
                $("#category-error").hide();
            else
                $("#category-error").show();

            if (isFormValid && isCategoryValid) {
                $.ajax({
                    url: "/Contact/Send/",
                    type: "POST",
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                        $(_this).html("<i class='fa fa-spinner fa-spin'></i> <i class='la la-check'></i> " + $(_this).text().trim());
                    },
                    data: {
                        Email: $("#Email").val(),
                        Description: $("#Description").val(),
                        Subject: $('#Subject').val(),
                        Category: $("#Category").val()
                    },
                    success: function (data) {
                        if (data.success) {
                            $("#form-contact").hide();
                            $("#contact-response").show();
                            toastr.success(data.message, 'Success!');
                        } else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $(_this).attr("disabled", false);
                        $(_this).html("<i class='la la-check'></i> " + $(_this).text().trim());
                    }
                });
            }
            return false;
        });
    }
};

$(document).ready(function () {
    Contact.init();
});