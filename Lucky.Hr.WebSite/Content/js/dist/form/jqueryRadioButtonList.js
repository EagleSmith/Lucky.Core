define(["$",  "dist/application/app" ], function(require, exports) {
    "use strict";
    return function(jQuery) {
        (function($) {
            $.fn.RadioButtonList = function() {
                var multiRadio = function(parentObj) {
                    parentObj.addClass("multi-radio"); //添加样式
                    parentObj.children().hide(); //隐藏内容
                    var divObj = $('<div class="boxwrap"></div>').prependTo(parentObj); //前插入一个DIV
                    parentObj.find('input[type="radio"]').each(function() {
                        var indexNum = parentObj.find('input[type="radio"]').index(this); //当前索引
                        var newObj = $('<a href="javascript:;">' + parentObj.find('label').eq(indexNum).text() + '</a>').appendTo(divObj); //查找对应Label创建选项
                        if ($(this).prop("checked") == true) {
                            newObj.addClass("selected"); //默认选中
                        }
                        //检查控件是否启用
                        if ($(this).prop("disabled") == true) {
                            newObj.css("cursor", "default");
                            return;
                        }
                        //绑定事件
                        $(newObj).click(function() {
                            $(this).siblings().removeClass("selected");
                            $(this).addClass("selected");
                            parentObj.find('input[type="radio"]').prop("checked", false);
                            parentObj.find('input[type="radio"]').eq(indexNum).prop("checked", true);
                            parentObj.find('input[type="radio"]').eq(indexNum).trigger("click"); //触发对应的radio的click事件
                            //alert(parentObj.find('input[type="radio"]').eq(indexNum).prop("checked"));
                        });
                    });
                };
                return $(this).each(function() {
                    multiRadio($(this));
                });
            }
        })(jQuery);
    }
});