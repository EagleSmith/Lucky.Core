/* 验证提示 */
define("dist/form/tip", ["$", "dist/application/app"], function(require, exports, module) {
    "use strict";
    var $ = require("$"), app = require("dist/application/app"), config = app.config;
    return exports.run = function(data) {
        switch (data.status) {
            case 0:
                data.message && data.message.length ? config.msg.suc(data.message, data.url) : data.url && data.url.length && (window.parent.mainFrame ? window.parent.document.getElementById("mainFrame").src = data.url : window.location = data.url);
                break;
            default:
                config.msg.info(data.message || config.lang.exception, data.url)
        }
        data.callback && eval(data.callback)
    }, exports
});
