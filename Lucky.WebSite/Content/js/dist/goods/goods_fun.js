/* 自定义分类集合 */
define("dist/goods/goods_fun", ["$", "dist/application/app"], function(require) {
    "use strict";
    {
        var $ = require("$");
        require("dist/application/app")
    }
    //更新商品标签
    window.updateLabel = function(item) {
        var label = $(".js_goods_label");
        label.append("<option value=" + item.key + " selected>" + item.value + "</option>"), label.trigger("liszt:updated"), label.chosen()
    };
    var group = $(".js_goods_group");
    group.find("optgroup").each(function() {
        $(this).find("option").length < 1 && $(this).append("<option style='display:none' value='' data-placeholder='true'>暂无数据</option>")
    });
    group.trigger("liszt:updated");
    //更新商品分组
    window.updateGroup = function(item) {
        var goodsGroup = $(".js_goods_group");
        "0" != item.groupId ? goodsGroup.find("#" + item.groupId).append("<option value=" + item.key + " selected>" + item.value + "</option>") : goodsGroup.append("<optgroup id=" + item.key + " label=" + item.value + "><option value='' data-placeholder='true'>暂无数据</option></optgroup>");
        goodsGroup.find("optgroup").each(function() {
            $(this).find("option").length > 1 && $(this).find("option[data-placeholder]").length > 0 && $(this).find("option[data-placeholder]").remove()
        });
        goodsGroup.trigger("liszt:updated"), $(".chzn-results .group-option").each(function() {
            "暂无数据" == $(this).text() && $(this).hide()
        })
    };
    //更新地址
    window.updateAddress = function(item) {
        var edit = $(".js_address_edit");
        edit.append("<option value=" + item.key + " selected>" + item.value + "</option>")
    }
});