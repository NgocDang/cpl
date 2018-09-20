var AllTopAgencyAffiliate = {
    AllTopAgencyAffiliateDataTable: null,
    init: function () {
        AllTopAgencyAffiliate.isCheckAllRow = false;
        AllTopAgencyAffiliate.AllTopAgencyAffiliateDataTable = AllTopAgencyAffiliate.loadAllTopAgencyAffiliateDataTable();
        AllTopAgencyAffiliate.initAllTopAgencyAffiliateDataTable();
        AllTopAgencyAffiliate.bindDoLock();
        AllTopAgencyAffiliate.bindDoUpdateRateMultipleRow();
    },
    initAllTopAgencyAffiliateDataTable: function () {
        AllTopAgencyAffiliate.AllTopAgencyAffiliateDataTable.on('responsive-display', function (e, datatable, row, showHide, update) {
            AllTopAgencyAffiliate.loadEditable();
        });
        AllTopAgencyAffiliate.AllTopAgencyAffiliateDataTable.column(0).checkboxes.deselectAll();
    },
    loadAllTopAgencyAffiliateDataTable: function () {
        debugger;
        return $('#dt-all-top-agency-affiliate').DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "stateSave": (Utils.getJsonFromUrl().search == null) ? true : false,
            "ajax": {
                url: "/Admin/SearchTopAgencyAffiliate",
                type: 'POST',
                data: function (data) {
                    if (data.search.value == "") {
                        data.search.value = Utils.getJsonFromUrl().search;
                    }
                },
                complete: function (data) {
                    AllTopAgencyAffiliate.loadEditable();
                    var table = AllTopAgencyAffiliate.AllTopAgencyAffiliateDataTable;
                    table.cells(
                        table.rows(function (idx, data, node) {
                            return data.isLocked === true;
                        }).indexes(),
                        0
                    ).checkboxes.disable();
                }
            },
            'columnDefs': [
                {
                    'targets': 0,
                    'render': function (data, type, row, meta) {
                        if (type === 'display') {
                            data = '<div class="checkbox"><input type="checkbox" class="dt-checkboxes"><label></label></div>';
                        }

                        return data;
                    },
                    'checkboxes': {
                        'selectRow': true,
                        'selectAllRender': '<div class="checkbox"><input type="checkbox" class="dt-checkboxes"><label></label></div>'
                    }
                }
            ],
            'deferRender': true,
            'select': {
                'style': 'multi'
            },
            'order': [[1, 'asc']],
            "language": DTLang.getLang(),
            "columns": [
                {
                    "data": "agencyId"
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
                    "data": "Email",
                    "render": function (data, type, full, meta) {
                        return "<span class='word-break'>" + full.email + "</span>";
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
                            return '<a class="editable editable-unlocked" id="' + full.id + '" data-name="Tier1DirectRate" data-pk=' + full.agencyId + ' href="#">' + full.tier1DirectRate + '</a>';
                        else
                            return '<a class="editable editable-locked" id="' + full.id + '" data-name="Tier1DirectRate" data-pk=' + full.agencyId + '>' + full.tier1DirectRate + '</a>';
                    },
                    "orderable": false
                },
                {
                    "data": "Tier2DirectRate",
                    "render": function (data, type, full, meta) {
                        if (full.isLocked === false)
                            return '<a class="editable editable-unlocked" id="' + full.id + '" data-name="Tier2DirectRate" data-pk=' + full.agencyId + ' href="#">' + full.tier2DirectRate + '</a>';
                        else
                            return '<a class="editable editable-locked" id="' + full.id + '" data-name="Tier2DirectRate" data-pk=' + full.agencyId + '>' + full.tier2DirectRate + '</a>';
                    },
                    "orderable": false
                },
                {
                    "data": "Tier3DirectRate",
                    "render": function (data, type, full, meta) {
                        if (full.isLocked === false)
                            return '<a class="editable editable-unlocked" id="' + full.id + '" data-name="Tier3DirectRate" data-pk=' + full.agencyId + ' href="#">' + full.tier3DirectRate + '</a>';
                        else
                            return '<a class="editable editable-locked" id="' + full.id + '" data-name="Tier3DirectRate" data-pk=' + full.agencyId + '>' + full.tier3DirectRate + '</a>';
                    },
                    "orderable": false
                },
                {
                    "data": "Tier2SaleToTier1Rate",
                    "render": function (data, type, full, meta) {
                        if (full.isLocked === false)
                            return '<a class="editable editable-unlocked" id="' + full.id + '" data-name="Tier2SaleToTier1Rate" data-pk=' + full.agencyId + ' href="#">' + full.tier2SaleToTier1Rate + '</a>';
                        else
                            return '<a class="editable editable-locked" id="' + full.id + '" data-name="Tier2SaleToTier1Rate" data-pk=' + full.agencyId + '>' + full.tier2SaleToTier1Rate + '</a>';
                    },
                    "orderable": false
                },
                {
                    "data": "Tier3SaleToTier1Rate",
                    "render": function (data, type, full, meta) {
                        if (full.isLocked === false)
                            return '<a class="editable editable-unlocked" id="' + full.id + '" data-name="Tier3SaleToTier1Rate" data-pk=' + full.agencyId + ' href="#">' + full.tier3SaleToTier1Rate + '</a>';
                        else
                            return '<a class="editable editable-locked" id="' + full.id + '" data-name="Tier3SaleToTier1Rate" data-pk=' + full.agencyId + '>' + full.tier3SaleToTier1Rate + '</a>';
                    },
                    "orderable": false
                },
                {
                    "data": "Tier3SaleToTier2Rate",
                    "render": function (data, type, full, meta) {
                        if (full.isLocked === false)
                            return '<a class="editable editable-unlocked" id="' + full.id + '" data-name="Tier3SaleToTier2Rate" data-pk=' + full.agencyId + ' href="#">' + full.tier3SaleToTier2Rate + '</a>';
                        else
                            return '<a class="editable editable-locked" id="' + full.id + '" data-name="Tier3SaleToTier2Rate" data-pk=' + full.agencyId + '>' + full.tier3SaleToTier2Rate + '</a>';
                    },
                    "orderable": false
                },
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        if (full.isLocked === false)
                            return "<div class='text-lg-center'>" +
                                "<button data-id='" + full.id + "'class='btn btn-sm btn-outline-secondary btn-lock'>" + $("#lock").val() + "</button> " +
                                "</div>";
                        else
                            return "<div class='text-lg-center'>" +
                                "<button data-id='" + full.id + "'class='btn btn-sm btn-outline-secondary btn-lock'>" + $("#unlock").val() + "</button> " +
                                "</div>";
                    },
                    "orderable": false
                }
            ]
        });
    },
    bindDoLock: function () {
        $("#dt-all-top-agency-affiliate").on("click", ".btn-lock", function () {
            var _this = this;
            var table = AllTopAgencyAffiliate.AllTopAgencyAffiliateDataTable;
            $.ajax({
                url: "/Admin/DoLockAffiliate/",
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
                            table.cell(table.row($(_this).closest("tr")).index(), 0).checkboxes.enable();
                            if (AllTopAgencyAffiliate.isCheckAllRow)
                                table.cell(table.row($(_this).closest("tr")).index(), 0).checkboxes.select(true);
                        }
                        else {
                            $(_this).html($("#unlock").val());
                            $(_this).closest("tr").find("a.editable").removeClass("editable-unlocked");
                            $(_this).closest("tr").find("a.editable").addClass("editable-locked");
                            $(_this).closest("tr").find("a.editable").removeAttr("href");
                            table.cell(table.row($(_this).closest("tr")).index(), 0).checkboxes.select(false);
                            table.cell(table.row($(_this).closest("tr")).index(), 0).checkboxes.disable();
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
                debugger;
                var requestData = '';
                if (params.name == 'Tier1DirectRate')
                    requestData = { Id: params.pk, Tier1DirectRate: params.value }
                else if (params.name == 'Tier2DirectRate')
                    requestData = { Id: params.pk, Tier2DirectRate: params.value }
                else if (params.name == 'Tier3DirectRate')
                    requestData = { Id: params.pk, Tier3DirectRate: params.value }
                else if (params.name == 'Tier2SaleToTier1Rate')
                    requestData = { Id: params.pk, Tier2SaleToTier1Rate: params.value }
                else if (params.name == 'Tier3SaleToTier1Rate')
                    requestData = { Id: params.pk, Tier3SaleToTier1Rate: params.value }
                else if (params.name == 'Tier3SaleToTier2Rate')
                    requestData = { Id: params.pk, Tier3SaleToTier2Rate: params.value }
                else
                    return;
                return $.ajax({
                    cache: false,
                    async: true,
                    type: 'POST',
                    data: requestData,
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
    bindDoUpdateRateMultipleRow: function () {
        $("#form-comission-rate-setting").on("click", "#btn-update", function () {
            debugger;
            var rows_selected = AllTopAgencyAffiliate.AllTopAgencyAffiliateDataTable.column(0).checkboxes.selected();
            var _postData = {};
            if (rows_selected.count() == 0)
                return false;
            else {
                if (AllTopAgencyAffiliate.isCheckAllRow == true) {
                    _postData["IsCheckedAll"] = true;
                }
                _postData["Ids"] = new Array(rows_selected.count());
                $.each(rows_selected, function (i, value) {
                    _postData["Ids"][i] = parseInt(value);
                });
            }
            var isFormValid = $("#form-comission-rate-setting")[0].checkValidity();
            $("#form-comission-rate-setting").addClass('was-validated');
            var _this = this;
            if (isFormValid) {
                var _formData = $("#form-comission-rate-setting").serializeArray();
                _formData.forEach(function (element) {
                    _postData[element['name']] = parseInt(element['value']);
                });
                var _data = _postData;
                $.ajax({
                    url: "/Admin/DoUpdateAllTopAgencyAffiliateRates/",
                    type: "POST",
                    dataType: 'json',
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                        $(_this).html("<i class='fa fa-spinner fa-spin'> </i> " + $(_this).text());
                    },
                    data: { 'viewModel': _data },
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message, 'Success!');
                            AllTopAgencyAffiliate.AllTopAgencyAffiliateDataTable.ajax.reload();
                            AllTopAgencyAffiliate.AllTopAgencyAffiliateDataTable.column(0).checkboxes.deselectAll();
                            $("#form-comission-rate-setting")[0].reset();
                        } else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $(_this).attr("disabled", false);
                        $(_this).html($(_this).text());
                        $("#form-comission-rate-setting").removeClass('was-validated');
                    }
                });
            }

            return false;
        });
    }
};

$(document).ready(function () {
    AllTopAgencyAffiliate.init();
});