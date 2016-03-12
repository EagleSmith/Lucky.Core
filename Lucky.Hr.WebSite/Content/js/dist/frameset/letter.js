/* 系统短信 */
define("dist/frameset/letter", ["$", "dist/application/template"], function(require) {
    "use strict";
    //标题跑马灯
    function marquee(title) {
        title.push(title.shift()), document.title = title.join("")
    }
    //停止跑马灯
    function stop() {
        window.clearInterval(letterTimer)
    }
    var $ = require("$"),
        Letter = require("./lettertemplate"),
        Template = require("dist/application/template"),
        formartTime = function(datetime) {
            var date, now, seconds, nowObj, dateObj, week = {0: "周日",1: "周一",2: "周二",3: "周三",4: "周四"};
            return datetime ? (now = new Date, date = "object" == typeof datetime && datetime.constructor === Date ? datetime : new Date(datetime), seconds = (now - date) / 1e3, 60 > seconds ? "刚刚" : 3600 > seconds ? Math.floor(seconds / 60) + "分钟前" : 10800 > seconds ? Math.floor(seconds / 3600) + "小时前" : (dateObj = {year: date.getFullYear(),month: date.getMonth() + 1,date: date.getDate(),day: date.getDay(),hours: date.getHours() >= 10 ? date.getHours() : "0" + date.getHours().toString(),min: date.getMinutes() >= 10 ? date.getMinutes() : "0" + date.getMinutes().toString()}, nowObj = {year: now.getFullYear(),month: now.getMonth() + 1,date: now.getDate(),day: now.getDay()}, dateObj.date === nowObj.date && dateObj.month === nowObj.month && dateObj.year === nowObj.year ? "今天" + dateObj.hours + ":" + dateObj.min : nowObj.date - dateObj.date === 1 && 172800 > seconds ? "昨天" + dateObj.hours + ":" + dateObj.min : nowObj.day - dateObj.day > 0 && 604800 > seconds ? week[dateObj.day] + dateObj.hours + ":" + dateObj.min : dateObj.year === nowObj.year ? dateObj.month + "月" + dateObj.date + "日" : dateObj.year + "年" + dateObj.month + "月" + dateObj.date + "日")) : ""
        };
    Template.helper("transMsgTime", function(time) {
        return formartTime(1e3 * parseInt(time))
    });
    var letterTimer,
    //定时获取系统信息
        heartbeat = function() {
            $.ajax({type: "POST",url: window.letter_path,success: function(date) {
                var html = Template.compile(Letter)(date) + $("#js_msg_content_footer").html();
                $(".js_msg_content").html(html);
                var titleNode = $("title"), msgNode = $(".js_show_msg"), defaultTitle = msgNode.data("default"), msg = msgNode.data("msg");
                date.count ? (titleNode.html(msg + defaultTitle), $(".js_msg_count").fadeOut(0, function() {
                    $(".js_msg_count").html(date.count), $(".js_msg_count").fadeIn(0)
                })) : ($(".js_msg_count").fadeOut(0), stop(), titleNode.html(defaultTitle)), $(".js_msg_empty").toggle(date.items.length > 0)
            },dataType: "json"})
        },
    //获取系统信息
        getLitter = function() {
            $.ajax({type: "POST",url: window.letter_path,success: function(date) {
                var html = Template.compile(Letter)(date) + $("#js_msg_content_footer").html();
                $(".js_msg_content").html(html);
                var titleNode = $("title"), msgNode = $(".js_show_msg"), defaultTitle = msgNode.data("default"), msg = msgNode.data("msg");
                if (date.count) {
                    titleNode.html(msg + defaultTitle), $(".js_msg_count").fadeOut(1e3, function() {
                        $(".js_msg_count").html(date.count), $(".js_msg_count").fadeIn(1e3)
                    });
                    var m = (msg + defaultTitle).split("");
                    void 0 == letterTimer && (letterTimer = setInterval(function() {
                        marquee(m)
                    }, 200))
                } else
                    $(".js_msg_count").fadeOut(0), stop(), titleNode.html(defaultTitle);
                $(".js_msg_empty").toggle(date.items.length > 0)
            },dataType: "json"}), setTimeout(getLitter, 6e4)
        };
    $(document).on("click", ".js_msg_empty", function() {
        var that = $(this), url = that.data("path");
        url && $.ajax(url, {type: "post",dataType: "json"}).done(function(data) {
            0 == data.status && ($(".js_msg_count,.js_msg_hdead").hide(), $(".js_msg_content_items").html('<div class="wrapper">您目前没有新消息</div>'))
        }).fail(function() {
            window.msg.info("网络异常请重试")
        })
    });
    window.letter_path && (getLitter(), $(".js_show_msg").on("hide.bs.dropdown", function() {
        setTimeout(heartbeat, 5e3)
    }))
});
