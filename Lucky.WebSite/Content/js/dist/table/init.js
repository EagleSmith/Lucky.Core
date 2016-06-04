/* 后台表格 */
define("dist/table/init", ["$", "dist/application/app", "./talbe-search"], function(require) {
    "use strict";
    var $ = require("$"),
        App = require("dist/application/app"),
        Config = App.config;
    //require("./paginator");
    require("./talbe-search");
    //分页
    //var pagination = $("ul.pagination");
    //pagination.length > 0 && pagination.Paginator({totalPages: pagination.data("pagesTotal"),currentPage: pagination.data("pageCurrent"),page: function(currentPage) {
    //    return $("#pageIndex").val(currentPage).trigger("pageIndex:change"), true;
    //}});
    //刷新
    $(document).on("click", '[data-toggle^="refresh"]', function(e) {
        e && e.preventDefault(), window.location.reload()
    });
    var responsive = $("div.table-responsive"), checkbox = $('table tbody tr td:first-child [type="checkbox"]', responsive), notBatch = $('[data-toggle^="batch"]'), batch = $('[data-trigger="batch"]');
    $(document).on("click", '[data-toggle="batch"]', function (e) {
        var href = $(this).data("href");
        var that = $(this); 
        Config.msg.msgbox.confirm(that.data("msg") || Config.lang.confirmAllremove, function (result) {
            
            if (result) {
                $.ajax({
                    url: href,
                    type: "Get",
                    dataType: "json",
                    success: function (res) {
                        window.location.href = res.Url;
                    }
                });
            }
        });
        
    });
    
    if (checkbox.length > 0) {
        //所有选中
        var checkItem = function() {
            var checked = $('table tbody tr td:first-child [type="checkbox"]:checked', responsive), checkedValue = checked.map(function() {
                return $(this).val()
            }).get().join(",");
            return {ckbox_list: checked,values: checkedValue}
        };
        //批量删除
        notBatch.on("click", function() {
            var that = $(this), href = $(this).data("href"), html = that.html();
            
            Config.msg.msgbox.confirm(that.data("msg") || Config.lang.confirmAllremove, function(result) {
                if (result) {
                    that.attr("disabled", "disabled");
                    var checked = $('table tbody tr td:first-child [type="checkbox"]:checked', responsive), checkedValue = checked.map(function() {
                        return $(this).val()
                    }).get();
                    that.html('<i class="fa fa-spinner fa-spin"></i>'), $.post(href, {ids: checkedValue.join(",")}).done(function(data) {
                        if (Config.issucceed(data)) {
                            Config.msg.suc(data.message || Config.lang.removeSuccess, data.url);
                            var deferred = $.Deferred(), delTr = function(deferred) {
                                var i = 0;
                                return checked.parents("tr").fadeOut(function() {
                                    $(this).remove(), i++, checked.length == i && deferred.resolve()
                                }), deferred
                            };
                            $.when(delTr(deferred)).done(function() {
                                notBatch.addAttr("disabled", 0 == $('table tbody tr td:first-child [type="checkbox"]:checked', responsive).length), 0 == $("table tbody tr", responsive).length && window.location.reload()
                            })
                        } else
                            Config.msg.error(data.message || Config.lang.removeError, data.url);
                        that.html(html)
                    }).fail(function() {
                        that.html(html), Config.msg.info(Config.lang.exception)
                    })
                }
            })
        });
        batch.on("click", function() {
            var all = checkItem(); alert("HI");
            $(this).data("set", { ids: all.values });
        });
        //全选
        $(document).on("change", 'div.table-responsive table thead th:first [type="checkbox"]', function(e) {
            e && e.preventDefault();
            var table = $(e.target).closest("table"), checked = $(e.target).is(":checked");
            $('tbody tr td:first-child  [type="checkbox"]', table).prop("checked", checked), notBatch.add(batch).addAttr("disabled", !checked)
        });
        //选中行
        $(document).on("change", 'div.table-responsive table tbody td:first-child [type="checkbox"]', function(e) {
            e && e.preventDefault();
            var table = $(e.target).closest("table"), targetChecked = $(e.target).is(":checked"), checked = $('tbody tr td:first-child  [type="checkbox"]:checked', table);
            $('thead th:first [type="checkbox"]', table).prop("checked", targetChecked && checked.length == checkbox.length), notBatch.add(batch).addAttr("disabled", 0 == checked.length)
        })
    }
    //删除
    $(document).on("click", 'div.table-responsive table tbody [data-toggle="ajaxRemove"]', function(e) {
        e.preventDefault();
        var that = $(this), remote = that.data("remote") || that.attr("href"), confirm = that.data("confirm");
        confirm = "undefined" == typeof confirm ? !0 : confirm, that.data("loadingText", '<i class="fa fa-spinner fa-spin"></i>');
        var ajaxRemove = function() {
            that.html();
            that.button("loading"), $.post(remote).done(function(data) {
                if (Config.issucceed(data)) {
                    Config.msg.suc(data.message || Config.lang.removeSuccess, data.url);
                    var tr = that.parents("tr");
                    tr.css({"background-color": "#dff0d8"}).find("td").css({"background-color": "#dff0d8"}), tr.fadeOut(function() {
                        tr.remove(), 0 == $("table tbody tr").length && window.location.reload()
                    })
                } else
                    that.button("reset"), Config.msg.error(data.message || Config.lang.removeError, data.url)
            }).fail(function() {
                that.button("reset"), Config.msg.info(Config.lang.exception)
            })
        };
        confirm && Config.msg.msgbox.confirm(that.data("msg") || Config.lang.confirmRemove, function(result) {
            result && ajaxRemove()
        }), !confirm && ajaxRemove()
    })
});
