var DTLang = {
    getLang: function () {
        if ($("#LangId").val() == 1) // English 
        {
            return {
                "sEmptyTable": "No data available in table",
                "sInfo": "Showing _START_ to _END_ of _TOTAL_ entries",
                "sInfoEmpty": "Showing 0 to 0 of 0 entries",
                "sInfoFiltered": "(filtered from _MAX_ total entries)",
                "sInfoPostFix": "",
                "sInfoThousands": ",",
                "sLengthMenu": "Show _MENU_ entries",
                "sLoadingRecords": "Loading...",
                "sProcessing": "",
                "sSearch": "Search:",
                "sZeroRecords": "No matching records found",
                "oPaginate": {
                    "sFirst": "First",
                    "sLast": "Last",
                    "sNext": "Next",
                    "sPrevious": "Previous"
                },
                "oAria": {
                    "sSortAscending": ": activate to sort column ascending",
                    "sSortDescending": ": activate to sort column descending"
                }
            }
        }
        else if ($("#LangId").val() == 2) { // Japanese
            return {
                "sEmptyTable": "テーブルにデータがありません",
                "sInfo": " _TOTAL_ 件中 _START_ から _END_ まで表示",
                "sInfoEmpty": " 0 件中 0 から 0 まで表示",
                "sInfoFiltered": "（全 _MAX_ 件より抽出）",
                "sInfoPostFix": "",
                "sInfoThousands": ",",
                "sLengthMenu": "_MENU_ 件表示",
                "sLoadingRecords": "読み込み中...",
                "sProcessing": "",
                "sSearch": "検索:",
                "sZeroRecords": "一致するレコードがありません",
                "oPaginate": {
                    "sFirst": "先頭",
                    "sLast": "最終",
                    "sNext": "次",
                    "sPrevious": "前"
                },
                "oAria": {
                    "sSortAscending": ": 列を昇順に並べ替えるにはアクティブにする",
                    "sSortDescending": ": 列を降順に並べ替えるにはアクティブにする"
                }
            }
        }
        else if ($("#LangId").val() == 3) { // Korean
            return {
                "sEmptyTable": "데이터가 없습니다",
                "sInfo": "_START_ - _END_ / _TOTAL_",
                "sInfoEmpty": "0 - 0 / 0",
                "sInfoFiltered": "(총 _MAX_ 개)",
                "sInfoPostFix": "",
                "sInfoThousands": ",",
                "sLengthMenu": "페이지당 줄수 _MENU_",
                "sLoadingRecords": "읽는중...",
                "sProcessing": "",
                "sSearch": "검색:",
                "sZeroRecords": "검색 결과가 없습니다",
                "oPaginate": {
                    "sFirst": "처음",
                    "sLast": "마지막",
                    "sNext": "다음",
                    "sPrevious": "이전"
                },
                "oAria": {
                    "sSortAscending": ": 오름차순 정렬",
                    "sSortDescending": ": 내림차순 정렬"
                }
            }
        }
        else if ($("#LangId").val() == 4) { // Chinese 
            return {
                "sProcessing": "",
                "sLengthMenu": "显示 _MENU_ 项结果",
                "sZeroRecords": "没有匹配结果",
                "sInfo": "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
                "sInfoEmpty": "显示第 0 至 0 项结果，共 0 项",
                "sInfoFiltered": "(由 _MAX_ 项结果过滤)",
                "sInfoPostFix": "",
                "sSearch": "搜索:",
                "sUrl": "",
                "sEmptyTable": "表中数据为空",
                "sLoadingRecords": "载入中...",
                "sInfoThousands": ",",
                "oPaginate": {
                    "sFirst": "首页",
                    "sPrevious": "上页",
                    "sNext": "下页",
                    "sLast": "末页"
                },
                "oAria": {
                    "sSortAscending": ": 以升序排列此列",
                    "sSortDescending": ": 以降序排列此列"
                }
            }
        }
        else if ($("#LangId").val() == 5) { // Chinese 
            return {
                "sProcessing": "",
                "sLengthMenu": "显示 _MENU_ 项结果",
                "sZeroRecords": "没有匹配结果",
                "sInfo": "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
                "sInfoEmpty": "显示第 0 至 0 项结果，共 0 项",
                "sInfoFiltered": "(由 _MAX_ 项结果过滤)",
                "sInfoPostFix": "",
                "sSearch": "搜索:",
                "sUrl": "",
                "sEmptyTable": "表中数据为空",
                "sLoadingRecords": "载入中...",
                "sInfoThousands": ",",
                "oPaginate": {
                    "sFirst": "首页",
                    "sPrevious": "上页",
                    "sNext": "下页",
                    "sLast": "末页"
                },
                "oAria": {
                    "sSortAscending": ": 以升序排列此列",
                    "sSortDescending": ": 以降序排列此列"
                }
            }
        }
    }
};
