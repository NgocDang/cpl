(function (c, h, f) {
    c.fn.fireModal = function (a) {
        a = c.extend({
            size: "modal-sm",
            center: !1,
            animation: !0,
            title: "Modal Title",
            closeButton: !0,
            header: !0,
            body: "",
            buttons: [],
            created: function () { },
            appended: function () { },
            modal: {}
        }, a);
        this.each(function () {
            f++;
            var b = "fire-modal-" + f;
            c(this).off("click").on("click", function () {
                var d = '   <div class="modal modal-mini modal-primary ' + (1 == a.animation ? " fade" : "") + '" tabindex="-1" role="dialog" id="' + b + '">       <div class="modal-dialog ' + a.size + (a.center ? " modal-dialog-centered" : "") + '" role="document">         <div class="modal-content">  ' +
                    (1 == a.header ? '<div class="modal-header justify-content-center"><div class="modal-profile"><i class="now-ui-icons media-1_button-power"></i></div></div>' : "") + '         <div class="modal-body text-center">  ' + a.body + "         </div>  " + (0 < a.buttons.length ? '         <div class="modal-footer">           </div>  ' : "") + "       </div>       </div>    </div>  ";
                d = c(d).modal(a.modal);
                var e;
                a.buttons.forEach(function (a) {
                    e = '<button class="' + a["class"] + '">' + a.text + "</button>";
                    e = c(e).off("click").on("click", function () {
                        a.handler.call(this, d)
                    });
                    c(d).find(".modal-footer").append(e)
                });
                a.created.call(this, d, a);
                c("body").append(d);
                a.appended.call(this, b, a);
                return !1
            })
        })
    };
    c.destroyModal = function (a) {
        a.modal("hide");
        a.on("hidden.bs.modal", function () {
            a.remove()
        })
    };
    c.cardProgress = function (a, b) {
        b = c.extend({
            dismiss: !1,
            dismissText: "Cancel",
            onDismiss: function () { }
        }, b);
        var d =
            c(a);
        d.addClass("card-progress");
        if (1 == b.dismiss) {
            var e = '<a class="btn btn-danger card-progress-dismiss">' + b.dismissText + "</a>";
            e = c(e).off("click").on("click", function () {
                d.removeClass("card-progress");
                d.find(".card-progress-dismiss").remove();
                b.onDismiss.call(this, d)
            });
            d.append(e)
        }
    };
    c.chatCtrl = function (a, b) {
        b = c.extend({
            position: "chat-right",
            text: "",
            time: moment((new Date).toISOString()).format("hh:mm"),
            picture: "",
            type: "text",
            timeout: 0,
            onShow: function () { }
        }, b);
        var d = c(a);
        a = '<div class="chat-item ' + b.position +
            '" style="display:none"><img src="' + b.picture + '"><div class="chat-details"><div class="chat-text">' + b.text + '</div><div class="chat-time">' + b.time + "</div></div></div>";
        var e = '<div class="chat-item chat-left chat-typing" style="display:none"><img src="' + b.picture + '"><div class="chat-details"><div class="chat-text"></div></div></div>',
            g = a;
        "typing" == b.type && (g = e);
        0 < b.timeout ? setTimeout(function () {
            d.find(".chat-content").append(c(g).fadeIn())
        }, b.timeout) : d.find(".chat-content").append(c(g).fadeIn());
        var f =
            0;
        d.find(".chat-content .chat-item").each(function () {
            f += c(this).outerHeight()
        });
        d.find(".chat-content").scrollTop(f, -1);
        b.onShow.call(this, g)
    }
})(jQuery, this, 0);