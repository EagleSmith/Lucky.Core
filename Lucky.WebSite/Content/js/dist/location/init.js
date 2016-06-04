/* 地区控件 */
define("dist/location/init", ["$", "dist/application/app", "./data"], function(require) {
    "use strict";
    var $ = require("$"), app = require("dist/application/app"), method = (app.config, app.method), data = require("./data"), select = new method.select({data: data}), address = $('[data-toggle="address"]');
    return $.each(address, function() {
        var that = $(this), location = that.data("location"), locationSelect = select.find(location);
        that.prepend(locationSelect.toString)
    }), $('[data-toggle="ajaxModal"]').bind("init", function(e, bindData) {
        var provinces = $('[data-location="provinces"]', bindData), city = $('[data-location="city"]', bindData), district = $('[data-location="district"]', bindData), bindSelect = new method.select({data: data});
        bindSelect.bind(provinces), bindSelect.bind(city), bindSelect.bind(district)
    }), {select: method.select,data: data}
});