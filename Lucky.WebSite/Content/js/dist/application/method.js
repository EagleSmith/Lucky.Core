/* 应用程序方法 */
define("dist/application/method", ["$", "dist/application/setting", "dist/tips/init"], function(require) {
    "use strict";
    var $ = require("$"),
        Setting = require("dist/application/setting"),
        ajax = function(url, success, error, type, data) {
            $.ajax(url, {type: type || "post", data: data, dataType: "json"}).done(function(data) {
                success && success(data)
            }).fail(function() {
                Setting.msg.info(error || Setting.lang.exception)
            })
        };
    return {
        
        get: function(url, success, error) {
            ajax(url, success, error, "get")
        },
        post: function(url, success, error, type, data) {
            ajax(url, success, error, type, data)
        },
        postd: function(url, success, data) {
            ajax(url, success, !1, !1, data)
        },
        OpenWindowWithPost: function(action, target, data, method) {
            var form = document.createElement("form");
            form.setAttribute("method", method), form.setAttribute("action", action), form.setAttribute("target", target);
            for (var i in data) {
                var input = document.createElement("input");
                input.type = "hidden", input.name = data[i].name, input.value = data[i].value, form.appendChild(input)
            }
            document.body.appendChild(form), form.submit(), document.body.removeChild(form)
        },
        select: function(config) {
            function d(element, value) {
                var selected = $(element).data("selected");
                selected && (value = selected);
                var key = selectList.length ? selectList[selectList.length - 1].key + "," + selectList[selectList.length - 1].value : defaults.root;
                selectList.push({element: element,key: key,value: value});
                var l = 0;
                for (var m in i)
                    l++;
                for (var n in selectList)
                    if (selectList[n].element == element)
                        var o = parseInt(n);
                for (var n in selectList)
                    o > n && selectList[n].element.change(function() {
                        e(element)
                    });
                o > 0 && selectList[o - 1].element.change(function() {
                    e(element, selectList[o].key)
                });
                element.change(function() {
                    var c = selectList[o - 1] ? selectList[o].key + "," + $(this).val() : "0," + $(this).val();
                    "undefined" != typeof selectList[o + 1] && (selectList[o + 1].key = c), defaults.field_name && $(defaults.field_name).val($(this).val()), defaults.auto && "undefined" == typeof selectList[o + 1] && f(c, function(c, f) {
                        if (f) {
                            var g = $("<select></select>");
                            element.after(g), d(g, ""), e(selectList[o + 1].element, c, f)
                        }
                    })
                }), e(element, key, value)
            }
            function e(select, key, value) {
                var g, h, i, k, l, m, n;
                if (select.empty(),
                    select.append('<option value="">{0}</option>'.format(defaults.default_text)),
                    g = f(key, function() { e(select, key, value) }),
                    !g)
                    return defaults.auto && select.hide(), !1;
                select.show();
                var o = [];
                for (var p in g)
                    o.push(p);
                0 == o.length && select.hide(), h = 1, i = 0;
                for (k in g)
                    g.hasOwnProperty(g[k]) || (l = g[k], m = "", k == value && (i = h, m = 'selected="selected"'), n = $('<option value="' + k + '" ' + m + ">" + l + "</option>"), select.append(n), h++);
                select[0] && (setTimeout(function() {
                    select[0].options[i].selected = !0
                }, 0), select[0].selectedIndex = 0, select.attr("selectedIndex", i))
            }
            function f(key, c) {
                var d, e;
                if ("undefined" == typeof key || "," == key[key.length - 1])
                    return null;
                if ("undefined" == typeof i[key]) {
                    d = 0;
                    for (e in i) {
                        d++;
                        break
                    }
                    defaults.ajax ? $.getJSON(defaults.ajax, {key: key}, function(data) { i[key] = data; c(key, data); }) : defaults.file && 0 == d && $.getJSON(defaults.file, function(data) {i = data, c(key, data)})
                }
                return i[key]
            }
            function query(element) {
                return "string" == typeof element ? $(element) : element
            }
            var selectList = [], i = {}, defaults = {data: {},file: null,root: "0",ajax: null,timeout: 30,method: "post",field_name: null,auto: !1,default_text: "请选择"};
            return config && jQuery.extend(defaults, config),
                i = defaults.data,
            {bind: function(element, data) {
                "object" != typeof element && (element = query(element)), data = data ? data : "", d(element, data)},
                find: function(a) {
                    if (a.length > 0)
                        var b = i[0][a[0]], d = i["0,{0}".format(a[0])][a[1]], e = i["0,{0},{1}".format(a[0], a[1])][a[2]];
                    return {p: b,c: d,d: e,toString: function() {
                        return e = "undefined" == typeof e ? Setting.empty : e, b + d + e
                    }}
                }
            }
        }
    }
});