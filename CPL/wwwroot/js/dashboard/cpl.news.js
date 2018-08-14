var News = {
    newsDataTable: null,
    init: function () {
        News.newsDataTable = News.loadNewsDatatable();
        News.loadLightBox();
        News.bindEditButton();
        News.bindSaveButton();
        News.bindDeleteButton();
        News.bindAddButton();
        News.bindSubmitButton();
    },
    bindEditButton: function () {
        $("#dt-news").on("click", ".btn-edit", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/EditNews",
                type: "GET",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: {
                    id: $(_this).data().id
                },
                success: function (data) {
                    $("#modal").html(data);
                    $("#update-news").modal("show");
                    $("#update-news #btn-submit").hide();
                    tinymce.remove();
                    News.initTinyMCE();
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                }
            });
            return false;
        });
    },
    bindAddButton: function () {
        $("#news-management").on("click", "#btn-add", function () {
            var _this = this;
            $.ajax({
                url: "/Admin/EditNews",
                type: "GET",
                beforeSend: function () {
                    $(_this).attr("disabled", true);
                },
                data: {
                    id: $(_this).data().id
                },
                success: function (data) {
                    $("#modal").html(data);
                    $("#update-news").modal("show");
                    $("#update-news #btn-save").hide();
                    tinymce.remove();
                    News.initTinyMCE();
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                }
            });
            return false;
        });
    },
    bindSaveButton: function () {
        $("#modal").on("click", "#btn-save", function () {
            var _this = this;
            var isFormValid = $("#form-news")[0].checkValidity();
            $("#form-news").addClass('was-validated');

            if (tinymce.EditorManager.activeEditor.plugins.wordcount.getCount() == 0) {
                $("#full-desc").find(".invalid-feedback").show();
                return false;
            } else {
                $("#full-desc").find(".invalid-feedback").hide();
            }
            if (isFormValid) {
                var formData = new FormData();
                var fileImage = $("#Image").get(0);
                if (fileImage != undefined && fileImage.files.length > 0) {
                    formData.append('FileImage', fileImage.files[0]);
                }
                formData.append('Id', $("#modal #Id").val());
                formData.append('Title', $("#modal #Title").val());
                formData.append('ShortDescription', $("#modal #short-desc").val());
                formData.append('Description', tinymce.activeEditor.getContent());

                $.ajax({
                    url: "/Admin/SaveEditNews/",
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
                            $("#update-news").modal("hide");
                            News.newsDataTable.ajax.reload();
                            toastr.success(data.message, "Success!");
                        } else {
                            toastr.error(data.message, "Error!");
                        }
                    },
                    complete: function (data) {
                        $(_this).attr("disabled", false);
                        $(_this).html($(_this).text());
                    }
                });
                return false;
            }
            return false;
        });
    },
    bindSubmitButton: function () {
        $("#modal").on("click", "#btn-submit", function () {
            var _this = this;
            var isFormValid = $("#form-news")[0].checkValidity();
            $("#form-news").addClass('was-validated');
            if (tinymce.EditorManager.activeEditor.plugins.wordcount.getCount() == 0) {
                $("#full-desc").find(".invalid-feedback").show();
                return false;
            } else {
                $("#full-desc").find(".invalid-feedback").hide();
            }
            if (isFormValid) {
                var formData = new FormData();
                var fileImage = $("#Image").get(0);
                if (fileImage != undefined && fileImage.files.length > 0) {
                    formData.append('FileImage', fileImage.files[0]);
                }
                formData.append('Title', $("#modal #Title").val());
                formData.append('ShortDescription', $("#modal #short-desc").val());
                formData.append('Description', tinymce.activeEditor.getContent());

                $.ajax({
                    url: "/Admin/AddNews/",
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
                            $("#update-news").modal("hide");
                            News.newsDataTable.ajax.reload();
                            toastr.success(data.message, "Success!");
                        } else {
                            toastr.error(data.message, "Error!");
                        }
                    },
                    complete: function (data) {
                        $(_this).attr("disabled", false);
                        $(_this).html($(_this).text());
                    }
                });
                return false;
            }
            return false;
        });
    },
    loadLightBox: function () {
        $(document).on('click', '[data-toggle="lightbox"]', function (event) {
            event.preventDefault();
            $(this).ekkoLightbox();
        });
    },
    loadNewsDatatable: function () {
        return $('#dt-news').DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "ajax": {
                url: "/Admin/SearchNews",
                type: 'POST',
            },
            "language": DTLang.getLang(),
            "columns": [
                {
                    "data": "Title",
                    "render": function (data, type, full, meta) {
                        return full.title;
                    }
                },
                {
                    "data": "ShortDescription",
                    "render": function (data, type, full, meta) {
                        return full.shortDescription;
                    }
                },
                {
                    "data": "Image",
                    "render": function (data, type, full, meta) {
                        return "<a data-toggle='lightbox' data-gallery='document-" + full.id + "'  href='/images/news/" + full.image + "' ><img src=/images/news/" + full.image + " class='img-thumbnail img-fluid border-0' alt='document-" + full.image + "'> </a>";
                    }
                },
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        return "<a style='line-height:12px;' data-id='" + full.id + "' class='btn btn-sm btn-outline-secondary btn-edit'><i class='text-white la la-pencil'></i></a> <button style='line-height:12px;' data-id='" + full.id + "' class='btn btn-sm btn-outline-danger btn-delete'><i class='la la-trash'></i></button>";
                    },
                    "orderable": false
                }
            ],
        });
    },
    bindDeleteButton: function () {
        $("#dt-news").on("click", ".btn-delete", function () {
            var result = confirm("Do you really want to delete this news?");
            if (result) {
                var _this = this;
                $.ajax({
                    url: "/Admin/DeleteNews",
                    type: "POST",
                    beforeSend: function () {
                        $(_this).attr("disabled", true);
                    },
                    data: {
                        Id: $(_this).data().id
                    },
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message, 'Success!');
                            News.newsDataTable.ajax.reload();
                        } else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function (data) {
                        $(".btn-delete").attr("disabled", false);
                        $(".btn-delete").html($(".btn-delete").text());
                    }
                });
                return false;
            }
        });
    },
    initTinyMCE: function () {
        tinymce.init({
            selector: '.tinymce',
            height: 300,
            plugins: 'print preview fullpage powerpaste searchreplace autolink directionality advcode visualblocks visualchars fullscreen image link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists textcolor wordcount tinymcespellchecker imagetools  contextmenu colorpicker textpattern help',
            toolbar1: 'formatselect | bold italic strikethrough forecolor backcolor | link | alignleft aligncenter alignright alignjustify  | numlist bullist outdent indent  | removeformat',
            image_advtab: true,
            content_css: [
                '//fonts.googleapis.com/css?family=Lato:300,300i,400,400i',
                '//www.tinymce.com/css/codepen.min.css'
            ]
        });
    }
}

$(document).ready(function () {
    News.init();
});