var AdminPricePredictionSetting = {
    pricePredictionSettingDataTable: null,
    init: function () {
        AdminPricePredictionSetting.pricePredictionSettingDataTable = AdminPricePredictionSetting.loadPricePredictionSettingDataTable();

        AdminPricePredictionSetting.bindAddPricePredictionSetting();
        AdminPricePredictionSetting.bindAddPricePredictionCategory();
        AdminPricePredictionSetting.bindDoAddPricePredictionCategory();
        AdminPricePredictionSetting.bindDoAddPricePredictionSetting();
        AdminPricePredictionSetting.bindDoDeletePricePredictionSetting();
    },
    bindAddPricePredictionSetting: function () {
        $("#price-prediction-setting-game-management").on("click", "#btn-add", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/AddPricePredictionSetting",
                type: "GET",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                success: function (data) {
                    $("#modal").html(data);
                    $("#modal #price-prediction-category").selectpicker('refresh');
                    $("#edit-price-prediction-setting").modal("show");
                },
                complete: function (data) {
                    $('#open-betting-time-picker').timepicker({
                        template: false,
                        showInputs: false,
                        minuteStep: 15,
                        showMeridian: false,
                        defaultTime: '00:00',
                    });
                    $('#close-betting-time-picker').timepicker({
                        template: false,
                        showInputs: false,
                        minuteStep: 15,
                        showMeridian: false,
                        defaultTime: '00:00',
                    });

                    $(_this).attr("disabled", false);
                }
            });
            return false;
        });
    },
    bindAddPricePredictionCategory: function () {
        $("#modal").on("change", "#price-prediction-category", function () {
            var _this = this;
            if ($(_this).val() === "0") {
                $.ajax({
                    url: "/Admin/AddPricePredictionCategory",
                    type: "GET",
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                        $(_this).html("<i class='fa fa-spinner fa-spin'></i> " + $(_this).text());
                    },
                    success: function (data) {
                        $("#edit-price-prediction-setting").modal("hide");
                        $("#modal #modal-price-prediction-category").html(data);
                        $("#edit-price-prediction-category").modal("show");
                    },
                    complete: function (data) {
                        $(_this).attr("disabled", false);
                        $(_this).html($(_this).text());
                    }
                });
            };
            return false;
        });
    },
    bindDoAddPricePredictionCategory: function () {
        $("#modal").on("click", "#edit-price-prediction-category .btn-do-add", function () {
            var isFormValid = $("#form-edit-price-prediction-category")[0].checkValidity();
            $("#form-edit-price-prediction-category").addClass('was-validated');
            var _this = this;

            if (isFormValid) {
                var _postData = {};
                $(_this).closest("#form-edit-price-prediction-category").find("div.tab-pane").each(function (i, e) {
                    _postData['PricePredictionCategoryDetailAdminViewModels[' + i + '].LangId'] = $(this).find("#lang-id").val();
                    _postData['PricePredictionCategoryDetailAdminViewModels[' + i + '].Name'] = $(this).find("#name").val();
                    _postData['PricePredictionCategoryDetailAdminViewModels[' + i + '].Description'] = $(this).find("#description").val();
                });

                $.ajax({
                    url: "/Admin/DoAddPricePredictionCategory",
                    type: "POST",
                    data: _postData,
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                        $(_this).html("<i class='fa fa-spinner fa-spin'></i> " + $(_this).text() + " <i class='la la-plus font-size-15px'></i>");
                    },
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message, 'Success!');
                            $("#edit-price-prediction-category").modal("hide");
                            $("#modal #modal-price-prediction-category").empty();
                            $("#edit-price-prediction-setting").modal("show");
                            $("#modal #price-prediction-category").append($("<option selected='selected'></option>").val(data.id).html(data.name));
                            $("#modal #price-prediction-category").selectpicker('refresh');
                        } else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $(_this).attr("disabled", false);
                        $(_this).html($(_this).text());
                    }
                });
            };
            return false;
        });
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
    bindDoAddPricePredictionSetting: function () {
        $('#modal').on('click', '#edit-price-prediction-setting .btn-do-add', function () {
            var _this = this;

            //Validate for category
            var isCategoryValid = $("#price-prediction-category").val() != "";
            if (isCategoryValid)
                $("#category-msg").hide();
            else
                $("#category-msg").show();

            var isFormValid = $(_this).parents("form")[0].checkValidity();
            $(_this).parents("form").addClass('was-validated');

            if (isFormValid && isCategoryValid) {

                var _postData = {};

                var _formData = $("#form-edit-price-prediction-setting").serializeArray();
                _formData.forEach(function (element) {
                    if (element['name'] != 'Title') {
                        _postData[element['name']] = element['value'];
                    }
                });
                $("#price-prediction-setting-multilanguage").find("div.tab-pane").each(function (i, e) {
                    _postData['PricePredictionSettingDetails[' + i + '].LangId'] = $(this).find("#lang-id").val();
                    _postData['PricePredictionSettingDetails[' + i + '].Title'] = $(this).find("#title").val();
                });

                $.ajax({
                    url: "/Admin/DoAddPricePredictionSetting/",
                    type: "POST",
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                        $(_this).html("<i class='fa fa-spinner fa-spin'></i> " + $(_this).text());
                    },
                    data: _postData,
                    success: function (data) {
                        if (data.success) {
                            $("#edit-price-prediction-setting").modal("hide");
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
            "columns": [{
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
                    } else {
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