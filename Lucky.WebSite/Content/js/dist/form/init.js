/* 表单模块 */
define("dist/form/init", ["$", "./controls", "dist/application/app", "./validate", "./tip"], function(require, exports) {
    "use strict";
    var controls = (require("$"), require("./controls")), validate = require("./validate");
    exports.init = function(form) {
        controls.init(form), validate.init(form);
    }
});