var StandardAffiliate = {
    standardAffiliateDataTable: null,
    init: function () {
        StandardAffiliate.standardAffiliateDataTable = StandardAffiliate.loadstandardAffiliateDataTable();
        StandardAffiliate.standardAffiliateDataTable.on('responsive-display', function (e, datatable, row, showHide, update) {
            StandardAffiliate.loadEditable();
        });
        StandardAffiliate.bindDoLock();
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
                complete: function (data) {
                    StandardAffiliate.loadEditable();
                }
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
                        return full.affiliateCreatedDateInString;
                    }
                },
                {
                    "data": "Tier1DirectRate",
                    "render": function (data, type, full, meta) {
                        if (full.isLocked === false)
                            return '<a style="border: none" class="editable editable-unlocked" id="' + full.id + '" data-name="Tier1DirectRate" data-pk=' + full.affiliateId + ' href="#">' + full.tier1DirectRate + '</a>';
                        else
                            return '<a style="border: none" class="editable editable-locked" id="' + full.id + '" data-name="Tier1DirectRate" data-pk=' + full.affiliateId + '>' + full.tier1DirectRate + '</a>';
                    },
                    "orderable": false
                },
                {
                    "data": "Tier2SaleToTier1Rate",
                    "render": function (data, type, full, meta) {
                        if (full.isLocked === false)
                            return '<a style="border: none" class="editable editable-unlocked" id="' + full.id + '" data-name="Tier2SaleToTier1Rate" data-pk=' + full.affiliateId + ' href="#">' + full.tier2SaleToTier1Rate + '</a>';
                        else
                            return '<a style="border: none" class="editable editable-locked" id="' + full.id + '" data-name="Tier2SaleToTier1Rate" data-pk=' + full.affiliateId + '>' + full.tier2SaleToTier1Rate + '</a>';
                    },
                    "orderable": false
                },
                {
                    "data": "Tier3SaleToTier1Rate",
                    "render": function (data, type, full, meta) {
                        if (full.isLocked === false)
                            return '<a style="border: none" class="editable editable-unlocked" id="' + full.id + '" data-name="Tier3SaleToTier1Rate" data-pk=' + full.affiliateId + ' href="#">' + full.tier3SaleToTier1Rate + '</a>';
                        else
                            return '<a style="border: none" class="editable editable-locked" id="' + full.id + '" data-name="Tier3SaleToTier1Rate" data-pk=' + full.affiliateId + '>' + full.tier3SaleToTier1Rate + '</a>';
                    },
                    "orderable": false
                },
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        if (full.isLocked === false)
                            return "<div class='text-lg-center'>" +
                                "<button data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-lock'>" + $("#lock").val() + "</button> " +
                                "</div>";
                        else
                            return "<div class='text-lg-center'>" +
                                "<button data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-lock'>" + $("#unlock").val() + "</button> " +
                                "</div>";
                    },
                    "orderable": false
                }
            ]
        });
    },
    bindDoLock: function () {
        $("#dt-standard-affiliate").on("click", ".btn-lock", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/DoLockStandardAffiliate/",
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
                        if (data.isLocked == false) {
                            $(_this).html($("#lock").val());
                            $(_this).closest("tr").find("a.editable").removeClass("editable-locked");
                            $(_this).closest("tr").find("a.editable").addClass("editable-unlocked");
                            $(_this).closest("tr").find("a.editable").attr("href", "#");
                        }
                        else {
                            $(_this).html($("#unlock").val());
                            $(_this).closest("tr").find("a.editable").removeClass("editable-unlocked");
                            $(_this).closest("tr").find("a.editable").addClass("editable-locked");
                            $(_this).closest("tr").find("a.editable").removeAttr("href");
                        }
                        $(_this).closest("tr").find('a.editable').editable('toggleDisabled');
                        toastr.success(data.message, 'Success!');
                    }
                    else
                        toastr.error(data.message, 'Error!');
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                }
            });
        });
    },
    loadEditable: function () {
        $.fn.editable.defaults.mode = 'popup';
        $.fn.editable.defaults.placement = 'top';
        $.fn.editable.defaults.type = 'number';
        $.fn.editable.defaults.step = '1.00';
        $.fn.editable.defaults.min = '0.00';
        $.fn.editable.defaults.max = '100.00';
        $("#dt-standard-affiliate tr").each(function (index, element) {
            StandardAffiliate.loadEditableOnRow(element);
        });
    },
    loadEditableOnRow: function (element) {
        $(element).find('a.editable').editable({
            url: function (params) {
                var requestData = '';
                if (params.name == 'Tier1DirectRate')
                    requestData = { Id: params.pk, Tier1DirectRate: params.value }
                else if (params.name == 'Tier2SaleToTier1Rate')
                    requestData = { Id: params.pk, Tier2SaleToTier1Rate: params.value }
                else if (params.name == 'Tier3SaleToTier1Rate')
                    requestData = { Id: params.pk, Tier3SaleToTier1Rate: params.value }
                else
                    return;
                return $.ajax({
                    cache: false,
                    async: true,
                    type: 'POST',
                    data: requestData,
                    url: 'DoUpdateStandardAffiliateRate',
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
    }
}

$(document).ready(function () {
    StandardAffiliate.init();
});