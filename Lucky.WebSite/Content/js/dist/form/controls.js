/* 表单控件 */
define("dist/form/controls", ["$", "dist/application/app"], function(require, exports) {
    "use strict";
    var $ = require("$"), app = require("dist/application/app"), config = app.config, method = app.method;
    exports.init = function(form) {
        function controls() {
            var editor = KindEditor.editor({allowFileManager: !1,extraFileUploadParams: window.extraFileUploadParams});
            $('#image_11').on("click", function (b) {
                var target = $(b.target), textInput = target.prevAll("input[type=text]"), inputGroup = target.closest("div.input-group").find("input[type=text]"), insert = target.data("insert"), dataInput = target.data("input"), inputValue = textInput.val();
                dataInput && (inputValue = inputGroup.val()), editor.loadPlugin("image", function() {
                    editor.plugin.imageDialog({imageUrl: inputValue,clickFn: function(url) {
                        if (insert || void 0 == insert) {
                            textInput.val(url).hide();
                            var prevAllImg = target.prevAll("img"), dataTarget = $(target.data("target"));
                            if (dataTarget.length > 0)
                                dataTarget.attr("src", url).removeClass("hide");
                            else if (prevAllImg.length > 0)
                                prevAllImg.attr("src", url).removeClass("hide");
                            else {
                                var imgHtml = '<img class="thumb_img" src="{0}" style="max-height: 100px;">';
                                textInput.before(imgHtml.format(url))
                            }
                        } else
                            target.trigger("insert", url);
                        dataInput && inputGroup.val(url), target.trigger("callback", url), editor.hideDialog()
                    }})
                })
            })
        }
        
        var formEditor = $('[data-toggle="kindeditor"]', form), formImg = $('[data-toggle="selectimg"]', form);
        formEditor.length > 0 ? require.async("kindeditor", function() {
            formEditor.each(function() {
                var that = $(this), thatConfig = that.data("config"), thatTongji = that.data("tongji"), currentConfig = thatConfig ? config.kindeditor[thatConfig] : config.kindeditor["default"];
                if (thatTongji) {
                    var tongjiTarget = that.data("tongjiTarget"), ruleRangelength = that.data("ruleRangelength")[1];
                    if (tongjiTarget) {
                        var $tongjiTarget = $(tongjiTarget), $tongjiTargetHtml = $tongjiTarget.html();
                        currentConfig = $.extend({}, currentConfig, {
                            afterChange: function() {
                                var count = "remain" == thatTongji ? ruleRangelength - this.count() : this.count();
                                0 > count || count > ruleRangelength ? $tongjiTarget.addClass("error") : $tongjiTarget.removeClass("error"), $tongjiTarget.html($tongjiTargetHtml.format(count))
                            }
                        })
                    }
                }
                KindEditor.create(that, {
                    height: "600px",
                    width: "700px",
                    allowFileManager: true, //是否可以浏览上传文件
                    allowUpload: true, //是否可以上传
                    fileManagerJson: '/SiteManager/Home/ProcessRequest', //浏览文件方法
                    uploadJson: '/SiteManager/Home/UploadImage' //上传文件方法  //注意这两个路径
                });
                //KindEditor.ready(function (K) {
                //    var editor = K.editor({

                //        allowFileManager: true,
                //        allowUpload: true, //是否可以上传
                //        fileManagerJson: '/SiteManager/Home/ProcessRequest', //浏览文件方法
                //        uploadJson: '/SiteManager/Home/UploadImage' //上传文件方法  //注意这两个路径
                //    });

                //    K('#image_11').click(function () {
                //        editor.loadPlugin('image', function () {
                //            editor.plugin.imageDialog({
                //                imageUrl: K('#url_11').val(),
                //                clickFn: function (url, title, width, height, border, align) {
                //                    K('#url_11').val(url);
                //                    editor.hideDialog();
                //                }
                //            });
                //        });
                //    });

                //});

            }), controls();
        }) : $('#image_11').length > 0 && require.async(["kindeditor", "Content/js/kindeditor/themes/default/default.css"], function () {
            controls();
        });
        var method = $('[data-toggle="method"]');
        if (method.length > 0) {
            var element = $("#method"), confirm = method.data("confirm");
            method.on("click", function() {
                var that = $(this), dataMethod = that.data("method"), methodFun = function() {
                    element.val(dataMethod), that.button("loading"), method.closest("form").submit()
                };
                confirm && config.msg.msgbox.confirm(method.data("msg") || config.lang.confirmCloseOrder, function(result) {
                    result && methodFun()
                }), !confirm && methodFun()
            })
        }
        var location = $('[data-toggle="location"]', form);
        location.length > 0 && require.async("dist/location/init.js", function(loc) {
            $.each(location, function() {
                var that = $(this), provinces = $('[data-location="provinces"]', that), city = $('[data-location="city"]', that), district = $('[data-location="district"]', that), locationSelect = new loc.select({data: loc.data});
                locationSelect.bind(provinces), locationSelect.bind(city), locationSelect.bind(district)
            })
        });
        var selectLevel2 = $('[data-toggle="select_level2"]', form);
        if (selectLevel2.length > 0) {
            var level1 = $('[data-location="select_level_1"]', form), level2 = $('[data-location="select_level_2"]', form), level2Select = new method.select({data: window.select_level2_data});
            level2Select.bind(level1), level2Select.bind(level2)
        }
        var jSortable = $(".js_sortable");
        jSortable.length && function() {
            jSortable.sortable()
        }()
    }
});