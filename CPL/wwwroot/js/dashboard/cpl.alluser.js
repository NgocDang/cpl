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
                    $("#Mobile").closest(".form-label-group").find(".invalid-feedback").show();
                } else {
                    $("#Mobile").removeClass("border-danger");
                    $("#Mobile").closest(".form-label-group").find(".invalid-feedback").hide();
                }
            } else {
                $("#Mobile").closest(".form-label-group").find(".invalid-feedback").show();
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
                        return full.streetAddress + " " + full.city + " " + full.country;
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
                        return "<a style='line-height:12px;' href='/Admin/User/" + full.id + "' target='_blank'  data-id='" + full.id + "' class='btn btn-sm btn-outline-info btn-view'>View</a> <button style='line-height:12px;' data-id='" + full.id + "' class='btn btn-sm btn-outline-primary btn-edit'>Edit</button>";
                    },
                    "orderable": false
                }
            ],
        });
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
    }
}

$(document).ready(function () {
    AllUser.init();
});