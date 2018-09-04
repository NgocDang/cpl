var Admin = {
    init: function () {
        Admin.bindAdminSearchAllUser();
        Admin.bindAdminSearchStandardAffiliate();
        Admin.bindAdminSearchAgencyAffiliate();
        Admin.bindDoGenerate();
        Admin.bindCopy();
    },
    bindAdminSearchAllUser: function () {
        $("#search-all-user").on("keypress", function (e) {
            var key = e.keyCode || e.which;
            if (key == 13) {
                var searchstring = document.getElementById('search-all-user');
                if (searchstring.value != null && searchstring.value.replace(/\s+/g, '') != "")
                    window.location.href = "/Admin/AllUser?search=" + searchstring.value.replace(/\s+/g, '');
            }
        });
    },
    bindAdminSearchStandardAffiliate: function () {
        $("#search-standard-affiliate").on("keypress", function (e) {
            var key = e.keyCode || e.which;
            if (key == 13) {
                var searchstring = document.getElementById('search-standard-affiliate');
                if (searchstring.value != null && searchstring.value.replace(/\s+/g, '') != "")
                    // TODO: Change xxxxx by Action for Standard Affiliate
                    window.location.href = "/Admin/xxxxx?search=" + searchstring.value.replace(/\s+/g, '');
            }
        });
    },
    bindAdminSearchAgencyAffiliate: function () {
        $("#search-agency-affiliate").on("keypress", function (e) {
            var key = e.keyCode || e.which;
            if (key == 13) {
                var searchstring = document.getElementById('search-agency-affiliate');
                if (searchstring.value != null && searchstring.value.replace(/\s+/g, '') != "")
                    // TODO: Change xxxxx by Action for Agency Affiliate
                    window.location.href = "/Admin/xxxxx?search=" + searchstring.value.replace(/\s+/g, '');
            }
        });
    },
    bindCopy: function () {
        if ($(".btn-copy").length > 0) {
            var clipboard = new ClipboardJS('.btn-copy');
            clipboard.on('success', function (e) {
                toastr.success($("#CopiedSuccessfully").val());
            });
        }
    },
    bindDoGenerate: function () {
        $("#btn-generate").on("click", function () {
            var isFormValid = $("#form-agency-affiliate")[0].checkValidity();

            if (isFormValid) {
                $.ajax({
                    url: "/Admin/GenerateAgencyAffiliateUrl/",
                    type: "POST",
                    beforeSend: function () {
                        $("#btn-generate").attr("disabled", true);
                        $("#btn-generate").html("<i class='fa fa-spinner fa-spin'></i> " + $("#btn-generate").text());
                    },
                    data: {
                        NumberOfExpiredDays: $("#NumberOfExpiredDays").val()
                    },
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message, 'Success!');
                            $("#Url").val(data.url);
                            $("#agency-url").removeClass("d-none");
                        } else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $("#btn-generate").attr("disabled", false);
                        $("#btn-generate").html($("#btn-generate").text());
                    }
                });
            }
            return false;
        });
    }
};

$(document).ready(function () {
    Admin.init();
});

