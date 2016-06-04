/* 默认设置 */
define("dist/application/setting", ["$", "dist/tips/init"], function(require) {
    "use strict";
    var Tips = (require("$"), require("dist/tips/init")),
        Setting = {
            kindeditor: {
                default: {items: ["source", "|", "undo", "redo", "|", "preview", "print", "template", "cut", "copy", "paste", "plainpaste", "wordpaste", "|", "justifyleft", "justifycenter", "justifyright", "justifyfull", "insertorderedlist", "insertunorderedlist", "indent", "outdent", "subscript", "superscript", "clearhtml", "quickformat", "selectall", "|", "fullscreen", "/", "formatblock", "fontname", "fontsize", "|", "forecolor", "hilitecolor", "bold", "italic", "underline", "strikethrough", "lineheight", "removeformat", "|", "image", "multiimage", "insertfile", "table", "hr", "emoticons", "baidumap", "code", "pagebreak", "link", "unlink"],height: "400px",width: "100%", afterCreate: function() { this.sync() }, afterBlur: function() { this.sync() },extraFileUploadParams: window.extraFileUploadParams},
                simple: {items: ["source", "undo", "redo", "plainpaste", "plainpaste", "wordpaste", "clearhtml", "quickformat", "selectall", "fullscreen", "fontname", "fontsize", "|", "forecolor", "hilitecolor", "bold", "italic", "underline", "hr", "removeformat", "|", "justifyleft", "justifycenter", "justifyright", "insertorderedlist", "insertunorderedlist", "|", "emoticons", "image", "link", "unlink", "preview"],height: "300px",width: "100%",afterCreate: function() {this.sync() },afterBlur: function() { this.sync() }},
                mini: {items: ["fontsize", "forecolor", "hilitecolor", "bold", "italic", "removeformat", "justifyleft", "justifycenter", "justifyright", "insertorderedlist", "insertunorderedlist", "link", "unlink"],height: "300px",width: "100%",afterCreate: function() {this.sync()},afterBlur: function() {this.sync()}}},
            loading: '<div class="load"></div>',
            issucceed: function(data) { return 0 == data.status },
            empty: "",
            lang: {
                saveSuccess: "保存成功",
                modifySuccess: "修改成功",
                saveError: "保存失败",
                modifyError: "修改失败",
                exception: "网络异常 请重试",
                confirmRemove: "确定删除 ?",
                confirmAllremove: "确定删除选定 ?",
                removeSuccess: "删除成功",
                removeError: "删除失败",
                attributeError: "获取商品属性出错,请重试",
                specificationsError: "获取商品规格出错,请重试",
                activitylabelError: "获取活动标签出错,请重试",
                pricelabelError: "获取价格出错,请重试",
                confirmPost: "确定提交",
                confirmChangedelivery: "切换计价方式后,所设置当前模版的运费信息将被清空,确认继续",
                confirmChangetemplet: "你要载入新模版吗？当前数据将被清空",
                shelvesSuccess: "上架成功",
                shelvesError: "上架失败 请重试",
                confirmRefund: "<p><b>确认退款</b></p><p>确认后钱将直接通过原支付方式返回，无法撤销，请再次确认。</p>",
                confirmRefundGoods: "<p><b>同意退款退货</b></p><p>退货地址：加载中...</p>",
                confirmCloseOrder: "确定关闭订单,关闭订单后将无法改价或发货",
                widgetError: "部件添加失败,请重试",
                pageInfoError: "部件添加失败,请重试",
                templateConfigLoadError: "模版配置加载失败,请重试",
                maxNavLength: "最多添加5个导航",
                maxFnLength: "最多添加4个导航",
                minOneItem: "至少保留一个",
                maxItem10: "最多添加10个"},
            statistics: {},
            domain: {www: window.location.protocol + "//" + window.location.host,"static": window.location.protocol + "//" + window.location.host}
        };
    return window.wm = Setting,
        Setting.msg = window.top != window.self ? window.parent.msg : Tips,
        Setting
});