var AdminAllUser = {
    allUserDataTable: null,
    init: function () {
        AdminAllUser.allUserDataTable = AdminAllUser.loadAllUserDataTable();
        AdminAllUser.loadLightBox();
        AdminAllUser.bindEdit();
        AdminAllUser.bindDoEdit();
        AdminAllUser.bindDoDelete();
    },
    bindEdit: function () {
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
                    $("#edit-user").modal("show");

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
    bindDoEdit: function () {
        $("#modal").on("click", "#btn-do-edit-user", function () {
            var isFormValid = $("#form-edit-user")[0].checkValidity();
            $("#form-edit-user").addClass('was-validated');
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
            } else {
                $("#Mobile").closest(".form-group").find(".invalid-feedback").show();
            }

            if (isFormValid && isMobileValid) {
                $.ajax({
                    url: "/Admin/DoEditUser/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-do-edit-user").attr("disabled", true);
                        $("#btn-do-edit-user").html("<i class='fa fa-spinner fa-spin'></i> " + $("#btn-do-edit-user").text());
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
                            $("#edit-user").modal("hide");
                            AdminAllUser.allUserDataTable.ajax.reload();
                        } else {
                            if (data.name == "email") {
                                $("#Email").addClass("border-danger");
                            }
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $("#btn-do-edit-user").attr("disabled", false);
                        $("#btn-do-edit-user").html($("#btn-update-user").text());
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
    loadAllUserDataTable: function () {
        return $('#dt-all-user').DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "searchDelay": 350,
            "stateSave": (Utils.getJsonFromUrl().search == null) ? true : false,
            "ajax": {
                url: "/Admin/SearchAllUser",
                type: 'POST',
                data: function (data) {
                    if (data.search.value == "") {
                        data.search.value = Utils.getJsonFromUrl().search;
                    }
                },
            },
            "language": DTLang.getLang(),
            "columns": [
                {
                    "data": "Email",
                    "render": function (data, type, full, meta) {
                        return full.email;
                    }
                },
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
                    "data": "Address",
                    "render": function (data, type, full, meta) {
                        if (full.streetAddress != null && full.city != null && full.country != null) {
                            return full.streetAddress + " " + full.city + " " + full.country;
                        }
                        else return "";
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
                    "data": "TotalCPLUsed",
                    "render": function (data, type, full, meta) {
                        return full.totalCPLUsedInString;
                    }
                },
                {
                    "data": "TotalCPLAwarded",
                    "render": function (data, type, full, meta) {
                        return full.totalCPLAwardedInString;
                    }
                },
                {
                    "data": "CreatedDate",
                    "render": function (data, type, full, meta) {
                        return full.createdDateInString;
                    }
                },
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        var actions = "<a href='/Admin/User/" + full.id + "' target='_blank'  data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-view'>" + $("#view").val() + "</a>";
                        if (!full.isDeleted) {
                            actions += " <a href='#' data-id='" + full.id + "' target='_blank' class='btn btn-sm btn-outline-secondary btn-edit'>" + $("#edit").val() + "</a>";
                        }
                        return actions;
                    },
                    "orderable": false
                }
            ],
        });
    },
    bindDoDelete: function () {
        $("#modal").on("click", "#btn-do-delete-user", function () {
            var result = confirm("Do you really want to delete this user?");
            if (result) {
                $.ajax({
                    url: "/Admin/DoDeleteUser/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-do-delete-user").attr("disabled", true);
                        $("#btn-do-delete-user").html("<i class='fa fa-spinner fa-spin'></i> " + $("#btn-do-delete-user").text());
                    },
                    data: {
                        id: $("#Id").val()
                    },
                    success: function (data) {
                        if (data.success) {
                            $("#edit-user").modal("hide");
                            toastr.success(data.message, 'Success!');
                            AdminAllUser.allUserDataTable.ajax.reload();
                        } else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $("#btn-do-delete-user").attr("disabled", false);
                        $("#btn-do-delete-user").html($("#btn-do-delete-user").text());
                    }
                });
            }
            return false;
        });
    }
}

$(document).ready(function () {
    AdminAllUser.init();
});