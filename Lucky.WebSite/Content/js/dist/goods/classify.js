/* 自定义分类 */
define("dist/goods/classify", ["$", "dist/application/app"], function(require) {
    "use strict";
    var $ = require("$"), App = require("dist/application/app"), Config = App.config, dragClassify = $("#dragClassify"), noClassify = ($("#noClassify"), $("#save_class")), nestableMenu = $("#nestableMenu");
    dragClassify.length > 0 && require.async("nestable", function() {
        dragClassify.nestable({maxDepth: 2}), $(".dd-handle a").on("mousedown", function(e) {
            e.stopPropagation()
        })
    });
    noClassify.on("click", function() {
        var that = $(this), url = that.data("remote") || that.attr("href");
        that.button("loading");
        var data = window.JSON.stringify(dragClassify.nestable("serialize"));
        $.post(url, {data: data}).done(function(data) {
            that.button("reset");
            Config.issucceed(data) ? Config.msg.suc(data.message || Config.lang.saveSuccess, data.url) : Config.msg.error(data.message || Config.lang.saveError, data.url)
        }).fail(function() {
            that.button("reset");
            Config.msg.info(Config.lang.exception, d.url)
        })
    });
    nestableMenu.on("click", function() {
        var active = $(this).hasClass("active");
        dragClassify.nestable(active ? "expandAll" : "collapseAll")
    })
});