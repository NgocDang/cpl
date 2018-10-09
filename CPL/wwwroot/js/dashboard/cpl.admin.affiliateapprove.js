var AdminAffiliateApprove = {
    affiliateApplicationDataTable: null,
    init: function () {
        AdminAffiliateApprove.affiliateApplicationDataTable = AdminAffiliateApprove.loadAffiliateApplicationDataTable();
        AdminAffiliateApprove.bindDoApprove();
    },
    bindDoApprove: function () {
        $("#dt-affiliate-application").on("click", ".btn-do-approve-affiliate-application", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/DoApproveAffiliateApplication/",
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
                        AdminAffiliateApprove.affiliateApplicationDataTable.ajax.reload();
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
    loadAffiliateApplicationDataTable: function () {
        return $('#dt-affiliate-application').DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "ajax": {
                url: "/Admin/SearchAffiliateApplication",
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
                    "data": "AffiliateId",
                    "render": function (data, type, full, meta) {
                        if (full.affiliateId == 0) { // Check EnumAffiliateApplicationStatus
                            return "<span class='badge badge-info'>Pending</span>";
                        }
                        else {
                            return "<span class='badge badge-success'>Approved</span>";
                        }
                    }
                },
                {
                    "data": "AffiliateCreatedDate",
                    "render": function (data, type, full, meta) {
                        return full.kycCreatedDateInString;
                    }
                },
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        if (!full.kycVerified && full.affiliateId == 0) {
                            return "<div>" +
                                "<a data-id='" + full.id + "' href='#'  target='_blank' class='btn btn-sm btn-outline-secondary btn-do-approve-affiliate-application'>" + $("#approve").val()+"</a> " +
                                "</div>";
                        }
                        return "";
                    },
                    "orderable": false
                }
            ],
        });
    }
}

$(document).ready(function () {
    AdminAffiliateApprove.init();
});