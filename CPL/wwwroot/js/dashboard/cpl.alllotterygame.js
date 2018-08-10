var LotteryGames = {
    lotteryGamesDataTable: null,
    init: function () {
        LotteryGames.lotteryGamesDataTable = LotteryGames.loadLotteryGamesDataTable();
        LotteryGames.loadLightBox();
        LotteryGames.bindAddButton();
        LotteryGames.bindImagePreview("slide-image");
        LotteryGames.bindImagePreview("desktop-image");
        LotteryGames.bindImagePreview("mobile-image");

        var _this = $("#lottery-game-management");
        $.ajax({
            url: "/Admin/EditLotteryGame",
            type: "GET",
            beforeSend: function () {
                $(_this).attr("disabled", true);
            },
            data: {
                id: $(_this).data().id
            },
            success: function (data) {
                $("#modal").html(data);
                $("#edit-lottery-game").modal("show");
                $("#edit-lottery-game #btn-save").hide();
            },
            complete: function (data) {
                $(_this).attr("disabled", false);
            }
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
                    "data": "UnitPrice",
                    "render": function (data, type, full, meta) {
                        return full.unitPrice;
                    }
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
                        return "<a data-toggle='lightbox' data-gallery='document-" + full.id + "' href='/images/lottery/" + full.slideImage + "' ><img src=/images/lottery/" + full.slideImage + " class='img-thumbnail img-fluid border-0' alt='document-" + full.slideImage + "'> </a>";
                    },
                    "orderable": false
                },
                {
                    "data": "DesktopListingImage",
                    "render": function (data, type, full, meta) {
                        return "<a data-toggle='lightbox' data-gallery='document-" + full.id + "'  href='/images/lottery/" + full.desktopListingImage + "' ><img src=/images/lottery/" + full.desktopListingImage + " class='img-thumbnail img-fluid border-0' alt='document-" + full.desktopListingImage + "'> </a>";
                    },
                    "orderable": false
                },
                {
                    "data": "MobileListingImage",
                    "render": function (data, type, full, meta) {
                        return "<a data-toggle='lightbox' data-gallery='document-" + full.id + "'  href='/images/lottery/" + full.mobileListingImage + "' ><img src=/images/lottery/" + full.mobileListingImage + " class='img-thumbnail img-fluid border-0' alt='document-" + full.mobileListingImage + "'> </a>";
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
    loadLightBox: function () {
        $(document).on('click', '[data-toggle="lightbox"]', function (event) {
            event.preventDefault();
            $(this).ekkoLightbox();
        });
    },
    bindAddButton: function () {
        $("#lottery-game-management").on("click", "#btn-add", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/EditLotteryGame",
                type: "GET",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: {
                    id: $(_this).data().id
                },
                success: function (data) {
                    $("#modal").html(data);
                    $("#edit-lottery-game").modal("show");
                    $("#edit-lottery-game #btn-save").hide();
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                }
            });
            return false;
        });
    },
    bindImagePreview: function (id) {
        $("#modal").on("change", "#"+id, function () {
            var _this = this;
            if (_this.files && _this.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('[data-toggle="popover-' + id + '"]').popover({
                        html: true,
                        trigger: 'hover',
                        trigger: 'focus',
                        content: function () {
                            return '<img class="img-fluid" src="' + e.target.result + '" />';
                        }
                    }).popover('show');
                }
                reader.readAsDataURL(_this.files[0]);
            }
            return false;
        });
    }
}

$(document).ready(function () {
    LotteryGames.init();
});