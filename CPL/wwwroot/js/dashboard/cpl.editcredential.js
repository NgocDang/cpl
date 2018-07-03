var EditCredential = {
    init: function () {
        EditCredential.bindSaveEmail();
        EditCredential.bindSavePassword();
    },
    bindSaveEmail: function () {
        $("#btn-save-email").on("click", function () {
            var isFormValid = $("#form-edit-email").valid();
            var isConfirmEmailValid = $("#NewEmail").val() == $("#NewEmailConfirm").val();
            if (!isConfirmEmailValid) {
                $("#new-email-confirm-msg").show();
            } else {
                $("#new-email-confirm-msg").hide();
            }

            if (isFormValid && isConfirmEmailValid) {
                $("#new-email-msg").hide();

                $.ajax({
                    url: "/Profile/EditEmail/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-save-email").attr("disabled", true);
                        $("#btn-save-email").html("<i class='fa fa-spinner fa-spin'></i> <i class='far fa-save'></i> " + $("#btn-save-email").text());
                    },
                    data: {
                        CurrentEmail: $("#CurrentEmail").val(),
                        NewEmail: $("#NewEmail").val(),
                        NewEmailConfirm: $("#NewEmailConfirm").val()
                    },
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message, 'Success!');
                        } else {
                            if (data.name == "new-email") 
                                $("#new-email-msg").show();
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $("#btn-save-email").attr("disabled", false);
                        $("#btn-save-email").html("<i class='far fa-save'></i> " + $("#btn-save-email").text());
                    }
                });
            }
            return false;
        });
    },
    bindSavePassword: function () {
        $("#btn-save-password").on("click", function () {
            var isFormValid = $("#form-edit-password").valid();
            var isConfirmPasswordValid = $("#NewPassword").val() == $("#NewPasswordConfirm").val();
            if (!isConfirmPasswordValid) {
                $("#new-password-confirm-msg").show();
            } else {
                $("#new-password-confirm-msg").hide();
            }

            if (isFormValid && isConfirmPasswordValid) {
                $("#current-password-msg").hide();

                $.ajax({
                    url: "/Profile/EditPassword/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-save-password").attr("disabled", true);
                        $("#btn-save-password").html("<i class='fa fa-spinner fa-spin'></i> <i class='far fa-save'></i> " + $("#btn-save-password").text());
                    },
                    data: {
                        CurrentPassword: $("#CurrentPassword").val(),
                        NewPassword: $("#NewPassword").val(),
                        NewPasswordConfirm: $("#NewPasswordConfirm").val()
                    },
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message, 'Success!');
                        } else {
                            if (data.name == "current-password")
                                $("#current-password-msg").show();
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $("#btn-save-password").attr("disabled", false);
                        $("#btn-save-password").html("<i class='far fa-save'></i> " + $("#btn-save-password").text());
                    }
                });
            }
            return false;
        });
    },
};


$(document).ready(function () {
    EditCredential.init();
});