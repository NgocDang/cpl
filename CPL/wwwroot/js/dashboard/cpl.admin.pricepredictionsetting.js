var AdminPricePredictionSetting = {
    PricePredictionSettingDataTable: null,
    init: function () {
        AdminPricePredictionSetting.PricePredictionSettingDataTable = AdminPricePredictionSetting.loadPricePredictionSettingDataTable();
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
                    "data": "BettingTime",
                    "render": function (data, type, full, meta) {
                        return full.bettingTime;
                    }
                },
                {
                    "data": "HoldingTime",
                    "render": function (data, type, full, meta) {
                        return full.holdingTime;
                    }
                },
                {
                    "data": "RaffleTime",
                    "render": function (data, type, full, meta) {
                        return full.raffleTime;
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
                        var actions = " <button style='line-height:12px;margin:2px' data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-edit'>" + $("#edit").val() + "</button>";
                        actions += "<button style='line-height:12px;margin:2px' data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-delete'>" + $("#delete").val() + "</button>";
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