/* 快捷菜单 */
define("dist/frameset/menu", ["$", "dist/application/template"], function(require) {
    "use strict";
    var $ = require("$");
    window.template = require("dist/application/template");
    var ajax = function(url, success, error, type, data) {
            $.ajax(url, { type: type || "post", data: data, dataType: "json" }).done(function(data) {
                success && success(data);
            }).fail(function() {
                window.msg.info(error || "网络异常 请重试");
            });
        },
        method = {postd: function(url, success, data) { ajax(url, success, !1, !1, data) }},
        Menu = function() {
            //载入快捷菜单
            function loadShortMenu(config) {
                var loadUrl = void 0 == config || void 0 == config.loadUrl ? "/Index/IndexMenu" : config.loadUrl, e = [], f = {}, g = [], h = [];
                $("#shortcutHrefList a").each(function() {
                    var url = $(this).data("url"), title = $(this).attr("title"), id = $(this).attr("id");
                    e.push({ url: url, id: id, title: title })
                }), f.slist = e, $("#shortmenuShow").append(template("shortMenuShowTem", f)), method.postd(loadUrl, function(a) {
                    $.each(a, function(a, b) {
                        g.push({ id: b.id, name: b.name }), h.push(b.list)
                    }), $.each(e, function(a, c) {
                        var d = c.id;
                        $.each(h, function(a, c) {
                            $.each(c, function(a, c) {
                                $.each(c.tlist, function(a, b) {
                                    d == b.id && (b.flag = !0);
                                });
                            });
                        });
                    }), f.firstmenu = g, f.secondmenu = h, $("#shortcutMenuFirst").append(template("shortcutMenuFirstTem", f)), $("#shortcutMenuList").append(template("shortcutMenuSecondTem", f))
                }, {});
            }
            //添加快捷菜单事件
            function initShortMenu() {
                $("#shortcutMenuBtn").click(function() {
                    $("#showShortcutMenuModal").modal("show");
                });
            }
            //添加删除快捷菜单
            function menuAction(config) {
                var addUrl = void 0 == config || void 0 == config.addUrl ? "/plus/formajax.php" : config.addUrl, delUrl = void 0 == config || void 0 == config.delUrl ? "/Index/DelShortcutMenu" : config.delUrl;
                $("#shortcutMenuList a").live("click", function() {
                    var a = $(this), title = a.attr("data-title"), id = a.attr("data-id"), url = a.attr("data-url"), shortmenuShow = $("#shortmenuShow"), shortcutHrefList = $("#shortcutHrefList"), shortMenu = {title: title,url: url,id: id};
                    if (a.hasClass("show-disabled"))
                        shortmenuShow.find("a[data-id=" + id + "]").parent().remove(), shortcutHrefList.find("#" + id).parent().remove(), a.find("i").attr("class", "fa fa-plus m-r-xs"), a.attr("title", "点击添加快捷菜单"), a.removeClass("show-disabled"), method.postd(delUrl, function() {
                        }, {id: id,url: url,title: title});
                    else {
                        shortmenuShow.append(template("addTem", shortMenu)), shortcutHrefList.append(template("addUrlTem", shortMenu)), a.find("i").attr("class", "fa fa-check m-r-xs"), a.attr("title", "已添加，点击取消"), a.addClass("show-disabled"), method.postd(addUrl, function() {
                        }, {id: id,url: url,title: title})
                    }
                });
                $("#shortmenuShow a i").live("click", function() {
                    var parent = $(this).parent(), id = parent.attr("data-id"), title = parent.attr("data-title"), url = parent.attr("data-url"), shortcutMenuList = $("#shortcutMenuList"), shortcutHrefList = $("#shortcutHrefList");
                    parent.parent().remove(), shortcutMenuList.find("a[data-id=" + id + "]").attr({title: "点击添加快捷菜单"}).removeClass("show-disabled"), shortcutMenuList.find("a[data-id=" + id + "]").find("i").attr("class", "fa fa-plus m-r-xs"), shortcutHrefList.find("#" + id).parent().remove(), method.postd(delUrl, function() {
                    }, {id: id,url: url,title: title})
                })
            }
            return {init: function(config) {
                loadShortMenu(config), initShortMenu(), menuAction(config)
            }}
        }();
    return Menu;
});
