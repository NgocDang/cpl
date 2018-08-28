var Resend = {
    init: function () {
        $("#btn-resend").on("click", function () {
            var isFormValid = $("#form-resend")[0].checkValidity();
            $("#form-resend").addClass('was-validated');

            if ($("#form-resend")[0].checkValidity()) {
                $("#resend-error").hide();
                $.ajax({
                    url: "/Authentication/Resend/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-resend").attr("disabled", true);
                        $("#btn-resend").html("<i class='fa fa-spinner fa-spin'></i> " + $("#btn-resend").text());
                    },
                    data: {
                        Email: $("#Email").val()
                    },
                    success: function (data) {
                        if (data.success) {
                            $("#resend-message").html(data.message);
                            $("#resend-message").show();
                            $("#form-resend").hide();
                        } else {
                            $("#resend-error").html(data.message);
                            $("#resend-error").show();
                        }
                    },
                    complete: function (data) {
                        $("#btn-resend").attr("disabled", false);
                        $("#btn-resend").html($("#btn-resend").text());
                    }
                });
            }
            return false;
        });
    }
};


$(document).ready(function () {
    Resend.init();
});