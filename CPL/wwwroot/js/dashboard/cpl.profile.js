var Profile = {
    init: function () {
        Profile.bindDoEdit();
        Profile.bindEdit();
    },
    bindInitForm: function () {
        // Initiate country
        if ($("#Country").data("value") != "") {
            $("#Country option[value=" + $("#Country").data("value") + "]").attr("selected", "selected");
        }
        $("#Country").selectpicker('refresh');

        // Initiate date of birth 
        //$.getScript("/js/dashboard/plugins/moment.min.js");
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
    },
    bindDoEdit: function () {
        $("#profile-edit").on("click", "#btn-do-edit-profile", function () {
            //Validate for firstName
            if ($("#FirstName").val() == "") {
                $("#FirstName").closest(".form-group").find(".invalid-feedback").show();
            }
            else {
                $("#FirstName").closest(".form-group").find(".invalid-feedback").hide();
            }

            //Validate for lastName
            if ($("#LastName").val() == "") {
                $("#LastName").closest(".form-group").find(".invalid-feedback").show();
            }
            else {
                $("#LastName").closest(".form-group").find(".invalid-feedback").hide();
            }

            var isFormValid = $('#form-edit-profile')[0].checkValidity();
            $("#form-edit-profile").addClass('was-validated');

            //Validate for Mobile
            var isMobileValid = $("#Mobile").intlTelInput("isValidNumber");
            if (isMobileValid)
                $("#mobile-msg").hide();
            else
                $("#mobile-msg").show();

            //Validate for DOB
            var isDOBValid = $("#Year").val() != "" && $("#Month").val() != "" && $("#Day").val() != "" && moment().date($("#Day").val()).month($("#Month").val() - 1).year($("#Year").val()).isValid();
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

            // Validate for city
            if ($("#City").val() == "")
                $("#City").closest(".form-group").find(".invalid-feedback").show();
            else
                $("#City").closest(".form-group").find(".invalid-feedback").hide();

            // Validate for street
            if ($("#StreetAddress").val() == "")
                $("#StreetAddress").closest(".form-group").find(".invalid-feedback").show();
            else
                $("#StreetAddress").closest(".form-group").find(".invalid-feedback").hide();

            if (isFormValid && isMobileValid && isDOBValid && isCountryValid) {
                var returnUrl = Profile.getUrlParameter("returnUrl");
                $.ajax({
                    url: "/Profile/Update/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-do-edit-profile").attr("disabled", true);
                        $("#btn-do-edit-profile").html("<i class='fa fa-spinner fa-spin'></i> <i class='far fa-save'></i> " + $("#btn-do-edit-profile").text());
                    },
                    data: {
                        FirstName: $("#FirstName").val(),
                        LastName: $("#LastName").val(),
                        Gender: $('#Male').is(':checked'),
                        DOB: moment().date($("#Day").val()).month($("#Month").val() - 1).year($("#Year").val()).format("YYYY-MM-DD"),
                        PostalCode: $("#PostalCode").val(),
                        Country: $("#Country").val(),
                        City: $("#City").val(),
                        StreetAddress: $("#StreetAddress").val(),
                        Mobile: $("#Mobile").intlTelInput("getNumber")
                    },
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message, 'Success!');
                            if (returnUrl == "/DepositAndWithdraw/Index")
                                window.location.href = "/Profile/KYC";
                            $("#profile-edit").hide();
                            $("#profile-detail").show();

                            //Update value for profile detail
                            $("#first-name-detail").html($("#FirstName").val());
                            $("#last-name-detail").html($("#LastName").val());
                            $("#gender-detail").html(data.gender);
                            $("#dob-detail").html(moment().date($("#Day").val()).month($("#Month").val() - 1).year($("#Year").val()).format("YYYY/MM/DD"));
                            $("#postal-code-detail").html($("#PostalCode").val());
                            $("#country-detail").html($("#Country").val());
                            $("#city-detail").html($("#City").val());
                            $("#street-address-detail").html($("#StreetAddress").val());
                            $("#mobile-detail").html($("#Mobile").intlTelInput("getNumber"));
                        } else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $("#btn-do-edit-profile").attr("disabled", false);
                        $("#btn-do-edit-profile").html("<i class='far fa-save'></i> " + $("#btn-do-edit-profile").text());
                    }
                });
            }
            return false;
        });
    },
    bindEdit: function () {
        $("#btn-edit-profile").on("click", function () {
            $.ajax({
                url: "/Profile/Edit/",
                type: "GET",
                beforeSend: function () {
                },
                data: {
                },
                success: function (data) {
                    $("#profile-edit").html(data);
                    $("#profile-edit").show();
                    $("#profile-detail").hide();
                    Profile.bindInitForm();
                }
            });
        });
    },
    getUrlParameter: function (sParam) {
        var sPageURL = decodeURIComponent(window.location.search.substring(1)), sURLVariables = sPageURL.split('&'), sParameterName, i;

        for (i = 0; i < sURLVariables.length; i++) {
            sParameterName = sURLVariables[i].split('=');

            if (sParameterName[0] === sParam) {
                return sParameterName[1] === undefined ? true : sParameterName[1];
            }
        }
    }
};

$(document).ready(function () {
    Profile.init();
        if (location.hash == "#edit-profile-btn") {
            $('#btn-edit-profile').click();
    }
});