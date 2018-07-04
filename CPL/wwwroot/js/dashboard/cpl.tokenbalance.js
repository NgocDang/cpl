var TokenBalance = {
    init: function () {
        TokenBalance.bindDecorateComponentView();
    },
    bindDecorateComponentView: function () {
        $("#txt-banlance").hide();
        $("#content-card").removeClass("pull-up");
    }
}

$(document).ready(function () {
    TokenBalance.init();
});