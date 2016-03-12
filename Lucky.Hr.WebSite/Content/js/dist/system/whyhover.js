define(function (require, exports, module) {
    return function (jquery) {
        (function ($) {
            //给当前行高亮
            $.fn.wyhhover = function (options) {//options 经常用这个表示有许多个参数。 
                var defaultVal = {
                    BackColor: '#ccc',
                };

                var obj = $.extend(defaultVal, options);

                return this.each(function () {
                    var tabObject = $(this); //获取当前对象 
                    var oldBgColor = tabObject.css("background-color"); //获取当前对象的背景色 
                    tabObject.hover(//定义一个hover方法。
                    function () { tabObject.css("background-color", obj.BackColor); },
                    function () { tabObject.css("background-color", oldBgColor); });
                });
            }
            //使奇偶行不同的颜色
            $.fn.wyhinterlaced = function (options) {//options 经常用这个表示有许多个参数。 
                var defaultVal = {
                    odd: '#DDEDFB',
                    even: '#fff',
                };

                var obj = $.extend(defaultVal, options);

                return this.each(function () {
                    var tabObject = $(this); //获取当前对象 
                    if (tabObject.index() % 2 == 0) {
                        tabObject.css("background-color", obj.odd);
                    } else {
                        tabObject.css("background-color", obj.even);
                    }
                });
            }
        })(jQuery);
    }

})