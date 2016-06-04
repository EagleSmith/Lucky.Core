/* 基本元素事件 */
define("dist/application/element", ["$", "dist/application/setting", "dist/tips/init", "dist/application/method"], function(require, exports, module) {
    "use strict";
    var $ = require("$"),
        Setting = require("dist/application/setting"),
        Method = require("dist/application/method");
    //触发事件
    $(document).on("click", '[data-toggle="trigger"]', function(e) {
        e && e.preventDefault();
        var target = $(this).data("target").split(":");
        target && $(target[0]).trigger(target[1])
    });
    //返回
    $(document).on("click", '[data-toggle="back"]', function(e) {
        e && e.preventDefault();
        var url = $(this).data("url");
        url ? window.location = url : window.history.go(-1)
    });
    //扩展关闭弹窗
    $.fn.extend({popoverClosable: function(itme) {
        var popover = {template: '<div class="popover"><div class="arrow"></div><div class="popover-header"><button type="button" class="close m-r-xs m-t-xs" data-dismiss="popover" aria-hidden="true">&times;</button><h3 class="popover-title"></h3></div><div class="popover-content"></div></div>'};
        itme = $.extend({}, popover, itme);
        var that = this;
        that.popover(itme), that.on("click", function(e) {
            e.preventDefault(), that.not(this).popover("hide")
        }),
            $(document).on("click", '[data-dismiss="popover"]', function() {
                that.popover("hide")
            })
    }});
    //默认关闭弹窗
    $('[data-toggle="popover"]').popoverClosable();
    //切换class
    $(document).on("click", '[data-toggle^="class"]', function(e) {
        "checkbox" != e.target.type && "radio" != e.target.type && e && e.preventDefault();
        var toggle, href, css, cssList, hrefList, target = $(cssList.target);
        !target.data("toggle") && (target = target.closest('[data-toggle^="class"]')), toggle = target.data().toggle, href = target.data("target") || target.attr("href"), toggle && (css = toggle.split(":")[1]) && (cssList = css.split(",")), href && (hrefList = href.split(",")), hrefList && hrefList.length && $.each(hrefList, function(index) {
            "#" != hrefList[index] && $(hrefList[index]).toggleClass(cssList[index])
        });
        target.toggleClass("active");
        var that = $(this), ajax = that.data("ajax"), fun = that.data("fun"), remote = that.data("remote") || that.attr("href"), success = null;
        if (fun)
            switch (fun) {
                case "shelves":
                    success = function(data) {
                        Setting.issucceed(data) ? Setting.msg.suc(data.message || Setting.lang.shelvesSuccess, data.url) : Setting.msg.error(data.message || Setting.lang.shelvesError, data.url)
                    }
            }
        ajax && Method.post(remote, success, null, null, {toggle: that.hasClass("active")})
    });
    //切换验证ajax模态框
    $(document).on("click", '[data-toggle="ajaxModal"]', function(e) {
        $("#ajaxModal").remove(), e.preventDefault();
        var that = $(this), remote = that.data("remote") || that.attr("href"), set = that.data("set"), ajaxModal = $('<div class="modal fade" id="ajaxModal"><div class="modal-body "></div></div>');
        $(document).append(ajaxModal), ajaxModal.modal(), $.ajax(remote, {type: "get",dataType: "html",data: set}).done(function(data) {
            ajaxModal.append2(data, function() {
                var validateAjaxModal = $("form.form-validate", ajaxModal);
                validateAjaxModal.length > 0 && require.async("dist/form/init", function(Form) {
                    $("button[type='submit']", ajaxModal).length && $("button[type='submit']", ajaxModal).removeAttr("disabled"), Form.init(validateAjaxModal)
                }), that.trigger("init", ajaxModal)
            })
        })
    });
    //选中下拉菜单的已选项
    $(document).on("click.dropdown-menu", ".dropdown-select > .js_custom_list", function(e) {
        return e.preventDefault(), !1
    });
    //选中下拉菜单的选项
    $(document).on("click.dropdown-menu", ".dropdown-select > li > a", function(a) {
        a.preventDefault();
        var input, menu, label, text, inputChecked, selectList, target = $(a.target), checked = !1;
        if (!target.is("a") && (target = target.closest("a")), menu = target.closest(".dropdown-menu"), label = menu.parent().find(".dropdown-label"), text = label.text(), input = target.find("input"), checked = input.is(":checked"), !(input.is(":disabled") || "radio" == input.attr("type") && checked)) {
            "radio" == input.attr("type") && menu.find("li").removeClass("active"), target.parent().removeClass("active"), !checked && target.parent().addClass("active"), input.prop("checked", !input.prop("checked")), inputChecked = menu.find("li > a > input:checked"), inputChecked.length ? (selectList = [], inputChecked.each(function() {
                var parentText = $(this).parent().text();
                parentText && selectList.push($.trim(parentText))
            }), selectList = selectList.length < 6 ? selectList.join(", ") : " 选中" + selectList.length + "项", label.html(selectList)) : label.html(label.data("placeholder")), input.trigger("change", [input.val()]);
            var change = menu.data("change");
            if (change)
                switch (change) {
                    case "submit":
                        menu.closest("form ").submit()
                }
        }
    });
    //移除表格的行
    $(document).on("click", 'table [data-toggle="removeRow"]', function() {
        var tr = $(this).closest("tr");
        tr.fadeOut(function() {
            tr.remove()
        })
    });
    //ajax提交表单
    $(document).on("click", '[data-toggle="ajaxPost"]', function(e) {
        e.preventDefault();
        var $this = $(this), $remote = $this.data("remote") || $this.attr("href"), $confirm = $this.data("confirm"), $msgType = $this.data("msgType"), $data = $this.data("set"), $remove = $this.data("remove");
        $confirm = "undefined" == typeof $confirm ? !0 : $confirm, $this.data("loadingText", '<i class="fa fa-spinner fa-spin"></i>');
        var done = function() {
                var org = $this.html();
                $this.button("loading"), $.post($remote, $data).done(function(data) {
                    if ($this.button("reset"), Setting.issucceed(data)) {
                        if ($remove) {
                            var $el = $this.closest($remove);
                            $el.fadeOut(function() {
                                $el.remove()
                            })
                        }
                        Setting.msg.suc(data.message || Setting.lang.removeSuccess, data.url), data.callback && eval(data.callback)
                    } else
                        Setting.msg.error(data.message || Setting.lang.removeError, data.url)
                }).fail(function() {
                    $this.button("reset"), Setting.msg.info(Setting.lang.exception)
                })
            },
            retrunMsg = function(a) {
                switch (a) {
                    case "refund":
                        return Setting.lang.confirmRefund;
                    case "refundGoods":
                        return Setting.lang.confirmRefundGoods
                }
            },
            confirmMsg = $msgType ? retrunMsg($msgType) : $this.data("msg");
        $confirm && Setting.msg.msgbox.confirm({message: confirmMsg || Setting.lang.confirmPost,buttons: {confirm: {label: $this.data("confirmText") || "确定"}},callback: function(fun) {
            fun && done()
        }}), !$confirm && done()
    });
    //移除dd项
    $(document).on("click", ".js_remove_dd_item", function() {
        var that = $(this), item = that.closest(".dd-item");
        item.fadeOut(function() {
            item.remove(), 0 == $(".dd-item").length && $("#noitems").removeClass("hide")
        })
    });
    //ajax移除ol li
    $(document).on("click", 'ol li [data-toggle="ajaxRemove"]', function(e) {
        e.preventDefault();
        var that = $(this), remote = that.data("remote") || that.attr("href"), confirm = that.data("confirm");
        confirm = "undefined" == typeof confirm ? !0 : confirm;
        var ajaxRemove = function() {
            var html = that.html();
            that.html('<i class="fa fa-spinner fa-spin"></i>'), $.post(remote).done(function(data) {
                if (Setting.issucceed(data)) {
                    Setting.msg.suc(data.message || Setting.lang.removeSuccess, data.url);
                    var firstLi = that.parents("li").first();
                    firstLi.find(".dd-handle").css({"background-color": "#dff0d8"}), firstLi.fadeOut(function() {
                        firstLi.remove(), 0 == $(".dd-item").length && $noClassify.removeClass("hide")
                    })
                } else
                    that.html(html), Setting.msg.error(data.message || Setting.lang.removeError, data.url)
            }).fail(function() {
                that.html(html), Setting.msg.info(Setting.lang.exception, d.url)
            })
        };
        confirm && Setting.msg.msgbox.confirm(that.data("msg") || Setting.lang.confirmRemove, function(result) {
            result && ajaxRemove()
        }), !confirm && ajaxRemove()
    })
});