var AdminSlider = {
    sliderDataTable: null,
    init: function () {
        $("#group-picker").selectpicker('refresh');
        AdminSlider.sliderDataTable = AdminSlider.loadSliderDataTable();
        AdminSlider.loadLightBox();
        AdminSlider.bindSliderGroupChange();
        AdminSlider.bindAddSlider();
        AdminSlider.bindDoAddSlider();
        AdminSlider.bindEditSlider();
        AdminSlider.bindDoEditSlider();
        AdminSlider.bindViewSlider();
        AdminSlider.bindDoActivateSlider();
        AdminSlider.bindDoDeactivateSlider();
        AdminSlider.bindDoDelete();
    },
    loadLightBox: function () {
        $(document).on('click', '[data-toggle="lightbox"]', function (event) {
            event.preventDefault();
            $(this).ekkoLightbox();
        });
    },

    loadSliderDataTable: function () {
        return $('#dt-all-slider').DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "ajax": {
                url: "/Admin/SearchSlider",
                type: 'POST',
                data: {
                    groupId: parseInt($("#group-id").val())
                }
            },
            "language": DTLang.getLang(),
            "columns": [
                {
                    "data": "Name",
                    "render": function (data, type, full, meta) {
                        return full.name;
                    },
                    "width": "20%"
                },
                {
                    "data": "Url",
                    "render": function (data, type, full, meta) {
                        return "<a href='" + full.url + "'>" + full.url + "</a>";
                    },
                    "width": "20%"
                },
                {
                    "data": "DesktopImage",
                    "render": function (data, type, full, meta) {
                        return "<a data-toggle='lightbox' data-gallery='document-" + full.id + "' href='/images/slider/" + full.sliderDetails[1].desktopImage + "' ><img src='/images/slider/" + full.sliderDetails[1].desktopImage + "' class='img-thumbnail img-fluid border-0' alt='document-" + full.sliderDetails[1].desktopImage + "'> </a>";
                    },
                    "orderable": false,
                    "width": "20%"
                },
                {
                    "data": "MobileImage",
                    "render": function (data, type, full, meta) {
                        return "<a data-toggle='lightbox' data-gallery='document-" + full.id + "' href='/images/slider/" + full.sliderDetails[1].mobileImage + "' ><img src='/images/slider/" + full.sliderDetails[1].mobileImage + "' class='img-thumbnail img-fluid border-0' alt='document-" + full.sliderDetails[1].mobileImage + "'> </a>";
                    },
                    "orderable": false,
                    "width": "20%"
                },
                {
                    "data": "Status",
                    "render": function (data, type, full, meta) {
                        if (full.status === 0) {
                            return "<p class='text-sm-center'><span class='badge badge-success'>" + $("#active").val() + "</span></p>";
                        }
                        else if (full.status === 1) {
                            return "<p class='text-sm-center'><span class='badge badge-warning'>" + $("#deactivated").val() + "</span></p>";
                        }
                        else {
                            return "";
                        }
                    },
                    "width": "20%"
                },
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        var html = "<button data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-view'>" + $("#view").val() + "</button>  <br />"
                            + "<button style='margin: 2px' data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-edit'>" + $("#edit").val() + "</button>";
                        if (full.status === 0) { // active
                            html += "<button style='margin: 2px' data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-deactivate'>" + $("#deactivate").val() + "</button>";
                        }
                        else if (full.status === 1) { // deactivate
                            html += "<button style='margin: 2px' data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-activate'>" + $("#activate").val() + "</button>";
                        }
                        return html;
                    },
                    "orderable": false,
                    "width": "20%"
                }
            ]
        });
    },

    bindSliderGroupChange: function () {
        $("#group-picker").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                var _this = this;

                $("#group-id").val($(_this).val());

                AdminSlider.sliderDataTable.destroy();
                AdminSlider.sliderDataTable = AdminSlider.loadSliderDataTable();
            });
    },

    bindAddSlider: function () {
        $("#slider-management").on("click", "#btn-add", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/AddSlider",
                type: "GET",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: {
                    id: $(_this).data().id
                },
                success: function (data) {
                    $("#modal").html(data);
                    $("#edit-slider").modal("show");
                    $("#edit-slider #btn-do-edit").hide();
                    $("#edit-slider #btn-do-delete").hide();
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                }
            });
            return false;
        });
    },

    bindDoAddSlider: function () {
        $('#modal').on('click', '#btn-do-add', function () {
            var _this = this;
            var isFormValid = $(_this).parents("form")[0].checkValidity();
            $(_this).parents("form").addClass('was-validated');

            if (isFormValid) {
                var formData = new FormData();
                $(_this).parents("#form-edit-slider").find("#slider-multilanguage div.tab-pane").each(function (i, e) {
                    var desktopImage = $(e).find("#desktop-image").get(0);
                    if (desktopImage !== undefined && desktopImage.files.length > 0) {
                        formData.append('SliderDetails[' + i + '].DesktopImageFile', desktopImage.files[0]);
                    }

                    var mobileImage = $(e).find("#mobile-image").get(0);
                    if (mobileImage !== undefined && mobileImage.files.length > 0) {
                        formData.append('SliderDetails[' + i + '].MobileImageFile', mobileImage.files[0]);
                    }

                    formData.append('SliderDetails[' + i + '].LangId', parseInt($(e).find("#lang-id").val()));
                    formData.append('SliderDetails[' + i + '].SliderId', $(_this).parents("#form-edit-slider").find("#slider-id").val());
                });

                formData.append('Name', $(_this).parents("#form-edit-slider").find("#Name").val());
                formData.append('Url', $(_this).parents("#form-edit-slider").find("#Url").val());
                formData.append('GroupId', parseInt($("#group-id").val()));

                $.ajax({
                    url: "/Admin/DoAddSlider/",
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
                            $("#edit-slider").modal("hide");
                            toastr.success(data.message, 'Success!');
                            AdminSlider.sliderDataTable.ajax.reload();
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

    bindEditSlider: function () {
        $("#dt-all-slider").on("click", ".btn-edit", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/EditSlider",
                type: "GET",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: {
                    id: $(_this).data().id
                },
                success: function (data) {
                    $("#modal").html(data);
                    $("#edit-slider").modal("show");
                    $("#edit-slider #btn-do-add").hide();
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                }
            });
            return false;
        });
    },

    bindDoEditSlider: function () {
        $('#modal').on('click', '#btn-do-edit', function () {
            var _this = this;
            var isFormValid = $(_this).parents("form")[0].checkValidity();
            $(_this).parents("form").addClass('was-validated');

            if (isFormValid) {
                var formData = new FormData();
                $(_this).parents("#form-edit-slider").find("#slider-multilanguage div.tab-pane").each(function (i, e) {
                    var desktopImage = $(e).find("#desktop-image").get(0);
                    if (desktopImage !== undefined && desktopImage.files.length > 0) {
                        formData.append('SliderDetails[' + i + '].DesktopImageFile', desktopImage.files[0]);
                    }

                    var mobileImage = $(e).find("#mobile-image").get(0);
                    if (mobileImage !== undefined && mobileImage.files.length > 0) {
                        formData.append('SliderDetails[' + i + '].MobileImageFile', mobileImage.files[0]);
                    }

                    formData.append('SliderDetails[' + i + '].LangId', parseInt($(e).find("#lang-id").val()));
                    formData.append('SliderDetails[' + i + '].Id', parseInt($(e).find("#detail-id").val()));
                    formData.append('SliderDetails[' + i + '].SliderId', $(_this).parents("#form-edit-slider").find("#slider-id").val());
                });

                formData.append('Name', $(_this).parents("#form-edit-slider").find("#Name").val());
                formData.append('Url', $(_this).parents("#form-edit-slider").find("#Url").val());
                formData.append('GroupId', parseInt($("#group-id").val()));
                formData.append('Id', parseInt($(_this).parents("#form-edit-slider").find("#slider-id").val()));

                $.ajax({
                    url: "/Admin/DoEditSlider/",
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
                            $("#edit-slider").modal("hide");
                            toastr.success(data.message, 'Success!');
                            AdminSlider.sliderDataTable.ajax.reload();
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

    bindViewSlider: function () {
        $("#dt-all-slider").on("click", ".btn-view", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/ViewSlider",
                type: "GET",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: {
                    id: $(_this).data().id
                },
                success: function (data) {
                    $("#modal").html(data);
                    $("#view-slider").modal("show");
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                }
            });
            return false;
        });
    },

    bindDoActivateSlider: function () {
        $('#dt-all-slider').on('click', '.btn-activate', function () {
            var _this = this;
            $.ajax({
                url: "/Admin/DoActivateSlider/",
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
                        AdminSlider.sliderDataTable.ajax.reload();
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

    bindDoDeactivateSlider: function () {
        $('#dt-all-slider').on('click', '.btn-deactivate', function () {
            var _this = this;
            $.ajax({
                url: "/Admin/DoDeactivateSlider/",
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
                        AdminSlider.sliderDataTable.ajax.reload();
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

    bindDoDelete: function () {
        $("#modal").on("click", "#btn-do-delete", function () {
            var result = confirm("Do you really want to delete this slider?");
            if (result) {
                var _this = this;
                $.ajax({
                    url: "/Admin/DoDeleteSlider",
                    type: "POST",
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                    },
                    data: {
                        Id: parseInt($(_this).parents("#form-edit-slider").find("#slider-id").val())
                    },
                    success: function (data) {
                        if (data.success) {
                            $("#edit-slider").modal("hide");
                            toastr.success(data.message, 'Success!');
                            AdminSlider.sliderDataTable.ajax.reload();
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
            }
        });
    }
};

$(document).ready(function () {
    AdminSlider.init();
});