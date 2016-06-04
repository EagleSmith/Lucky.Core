/* 主页面 */
define("dist/home/init", ["$", "dist/application/app", "sparkline", "dist/charts/init"], function(require) {
    "use strict";
    var $ = require("$"),
        App = require("dist/application/app"),
        Method = (App.config, App.method);
    require("sparkline");
    var Charts = require("dist/charts/init.js"),
        visitCharts = $("#visit_charts"),
        turnover = $(".js_turnover"),
        refund = $(".js_refund");
    //订单数
    visitCharts.length && function() {
        var path = visitCharts.data("path");
        path && Method.get(path, function(data) {
            Charts.line("visit_charts", data)
        })
    }();
    //收款数
    turnover.length && function() {
        var path = turnover.data("path");
        path && Method.get(path, function(data) {
            turnover.sparkline(data, {fillColor: "",height: 100,highlightLineColor: "#fff",lineColor: "#dddddd",lineWidth: 2,resize: !0,spotColor: "#bbbbbb",spotRadius: 3,type: "line",width: "100%"})
        })
    }();
    //退款数
    refund.length && function() {
        var path = refund.data("path");
        path && Method.get(path, function(data) {
            refund.sparkline(data, {type: "bar",height: 50,barWidth: 6,barSpacing: 6,barColor: "#65bd77"})
        })
    }();
});
