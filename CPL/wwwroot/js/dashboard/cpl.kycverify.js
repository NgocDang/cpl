var KYCVerify = {
    init: function () {
        KYCVerify.loadKYCDatatable();
        KYCVerify.loadLightBox();
        KYCVerify.bindAccept();
        KYCVerify.bindCancel();
    },
    bindAccept: function () {
        $("#dt-kyc").on("click", ".btn-kyc-verify", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/UpdateKYCVerify/",
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
                        KYCVerify.destroyKYCDatatable();
                        KYCVerify.loadKYCDatatable();
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
    bindCancel: function () {
        $("#dt-kyc").on("click", ".btn-kyc-cancel", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/CancelKYCVerify/",
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
                        KYCVerify.destroyKYCDatatable();
                        KYCVerify.loadKYCDatatable();
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
    loadKYCDatatable: function () {
        $('#dt-kyc').DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
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
                        return full.email;
                    }
                },
                {
                    "data": "Address",
                    "render": function (data, type, full, meta) {
                        return full.streetAddress + " " + full.city + " " + full.country;
                    }
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
                                "<button style='line-height:12px;' data-id='" + full.id + "' class='btn btn-sm btn-primary btn-kyc-verify'><i class='fas fa-check'></i></button> " +
                                "<button style='line-height:12px;' data-id='" + full.id + "' class='btn btn-sm btn-danger btn-kyc-cancel'><i class='fas fa-ban'></i></button>" +
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
    KYCVerify.init();
});