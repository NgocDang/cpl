var DepositAndWithdrawViewComponent = {
    init: function () {
        DepositAndWithdrawViewComponent.bindCopy();
        DepositAndWithdrawViewComponent.bindMax();
        DepositAndWithdrawViewComponent.bindDoWithdraw();
        DepositAndWithdrawViewComponent.bindDoReadQrCode();
    },
    bindCopy: function () {
        if ($(".btn-copy").length > 0) {
            var clipboard = new ClipboardJS('.btn-copy');
            clipboard.on('success', function (e) {
                toastr.success($("#CopiedSuccessfully").val());
            });
        }
    },
    bindMax: function () {
        $("#depositwithdraw-content").on("click", ".btn-max", function () {
            $(this).parents("form").find(".amount-value").val($(this).parents("form").find(".amount-value").attr("max"));
        });
    },
    bindDoWithdraw: function () {
        $("#depositwithdraw-content").on("click", ".btn-do-withdraw", function () {
            var _this = this;
            var isFormValid = $(_this).parents("form")[0].checkValidity();
            $(_this).parents("form").addClass('was-validated');
            if (isFormValid) {
                $.ajax({
                    url: "/DepositAndWithdraw/DoWithdraw/",
                    type: "POST",
                    data: {
                        Currency: $(_this).parents("form").find(".currency").val(),
                        Amount: $(_this).parents("form").find(".amount-value").val(),
                        Address: $(_this).parents("form").find(".address-value").val(),
                    },
                    success: function (data) {
                        if (data.success) {
                            $(_this).parents("form").find(".address-error").hide();
                            $(_this).parents("form").find(".amount-error").hide();
                            toastr.success(data.message, 'Success!');
                            ViewComponent.getDepositWithdrawViewComponent("withdraw");
                        }
                        else {
                            if (data.requireProfile != null && !data.requireProfile) {
                                DepositAndWithdrawViewComponent.getRequireProfilePartialView();
                            }
                            else if (data.requireKyc != null && !data.requireKyc) {
                                DepositAndWithdrawViewComponent.getRequireKYCPartialView();
                            } else {
                                if (data.name === "wallet") {
                                    $(_this).parents("form").find(".amount-error").hide();
                                    $(_this).parents("form").find(".address-error").show();
                                    $(_this).parents("form").find(".address-error").html(data.message);
                                }
                                if (data.name === "amount") {
                                    $(_this).parents("form").find(".address-error").hide();
                                    $(_this).parents("form").find(".amount-error").html(data.message);
                                    $(_this).parents("form").find(".amount-error").show();
                                }

                                toastr.error(data.message, 'Error!');
                            }

                        }
                    }
                });
            }
           
            return false;
        });
    },
    bindDoReadQrCode: function () {
        $("#depositwithdraw-content").on("click", ".btn-qrcode", function () {
            $(this).parents("form").find(".file-qrcode").click();
        });

        $("#depositwithdraw-content").on("change", ".file-qrcode", function () {
            var _this = this;
            var image = $(_this).parents("form").find(".file-qrcode")[0].files[0];
            if (image.size > 0) {
                var formData = new FormData();
                formData.append('FormFile', image);
                $.ajax({
                    url: "/DepositAndWithdraw/DecodeQR/",
                    type: "POST",
                    processData: false,
                    contentType: false,
                    data: formData,
                    success: function (data) {
                        if (data.success) {
                            $(_this).parents("form").find(".address-value").val(data.address);
                            toastr.success(data.message, 'Success!');
                        } else {
                            $(_this).parents("form").find(".address-value").val("");
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                    }
                })
            };
            return false;
        });
    },
    getRequireProfilePartialView: function () {
        $.ajax({
            url: "/DepositAndWithdraw/GetRequireProfile/",
            type: "GET",
            processData: false,
            contentType: false,
            success: function (data) {
                $("#modal").html(data);
                $("#depositandwithdraw-profile").modal("show");
            }
        });
    },
    getRequireKYCPartialView: function () {
        $.ajax({
            url: "/DepositAndWithdraw/GetRequireKYC/",
            type: "GET",
            processData: false,
            contentType: false,
            success: function (data) {
                $("#modal").html(data);
                $("#depositandwithdraw-kyc").modal("show");
            }
        });
    }
}

$(document).ready(function () {
    DepositAndWithdrawViewComponent.init();
});