var LotteryGames = {
    lotteryGamesDataTable: null,
    init: function () {
        LotteryGames.lotteryGamesDataTable = LotteryGames.loadLotteryGamesDataTable();
        LotteryGames.loadLightBox();
    },
    loadLightBox: function () {
        $(document).on('click', '[data-toggle="lightbox"]', function (event) {
            event.preventDefault();
            $(this).ekkoLightbox();
        });
    },
    loadLotteryGamesDataTable: function () {
        return $('#dt-all-lottery-game').DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "stateSave": true,
            "ajax": {
                url: "/Admin/SearchLotteryGame",
                type: 'POST',
            },
            "language": DTLang.getLang(),
            "columns": [
                {
                    "data": "Phase",
                    "render": function (data, type, full, meta) {
                        return full.phase;
                    }
                },
                {
                    "data": "CreatedDate",
                    "render": function (data, type, full, meta) {
                        return full.createdDateInString;
                    }
                },
                {
                    "data": "Status",
                    "render": function (data, type, full, meta) {
                        if (full.status == 1) {
                            return "<p class='text-center'><span class='badge badge-warning'>" + $("#pending").val() + "</span></p>";
                        }
                        else if (full.status == 2) {
                            return "<p class='text-center'><span class='badge badge-success'>" + $("#active").val() + "</span></p>";
                        }
                        else if (full.status == 3) {
                            return "<p class='text-center'><span class='badge badge-danger'>" + $("#completed").val() + "</span></p>";
                        }
                    }
                },
                {
                    "data": "Volume",
                    "render": function (data, type, full, meta) {
                        return full.volume;
                    },
                },
                {
                    "data": "Title",
                    "render": function (data, type, full, meta) {
                        return full.title;
                    }
                },
                {
                    "data": "SlideImage",
                    "render": function (data, type, full, meta) {
                        return "<a data-toggle='lightbox' data-gallery='document-" + full.id + "' href='/images/lottery/" + full.slideImage + "' ><img src=/images/lottery/" + full.slideImage + " class='img-thumbnail img-fluid' alt='document-" + full.slideImage + "'> </a>";
                    },
                    "orderable": false
                },
                {
                    "data": "DesktopListingImage",
                    "render": function (data, type, full, meta) {
                        return "<a data-toggle='lightbox' data-gallery='document-" + full.id + "'  href='/images/lottery/" + full.desktopListingImage + "' ><img src=/images/lottery/" + full.desktopListingImage + " class='img-thumbnail img-fluid' alt='document-" + full.desktopListingImage + "'> </a>";
                    },
                    "orderable": false
                },
                {
                    "data": "MobileListingImage",
                    "render": function (data, type, full, meta) {
                        return "<a data-toggle='lightbox' data-gallery='document-" + full.id + "'  href='/images/lottery/" + full.mobileListingImage + "' ><img src=/images/lottery/" + full.mobileListingImage + " class='img-thumbnail img-fluid' alt='document-" + full.mobileListingImage + "'> </a>";
                    },
                    "orderable": false
                },
                
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        if (full.status == 1) {
                            return "<button style='line-height:12px;' data-id='" + full.id + "' class='btn btn-sm btn-primary btn-edit'>" + $("#edit").val()
                                + "</button> <button style='line-height:12px;' data-id='" + full.id + "' class='btn btn-sm btn-success btn-active'>" + $("#active").val()
                                + "</button><button style='line-height:12px;' data-id='" + full.id + "' class='btn btn-sm btn-warning btn-delete'>" + $("#delete").val() + "</button>";
                        }
                        else
                            return "<button style='line-height:12px;' data-id='" + full.id + "' class='btn btn-sm btn-info btn-view'>" + $("#view").val() + "</button>";
                    },
                    "orderable": false
                }
            ],
        });
    },
}

$(document).ready(function () {
    LotteryGames.init();
});