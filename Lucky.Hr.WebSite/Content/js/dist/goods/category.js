/* 产品类别 */
define("dist/goods/category", ["$", "dist/application/app"], function(require) {
    "use strict";
    var $ = require("$"), App = require("dist/application/app"), Config = App.config, Method = App.method, itemwrp = $("div.category_item_wrp");
    if (window.category && (window.category.automatic = !0), itemwrp.length > 0) {
        var submit = $('button[type="submit"]'),
            categoryContainer = $('[data-toggle="category"]'),
            categorytemplate = require("./categorytemplate"),
            url = Config.empty;
        var category = function(element) {
            this.element = element, this.category_items = this.element.find("ul.category_items"), this.data = null;
            var that = this;
            $(".input_search", this.element).on("keyup propertychange", function() {
                that.search($(this).val().trim())
            })
        };
        category.prototype.init = function(data, fun) {
            if (this.data = data, this.childs(data, this), fun(), window.category.selected) {
                var selectItem = $('li[wid="{0}"] a'.format(window.category.selected[0]), this.element);
                if (selectItem.length > 0)
                    return selectItem.trigger("click", this), selectItem.closest("ul").scrollTop(selectItem.position().top - 100), !0
            }
        };
        category.prototype.search = function(a) {
            this.category_items.find("li").each(function() {
                var c = $(this);
                c.text().indexOf(a) > -1 ? c.show() : c.hide()
            })
        };
        category.prototype.childs = function(data, element) {
            if (data && data.length > 0) {
                var sb = new StringBuilder;
                if ($.each(data, function(index, item) {
                    sb.Append(categorytemplate.format(item.key, item.val))
                }), this.category_items.html(sb.toString()), element.show(), window.category.selected && window.category.automatic)
                    for (var i = 1; i < window.category.selected.length; i++) {
                        var selectItem = $('li[wid="{0}"] a'.format(window.category.selected[i]), this.element);
                        if (selectItem.length > 0)
                            return selectItem.trigger("click"), selectItem.closest("ul").scrollTop(selectItem.position().top - 100), !0
                    }
            } else
                submit.removeAttr("disabled"), url = submit.data("remote").format($(".category_item_wrp:visible .selected").last().find("a").data("id"))
        };
        category.prototype.hide = function() {
            this.element.hide()
        };
        category.prototype.show = function() {
            this.element.show()
        };
        category.prototype.text = function() {
            return $(".selected", this.category_items).text()
        };
        var setSelect = function() {
            var selectTree = [], selectList = $(".category_item_wrp:visible .selected");
            $.each(selectList, function() {
                selectTree.push($(this).text())
            }), $(".js_selected_text").html(selectTree.join(">"))
        }, categoryList = [];
        $.each(categoryContainer, function(index, item) {
            categoryList.push(new category($(item)))
        });
        var subCategory = categoryList.slice(1);
        categoryList[0].init(window.category.data, function() {
            $("li a", categoryList[0].category_items).on("click", function(e, s) {
                var that = $(this), id = that.data("id");
                that.parent().addClass("selected").siblings().removeClass("selected");
                for (var i = subCategory.length - 1; i >= 0; i--)
                    subCategory[i].hide();
                setSelect(), submit.attr("disabled", "disabled"), Method.get(window.category.url.format(id), function(data) {
                    categoryList[1].childs(data, categoryList[1])
                }), s || (window.category.automatic = !1)
            })
        });
        for (var i = 1; i < categoryList.length; i++) {
            categoryList[i].element.attr("index", i), $(categoryList[i].category_items).on("click", "li a", function () {
                var that = $(this), id = that.data("id"), itemwrp = that.parents("div.category_item_wrp"), itemNext = itemwrp.next(), nextIndex = itemNext.attr("index");
                that.parent().addClass("selected").siblings().removeClass("selected"), itemwrp.nextAll().hide(), setSelect(), submit.attr("disabled", "disabled"), categoryList[nextIndex] ? Method.get(window.category.url.format(id), function (data) {
                    categoryList[nextIndex].childs(data, itemNext)
                }) : (submit.removeAttr("disabled"), url = submit.data("remote").format($(".category_item_wrp:visible .selected").last().find("a").data("id")))
            });
        }
        submit.on("click", function() {
            url.length > 0 && Config.msg.redirect(url)
        })
    }
});