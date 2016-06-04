/* 消息提示控件 */
define("dist/tips/init", ["$", "./bootbox", "./notify", "jquery"], function(require) {
    "use strict";
    function redirect(url) {
        window.mainFrame ? document.getElementById("mainFrame").src = url : window.location = url
    }
    function tips(option, msg, url, callback) {
        var box = null;
        if (msg.length > 17)
            return void Bootbox.alert(msg, function() {
                url && redirect(url)
            });
        var config = $.extend({}, {message: {text: msg},onClose: callback || url && function() {
            redirect(url)
        }}, option);
        box = msgbox.notify(config), box.show(), url && $(window.mainFrame.document || document).on("click", function(e) {
            e && e.preventDefault(), box.close()
        })
    }
    var $ = require("$"),
    Bootbox = require("./bootbox");
    window.msgbox = Bootbox,
    0 == $("div.msgbox").length && $("body").append('<div class="msgbox"></div>'),
    require("./notify");
    var msgbox = $("div.msgbox");
    return {
        redirect: function(url) {
            redirect(url)
        },
        msgbox: Bootbox,
        suc: function(msg, url, callback) {
            tips({fadeOut: {enabled: !0,delay: 1e3},type: "success"}, msg, url, callback)
        },
        error: function(msg, url, callback) {
            tips({type: "danger",closable: !0,fadeOut: {enabled: !0,delay: 3e3}}, msg, url, callback)
        },
        info: function(msg, url, callback) {
            tips({closable: !0,fadeOut: {enabled: !0,delay: 3e3},type: "info"}, msg, url, callback)
        }
    }
});
