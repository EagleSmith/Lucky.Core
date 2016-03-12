/* 系统首页 */
define("dist/frameset/init", ["$", "slimscroll", "./nav", "./letter", "./lettertemplate", "dist/application/template", "./menu", "dist/tips/init"], function(require, exports) {
    "use strict";
    var $ = require("$");
    require("slimscroll"),
        require("./nav"),
        require("./letter"),
        exports.shortcutMenu = require("./menu"),
        window.msg = require("dist/tips/init"),
        navigator.userAgent.match(/iPad|iPod/i) && (window.onorientationchange = function() {
            90 != window.orientation && ($.cookie("onorientationchange") || ($.cookie("onorientationchange", www_version), alert("竖屏 可能导致部分功能无法使用 请切换到横屏模式")))
        });
    //导航菜单切换效果
    var dom = $(document);
    dom.on("click", '[data-toggle^="class"]', function(e) {
        e && e.preventDefault();
        var toggle, target, css, navxs, nav, pull = $(e.target);
        !pull.data("toggle") && (pull = pull.closest('[data-toggle^="class"]')), toggle = pull.data().toggle, target = pull.data("target") || pull.attr("href"), toggle && (css = toggle.split(":")[1]) && (navxs = css.split(",")), target && (nav = target.split(",")), nav && nav.length && $.each(nav, function(a) {
            "#" != nav[a] && $(nav[a]).toggleClass(navxs[a])
        }), pull.toggleClass("active")
    });
    $(function() {
        //隐藏载入中
        $("section.hidden-bsection").show(), $("#loading_done").add("div.pageload-overlay").removeClass("show").hide();
        var ieBox = $(".ie11 .vbox").add(".ie .vbox");
        ieBox.length > 0 && (ieBox.each(function() {
            $(this).height($(this).parent().height())
        }), $(window).resize(function() {
            var height = $(window).height();
            ieBox.each(function() {
                $(this).height(height - $(this).offset().top)
            })
        }));
        //微型滚动条
        $(".no-touch .slim-scroll").each(function() {
            var slimTimer, that = $(this), data = that.data();
            that.slimScroll(data), $(window).resize(function() {
                clearTimeout(slimTimer), slimTimer = setTimeout(function() {
                    that.slimScroll(data)
                }, 100)
            })
        });
    });
});

