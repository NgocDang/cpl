var AdminPricePredictionSetting = {
    pricePredictionSettingDataTable: null,
    init: function () {
        AdminPricePredictionSetting.pricePredictionSettingDataTable = AdminPricePredictionSetting.loadPricePredictionSettingDataTable();
        AdminPricePredictionSetting.bindDoDeletePricePredictionSetting();
    },
    bindDoDeletePricePredictionSetting: function () {
        $("#dt-price-prediction-setting").on("click", ".btn-do-delete", function () {
            var _this = this;
            var result = confirm("Do you really want to delete this price prediction setting?");
            if (result) {
                $.ajax({
                    url: "/Admin/DoDeletePricePredictionSetting/",
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
                            toastr.success(data.message, 'Success!');
                            AdminPricePredictionSetting.pricePredictionSettingDataTable.ajax.reload();
                        } else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $(_this).attr("disabled", false);
                        $(_this).html($(_this).text());
                    }
                });
            }
            return false;
        });
    },
    loadPricePredictionSettingDataTable: function () {
        return $('#dt-price-prediction-setting').DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "stateSave": true,
            "ajax": {
                url: "/Admin/SearchPricePredictionSetting",
                type: 'POST'
            },
            "language": DTLang.getLang(),
            "columns": [
                {
                    "data": "CreatedDate",
                    "render": function (data, type, full, meta) {
                        return full.createdDate;
                    }
                },
                {
                    "data": "Title",
                    "render": function (data, type, full, meta) {
                        if ($("#LangId").val() == 1) // english
                            return full.pricePredictionSettingDetails[0].title;
                        else if ($("#LangId").val() == 2) // japanese
                            return full.pricePredictionSettingDetails[1].title;
                        else
                            return "";
                    },
                    "orderable": false
                },
                {
                    "data": "Status",
                    "render": function (data, type, full, meta) {
                        if (full.status === 0) {
                            return "<p class='text-sm-center'><span class='badge badge-success'>" + $("#active").val() + "</span></p>";
                        }
                        else {
                            return "";
                        }
                    }
                },
                {
                    "data": "BettingTimeInString",
                    "render": function (data, type, full, meta) {
                        return full.bettingTimeInString;
                    }
                },
                {
                    "data": "HoldingTimeInterval",
                    "render": function (data, type, full, meta) {
                        return full.holdingTimeInterval;
                    }
                },
                {
                    "data": "ResultTimeInterval",
                    "render": function (data, type, full, meta) {
                        return full.resultTimeInterval;
                    }
                },
                {
                    "data": "DividendRate",
                    "render": function (data, type, full, meta) {
                        return full.dividendRate;
                    }
                },
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        var actions = " <button style='margin:2px' data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-edit'>" + $("#edit").val() + "</button>";
                        actions += "<button style='margin:2px' data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-do-delete'>" + $("#delete").val() + "</button>";
                        return actions;
                    },
                    "orderable": false
                }
            ],
        });
    }
};

$(document).ready(function () {
    AdminPricePredictionSetting.init();
});