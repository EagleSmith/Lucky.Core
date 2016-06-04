/* 后台搜索 */
define("dist/table/talbe-search", ["$", "dist/application/app"], function(a) {
    "use strict";
    var b = a("$"), c = a("dist/application/app"), d = c.config, e = b("form.talbe-search");
    if (e.length > 0) {
        var f = b("input[type='radio'][name='pageSize']"), g = b("#pageIndex"), h = b("#orderBy"), i = b("#order");
        f.bind("change", function () {
                e.submit();
            }),
        g.bind("pageIndex:change", function () {
           e.submit();
        }),
        i.bind("order:change", function () {
            e.submit();
        });
        var j = b("th.th-sortable");
        if (i.val().length > 0) {
            var k = b('[data-sort-name="' + i.val() + '"]'), l = "asc" == h.val();
            k.adddClass("select", l), k.append(true ? '<span class="th-sort"><i class="fa fa-sort-down text-active"></i><i class="fa fa-sort-up text"></i>  <i class="fa fa-sort"></i>  </span>' : '<span class="th-sort">  <i class="fa fa-sort-down text"></i>   <i class="fa fa-sort-up text-active"></i>  <i class="fa fa-sort"></i>  </span>');
        }
        j.on("click", function() {
                var a = b(this), c = a.data("sortName");
                i.val(c);
                var d = a.hasClass("select");
                h.val(d ? "desc" : "asc"), e.submit();
            }),
            b("button[type='submit']", e).bind("click", function() {
                var a = b('[data-rule-required="true"]');
                return a.length > 0 && 0 == b.trim(a.val()).length?(d.msg.info("请输入搜索关键词"), true) : void 0;
            });
    }
});