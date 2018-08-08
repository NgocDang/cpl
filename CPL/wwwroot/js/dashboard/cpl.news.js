var News = {
    newsDataTable: null,
    init: function () {
        News.newsDataTable = News.loadNewsDatatable();
        News.loadLightBox();
        News.bindEditButton();
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
                },
                complete: function (data) {
                    $(_this).attr("disabled", false);
                }
            });
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
                    "data": "Action",
                    "render": function (data, type, full, meta) {
                        return "<a style='line-height:12px;' href='' data-id='" + full.id + "' class='btn btn-sm btn-info btn-edit'><i class='la la-pencil'></i></a> <button style='line-height:12px;' data-id='" + full.id + "' class='btn btn-sm btn-primary btn-delete'><i class='la la-trash'></i></button>";
                    },
                    "orderable": false
                }
            ],
        });
    },
}

$(document).ready(function () {
    News.init();
});