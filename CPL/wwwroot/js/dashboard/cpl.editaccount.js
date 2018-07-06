var EditAccount = {
    init: function () {
        // Initiate country
        if ($("#Country").data("value") != "") {
            $("#Country option[value=" + $("#Country").data("value") + "]").attr("selected", "selected");
            $("#Country").selectpicker('refresh');
        }

        // Initiate date of birth 
        var year = moment().year();
        for (i = year; i >= year - 100; i--) {
            if ($("#DOB").val() != "" && moment($("#DOB").val()).year() == i) 
                $("#Year").append($("<option selected='selected'></option>").val(i).html(i));
            else 
                $("#Year").append($("<option></option>").val(i).html(i));
        }
        for (i = 1; i <= 12; i++) {
            if ($("#DOB").val() != "" && moment($("#DOB").val()).month() + 1 == i)
                $("#Month").append($("<option selected='selected'></option>").val(i).html(i));
            else
                $("#Month").append($("<option></option>").val(i).html(i));
        }
        for (i = 1; i <= 31; i++) {
            if ($("#DOB").val() != "" && moment($("#DOB").val()).date() == i)
                $("#Day").append($("<option selected='selected'></option>").val(i).html(i));
            else
                $("#Day").append($("<option></option>").val(i).html(i));
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

        $("#PostalCode").on("blur", function () {
            $.getJSON("https://maps.googleapis.com/maps/api/geocode/json?address=" + $("#PostalCode").val())
                .done(function (data) {
                    debugger;
                    if (data.status == "OK") {
                        var country = data.results[0].address_components[data.results[0].address_components.length - 1].short_name;
                        $("#Country option").removeAttr("selected");
                        $("#Country option[value=" + country + "]").attr("selected", "selected");
                        $("#Country").selectpicker('refresh');

                        var city = data.results[0].address_components[data.results[0].address_components.length - 2].long_name;
                        $("#City").val(city);
                    }
                });
        });

        $("#btn-save-account").on("click", function () {
            var fsFileUpload = $("#FrontSideImage").get(0);
            var fsFile = null;
            if (fsFileUpload.files.length > 0) {
                fsFile = fsFileUpload.files[0];
                $("#BackSideImage").attr('required', true);
            }
            var bsFileUpload = $("#BackSideImage").get(0);
            var bsFile = null;
            if (bsFileUpload.files.length > 0) {
                bsFile = bsFileUpload.files[0];
                $("#FrontSideImage").attr('required', true);
            }

            var isFormValid = $("#form-edit-account").valid();

            //Validate for Mobile
            var isMobileValid = $("#Mobile").intlTelInput("isValidNumber");
            if (isMobileValid)
                $("#mobile-msg").hide();
            else
                $("#mobile-msg").show();

            //Validate for DOB
            var isDOBValid = $("#Year").val() != "" && $("#Month").val() != "" && $("#Day").val() != "" && moment().date($("#Day").val()).month($("#Month").val()-1).year($("#Year").val()).isValid();
            if (isDOBValid)
                $("#dob-msg").hide();
            else
                $("#dob-msg").show();

            //Validate for Country
            var isCountryValid = $("#Country").val() != "";
            if (isCountryValid)
                $("#country-msg").hide();
            else
                $("#country-msg").show();

            if (isFormValid && isMobileValid && isDOBValid && isCountryValid) {
                var formData = new FormData();
                formData.append('Id', $("#Id").val());
                formData.append('FirstName', $("#FirstName").val());
                formData.append('LastName', $("#LastName").val());
                formData.append('Gender', $('#Male').is(':checked'));
                formData.append('DOB', moment().date($("#Day").val()).month($("#Month").val() - 1).year($("#Year").val()).format("YYYY-MM-DD"));
                formData.append('PostalCode', $("#PostalCode").val());
                formData.append('Country', $("#Country").val());
                formData.append('City', $("#City").val());
                formData.append('StreetAddress', $("#StreetAddress").val());
                formData.append('Mobile', $("#Mobile").intlTelInput("getNumber"));
                formData.append('FrontSideImage', fsFile);
                formData.append('BackSideImage', bsFile);
                $.ajax({
                    url: "/Profile/EditAccount/",
                    type: "POST",
                    processData: false,
                    contentType: false,
                    beforeSend: function () {
                        $("#btn-save-account").attr("disabled", true);
                        $("#btn-save-account").html("<i class='fa fa-spinner fa-spin'></i> <i class='far fa-save'></i> " + $("#btn-save-account").text());
                    },
                    data: formData,
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