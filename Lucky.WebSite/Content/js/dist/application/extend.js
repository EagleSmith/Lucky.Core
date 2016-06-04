/* js扩展方法 */
define("dist/application/extend", [], function() {
    "use strict";
    //格式化
    String.prototype.format || (String.prototype.format = function() {
        var arg = arguments;
        return this.replace(/{(\d+)}/g, function(input, index) {
            return "undefined" != typeof arg[index] ? arg[index] : input
        })
    });
    //去除前后空格
    String.prototype.trim || (String.prototype.trim = function() {
        return this.replace(/^\s*/, "").replace(/\s*$/, "")
    });
    //获取查询参数
    String.prototype.parameters || (String.prototype.parameters = function() {
        for (var params = {}, reg = new RegExp("([\\?|&])(.+?)=([^&?]*)", "ig"), result = reg.exec(this); result; )
            params[result[2]] = result[3], result = reg.exec(this);
        return params
    });
    //清除脚本
    String.prototype.stripTags || (String.prototype.stripTags = function() {
        return this.replace(/<\/?[^>]+>/gi, "")
    });
    //清除数字
    String.prototype.getNum || (String.prototype.getNum = function() {
        return this.replace(/[^d]/g, "")
    });
    //清除英文
    String.prototype.getEn || (String.prototype.getEn = function() {
        return this.replace(/[^A-Za-z]/g, "")
    });
    //清除中文
    String.prototype.getCn || (String.prototype.getCn = function() {
        return this.replace(/[^u4e00-u9fa5uf900-ufa2d]/g, "")
    });
    //格式化日期
    Date.prototype.format || (Date.prototype.format = function(datetime) {
        var dateObj = {"M+": this.getMonth() + 1,"d+": this.getDate(),"h+": this.getHours(),"m+": this.getMinutes(),"s+": this.getSeconds(),"q+": Math.floor((this.getMonth() + 3) / 3),"S+": this.getMilliseconds()};
        /(y+)/i.test(datetime) && (datetime = datetime.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length)));
        for (var item in dateObj)
            new RegExp("(" + item + ")").test(datetime) && (datetime = datetime.replace(RegExp.$1, 1 == RegExp.$1.length ? dateObj[item] : ("00" + dateObj[item]).substr(("" + dateObj[item]).length)));
        return datetime
    });
    //StringBuilder
    window.StringBuilder = function() {
        this.tmp = []
    };
    StringBuilder.prototype.Append = function(a) {
        return this.tmp.push(a), this
    };
    StringBuilder.prototype.Clear = function() {
        this.tmp.length = 1
    };
    StringBuilder.prototype.toString = function() {
        return this.tmp.join("")
    };
});