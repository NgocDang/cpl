var AllUser = {
    init: function () {
        AllUser.loadAllUserDatatable();
        AllUser.loadLightBox();
        AllUser.bindEditButton();
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
        $('#dt-all-user').DataTable({
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
                        return "<a style='line-height:12px;' href='/Dashboard/User/" + full.id + "' target='_blank'  data-id='" + full.id + "' class='btn btn-sm btn-info btn-view'>View</a> <button style='line-height:12px;' data-id='" + full.id + "' class='btn btn-sm btn-primary btn-edit'>Edit</button>";
                    },
                    "orderable": false
                }
            ],
        });
    },
    destroyAllUserDatatable: function () {
        $('#dt-all-user').DataTable().destroy();
    }
}

$(document).ready(function () {
    AllUser.init();
});