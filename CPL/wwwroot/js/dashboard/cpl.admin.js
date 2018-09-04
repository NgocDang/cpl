var Admin = {
    init: function () {
        Admin.bindAdminSearchAllUser();
        Admin.bindAdminSearchStandardAffiliate();
        Admin.bindAdminSearchAgencyAffiliate();
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
    }
};

$(document).ready(function () {
    Admin.init();
});

