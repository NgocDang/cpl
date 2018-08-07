var AllUser = {
    allUserDataTable: null,
    init: function () {
        AllUser.allUserDataTable = AllUser.loadAllUserDatatable();
        AllUser.loadLightBox();
        AllUser.bindEditButton();
        AllUser.bindUpdateButton();
        AllUser.bindDeleteButton();
    },
    bindEditButton: function () {
        $("#dt-all-user").on("click", ".btn-edit", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/EditUser",
                type: "GET",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: {
                    id: $(_this).data().id
                },
                success: function (data) {
                    $("#modal").html(data);
                    $("#update-user").modal("show");

                    $.each($(".js-switch"), function (index, element) {
                        var switches = new Switchery(element, { size: 'small' });
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
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                }
            });
            return false;
        });
    },
    bindUpdateButton: function () {
        $("#modal").on("click", "#btn-update-user", function () {
            var isFormValid = $("#form-update-user")[0].checkValidity();
            $("#form-update-user").addClass('was-validated');
            $("#Email").removeClass("border-danger");
            var isMobileValid = true;
            if ($("#Mobile").intlTelInput("getNumber") != "") {
                isMobileValid = $("#Mobile").intlTelInput("isValidNumber");
                if (!isMobileValid) {
                    $("#Mobile").addClass("border-danger");
                    $("#Mobile").closest(".form-group").find(".invalid-feedback").show();
                } else {
                    $("#Mobile").removeClass("border-danger");
                    $("#Mobile").closest(".form-group").find(".invalid-feedback").hide();
                }
            }

            if (isFormValid && isMobileValid) {
                $.ajax({
                    url: "/Admin/UpdateUser/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-update-user").attr("disabled", true);
                        $("#btn-update-user").html("<i class='fa fa-spinner fa-spin'></i> " + $("#btn-update-user").text());
                    },
                    data: {
                        FirstName: $("#FirstName").val(),
                        LastName: $("#LastName").val(),
                        Mobile: $("#Mobile").intlTelInput("getNumber"),
                        TwoFactorAuthenticationEnable: $("#TwoFactorAuthenticationEnable").is(":checked"),
                        Password: $("#Password").val(),
                        StreetAddress: $("#Address").val(),
                        Email: $("#Email").val(),
                        Id: $("#Id").val()
                    },
                    success: function (data) {
                        if (data.success) {
                            $("#first-name-" + $("#Id").val()).html($("#FirstName").val());
                            $("#last-name-" + $("#Id").val()).html($("#LastName").val());
                            $("#mobile-" + $("#Id").val()).html($("#Mobile").intlTelInput("getNumber"));
                            $("#address-" + $("#Id").val()).html($("#Address").val());
                            $("#email-" + $("#Id").val()).html($("#Email").val());
                            toastr.success(data.message, 'Success!');
                            $("#update-user").modal("hide");
                            AllUser.allUserDataTable.ajax.reload();
                        } else {
                            if (data.name == "email") {
                                $("#Email").addClass("border-danger");
                            }
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $("#btn-update-user").attr("disabled", false);
                        $("#btn-update-user").html($("#btn-update-user").text());
                    }
                });
            }
            return false;
        });
    },
    loadLightBox: function () {
        $(document).on('click', '[data-toggle="lightbox"]', function (event) {
            event.preventDefault();
            $(this).ekkoLightbox();
        });
    },
    getJsonFromUrl: function () {
        var query = location.search.substr(1);
        var result = {};
        query.split("&").forEach(function (part) {
            var item = part.split("=");
            result[item[0]] = decodeURIComponent(item[1]);
        });
        return result;
    },
    loadAllUserDatatable: function () {
        return $('#dt-all-user').DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "stateSave": (AllUser.getJsonFromUrl().search == null) ? true : false,
            "ajax": {
                url: "/Admin/SearchAllUser",
                type: 'POST',
                data: function (data) {
                    if (data.search.value == "") {
                        data.search.value = AllUser.getJsonFromUrl().search;
                    }
                },
            },
            "language": DTLang.getLang(),
            "columns": [
                {
                    "data": "FirstName",
                    "render": function (data, type, full, meta) {
                        return full.firstName;
                    }
                },
                {
                    "data": "LastName",
                    "render": function (data, type, full, meta) {
                        return full.lastName;
                    }
                },
                {
                    "data": "Email",
                    "render": function (data, type, full, meta) {
                        return full.email;
                    }
                },
                {
                    "data": "Address",
                    "render": function (data, type, full, meta) {
                        return full.streetAddress + " " + full.city + " " + AllUser.bindShowCountry(full.country);
                    },
                    "orderable": false
                },
                {
                    "data": "Mobile",
                    "render": function (data, type, full, meta) {
                        if (full.mobile) {
                            return "<span id='mobile-" + full.id + "'>" + full.mobile + "</span>";
                        }
                        else
                            return "<span id='mobile-" + full.id + "'>" + "</span>";
                    }
                },
                {
                    "data": "TokenAmount",
                    "render": function (data, type, full, meta) {
                        return full.tokenAmount;
                    }
                },
                {
                    "data": "CreatedDate",
                    "render": function (data, type, full, meta) {
                        return full.createdDateInString;
                    }
                },
                {
                    "data": "IsDeleted",
                    "render": function (data, type, full, meta) {
                        if (full.isDeleted) {
                            return "<span class='badge badge-success'>Yes</span>";
                        } else {
                            return "<span class='badge badge-info'>No</span>";
                        }
                    }
                },
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        return "<a style='line-height:12px;' href='/Admin/User/" + full.id + "' target='_blank'  data-id='" + full.id + "' class='btn btn-sm btn-info btn-view'>View</a> <button style='line-height:12px;' data-id='" + full.id + "' class='btn btn-sm btn-primary btn-edit'>Edit</button>";
                    },
                    "orderable": false
                }
            ],
        });
    },
    destroyAllUserDatatable: function () {
        $('#dt-all-user').DataTable().destroy();
    },
    bindDeleteButton: function () {
        $("#modal").on("click", "#btn-delete-user", function () {
            var result = confirm("Do you really want to delete this user?");
            if (result) {
                $.ajax({
                    url: "/Admin/DeleteUser/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-delete-user").attr("disabled", true);
                        $("#btn-delete-user").html("<i class='fa fa-spinner fa-spin'></i> " + $("#btn-delete-user").text());
                    },
                    data: {
                        Id: $("#Id").val()
                    },
                    success: function (data) {
                        if (data.success) {
                            $("#update-user").modal("hide");
                            toastr.success(data.message, 'Success!');
                            AllUser.allUserDataTable.ajax.reload();
                        } else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $("#btn-delete-user").attr("disabled", false);
                        $("#btn-delete-user").html($("#btn-delete-user").text());
                    }
                });
            }
            return false;
        });
    },
    bindShowCountry: function (code) {
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
        return country = countries[code];
    }
}

$(document).ready(function () {
    AllUser.init();
});