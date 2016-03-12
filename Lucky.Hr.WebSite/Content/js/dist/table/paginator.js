/* 后台分页 */
define("dist/table/paginator", ["$"], function(require) {
    "use strict";
    require("$");
    !function ($) {
        var pageNo = function (item, currentPage) {
            this.isCurrent = function () {
                return currentPage == item.currentPage;
            },
                this.isFirst = function () {
                    return 1 == currentPage;
                },
                this.isLast = function () {
                    return currentPage == item.totalPages;
                },
                this.isPrev = function () {
                    return currentPage == item.currentPage - 1;
                },
                this.isNext = function () {
                    return currentPage == item.currentPage + 1;
                },
                this.isLeftOuter = function () {
                    return currentPage <= item.outerWindow;
                },
                this.isRightOuter = function () {
                    return item.totalPages - currentPage < item.outerWindow;
                },
                this.isInsideWindow = function () {
                    if (item.currentPage < item.innerWindow + 1) {
                        return currentPage <= 2 * item.innerWindow + 1;
                    } else {
                        if (item.currentPage > item.totalPages - item.innerWindow) {
                            return item.totalPages - currentPage <= 2 * item.innerWindow;
                        } else {
                            return Math.abs(item.currentPage - currentPage) <= item.innerWindow;
                        }
                    }

                },
                this.number = function () {
                    return currentPage;
                }
        },
            page = {
                firstPage: function (paginator, option, current) {
                    var page = $("<li>").append($('<a href="#">').html(option.first).bind("click.bs-Paginator", function () {
                        
                        return paginator.firstPage(),false;
                    }));

                    return current.isFirst() && page.addClass("disabled"), page;
                },
                prevPage: function (paginator, option, current) {
                    var page = $("<li>").append($('<a href="#">').attr("rel", "prev").html(option.prev).bind("click.bs-Paginator", function () {
                        
                        return paginator.prevPage(),false;
                    }));

                    return current.isFirst() && page.addClass("disabled"), page;
                },
                nextPage: function (paginator, option, current) {
                    var page = $("<li>").append($('<a href="#">').attr("rel", "next").html(option.next).bind("click.bs-Paginator", function () {
                        
                        return paginator.nextPage(),false;
                    }));
                    return current.isLast() && page.addClass("disabled"), page;
                },
                lastPage: function (paginator, option, current) {
                    var page = $("<li>").append($('<a href="#">').html(option.last).bind("click.bs-Paginator", function () {
                        return paginator.lastPage(), false;
                    }));
                    return current.isLast() && page.addClass("disabled"), page;
                },
                gap: function (paginator, option) {
                    return $("<li>").addClass("disabled").append($('<a href="#">').html(option.gap));
                },
                page: function (paginator, option, current) {
                    var page = $("<li>").append(function () {
                        var html = $('<a href="#">');
                        return current.isNext() && html.attr("rel", "next"), current.isPrev() && html.attr("rel", "prev"), html.html(current.number()), html.bind("click.bs-Paginator", function () {
                            return paginator.page(current.number()), false;
                        }), html;
                    });
                    return current.isCurrent() && page.addClass("active"), page;
                }
            },
            paginator = function (element, config) {
                this.$element = $(element),
                    this.options = $.extend({}, paginator.DEFAULTS, config),
                    this.$ul = $(element),
                
                    this.render();
            };
        paginator.DEFAULTS = {
            currentPage: null,
            totalPages: null,
            innerWindow: 2,
            outerWindow: 1,
            first: '<i class="fa " title="第一页">首页</i>',
            prev: '<i class="fa" title="上一页">上一页</i>',
            next: '<i class="fa " title="下一页">下一页</i>',
            last: '<i class="fa " title="最后页">尾页</i>',
            gap: "...",
            truncate: false,
            page: function () { return true; }
        },
            paginator.prototype.render = function () {
                var option = this.options;
                if (!option.totalPages)
                    return void this.$element.hide();
                this.$element.show();
                var currentPageNo = new pageNo(option, option.currentPage);
                currentPageNo.isFirst() && option.truncate || (option.first && this.$ul.append(page.firstPage(this, option, currentPageNo)), option.prev && this.$ul.append(page.prevPage(this, option, currentPageNo)));
                for (var e = false, currentPage = 1, totalPage = option.totalPages; totalPage >= currentPage; currentPage++) {
                    var itemPageNo = new pageNo(option, currentPage);
                    itemPageNo.isLeftOuter() || itemPageNo.isRightOuter() || itemPageNo.isInsideWindow() ? (this.$ul.append(page.page(this, option, itemPageNo)), e = false) : !e && option.outerWindow > 0 && (this.$ul.append(page.gap(this, option)), e = true);
                }
                currentPageNo.isLast() && option.truncate || (option.next && this.$ul.append(page.nextPage(this, option, currentPageNo)), option.last && this.$ul.append(page.lastPage(this, option, currentPageNo)));
            };
        paginator.prototype.page = function (currentPage, totalPage) {
            var option = this.options;
            return void 0 === totalPage && (totalPage = option.totalPages), currentPage > 0 && totalPage >= currentPage && option.page(currentPage) && (this.$ul.empty(), option.currentPage = currentPage, option.totalPages = totalPage, this.render()), false;
        };
        paginator.prototype.firstPage = function () {
            return this.page(1);
        };
        paginator.prototype.lastPage = function () {
            return this.page(this.options.totalPages);
        };
        paginator.prototype.nextPage = function () {
            return this.page(this.options.currentPage + 1);
        };
        paginator.prototype.prevPage = function () {
            return this.page(this.options.currentPage - 1);
        };
        var otherPaginator = $.fn.Paginator;
        $.fn.Paginator = function (element) {
            var arg = arguments;
            return this.each(function () {
                var that = $(this),
                    bsPaginator = that.data("bs.Paginator"),
                    g = "object" == typeof element && element;
                bsPaginator || that.data("bs.Paginator", bsPaginator = new paginator(this, g)),
                    "string" == typeof element && bsPaginator[element].apply(bsPaginator, Array.prototype.slice.call(arg, 1));
            });
        };
        $.fn.Paginator.Constructor = paginator;
        $.fn.Paginator.noConflict = function () {
            return $.fn.Paginator = otherPaginator, this;
        }
    }(jQuery);
});