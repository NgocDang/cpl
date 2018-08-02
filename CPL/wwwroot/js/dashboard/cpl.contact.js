var Contact = {
    init: function () {
        Contact.bindSend();
    },
    bindSend: function () {
        $("#btn-send").on("click", function () {
            var _this = this;
            var isFormValid = $("#form-contact").valid();
            //var isCountryValid = $("#Country").val() != "";
            //if (isCountryValid)
            //    $("#country-msg").hide();
            //else
            //    $("#country-msg").show();

            if (isFormValid) {
                $.ajax({
                    url: "/Contact/Send/",
                    type: "POST",
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                        $(_this).html("<i class='fa fa-spinner fa-spin'></i> <i class='la la-check'></i> " + $(_this).text().trim());
                    },
                    data: {
                        Email: $("#Email").val(),
                        Des: $("#LastName").val(),
                        Gender: $('#Male').is(':checked'),
                        DOB: moment().date($("#Day").val()).month($("#Month").val()-1).year($("#Year").val()).format("YYYY-MM-DD"),
                        PostalCode: $("#PostalCode").val(),
                        Country: $("#Country").val(),
                        City: $("#City").val(),
                        StreetAddress: $("#StreetAddress").val(),
                        Mobile: $("#Mobile").intlTelInput("getNumber")
                    },
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message, 'Success!');
                        } else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $("#btn-save-account").attr("disabled", false);
                        $("#btn-save-account").html("<i class='far fa-save'></i> " + $("#btn-save-account").text());
                    }
                });
            }
            return false;
        });
    }
};

$(document).ready(function () {
    EditAccount.init();
});