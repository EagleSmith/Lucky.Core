/* 导航菜单 */
define("dist/frameset/nav", ["$"], function(require) {
    "use strict";
    var $ = require("$"), dom = $(document);
    dom.on("click", ".nav-primary a", function(e) {
        var currentNode, clickNode = $(e.target);
        clickNode.is("a") || (clickNode = clickNode.closest("a")), $(".nav-vertical").length || (currentNode = clickNode.parent().siblings(".active"), currentNode && currentNode.find("> a").toggleClass("active") && currentNode.toggleClass("active").find("> ul:visible").slideUp(200), clickNode.hasClass("active") && clickNode.next().slideUp(200) || clickNode.next().slideDown(200), clickNode.toggleClass("active").parent().toggleClass("active"), clickNode.next().is("ul") && e.preventDefault())
    })
});
