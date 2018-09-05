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
            "ajax": {
                url: "/Admin/SearchStandardAffiliate",
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
                    "data": "TotalSale",
                    "render": function (data, type, full, meta) {
                        return full.totalSale;
                    }
                },
                {
                    "data": "TotalTntroducer",
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
                        if (!full.isblocked || full.isblocked === false) {
                            return "<div>" +
                                "<button data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary '>" + "Block" +"</button> " +
                                "</div>";
                        }
                        return "<div>" +
                            "<button data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary '>" + "UnBlock" + "</button> " +
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