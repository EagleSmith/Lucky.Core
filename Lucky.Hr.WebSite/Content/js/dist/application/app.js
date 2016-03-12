/* 应用框架 */
define("dist/application/app", ["$","validate","datepicker", "unobtrusive","jform", "./setting", "dist/tips/init", "./method", "./extend", "./element", "./plugins", "./template"], function (require) {
    "use strict";
    var $ = require("$"), setting = require("./setting"), method = require("./method");
    require("validate")($);
    require("unobtrusive")($);
    require("jform");
    var dp = require("datepicker");
    //日期
    var datepicker = $('[data-toggle="datepicker"]');
    datepicker.length > 0 && datepicker.each(
        function () {
            var that = $(this);
            that.datepicker().on("changeDate", function (text) {
                var formatDate = text.date.format("yyyy-MM-dd");
                that.val(formatDate);

            });
        });
    var dd = $("dd");
    dd.length > 0 && dd.each(function () {
        var that = $(this);
            that.html(function(index, html) { return html+"&nbsp;"; });
    });
    //全选
    var checkall = $('[data-toggle="checkall"]');
    
    checkall.length > 0 && checkall.each(function() {
        var that = $(this);
        that.on("click", function() {
            if ($(".check-item").attr('checked'))
                $(".check-item").attr("checked", false);
            else
                $(".check-item").attr("checked", true);
        });
    });
    //批量删除
    var trashmore = $('[data-toggle="trashmore"]');
    trashmore.length > 0 && trashmore.each(function() {
        var that = $(this);
        var href = $(this).data("href");
        that.on("click", function() {
            setting.msg.msgbox.confirm("批量删除将不可撤销，确认删除？" || setting.lang.confirmAllremove, function(result) {
                if (result) {
                    var responsive = $("div.table-responsive");
                    var checked = $('table tbody tr td:first-child [type="checkbox"]:checked', responsive), checkedValue = checked.map(function () {
                        return $(this).attr("ID");
                    }).get().join(",");
                    if (checked.length > 0) {
                        $.ajax({
                            url: href,
                            type: "Get",
                            data: { ids: checkedValue },
                            dataType: "json",
                            success: function (res) {
                                window.location.href = res.Url;
                            }
                        });
                    }
                    
                }
            });
        
        });
    });
        
    
        return require("./extend"),
        require("./element"),
        require("./plugins"),
        $('[data-toggle="selectGoods"]').length > 0 && require.async("dist/selectgoods2/init", function(Selectgoods2) { Selectgoods2.init(document) }),
        window.template = require("./template"),
        $.ajaxSetup({cache: !1,dataType: "json"}),
        $("form.form-validate").length > 0 && require.async("dist/form/init", function(Form) {var validate = $("form.form-validate"); validate.length && Form.init(validate)}),
        $("table").length > 0 && require.async("dist/table/init"),require.async("dist/table/init"),
    {
        config: setting, method: method, v: "1.0.1"
        
    }
           
           
});

