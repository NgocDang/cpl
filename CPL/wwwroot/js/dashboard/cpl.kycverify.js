var KYCVerify = {
    init: function () {
        KYCVerify.loadKYCDatatable();
    },
    loadKYCDatatable: function () {
        $('#dt-kyc').DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "ajax": {
                url: "/Dashboard/SearchKYC",
                type: 'POST'
            },
            "language": DTLang.getLang(),
            "columns": [
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
                        return full.email;
                    }
                },
                {
                    "data": "Address",
                    "render": function (data, type, full, meta) {
                        return full.streetAddress + " " + full.city + " " + full.country;
                    }
                },
                {
                    "data": "DOB",
                    "render": function (data, type, full, meta) {
                        if (full.result == "1") {
                            return "<div class='badge badge-success'>Win</div>";
                        } else if (full.result == "0")
                            return "<div class='badge badge-danger'>Lose</div>";
                        else
                            return "";
                    }
                },
                {
                    "data": "AwardInString",
                    "render": function (data, type, full, meta) {
                        return full.awardInString;
                    }
                },
                {
                    "data": "BalanceInString",
                    "render": function (data, type, full, meta) {
                        return full.balanceInString;
                    }
                }
            ],
        });
    }
}

$(document).ready(function () {
    Dashboard.init();
});