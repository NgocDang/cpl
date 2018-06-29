var EditAccount = {
    init: function () {
        // Initiate date of birth 
        var year = moment().year();
        for (i = year; i >= year-100; i--) {
            $("#Year").append($('<option></option>').val(i).html(i));
        }
        for (i = 1; i <= 12; i++) {
            $("#Month").append($('<option></option>').val(i).html(i));
        }
        for (i = 1; i <= 31; i++) {
            $("#Day").append($('<option></option>').val(i).html(i));
        }

        $("#Year").on("change", function () {
            var date = moment($("#Year").val() + "-" + $("#Month").val(), "YYYY-MM");
            $("#Month").empty();
            $("#Day").empty();

            if ($("#Year").val() == moment().year()) {
                for (i = 1; i <= moment().month() + 1; i++) {
                    $("#Month").append($('<option></option>').val(i).html(i));
                }

                if (moment().month() == 0) { //first month 
                    for (i = 1; i <= moment().date(); i++) {
                        $("#Day").append($('<option></option>').val(i).html(i));
                    }
                } else {
                    for (i = 1; i <= 31; i++) {
                        $("#Day").append($('<option></option>').val(i).html(i));
                    }
                }
            }
            else {
                for (i = 1; i <= 12; i++) {
                    $("#Month").append($('<option></option>').val(i).html(i));
                }

                for (i = 1; i <= date.daysInMonth(); i++) {
                    $("#Day").append($('<option></option>').val(i).html(i));
                }
            }
            $("#Month").selectpicker('refresh');
            $("#Day").selectpicker('refresh');
        });

        $("#Month").on("change", function () {
            var date = moment($("#Year").val() + "-" + $("#Month").val(), "YYYY-MM");
            $("#Day").empty();
            if (moment().year() == $("#Year").val() && moment().month() + 1 == $("#Month").val()) {
                for (i = 1; i <= moment().date(); i++) {
                    $("#Day").append($('<option></option>').val(i).html(i));
                }
            } else {
                for (i = 1; i <= date.daysInMonth(); i++) {
                    $("#Day").append($('<option></option>').val(i).html(i));
                }
            }
            $("#Day").selectpicker('refresh');
        });

        $("#Mobile").intlTelInput({
            initialCountry: 'auto',
            geoIpLookup: function (callback) {
                $.get("https://ipinfo.io", function () { }, "jsonp").always(function (resp) {
                    var countryCode = (resp && resp.country) ? resp.country : "";
                    callback(countryCode);
                }).fail(function () {
                    callback('jp');
                });
            }
        });

        $("#btn-save-account").on("click", function () {
            var isFormValid = $("#form-edit-account").valid();
            var isMobileValid = $("#Mobile").intlTelInput("isValidNumber");
            var isDOBValid = moment($("#DOB").val()).isValid();
            if (isFormValid && isMobileValid && isDOBValid) {
                $.ajax({
                    url: "/Profile/EditAccount/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-save-account").attr("disabled", true);
                        $("#btn-save-account").html("<i class='fa fa-spinner fa-spin'></i> <i class='far fa-save'></i> " + $("#btn-save-account").text());
                    },
                    data: {
                        FirstName: $("#FirstName").val(),
                        LastName: $("#LastName").val(),
                        Gender: $('#Male').is(':checked'),
                        DOB: moment(),
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
                        $("#btn-profile-update").attr("disabled", false);
                        $("#btn-profile-update").html("<i class='far fa-save'></i> " + $("#btn-profile-update").text());
                    }
                });
            }
            return false;
        });
    }
};


$(document).ready(function () {
    EditAccount.init();
    DashboardLayout.setFormValidate("#form-edit-account");
});