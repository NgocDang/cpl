var AllTopAgencyAffiliate = {
    AllTopAgencyAffiliateDataTable: null,
    init: function () {
        AllTopAgencyAffiliate.AllTopAgencyAffiliateDataTable = AllTopAgencyAffiliate.loadAllTopAgencyAffiliateDataTable();
        AllTopAgencyAffiliate.initAllTopAgencyAffiliateDataTable();
    },
    initAllTopAgencyAffiliateDataTable: function () {
        AllTopAgencyAffiliate.AllTopAgencyAffiliateDataTable.on('responsive-display', function (e, datatable, row, showHide, update) {
            AllTopAgencyAffiliate.loadEditable();
        });
    },
    loadAllTopAgencyAffiliateDataTable: function () {
        return $('#dt-all-top-agency-affiliate').DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "stateSave": (Utils.getJsonFromUrl().search == null) ? true : false,
            "ajax": {
                url: "/Admin/SearchAllTopAgencyAffiliate",
                type: 'POST',
                data: function (data) {
                    if (data.search.value == "") {
                        data.search.value = Utils.getJsonFromUrl().search;
                    }
                },
                complete: function (data) {
                    AllTopAgencyAffiliate.loadEditable();
                    var table = AllTopAgencyAffiliate.AllTopAgencyAffiliateDataTable;
                }
            },
            'deferRender': true,
            'select': {
                'style': 'multi'
            },
            'order': [[1, 'asc']],
            "language": DTLang.getLang(),
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
                        return full.totalSaleInString;
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
                        return full.affiliateCreatedDateInString;
                    }
                },
                {
                    "data": "Tier1DirectRate",
                    "render": function (data, type, full, meta) {
                        if (full.isLocked === false)
                            return '<a class="editable editable-unlocked" id="' + full.id + '" data-value="' + full.tier1DirectRate + '" data-name="Tier1DirectRate" data-pk=' + full.agencyId + ' href="#">' + full.tier1DirectRate + '</a>';
                        else
                            return '<a class="editable editable-locked" id="' + full.id + '" data-value="' + full.tier1DirectRate + '"data-name="Tier1DirectRate" data-pk=' + full.agencyId + '>' + full.tier1DirectRate + '</a>';
                    },
                    "orderable": false
                },
                {
                    "data": "Tier2DirectRate",
                    "render": function (data, type, full, meta) {
                        if (full.isLocked === false)
                            return '<a class="editable editable-unlocked" id="' + full.id + '" data-value="' + full.tier2DirectRate + '" data-name="Tier2DirectRate" data-pk=' + full.agencyId + ' href="#">' + full.tier2DirectRate + '</a>';
                        else
                            return '<a class="editable editable-locked" id="' + full.id + '" data-value="' + full.tier2DirectRate + '" data-name="Tier2DirectRate" data-pk=' + full.agencyId + '>' + full.tier2DirectRate + '</a>';
                    },
                    "orderable": false
                },
                {
                    "data": "Tier3DirectRate",
                    "render": function (data, type, full, meta) {
                        if (full.isLocked === false)
                            return '<a class="editable editable-unlocked" id="' + full.id + '" data-value="' + full.tier3DirectRate + '" data-name="Tier3DirectRate" data-pk=' + full.agencyId + ' href="#">' + full.tier3DirectRate + '</a>';
                        else
                            return '<a class="editable editable-locked" id="' + full.id + '" data-value="' + full.tier3DirectRate + '" data-name="Tier3DirectRate" data-pk=' + full.agencyId + '>' + full.tier3DirectRate + '</a>';
                    },
                    "orderable": false
                },
                {
                    "data": "Tier2SaleToTier1Rate",
                    "render": function (data, type, full, meta) {
                        if (full.isLocked === false)
                            return '<a class="editable editable-unlocked" id="' + full.id + '" data-value="' + full.tier2SaleToTier1Rate + '" data-name="Tier2SaleToTier1Rate" data-pk=' + full.agencyId + ' href="#">' + full.tier2SaleToTier1Rate + '</a>';
                        else
                            return '<a class="editable editable-locked" id="' + full.id + '" data-value="' + full.tier2SaleToTier1Rate + '" data-name="Tier2SaleToTier1Rate" data-pk=' + full.agencyId + '>' + full.tier2SaleToTier1Rate + '</a>';
                    },
                    "orderable": false
                },
                {
                    "data": "Tier3SaleToTier1Rate",
                    "render": function (data, type, full, meta) {
                        if (full.isLocked === false)
                            return '<a class="editable editable-unlocked" id="' + full.id + '" data-value="' + full.tier3SaleToTier1Rate + '" data-name="Tier3SaleToTier1Rate" data-pk=' + full.agencyId + ' href="#">' + full.tier3SaleToTier1Rate + '</a>';
                        else
                            return '<a class="editable editable-locked" id="' + full.id + '" data-value="' + full.tier3SaleToTier1Rate + '" data-name="Tier3SaleToTier1Rate" data-pk=' + full.agencyId + '>' + full.tier3SaleToTier1Rate + '</a>';
                    },
                    "orderable": false
                },
                {
                    "data": "Tier3SaleToTier2Rate",
                    "render": function (data, type, full, meta) {
                        if (full.isLocked === false)
                            return '<a class="editable editable-unlocked" id="' + full.id + '" data-value="' + full.tier3SaleToTier2Rate + '" data-name="Tier3SaleToTier2Rate" data-pk=' + full.agencyId + ' href="#">' + full.tier3SaleToTier2Rate + '</a>';
                        else
                            return '<a class="editable editable-locked" id="' + full.id + '" data-value="' + full.tier3SaleToTier2Rate + '" data-name="Tier3SaleToTier2Rate" data-pk=' + full.agencyId + '>' + full.tier3SaleToTier2Rate + '</a>';
                    },
                    "orderable": false
                },
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        var html = "<a style='margin: 2px' href='/Admin/TopAgencyAffiliate/" + full.id + "' target='_blank'  data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary'>" + $("#Affiliate").val() + "</a>";
                        html += "<a style='margin:2px' href='/Admin/User/" + full.id + "' target='_blank'  data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary'>" + $("#View").val() + "</a>";

                        return html;

                    },
                    "orderable": false
                }
            ]
        });
    },
    loadEditable: function () {
        $.fn.editable.defaults.clear = false;
        $.fn.editable.defaults.mode = 'popup';
        $.fn.editable.defaults.placement = 'top';
        $.fn.editable.defaults.type = 'number';
        $.fn.editable.defaults.step = '1.00';
        $.fn.editable.defaults.min = '0.00';
        $.fn.editable.defaults.max = '100.00';
        $("#dt-all-top-agency-affiliate tr").each(function (index, element) {
            AllTopAgencyAffiliate.loadEditableOnRow(element);
        });
    },
    loadEditableOnRow: function (element) {
        $(element).find('a.editable').editable({
            url: function (params) {
                return $.ajax({
                    cache: false,
                    async: true,
                    type: 'POST',
                    data: { agencyId: params.pk, value: params.value, name:params.name },
                    url: 'DoUpdateAllTopAgencyAffiliateRate',
                    success: function (data) {
                        toastr.success(data.message, 'Success!');
                    },
                    error: function (data) {
                        toastr.error(data.message, 'Error!');
                    }
                });
            },
        });
        $(element).find('a.editable-locked').editable('toggleDisabled');
    },
};

$(document).ready(function () {
    AllTopAgencyAffiliate.init();
});