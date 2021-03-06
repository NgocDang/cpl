﻿var AdminKYCVerify = {
    init: function () {
        AdminKYCVerify.loadKYCVerifyDataTable();
        AdminKYCVerify.loadLightBox();
        AdminKYCVerify.bindDoAccept();
        AdminKYCVerify.bindDoCancel();
    },
    bindDoAccept: function () {
        $("#dt-kyc").on("click", ".btn-do-accept-kyc-verify", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/DoAcceptKYCVerify/",
                type: "POST",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                    $(_this).html("<i class='fa fa-spinner fa-spin'></i> " + $(_this).text());
                },
                data: {
                    id: $(_this).data().id
                },
                success: function (data) {
                    if (data.success) {
                        AdminKYCVerify.destroyKYCDatatable();
                        AdminKYCVerify.loadKYCVerifyDataTable();
                        toastr.success(data.message, 'Success!');
                    } else {
                        toastr.error(data.message, 'Error!');
                    }
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                    $(_this).html($(_this).text());
                }
            });
        });
    },
    bindDoCancel: function () {
        $("#dt-kyc").on("click", ".btn-do-cancel-kyc-verify", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/DoCancelKYCVerify/",
                type: "POST",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                    $(_this).html("<i class='fa fa-spinner fa-spin'></i> " + $(_this).text());
                },
                data: {
                    id: $(_this).data().id
                },
                success: function (data) {
                    if (data.success) {
                        AdminKYCVerify.destroyKYCDatatable();
                        AdminKYCVerify.loadKYCVerifyDataTable();
                        toastr.success(data.message, 'Success!');
                    } else {
                        toastr.error(data.message, 'Error!');
                    }
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                    $(_this).html($(_this).text());
                }
            });
        });
    },
    loadLightBox: function () {
        $(document).on('click', '[data-toggle="lightbox"]', function (event) {
            event.preventDefault();
            $(this).ekkoLightbox();
        });
    },
    loadKYCVerifyDataTable: function () {
        $('#dt-kyc').DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "searchDelay": 350,
            "ajax": {
                url: "/Admin/SearchKYCVerify",
                type: 'POST'
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
                        return "<span class='word-break'>" + full.email + "</span>";
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
                    "data": "DOB",
                    "render": function (data, type, full, meta) {
                        return full.dobInString;
                    }
                },
                {
                    "data": "Document",
                    "render": function (data, type, full, meta) {
                        var html = "";
                        if (full.frontSide) {
                            html += "<a href='/images/kyc/" + full.frontSide + "' data-toggle='lightbox' data-gallery='document-" + full.id + "'>" +
                                "<i class='fas fa-image'></i>" +
                                "</a> ";
                        }

                        if (full.backSide) {
                            html += "<a href='/images/kyc/" + full.backSide + "' data-toggle='lightbox' data-gallery='document-" + full.id + "'>" +
                                "<i class='fas fa-image'></i>" +
                                "</a>";
                        }
                        return html;
                    },
                    "orderable": false
                },
                {
                    "data": "KYCVerified",
                    "render": function (data, type, full, meta) {
                        if (!full.kycVerified) {
                            return "<div id='kyc-status-" + full.id + "'><span class='badge badge-info'>Pending</span></div>";
                        }
                        else {
                            return "<div id='kyc-status-" + full.id + "'><span class='badge badge-success'>Verified</span></div>";
                        }
                    }
                },
                {
                    "data": "KYCCreatedDate",
                    "render": function (data, type, full, meta) {
                        return full.kycCreatedDateInString;
                    }
                },
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        if (!full.kycVerified) {
                            return "<div class='kyc-action-" + full.id + "'>" +
                                "<a data-id='" + full.id + "' target='_blank' href='#' class='btn btn-sm btn-outline-secondary btn-do-accept-kyc-verify'>" + $("#accept").val()+"</a> " +
                                "<a data-id='" + full.id + "' target='_blank' href='#' class='btn btn-sm btn-outline-danger btn-do-cancel-kyc-verify'>" + $("#cancel").val() +"</a>" +
                                "</div>";
                        }
                        return "";
                    },
                    "orderable": false
                }
            ],
        });
    },
    destroyKYCDatatable: function () {
        $('#dt-kyc').DataTable().destroy();
    }
}

$(document).ready(function () {
    AdminKYCVerify.init();
});