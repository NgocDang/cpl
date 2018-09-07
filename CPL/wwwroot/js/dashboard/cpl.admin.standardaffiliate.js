var StandardAffiliate = {
    standardAffiliateDataTable: null,
    init: function () {
        StandardAffiliate.standardAffiliateDataTable = StandardAffiliate.loadstandardAffiliateDataTable();
    },
    loadstandardAffiliateDataTable: function () {
        return $('#dt-standard-affiliate').DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "stateSave": (Utils.getJsonFromUrl().search == null) ? true : false,
            "ajax": {
                url: "/Admin/SearchStandardAffiliate",
                type: 'POST',
                data: function (data) {
                    if (data.search.value == "") {
                        data.search.value = Utils.getJsonFromUrl().search;
                    }
                },
            },
            "language": DTLang.getLang(),
            "createdRow": function (row, data, dataIndex) {
                if (data.isLocked == true) {
                    $(row).addClass("disabled");
                }
            },
            "columns": [
                {
                    "data": "Email",
                    "render": function (data, type, full, meta) {
                        return "<span class='word-break'>" + full.email + "</span>";
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
                    "data": "TotalSale",
                    "render": function (data, type, full, meta) {
                        return full.totalSale;
                    }
                },
                {
                    "data": "TotalIntroducer",
                    "render": function (data, type, full, meta) {
                        return full.totalIntroducer;
                    }
                },
                {
                    "data": "AffiliateCreatedDate",
                    "render": function (data, type, full, meta) {
                        return full.affiliateCreatedDateInString    ;
                    }
                },
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        if (full.isLocked === false) {
                            return "<div class='text-lg-center'>" +
                                "<button data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary '>" + $("#lock").val() +"</button> " +
                                "</div>";
                        }
                        return "<div class='text-lg-center'>" +
                            "<button data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary '>" + $("#unlock").val() + "</button> " +
                            "</div>";
                    },
                    "orderable": false
                },
                {
                    "data": "Tier1DirectRate",
                    "render": function (data, type, full, meta) {
                        return full.tier1DirectRate;
                    },
                     "orderable": false
                },
                {
                    "data": "Tier2SaleToTier1Rate",
                    "render": function (data, type, full, meta) {
                        return full.tier2SaleToTier1Rate;
                    },
                     "orderable": false
                },
                {
                    "data": "Tier3SaleToTier1Rate",
                    "render": function (data, type, full, meta) {
                        return full.tier3SaleToTier1Rate;
                    },
                     "orderable": false
                }
            ],
        });
    }
}

$(document).ready(function () {
    StandardAffiliate.init();
});