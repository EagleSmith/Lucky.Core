/* 表单验证 */
define("dist/form/validate", ["$", "dist/application/app", "dist/form/tip"], function(require, exports) {
    "use strict";
    var $ = require("$"), app = require("dist/application/app"), config = app.config;
    exports.init = function(form) {
        var hasModal = form.hasClass("form-modal"), modal = null;
        hasModal && (modal = form.parents(".modal"), modal.on("hidden", function() {
            form.resetForm()
        }));
        var validItem = {
            errorElement: "span",
            errorClass: "help-block error",
            errorPlacement: function(element, other) {
                var inputGroup = other.parents(".input-group");
                inputGroup.length > 0 ? inputGroup.after(element) : other.after(element)
            },
            highlight: function(element) {
                $(element).removeClass("error has-success").addClass("error")
            },
            success: function(element) {
                element.addClass("valid")
            },
            onkeyup: function(element) {
                $(element).valid()
            },
            onfocusout: function(element) {
                $(element).valid()
            },
            submitHandler: function(element) {
                var flag = !0;
                if ($('[data-toggle="kindeditor"]', element).length > 0 && "undefined" != typeof KindEditor && KindEditor.instances && $.each(KindEditor.instances, function() {
                    this.sync();
                    var thatElement = $(this.srcElement[0]), ruleRequired = thatElement.data("ruleRequired"), msgRequired = thatElement.data("msgRequired"), ruleRangelength = thatElement.data("ruleRangelength"), msgRangelength = thatElement.data("msgRangelength"), thatValue = $.trim(thatElement.val()).replace(/(&nbsp;)|\s|\u00a0/g, "");
                    if (ruleRequired && 0 == thatValue.length) {
                        return config.msg.error(msgRequired || "内容不能为空"), flag = !1, !1
                    }
                    if (ruleRangelength) {
                        var rrl0 = ruleRangelength[0], rrl1 = ruleRangelength[1], thatVal = thatElement.val();
                        if (thatVal.length < rrl0 || thatVal.length > rrl1)
                            return config.msg.error(msgRangelength || "内容不能小于{0}且大于{1}".format(rrl0, rrl1)), flag = !1, !1
                    }
                }), flag) {
                    var submitBtn = $("button[type='submit']", element);
                    submitBtn.button("loading"), $(element).ajaxSubmit({dataType: "json",success: function(data) {
                        submitBtn.button("reset"), modal && modal.modal("hide"), f.run(data)
                    },error: function(data) {
                        submitBtn.button("reset"), modal && modal.modal("hide"), config.msg.msgbox.alert(data.responseText)
                    }})
                }
            }
        }, ignore = form.data("ignore");
        ignore && (validItem.ignore = ignore);
        var errorContainer = $("div.errorContainer"), errorConfig = {errorContainer: $("div.errorContainer"),errorLabelContainer: $("div.errorLabelContainer"),errorElement: "label"};
        errorContainer.length > 0 && $.extend(validItem, errorConfig), require.async(["validate", "jform"], function() {
            form.each(function() {
                $(this).validate(validItem)
            })
        })
    };
    var f = require("dist/form/tip")
});