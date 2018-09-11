var AdminLottery = {
    lotteryDataTable: null,
    init: function () {
        AdminLottery.lotteryDataTable = AdminLottery.loadLotteryDataTable();
        AdminLottery.loadLightBox();
        AdminLottery.bindAddLottery();
        AdminLottery.bindEditLottery();
        AdminLottery.bindDeleteLottery();
        AdminLottery.bindAddPrize();
        AdminLottery.bindDeletePrize();
        AdminLottery.bindDoAddLottery();
        AdminLottery.bindDoAddAndPublishLottery();
        AdminLottery.bindDoEditLottery();
        AdminLottery.bindDoEditAndPublishLottery();
        AdminLottery.bindDoActivateLottery();
        AdminLottery.bindDoDeactivateLottery();
        AdminLottery.bindDoDeleteLottery();
        AdminLottery.bindViewLottery();
        AdminLottery.bindViewLotteryPrize();
    },
    loadLotteryDataTable: function () {
        return $('#dt-all-lottery-game').DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "stateSave": true,
            "ajax": {
                url: "/Admin/SearchLottery",
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
                            return "<p class='text-sm-center'><span class='badge badge-info'>" + $("#pending").val() + "</span></p>";
                        }
                        else if (full.status == 2) {
                            return "<p class='text-sm-center'><span class='badge badge-success'>" + $("#active").val() + "</span></p>"; 
                        }
                        else if (full.status == 3) {
                            return "<p class='text-sm-center'><span class='badge badge-secondary'>" + $("#completed").val() + "</span></p>";
                        }
                        else if (full.status == 4) {
                            return "<p class='text-sm-center'><span class='badge badge-warning'>" + $("#deactivated").val() + "</span></p>";
                        }
                        else {
                            return "";
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
                    "data": "DesktopTopImage",
                    "render": function (data, type, full, meta) {
                        return "<a data-toggle='lightbox' data-gallery='document-" + full.id + "' href='/images/lottery/" + full.desktopTopImage + "' ><img src='/images/lottery/" + full.desktopTopImage + "' class='img-thumbnail img-fluid border-0' alt='document-" + full.desktopTopImage + "'> </a>";
                    },
                    "orderable": false
                },
                {
                    "data": "DesktopListingImage",
                    "render": function (data, type, full, meta) {
                        return "<a data-toggle='lightbox' data-gallery='document-" + full.id + "'  href='/images/lottery/" + full.desktopListingImage + "' ><img src='/images/lottery/" + full.desktopListingImage + "' class='img-thumbnail img-fluid border-0' alt='document-" + full.desktopListingImage + "'> </a>";
                    },
                    "orderable": false
                },
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        if (full.status == 1) { // pendding
                            return "<button style='line-height:12px;margin: 2px' data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-edit'>" + $("#edit").val()
                                + "</button> <button style='line-height:12px;margin: 2px' data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-activate'>" + $("#activate").val()
                                + "</button><button style='line-height:12px;margin: 2px' data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-delete'>" + $("#delete").val() + "</button>";
                        }
                        else {
                            var html = "<button data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-view'>" + $("#view").val() + "</button>";
                            if (full.status == 2) { // active
                                html += "<button style='line-height:12px;margin: 2px' data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-deactivate'>" + $("#deactivate").val() + "</button>";
                            }
                            else if (full.status == 4) { // deactivate
                                html += "<button style='line-height:12px;margin: 2px' data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-activate'>" + $("#activate").val() + "</button>";
                            }
                            if (full.status != 3) { // completed
                                html += "<button style='line-height:12px;margin: 2px' data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-delete'>" + $("#delete").val() + "</button>";
                            }
                            return html;
                        }
                    },
                    "orderable": false
                }
            ],
        });
    },

    loadUserPrizeTable: function () {
        var datatable = $("#dt-user-prize").DataTable({
            //"order": [[1, 'asc']], // keep this to be an example .
            "order": [],
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "ajax": {
                url: "/Admin/SearchUserLotteryPrize",
                type: 'POST',
                data: {
                    lotteryId: $("#LotteryId").val(),
                    lotteryPrizeId: $("#LotteryPrizeId").val()
                }
            },
            "language": DTLang.getLang(),
            "columns": [
                {
                    "data": "#",
                    "render": function (data, type, full, meta) {
                        return "";
                    },
                    "className": "text-center",
                    "orderable": false
                },
                {
                     "data": "Email",
                    "render": function (data, type, full, meta) {
                        return full.email;
                    },
                    "className": "text-center",
                    "orderable": false
                },
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        return "<a class='btn btn-sm btn-outline-secondary btn-view' target='_blank' href='/history/lottery?sysUserId=" + (full.id) + "'" + ">" + $("#view").val() + "</a>";
                    },
                    "orderable": false
                }
            ]
        });

        // Here we create the index column in jquery datatable
        datatable.on('draw.dt', function () {
            var info = datatable.page.info();
            datatable.column(0, { search: 'applied', order: 'removed', page: 'applied', }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1 + info.start;
            });
        });
    },

    loadLightBox: function () {
        $(document).on('click', '[data-toggle="lightbox"]', function (event) {
            event.preventDefault();
            $(this).ekkoLightbox();
        });
    },

    bindAddLottery: function () {
        $("#lottery-game-management").on("click", "#btn-add", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/AddLottery",
                type: "GET",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                success: function (data) {
                    $("#modal").html(data);
                    $("#edit-lottery").modal("show");
                    $("#edit-lottery .btn-do-edit").hide();
                    $("#edit-lottery .btn-do-edit-publish").hide();
                    $("#prize-lottery")
                        .find("div.row.row-prize")
                        .map(function () {
                            $(this).find("#prize-title").html("#" + $(this).find("#prize-title-id").val());
                        });
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                }
            });
            return false;
        });
    },

    bindViewLottery: function () {
        $("#dt-all-lottery-game").on("click", ".btn-view", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/ViewLottery",
                type: "GET",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: {
                    id: $(_this).data().id
                },
                success: function (data) {
                    $("#modal").html(data);
                    $("#view-lottery").modal("show");
                    AdminLottery.LoadLottery1stPrize();
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                }
            });
            return false;
        });
    },

    bindViewLotteryPrize: function () {
        $("#modal").on("click", "#view-lottery .btn-prize", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/ViewLotteryPrize",
                type: "GET",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: {
                    lotteryId: $("#Id").val(),
                    lotteryPrizeId: $(_this).data().prizeId,
                },
                success: function (data) {
                    $("#lottery-result").html(data);
                    AdminLottery.loadUserPrizeTable();
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                    $("#view-lottery .btn-prize").removeClass("active");
                    $(_this).addClass("active");
                }
            });
            return false;
        });
    },

    LoadLottery1stPrize: function () {
        if ($("#modal #view-lottery .btn-prize").length > 1) {
            $.ajax({
                url: "/Admin/ViewLotteryPrize",
                type: "GET",
                beforeSend: function () {
                },
                data: {
                    lotteryId: $("#Id").val(),
                    lotteryPrizeId: 1,
                },
                success: function (data) {
                    $("#lottery-result").html(data);
                    $("#view-lottery .btn-prize:first").addClass("active");
                    AdminLottery.loadUserPrizeTable();
                },
                complete: function (data) {
                }
            });
        }
    },

    bindEditLottery: function () {
        $("#dt-all-lottery-game").on("click", ".btn-edit", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/EditLottery",
                type: "GET",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: {
                    id: $(_this).data().id
                },
                success: function (data) {
                    $("#modal").html(data);
                    $("#edit-lottery").modal("show");
                    $("#edit-lottery .btn-do-add").hide();
                    $("#edit-lottery .btn-do-add-publish").hide();
                    $("#prize-lottery")
                        .find("div.row.row-prize")
                        .map(function () {
                            $(this).find("#prize-title").html("#" + $(this).find("#prize-title-id").val());
                        });
                    $($("#prize-lottery").find("div.row.row-prize").last().prev()).find(".btn-remove-prize").removeClass("d-none");
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                }
            });
            return false;
        });
    },

    bindDeleteLottery: function () {
        $("#dt-all-lottery-game").on("click", ".btn-delete", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/ConfirmDeleteLottery",
                type: "GET",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: {
                    id: $(_this).data().id
                },
                success: function (data) {
                    $("#modal").html(data);
                    $("#delete-lottery").modal("show");
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                }
            });
            return false;
        });
    },

    bindAddPrize: function () {
        $("#modal").on("click", ".btn-add-prize", function () {
            var _this = this;

            var _parent = $(_this).closest("div.row");
            var _newPrize = _parent.clone();
            var _uncles = $(_this).parents("#prize-lottery");

            _parent.prev().find(".btn-delete-prize").addClass("d-none");

            _parent.removeClass("new-prize");
            _parent.find("div").removeClass("blurry-text");
            _parent.find(".form-control").removeAttr("disabled");
            _parent.find(".btn-add-prize").addClass("d-none");
            _parent.find(".btn-delete-prize").removeClass("d-none");

            _newPrize.find("h6#prize-title").html("#" + (parseInt(_uncles[0].children.length) + 1));

            _uncles.append('<div class="row pt-1 row-prize new-prize">' + _newPrize.html()) + '</div>';

            return false;
        });
    },

    bindDeletePrize: function () {
        $("#modal").on("click", ".btn-delete-prize", function () {
            var _this = this;
            var _uncles = $(_this).parents("#prize-lottery");

            var _prevUncle = $(_this).closest("div.row").prev();
            var _nextUncle = $(_this).closest("div.row").next();

            $(_this).closest("div.row").remove();
            if (_uncles[0].children.length > 2) {
                _prevUncle.find(".btn-delete-prize").removeClass("d-none");
            };

            _nextUncle.find("h6#prize-title").html("#" + parseInt(_uncles[0].children.length));

            return false;
        });
    },

    bindDoAddLottery: function () {
        $('#modal').on('click', '.btn-do-add', function () {
            var _this = this;
            var isFormValid = $(_this).parents("form")[0].checkValidity();
            $(_this).parents("form").addClass('was-validated');
            var prizeCounter = $(_this).parents("#form-edit-lottery").find("#prize-lottery div.row.row-prize").length;
            if (prizeCounter <= 1) {
                $("#prize-required").addClass("d-block");
                return false;
            }
            else {
                $("#prize-required").removeClass("d-block");
            }
            if (isFormValid) {
                var formData = new FormData();

                var desktopTopImage = $("#desktop-slide-image").get(0);
                if (desktopTopImage !== undefined && desktopTopImage.files.length > 0) {
                    formData.append('DesktopTopImageFile', desktopTopImage.files[0]);
                }

                var mobileTopImage = $("#mobile-slide-image").get(0);
                if (mobileTopImage !== undefined && mobileTopImage.files.length > 0) {
                    formData.append('MobileTopImageFile', mobileTopImage.files[0]);
                }

                var desktopListingImage = $("#desktop-listing-image").get(0);
                if (desktopListingImage !== undefined && desktopListingImage.files.length > 0) {
                    formData.append('DesktopListingImageFile', desktopListingImage.files[0]);
                }

                var mobileListingImage = $("#mobile-listing-image").get(0);
                if (mobileListingImage !== undefined && mobileListingImage.files.length > 0) {
                    formData.append('MobileListingImageFile', mobileListingImage.files[0]);
                }

                var prizeImage = $("#prize-image").get(0);
                if (prizeImage !== undefined && prizeImage.files.length > 0) {
                    formData.append('PrizeImageFile', prizeImage.files[0]);
                }

                formData.append('Title', $(_this).parents("#form-edit-lottery").find("#title").val());
                formData.append('UnitPrice', $(_this).parents("#form-edit-lottery").find("#ticket-price").val());
                formData.append('Volume', $(_this).parents("#form-edit-lottery").find("#volume").val());
                formData.append('IsPublished', false);

                $(_this).parents("#form-edit-lottery").find("#prize-lottery div.row.row-prize").map(function (i, e) {
                    formData.append('LotteryPrizes[' + i + '].Value', $(this).find("#prize-award").val());
                    formData.append('LotteryPrizes[' + i + '].Volume', $(this).find("#prize-number-of-ticket").val());
                });

                $.ajax({
                    url: "/Admin/DoAddLottery/",
                    type: "POST",
                    processData: false,
                    contentType: false,
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                        $(_this).html("<i class='fa fa-spinner fa-spin'></i> " + $(_this).text());
                    },
                    data: formData,
                    success: function (data) {
                        if (data.success) {
                            $("#edit-lottery").modal("hide");
                            toastr.success(data.message, 'Success!');
                            AdminLottery.lotteryDataTable.ajax.reload();
                        } else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $(_this).attr("disabled", false);
                        $(_this).html($(_this).text() + "<i class='la la-plus'></i>");
                    }
                });
            }
            return false;
        });
    },

    bindDoAddAndPublishLottery: function () {
        $('#modal').on('click', '.btn-do-add-publish', function () {
            var _this = this;
            var isFormValid = $(_this).parents("form")[0].checkValidity();
            $(_this).parents("form").addClass('was-validated');
            var prizeCounter = $(_this).parents("#form-edit-lottery").find("#prize-lottery div.row.row-prize").length;
            if (prizeCounter <= 1) {
                $("#prize-required").addClass("d-block");
                return false;
            }
            else {
                $("#prize-required").removeClass("d-block");
            }
            if (isFormValid) {
                var formData = new FormData();

                var desktopTopImage = $("#desktop-slide-image").get(0);
                if (desktopTopImage !== undefined && desktopTopImage.files.length > 0) {
                    formData.append('DesktopTopImageFile', desktopTopImage.files[0]);
                }

                var mobileTopImage = $("#mobile-slide-image").get(0);
                if (mobileTopImage !== undefined && mobileTopImage.files.length > 0) {
                    formData.append('MobileTopImageFile', mobileTopImage.files[0]);
                }

                var desktopListingImage = $("#desktop-listing-image").get(0);
                if (desktopListingImage !== undefined && desktopListingImage.files.length > 0) {
                    formData.append('DesktopListingImageFile', desktopListingImage.files[0]);
                }

                var mobileListingImage = $("#mobile-listing-image").get(0);
                if (mobileListingImage !== undefined && mobileListingImage.files.length > 0) {
                    formData.append('MobileListingImageFile', mobileListingImage.files[0]);
                }

                var prizeImage = $("#prize-image").get(0);
                if (prizeImage !== undefined && prizeImage.files.length > 0) {
                    formData.append('PrizeImageFile', prizeImage.files[0]);
                }

                formData.append('Title', $(_this).parents("#form-edit-lottery").find("#title").val());
                formData.append('UnitPrice', $(_this).parents("#form-edit-lottery").find("#ticket-price").val());
                formData.append('Volume', $(_this).parents("#form-edit-lottery").find("#volume").val());
                formData.append('IsPublished', true);

                $(_this).parents("#form-edit-lottery").find("#prize-lottery div.row.row-prize").map(function (i, e) {
                    formData.append('LotteryPrizes[' + i + '].Value', $(this).find("#prize-award").val());
                    formData.append('LotteryPrizes[' + i + '].Volume', $(this).find("#prize-number-of-ticket").val());
                });

                $.ajax({
                    url: "/Admin/DoAddLottery/",
                    type: "POST",
                    processData: false,
                    contentType: false,
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                        $(_this).html("<i class='fa fa-spinner fa-spin'></i> " + $(_this).text());
                    },
                    data: formData,
                    success: function (data) {
                        if (data.success) {
                            $("#edit-lottery").modal("hide");
                            toastr.success(data.message, 'Success!');
                            AdminLottery.lotteryDataTable.ajax.reload();
                        } else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $(_this).attr("disabled", false);
                        $(_this).html($(_this).text() + "<i class='la la-plus'></i>");
                    }
                });
            }
            return false;
        });
    },

    bindDoEditLottery: function () {
        $('#modal').on('click', '.btn-do-edit', function () {
            var _this = this;
            var isFormValid = $(_this).parents("form")[0].checkValidity();
            $(_this).parents("form").addClass('was-validated');
            var prizeCounter = $(_this).parents("#form-edit-lottery").find("#prize-lottery div.row.row-prize").length;
            if (prizeCounter <= 1) {
                $("#prize-required").addClass("d-block");
                return false;
            }
            else {
                $("#prize-required").removeClass("d-block");
            }
            if (isFormValid) {
                var formData = new FormData();

                var desktopTopImage = $("#desktop-slide-image").get(0);
                if (desktopTopImage !== undefined && desktopTopImage.files.length > 0) {
                    formData.append('DesktopTopImageFile', desktopTopImage.files[0]);
                }

                var mobileTopImage = $("#mobile-slide-image").get(0);
                if (mobileTopImage !== undefined && mobileTopImage.files.length > 0) {
                    formData.append('MobileTopImageFile', mobileTopImage.files[0]);
                }

                var desktopListingImage = $("#desktop-listing-image").get(0);
                if (desktopListingImage !== undefined && desktopListingImage.files.length > 0) {
                    formData.append('DesktopListingImageFile', desktopListingImage.files[0]);
                }

                var mobileListingImage = $("#mobile-listing-image").get(0);
                if (mobileListingImage !== undefined && mobileListingImage.files.length > 0) {
                    formData.append('MobileListingImageFile', mobileListingImage.files[0]);
                }

                var prizeImage = $("#prize-image").get(0);
                if (prizeImage !== undefined && prizeImage.files.length > 0) {
                    formData.append('PrizeImageFile', prizeImage.files[0]);
                }

                formData.append('Id', $(_this).parents("#form-edit-lottery").find("#lottery-id").val());
                formData.append('Title', $(_this).parents("#form-edit-lottery").find("#title").val());
                formData.append('UnitPrice', $(_this).parents("#form-edit-lottery").find("#ticket-price").val());
                formData.append('Volume', $(_this).parents("#form-edit-lottery").find("#volume").val());
                formData.append('IsPublished', false);

                $(_this).parents("#form-edit-lottery").find("#prize-lottery div.row.row-prize").map(function (i, e) {
                    formData.append('LotteryPrizes[' + i + '].Value', $(this).find("#prize-award").val());
                    formData.append('LotteryPrizes[' + i + '].Volume', $(this).find("#prize-number-of-ticket").val());
                });

                $.ajax({
                    url: "/Admin/DoEditLottery/",
                    type: "POST",
                    processData: false,
                    contentType: false,
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                        $(_this).html("<i class='fa fa-spinner fa-spin'></i> " + $(_this).text());
                    },
                    data: formData,
                    success: function (data) {
                        if (data.success) {
                            $("#edit-lottery").modal("hide");
                            toastr.success(data.message, 'Success!');
                            AdminLottery.lotteryDataTable.ajax.reload();
                        } else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $(_this).attr("disabled", false);
                        $(_this).html($(_this).text() + "<i class='la la-plus'></i>");
                    }
                });
            }
            return false;
        });
    },

    bindDoEditAndPublishLottery: function () {
        $('#modal').on('click', '.btn-do-edit-publish', function () {
            var _this = this;
            var isFormValid = $(_this).parents("form")[0].checkValidity();
            $(_this).parents("form").addClass('was-validated');
            var prizeCounter = $(_this).parents("#form-edit-lottery").find("#prize-lottery div.row.row-prize").length;
            if (prizeCounter <= 1) {
                $("#prize-required").addClass("d-block");
                return false;
            }
            else {
                $("#prize-required").removeClass("d-block");
            }
            if (isFormValid) {
                var formData = new FormData();

                var desktopTopImage = $("#desktop-slide-image").get(0);
                if (desktopTopImage !== undefined && desktopTopImage.files.length > 0) {
                    formData.append('DesktopTopImageFile', desktopTopImage.files[0]);
                }

                var mobileTopImage = $("#mobile-slide-image").get(0);
                if (mobileTopImage !== undefined && mobileTopImage.files.length > 0) {
                    formData.append('MobileTopImageFile', mobileTopImage.files[0]);
                }

                var desktopListingImage = $("#desktop-listing-image").get(0);
                if (desktopListingImage !== undefined && desktopListingImage.files.length > 0) {
                    formData.append('DesktopListingImageFile', desktopListingImage.files[0]);
                }

                var mobileListingImage = $("#mobile-listing-image").get(0);
                if (mobileListingImage !== undefined && mobileListingImage.files.length > 0) {
                    formData.append('MobileListingImageFile', mobileListingImage.files[0]);
                }

                var prizeImage = $("#prize-image").get(0);
                if (prizeImage !== undefined && prizeImage.files.length > 0) {
                    formData.append('PrizeImageFile', prizeImage.files[0]);
                }

                formData.append('Id', $(_this).parents("#form-edit-lottery").find("#lottery-id").val());
                formData.append('Title', $(_this).parents("#form-edit-lottery").find("#title").val());
                formData.append('UnitPrice', $(_this).parents("#form-edit-lottery").find("#ticket-price").val());
                formData.append('Volume', $(_this).parents("#form-edit-lottery").find("#volume").val());
                formData.append('IsPublished', true);

                $(_this).parents("#form-edit-lottery").find("#prize-lottery div.row.row-prize").map(function (i, e) {
                    formData.append('LotteryPrizes[' + i + '].Value', $(this).find("#prize-award").val());
                    formData.append('LotteryPrizes[' + i + '].Volume', $(this).find("#prize-number-of-ticket").val());
                });

                $.ajax({
                    url: "/Admin/DoEditLottery/",
                    type: "POST",
                    processData: false,
                    contentType: false,
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                        $(_this).html("<i class='fa fa-spinner fa-spin'></i> " + $(_this).text());
                    },
                    data: formData,
                    success: function (data) {
                        if (data.success) {
                            $("#edit-lottery").modal("hide");
                            toastr.success(data.message, 'Success!');
                            AdminLottery.lotteryDataTable.ajax.reload();
                        } else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $(_this).attr("disabled", false);
                        $(_this).html($(_this).text() + "<i class='la la-plus'></i>");
                    }
                });
            }
            return false;
        });
    },

    bindDoActivateLottery: function () {
        $('#dt-all-lottery-game').on('click', '.btn-activate', function () {
            var _this = this;
            $.ajax({
                url: "/Admin/DoActivateLottery/",
                type: "POST",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: {
                    id: $(_this).data().id
                },
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message, 'Success!');
                        AdminLottery.lotteryDataTable.ajax.reload();
                    } else {
                        toastr.error(data.message, 'Error!');
                    }
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                    $(_this).html($(_this).text());
                }
            });
            return false;
        });
    },

    bindDoDeactivateLottery: function () {
        $('#dt-all-lottery-game').on('click', '.btn-deactivate', function () {
            var _this = this;
            $.ajax({
                url: "/Admin/DoDeactivateLottery/",
                type: "POST",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: {
                    id: $(_this).data().id
                },
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message, 'Success!');
                        AdminLottery.lotteryDataTable.ajax.reload();
                    } else {
                        toastr.error(data.message, 'Error!');
                    }
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                    $(_this).html($(_this).text());
                }
            });
            return false;
        });
    },


    bindDoDeleteLottery: function () {
        $('#modal').on('click', '.btn-do-delete', function () {
            var _this = this;
            $.ajax({
                url: "/Admin/DoDeleteLottery/",
                type: "POST",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: {
                    id: $(_this).parents("#delete-lottery").find("#game-id").val()
                },
                success: function (data) {
                    if (data.success) {
                        $("#delete-lottery").modal("hide");
                        toastr.success(data.message, 'Success!');
                        AdminLottery.lotteryDataTable.ajax.reload();
                    } else {
                        toastr.error(data.message, 'Error!');
                    }
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                    $(_this).html($(_this).text());
                }
            });
            return false;
        });
    },

}

$(document).ready(function () {
    AdminLottery.init();
});