var EditAccount = {
    init: function () {
        EditAccount.bindInitForm();
        EditAccount.bindSaveButton();
        EditAccount.bindShowCountry();
    },
    bindInitForm: function () {
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
    },
    bindSaveButton: function () {
        $("#btn-save-account").on("click", function () {
            var isFormValid = $("#form-edit-account").valid();

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

            if (isFormValid && isMobileValid && isDOBValid && isCountryValid) {
                var returnUrl = EditAccount.getUrlParameter("returnUrl").trim();
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
                            if (returnUrl == "/DepositAndWithdraw/DoDepositWithdraw") {
                                $("#btn-save-account").hide();
                                $("#btn-continue-verify-kyc").show();
                            }
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
    },
    bindShowCountry: function () {
        var countries = {
            "AF": "Afghanistan",
            "AX": "Åland Islands",
            "AL": "Albania",
            "DZ": "Algeria",
            "AS": "American Samoa",
            "AD": "Andorra",
            "AO": "Angola",
            "AI": "Anguilla",
            "AQ": "Antarctica",
            "AG": "Antigua and Barbuda",
            "AR": "Argentina",
            "AM": "Armenia",
            "AW": "Aruba",
            "AU": "Australia",
            "AT": "Austria",
            "AZ": "Azerbaijan",
            "BS": "Bahamas",
            "BH": "Bahrain",
            "BD": "Bangladesh",
            "BB": "Barbados",
            "BY": "Belarus",
            "BE": "Belgium",
            "BZ": "Belize",
            "BJ": "Benin",
            "BM": "Bermuda",
            "BT": "Bhutan",
            "BO": "Bolivia, Plurinational State of",
            "BA": "Bosnia and Herzegovina",
            "BW": "Botswana",
            "BV": "Bouvet Island",
            "BR": "Brazil",
            "IO": "British Indian Ocean Territory",
            "BN": "Brunei Darussalam",
            "BG": "Bulgaria",
            "BF": "Burkina Faso",
            "BI": "Burundi",
            "KH": "Cambodia",
            "CM": "Cameroon",
            "CA": "Canada",
            "CV": "Cape Verde",
            "KY": "Cayman Islands",
            "CF": "Central African Republic",
            "TD": "Chad",
            "CL": "Chile",
            "CN": "China",
            "CX": "Christmas Island",
            "CC": "Cocos (Keeling) Islands",
            "CO": "Colombia",
            "KM": "Comoros",
            "CG": "Congo",
            "CD": "Congo, The Democratic Republic of the",
            "CK": "Cook Islands",
            "CR": "Costa Rica",
            "CI": "Côte d'Ivoire",
            "HR": "Croatia",
            "CU": "Cuba",
            "CY": "Cyprus",
            "CZ": "Czech Republic",
            "DK": "Denmark",
            "DJ": "Djibouti",
            "DM": "Dominica",
            "DO": "Dominican Republic",
            "EC": "Ecuador",
            "EG": "Egypt",
            "SV": "El Salvador",
            "GQ": "Equatorial Guinea",
            "ER": "Eritrea",
            "EE": "Estonia",
            "ET": "Ethiopia",
            "FK": "Falkland Islands (Malvinas)",
            "FO": "Faroe Islands",
            "FJ": "Fiji",
            "FI": "Finland",
            "FR": "France",
            "GF": "French Guiana",
            "PF": "French Polynesia",
            "TF": "French Southern Territories",
            "GA": "Gabon",
            "GM": "Gambia",
            "GE": "Georgia",
            "DE": "Germany",
            "GH": "Ghana",
            "GI": "Gibraltar",
            "GR": "Greece",
            "GL": "Greenland",
            "GD": "Grenada",
            "GP": "Guadeloupe",
            "GU": "Guam",
            "GT": "Guatemala",
            "GG": "Guernsey",
            "GN": "Guinea",
            "GW": "Guinea-Bissau",
            "GY": "Guyana",
            "HT": "Haiti",
            "HM": "Heard Island and McDonald Islands",
            "VA": "Holy See (Vatican City State)",
            "HN": "Honduras",
            "HK": "Hong Kong",
            "HU": "Hungary",
            "IS": "Iceland",
            "IN": "India",
            "ID": "Indonesia",
            "IR": "Iran, Islamic Republic of",
            "IQ": "Iraq",
            "IE": "Ireland",
            "IM": "Isle of Man",
            "IL": "Israel",
            "IT": "Italy",
            "JM": "Jamaica",
            "JP": "Japan",
            "JE": "Jersey",
            "JO": "Jordan",
            "KZ": "Kazakhstan",
            "KE": "Kenya",
            "KI": "Kiribati",
            "KP": "Korea, Democratic People's Republic of",
            "KR": "Korea, Republic of",
            "KW": "Kuwait",
            "KG": "Kyrgyzstan",
            "LA": "Lao People's Democratic Republic",
            "LV": "Latvia",
            "LB": "Lebanon",
            "LS": "Lesotho",
            "LR": "Liberia",
            "LY": "Libyan Arab Jamahiriya",
            "LI": "Liechtenstein",
            "LT": "Lithuania",
            "LU": "Luxembourg",
            "MO": "Macao",
            "MK": "Macedonia, The Former Yugoslav Republic of",
            "MG": "Madagascar",
            "MW": "Malawi",
            "MY": "Malaysia",
            "MV": "Maldives",
            "ML": "Mali",
            "MT": "Malta",
            "MH": "Marshall Islands",
            "MQ": "Martinique",
            "MR": "Mauritania",
            "MU": "Mauritius",
            "YT": "Mayotte",
            "MX": "Mexico",
            "FM": "Micronesia, Federated States of",
            "MD": "Moldova, Republic of",
            "MC": "Monaco",
            "MN": "Mongolia",
            "ME": "Montenegro",
            "MS": "Montserrat",
            "MA": "Morocco",
            "MZ": "Mozambique",
            "MM": "Myanmar",
            "NA": "Namibia",
            "NR": "Nauru",
            "NP": "Nepal",
            "NL": "Netherlands",
            "AN": "Netherlands Antilles",
            "NC": "New Caledonia",
            "NZ": "New Zealand",
            "NI": "Nicaragua",
            "NE": "Niger",
            "NG": "Nigeria",
            "NU": "Niue",
            "NF": "Norfolk Island",
            "MP": "Northern Mariana Islands",
            "NO": "Norway",
            "OM": "Oman",
            "PK": "Pakistan",
            "PW": "Palau",
            "PS": "Palestinian Territory, Occupied",
            "PA": "Panama",
            "PG": "Papua New Guinea",
            "PY": "Paraguay",
            "PE": "Peru",
            "PH": "Philippines",
            "PN": "Pitcairn",
            "PL": "Poland",
            "PT": "Portugal",
            "PR": "Puerto Rico",
            "QA": "Qatar",
            "RE": "Réunion",
            "RO": "Romania",
            "RU": "Russian Federation",
            "RW": "Rwanda",
            "BL": "Saint Barthélemy",
            "SH": "Saint Helena, Ascension and Tristan Da Cunha",
            "KN": "Saint Kitts and Nevis",
            "LC": "Saint Lucia",
            "MF": "Saint Martin",
            "PM": "Saint Pierre and Miquelon",
            "VC": "Saint Vincent and the Grenadines",
            "WS": "Samoa",
            "SM": "San Marino",
            "ST": "Sao Tome and Principe",
            "SA": "Saudi Arabia",
            "SN": "Senegal",
            "RS": "Serbia",
            "SC": "Seychelles",
            "SL": "Sierra Leone",
            "SG": "Singapore",
            "SK": "Slovakia",
            "SI": "Slovenia",
            "SB": "Solomon Islands",
            "SO": "Somalia",
            "ZA": "South Africa",
            "GS": "South Georgia and the South Sandwich Islands",
            "ES": "Spain",
            "LK": "Sri Lanka",
            "SD": "Sudan",
            "SR": "Suriname",
            "SJ": "Svalbard and Jan Mayen",
            "SZ": "Swaziland",
            "SE": "Sweden",
            "CH": "Switzerland",
            "SY": "Syrian Arab Republic",
            "TW": "Taiwan, Province of China",
            "TJ": "Tajikistan",
            "TZ": "Tanzania, United Republic of",
            "TH": "Thailand",
            "TL": "Timor-Leste",
            "TG": "Togo",
            "TK": "Tokelau",
            "TO": "Tonga",
            "TT": "Trinidad and Tobago",
            "TN": "Tunisia",
            "TR": "Turkey",
            "TM": "Turkmenistan",
            "TC": "Turks and Caicos Islands",
            "TV": "Tuvalu",
            "UG": "Uganda",
            "UA": "Ukraine",
            "AE": "United Arab Emirates",
            "GB": "United Kingdom",
            "US": "United States",
            "UM": "United States Minor Outlying Islands",
            "UY": "Uruguay",
            "UZ": "Uzbekistan",
            "VU": "Vanuatu",
            "VE": "Venezuela, Bolivarian Republic of",
            "VN": "Viet Nam",
            "VG": "Virgin Islands, British",
            "VI": "Virgin Islands, U.S.",
            "WF": "Wallis and Futuna",
            "EH": "Western Sahara",
            "YE": "Yemen",
            "ZM": "Zambia",
            "ZW": "Zimbabwe"
        };
        var country = countries[$("#country-value").val()];
        $("#country-show").text(country);
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
    EditAccount.init();
});