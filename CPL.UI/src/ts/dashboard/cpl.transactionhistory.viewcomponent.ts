class TransactionHistoryViewComponent extends Base {

    transactionHistoryDataTable: DataTables.Api;
    // Define here the abstract method
    public init() {
        this.transactionHistoryDataTable = this.getTransactionHistoryDataTable();

    };

    public getTransactionHistoryDataTable() {
        return $("#dt-transaction-history").DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "ajax": {
                url: "/History/SearchTransactionHistory",
                type: 'POST',
                data: {
                    sysUserId: $("#transaction-history-view-component #SysUserId").val(),
                    currencyId: $("#transaction-history-view-component #CurrencyId").val()
                }
            },
            //"language": DTLang.getLang(),
            "columns": [
                {
                    "data": "CreatedDate",
                    "render": function (data, type, full, meta) {
                        return full.createdDateInString;
                    },
                    "className": "text-center",
                },
                {
                    "data": "ToWalletAddress",
                    "render": function (data, type, full, meta) {
                        return full.toWalletAddress;
                    },
                    "orderable": false
                },
                {
                    "data": "CoinAmount",
                    "render": function (data, type, full, meta) {
                        return full.coinAmountInString;
                    },
                    "className": "text-center",
                },
                {
                    "data": "CurrencyInString",
                    "render": function (data, type, full, meta) {
                        if (full.currencyInString == "ETH") {
                            return "<div class='badge badge-secondary'><i class='cc ETH-alt'></i> ETH</div>";
                        } else if (full.currencyInString == "BTC") {
                            return "<div class='badge badge-secondary'><i class='cc BTC-alt'></i> BTC</div>";
                        }
                    },
                    "className": "text-center",
                },
                {
                    "data": "TypeInString",
                    "render": function (data, type, full, meta) {
                        return full.typeInString;
                    },
                    "className": "text-center",
                },
                {
                    "data": "StatusInEnum",
                    "render": function (data, type, full, meta) {
                        if (full.statusInString == "PENDING")
                            return "<div class='badge badge-info'>Pending</div>";
                        else if (full.statusInString == "FAIL")
                            return "<div class='badge badge-danger'>Fail</div>";
                        else if (full.statusInString == "SUCCESS")
                            return "<div class='badge badge-success'>Success</div>";
                        else
                            return "";
                    },
                    "className": "text-center",
                },
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        return "<a href='/History/TransactionDetail/" + full.id + "' target='_blank'  data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-view'>" + $("#view").val() + "</a>";
                    },
                    "orderable": false
                }
            ],
        });
    }
};

new TransactionHistoryViewComponent();