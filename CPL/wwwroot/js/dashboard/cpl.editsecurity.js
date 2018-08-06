var EditSecurity = {
    init: function () {
        EditSecurity.bindSaveKYCButton();
    },
    bindSaveKYCButton: function () {
        $("#form-edit-account").on("click", "#btn-save-account", function () {
            if ($("#KYCVerified").val() !== "") {
                $("#btn-save-account").attr("disabled", true);
            }
            else {
                $("#btn-save-account").attr("disabled", false);
                var fsFileUpload = $("#FrontSideImage").get(0);
                if (fsFileUpload !== undefined && fsFileUpload.files.length > 0) {
                    var fsFile = fsFileUpload.files[0];
                }
                var bsFileUpload = $("#BackSideImage").get(0);
                if (bsFileUpload !== undefined && bsFileUpload.files.length > 0) {
                    var bsFile = bsFileUpload.files[0];
                }

                var isFormValid = $("#form-edit-account").valid();

                if (isFormValid) {
                    var formData = new FormData();
                    formData.append('FrontSideImage', fsFile);
                    formData.append('BackSideImage', bsFile);
                    $.ajax({
                        url: "/Profile/UpdateKYC/",
                        type: "POST",
                        processData: false,
                        contentType: false,
                        beforeSend: function () {
                            $("#btn-save-account").attr("disabled", true);
                            $("#btn-save-account").html("<i class='fa fa-spinner fa-spin'></i> <i class='far fa-save'></i> " + $("#btn-save-account").text());
                        },
                        data: formData,
                        success: function (data) {
                            if (data.success) {
                                toastr.success(data.message, 'Success!');
                                if (fsFile !== undefined && bsFile !== undefined) {
                                    $("#kyc-verify").replaceWith("<div class='row mb-1 col-sm-12' id='kyc-verify'><p class='text-muted'>" + data.kycconfirm + "</p><span class='badge badge-info h-50'>" + data.kycverify + "</span></div>");
                                }
                                $("#KYCVerified").val(0);
                            } else {
                                toastr.error(data.message, 'Error!');
                            }
                        },
                        complete: function (data) {
                            $("#btn-save-account").attr("disabled", false);
                            $("#btn-save-account").html("<i class='far fa-save'></i> " + $("#btn-save-account").text());
                        }
                    });
                }
                return false;
            }
        });
    },
    
};

$(document).ready(function () {
    EditSecurity.init();
});