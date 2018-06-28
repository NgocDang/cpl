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
    }
};


$(document).ready(function () {
    EditAccount.init();
});