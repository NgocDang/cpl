var Utils = {
    addOrdinalSuffix: function (num) {
        if (parseInt($("#LangId").val()) == 1) // English
        {
            if (num <= 0) return num.ToString();
            switch (num % 100) {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }
            switch (num % 10) {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }
        }
        else if (parseInt($("#LangId").val()) == 2)  // Japanese
        {
            switch (num) {
                case 1:
                    return "一番目";
                case 2:
                    return "二番目";
                case 3:
                    return "三番目";
                case 4:
                    return "四番目";
                case 5:
                    return "五番目";
                case 6:
                    return "六番目";
                case 7:
                    return "七番目";
                case 8:
                    return "八番目";
                case 9:
                    return "九番目";
                case 10:
                    return "十番目";
                case 11:
                    return "十一番目";
                case 12:
                    return "十二番目";
                case 13:
                    return "十三番目";
                case 14:
                    return "十四番目";
                case 15:
                    return "十五番目";
            }
        }
        else if (parseInt($("#LangId").val()) == 3) // Korean
        {
            switch (num) {
                case 1:
                    return "첫째";
                case 2:
                    return "둘째";
                case 3:
                    return "셋째";
                case 4:
                    return "넷째";
                case 5:
                    return "다섯째";
                case 6:
                    return "여섯째";
                case 7:
                    return "일곱째";
                case 8:
                    return "여덟째";
                case 9:
                    return "아홉째";
                case 10:
                    return "열째";
                case 11:
                    return "열한째";
                case 12:
                    return "열둘째";
                case 13:
                    return "열셋째";
                case 14:
                    return "열넷째";
                case 15:
                    return "열다섯째";
            }
        }
        else if (parseInt($("#LangId").val()) == 4) //  Chinese
        {
            switch (num) {
                case 1:
                    return "一";
                case 2:
                    return "二";
                case 3:
                    return "三";
                case 4:
                    return "四";
                case 5:
                    return "五";
                case 6:
                    return "六";
                case 7:
                    return "七";
                case 8:
                    return "八";
                case 9:
                    return "九";
                case 10:
                    return "十";
                case 11:
                    return "十一";
                case 12:
                    return "十二";
                case 13:
                    return "十三";
                case 14:
                    return "十四";
                case 15:
                    return "十五";
            }
        }
        else if (parseInt($("#LangId").val()) == 5) // Chinese
        {
            switch (num) {
                case 1:
                    return "一";
                case 2:
                    return "二";
                case 3:
                    return "三";
                case 4:
                    return "四";
                case 5:
                    return "五";
                case 6:
                    return "六";
                case 7:
                    return "七";
                case 8:
                    return "八";
                case 9:
                    return "九";
                case 10:
                    return "十";
                case 11:
                    return "十一";
                case 12:
                    return "十二";
                case 13:
                    return "十三";
                case 14:
                    return "十四";
                case 15:
                    return "十五";
            }
        }
    }
};