var AdminNews = {
    newsDataTable: null,
    init: function () {
        AdminNews.newsDataTable = AdminNews.loadNewsDataTable();
        AdminNews.loadLightBox();
        AdminNews.bindEdit();
        AdminNews.bindDoEdit();
        AdminNews.bindDoDelete();
        AdminNews.bindAdd();
        AdminNews.bindDoAdd();
    },
    bindEdit: function () {
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
                    $("#edit-news").modal("show");
                    $("#edit-news #btn-do-add").hide();
                    tinymce.remove();
                    AdminNews.initTinyMCE();
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                }
            });
            return false;
        });
    },
    bindAdd: function () {
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
                    $("#edit-news").modal("show");
                    $("#edit-news #btn-do-edit").hide();
                    tinymce.remove();
                    AdminNews.initTinyMCE();
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                }
            });
            return false;
        });
    },
    bindDoEdit: function () {
        $("#modal").on("click", "#btn-do-edit", function () {
            var _this = this;
            var isFormValid = $("#form-edit-news")[0].checkValidity();
            $("#form-edit-news").addClass('was-validated');

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
                    url: "/Admin/DoEditNews/",
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
                            $("#edit-news").modal("hide");
                            AdminNews.newsDataTable.ajax.reload();
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
    bindDoAdd: function () {
        $("#modal").on("click", "#btn-do-add", function () {
            var _this = this;
            var isFormValid = $("#form-edit-news")[0].checkValidity();
            $("#form-edit-news").addClass('was-validated');
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
                    url: "/Admin/DoAddNews/",
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
                            $("#edit-news").modal("hide");
                            AdminNews.newsDataTable.ajax.reload();
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
    loadNewsDataTable: function () {
        return $('#dt-news').DataTable({
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "searchDelay": 350,
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
                        if (full.image != null)
                            return "<a data-toggle='lightbox' data-gallery='document-" + full.id + "'  href='/images/news/" + full.image + "' ><img src=/images/news/" + full.image + " class='img-thumbnail img-fluid border-0' alt='document-" + full.image + "'> </a>";
                        else
                            return "";
                    }
                },
                {
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        return "<a data-id='" + full.id + "'  href='#'  target='_blank' class='btn btn-sm btn-outline-secondary btn-edit'>" + $("#edit").val() + "</a> <a data-id='" + full.id + "' class='btn btn-sm btn-outline-danger btn-do-delete'>" + $("#delete").val() +"</a>";
                    },
                    "orderable": false
                }
            ],
        });
    },
    bindDoDelete: function () {
        $("#dt-news").on("click", ".btn-do-delete", function () {
            var result = confirm("Do you really want to delete this news?");
            if (result) {
                var _this = this;
                $.ajax({
                    url: "/Admin/DoDeleteNews",
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
                            AdminNews.newsDataTable.ajax.reload();
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
    AdminNews.init();
});