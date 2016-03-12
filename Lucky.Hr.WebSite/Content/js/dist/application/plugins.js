/* 应用插件 */
define("dist/application/plugins", ["$", "dist/application/setting", "dist/tips/init"], function(require) {
    "use strict";
    var $ = require("$"), setting = require("dist/application/setting");
    //提示窗口
    $("[data-toggle=tooltip]").tooltip();
    //微滚动条
    $(function() {
        var slimScroll = $(".no-touch .slim-scroll");
        slimScroll.length > 0 && require.async(["slimscroll"], function() {
            slimScroll.each(function() {
                var a, that = $(this), data = that.data();
                that.slimScroll(data), $(window).resize(function() {
                    clearTimeout(a), a = setTimeout(function() {
                        that.slimScroll(data)
                    }, 100)
                })
            })
        })
    });
    //必填项
    var chosen = $('[data-toggle="chosen"]');
    chosen.length > 0 && require.async(["chosen"], function() {
        chosen.each(function() {
            var that = $(this), nosearch = that.data("nosearch"), item = {}, maxlength = that.data("maxlength"), ruleRequired = that.data("ruleRequired"), msgRequired = that.data("msgRequired") || that.data("placeholder"), remote = that.data("remote") || that.attr("href");
            if (maxlength && (item.max_selected_options = maxlength), nosearch && (item.disable_search_threshold = 9999999), item.hide = !1, item.no_results_text = "找不到", ruleRequired) {
                var form = $(this).closest("form");
                $(form).on("click", 'button[type="submit"]', function() {
                    var vaule = that.val();
                    return vaule ? void 0 : (setting.msg.info(msgRequired || "请输入必填项"), !1)
                })
            }
            remote ? (console.log(remote), $.ajax(remote, {type: "post",dataType: "json"}).done(function(data) {
                $.each(data, function(index, element) {
                    var option = '<option value="{1}">{0}</option>';
                    that.append(option.format(element.text, element.value))
                }), that.chosen(item)
            }).fail(function() {
                setting.msg.info("网络异常")
            })) : that.chosen(item)
        })
    });
    //下拉组合框
    var select2 = $('[data-toggle="select2"]');
    select2.length > 0 && require.async(["select2"], function() {
        select2.each(function() {
            var that = $(this), item = {}, maxcount = that.data("maxcount"), chang = that.data("chang"), remote = (that.data("remote") || that.attr("href"), that.data("ruleRequired")), msgRequired = that.data("msgRequired") || that.data("placeholder");
            if (maxcount && (item.maximumSelectionSize = maxcount), remote) {
                var form = $(this).closest("form");
                $(form).on("click", 'button[type="submit"]', function() {
                    var value = that.val();
                    return value ? void 0 : (setting.msg.info(msgRequired || "请输入必填项"), !1)
                })
            }
            if (chang)
                switch (chang) {
                    case "submit":
                        that.on("change", function() {
                            that.closest("form ").submit()
                        })
                }
            that.select2(item)
        })
    });
    //日期范围控件
    var daterangepicker = $('[data-toggle="daterangepicker"]');
    daterangepicker.length > 0 && require.async(["moment", "daterangepicker"], function() {
        daterangepicker.each(function() {
            var that = $(this);
            that.daterangepicker({timePicker: !0,timePickerIncrement: 1,timePicker12Hour: !1,format: "YYYY/MM/DD HH:mm"});
            var nextBtn = that.next("span.input-group-addon"), prevBtn = that.prev("span.input-group-addon");
            nextBtn.add(prevBtn).on("click", function() {
                that.trigger("click")
            })
        })
    });
    //日期时间控件
    var datetimepicker = $('[data-toggle="datetimepicker"]');
    datetimepicker.length > 0 && require.async(["moment", "datetimepicker", "style/plugins/datetimepicker/datetimepicker.min.css"], function() {
        datetimepicker.each(function() {
            var that = $(this), minutestep = that.data("minutestep") || 5, position = that.data("position") || "bottom-left", startNow = "", startdate = that.data("startdate"), endNow = "", enddate = that.data("enddate"), after = that.data("after"), before = that.data("before"), offsetday = that.data("offsetday") - 0 || 0;
            "now" == startdate ? (startNow = new Date, startNow.setDate(startNow.getDate() + offsetday)) : (startNow = new Date(startdate), startNow.setDate(startNow.getDate() + offsetday)), "now" == enddate ? (endNow = new Date, endNow.setDate(endNow.getDate())) : (endNow = new Date(enddate), endNow.setDate(endNow.getDate())), that.datetimepicker({language: "zh-CN",startDate: startNow,endDate: endNow,autoclose: !0,minuteStep: minutestep,pickerPosition: position}).on("show", function() {
                if (after && $(after).val()) {
                    var afterValue = new Date($(after).val());
                    afterValue.setDate(afterValue.getDate() + offsetday), that.datetimepicker("setStartDate", afterValue)
                }
                if (before && $(before).val()) {
                    var beforeValue = new Date($(before).val());
                    beforeValue.setDate(beforeValue.getDate() + offsetday), that.datetimepicker("setEndDate", beforeValue)
                }
            });
            var nextBtn = that.next("span.input-group-addon"), prevBtn = that.prev("span.input-group-addon");
            nextBtn.add(prevBtn).on("click", function() {
                that.datetimepicker("show")
            })
        })
    });
    //处理ajax数据
    var ajaxData = $('[data-table="ajax"]');
    ajaxData.length > 0 && require.async(["datatables"], function() {
        ajaxData.each(function() {
            var that = $(this), path = that.data("path"), search = that.data("search") || !1, placeholder = that.data("placeholder") || "", bsort = that.data("bsort") || !1, aocolumndefs = that.data("aocolumndefs") || !1;
            aocolumndefs && (bsort = !0), that.dataTable({processing: !0,serverSide: !0,aLengthMenu: [20, 30, 50],searching: search,bSort: bsort,aoColumnDefs: aocolumndefs,sPaginationType: "full_numbers",ajax: {url: path,type: "post",data: function(data) {
                data.myKey = "myValue"
            }},oLanguage: {sLengthMenu: " <span>每页显示</span>_MENU_ <span>条记录</span> ",sZeroRecords: "对不起，查询不到任何相关数据",sInfo: "当前显示 _START_ 到 _END_ 条，共 _TOTAL_ 条记录",sInfoEmtpy: "找不到相关数据",sInfoFiltered: "数据表中共为 _MAX_ 条记录)",sProcessing: "正在加载中...",sSearch: "<span>搜索:</span> ",sUrl: "",oPaginate: {sFirst: "首页",sPrevious: " 上一页 ",sNext: " 下一页 ",sLast: " 末页 "}}}), that.closest(".dataTables_wrapper").find(".dataTables_filter input").attr("placeholder", placeholder)
        })
    });
    //选择报告日期范围
    var reportrange = $('[data-toggle="reportrange"]');
    reportrange.length > 0 && require.async(["moment", "daterangepicker"], function() {
        reportrange.each(function() {
            {
                var that = $(this);
                that.data("selected")
            }
            console.log();
            var span = that.find("span"), text = span.text().split("-"), startDate = moment(text[0]), endDate = moment(text[1]), days = endDate.diff(startDate, "days"), position = that.data("position") || "right", minDate = that.data("minDate") || !1, maxDate = that.data("maxDate") || !1;
            that.daterangepicker({format: "YYYY/MM/DD HH:mm",ranges: {"今天": [moment(), moment()],"昨天": [moment().subtract("days", 1), moment().subtract("days", 1)],"最近7天": [moment().subtract("days", 6), moment()],"最近30天": [moment().subtract("days", 29), moment()],"这个月": [moment().startOf("month"), moment().endOf("month")],"上个月": [moment().subtract("month", 1).startOf("month"), moment().subtract("month", 1).endOf("month")]},minDate: minDate,maxDate: maxDate,opens: position,startDate: startDate || moment().subtract("days", days),endDate: endDate || moment()}, function(startDate, endDate) {
                var dateRange = startDate.format("YYYY/MM/DD") + " - " + endDate.format("YYYY/MM/DD");
                $("span", that).html(dateRange), $('input[type="hidden"]', that).val(dateRange);
                var change = that.data("change");
                if (change)
                    switch (change) {
                        case "submit":
                            that.closest("form ").submit()
                    }
            })
        })
    });
    //选择报告简易日期范围
    var reportrange1 = $('[data-toggle="reportrange1"]');
    reportrange1.length > 0 && require.async(["moment", "daterangepicker"], function() {
        reportrange1.each(function() {
            var that = $(this), selected = (that.data("selected"), that.find("span")), text = selected.text().split("-"), startDate = moment(text[0]), endDate = moment(text[1]), g = endDate.diff(startDate, "days"), position = that.data("position") || "right", minDate = that.data("minDate") || !1, maxDate = that.data("maxDate") || !1;
            that.daterangepicker({format: "YYYY/MM/DD",ranges: {"最近30天": [moment(), moment().add("days", 29)],"最近90天": [moment(), moment().add("days", 89)]},minDate: minDate,maxDate: maxDate,opens: position,startDate: startDate || moment().subtract("days", g),endDate: endDate || moment()}, function(startDate, endDate) {
                var dateRange = startDate.format("YYYY/MM/DD") + " - " + endDate.format("YYYY/MM/DD");
                $("span", that).html(dateRange), $('input[type="hidden"]', that).val(dateRange);
                var change = that.data("change");
                if (change)
                    switch (change) {
                        case "submit":
                            that.closest("form ").submit()
                    }
            })
        })
    });
    //选择日期范围
    var dateRange = $('[data-toggle="dateRange"]');
    dateRange.length > 0 && require.async(["moment", "daterangepicker"], function() {
        dateRange.each(function() {
            var that = $(this), format = $(this).data("format"), timePicker = $(this).data("timePicker");
            that.daterangepicker({timePicker: timePicker,format: format})
        })
    });
    //选择子日期范围
    var datepickersub = $('[data-toggle="datepickersub"]');
    datepickersub.length > 0 && require.async(["datepicker"], function() {
        datepickersub.each(function() {
            var that = $(this), startdate = that.data("startdate"), date = "", format = that.data("format") || "yyyy-MM-dd";
            date = "now" == startdate || "" == startdate ? new Date : new Date(startdate), that.datepicker({startDate: date,format: format}).on("changeDate", function(text) {
                var formatDate = text.date.format(format);
                $("span", that).html(formatDate), $('input[type="hidden"]', that).val(formatDate);
                var change = that.data("change");
                if (change)
                    switch (change) {
                        case "submit":
                            that.closest("form ").submit()
                    }
            })
        })
    });
    //js复制文本
    var copyText = $(".js_copy_text", document);
    copyText.length > 0 && require.async("zeroclipboard", function() {
        ZeroClipboard.setDefaults({moviePath: "/assets/swf/zeroclipboard.swf"});
        $.each(copyText, function(index) {
            var copyBtn = '<button class="btn btn-default js_copy btn-sm" id="{0}" type="button" data-clipboard-target="{2}" data-clipboard-text="{1}"><i class="fa fa-copy m-r-xs"></i>复制</button>  <span class="alert copy-success  alert-success hd "  >复制成功,请粘帖到您需要的地方</span>';
            $(this).data("icon") && (copyBtn = '<i id="{0}" title="复制" data-clipboard-target="{2}" data-clipboard-text="{1}" class="fa fa-copy js_copy"></i><span class="alert copy-success  alert-success hd "  >复制成功,请粘帖到您需要的地方</span>');
            var itemId = "copy_button{0}".format(index), e = copyBtn, that = $(this), text = that.text().trim(), id = that.attr("id");
            id ? that.after(e.format(itemId, "", id)) : that.append(e.format(itemId, text));
            var itemBtn = $("#" + itemId), clipboard = new ZeroClipboard(itemBtn), success = function(element) {
                var copySuccess = element.nextAll("span.copy-success");
                copySuccess.show(), setTimeout(function() {
                    copySuccess.fadeOut()
                }, 1e3)
            };
            clipboard.on("complete", function(item) {
                success($(item.options.button))
            })
        });
        $(document).on("click", "button.js_copy", function() {
            var that = $(this), text = that.data("clipboard-text").trim();
            $.browser.msie ? (window.clipboardData.setData("Text", text), success(that)) : prompt("按下 Ctrl+C 复制到剪贴板", text)
        })
    });
    $.fn.addAttr = function(attribute, value) {
        return this.each(function() {
            var that = $(this);
            value ? that.attr(attribute, attribute) : that.removeAttr(attribute)
        })
    };
    $.fn.adddClass = function(className, flag) {
        return this.each(function() {
            var that = $(this);
            flag && that.addClass(className)
        })
    };
    //上传处理
    !function($) {
        var fileupload = function(config, fileType, container) {
            this.element = container.get(0), this.defaults_file_type = fileType, this.init(config)
        };
        fileupload.defaults = {
            url: setting.empty,multi: !1,
            prevent_duplicates: !1,mime_types: [],
            max_file_size: 0,
            data: {},
            max_count: 0,
            FilesAdded: function() {},
            UploadProgress: function() {},
            FileUploaded: function() {},
            UploadComplete: function() {}
        };
        fileupload.prototype = {
            init: function(a) {
                var that = this;
                if (this.defaults_file_type) {
                    switch (this.defaults_file_type) {
                        case "picture":
                            fileupload.defaults.max_file_size = "2mb", fileupload.defaults.mime_types = [{title: "图片文件",extensions: "bmp,png,jpeg,jpg,gif"}], plupload.addI18n({"File extension error.": "图片格式必须为以下格式：bmp, jpeg, jpg, gif","File size error.": "文件大小错误。"});
                            break;
                        case "voice":
                            fileupload.defaults.max_file_size = "5mb", fileupload.defaults.mime_types = [{title: "音频文件",extensions: "mp3,wma,wav,amr"}], plupload.addI18n({"File extension error.": "语音格式必须为以下格式：mp3, wma, wav, amr"});
                            break;
                        case "video":
                            fileupload.defaults.max_file_size = "20mb", fileupload.defaults.mime_types = [{title: "视频文件",extensions: "rm,rmvb,wmv,avi,mpg,mpeg,mp4"}], plupload.addI18n({"File extension error.": "视频格式必须为以下格式：rm, rmvb, wmv, avi, mpg, mpeg, mp4"});
                            break;
                        case "file":
                            fileupload.defaults.max_file_size = "10mb", fileupload.defaults.mime_types = [{title: "自定义文件",extensions: "txt,xml,pdf,zip,doc,ppt,xls,docx,pptx,xlsx"}], plupload.addI18n({"File extension error.": "文件格式必须为以下格式：txt, xml, pdf, zip, doc, ppt, xls, docx, pptx, xlsx"})
                    }
                    plupload.addI18n({"File size error.": "最大只能上传{0}的文件".format(fileupload.defaults.max_file_size)})
                }
                this.options = $.extend(!0, {}, fileupload.defaults, a);
                this.plupload = new plupload.Uploader({
                    runtimes: "html5,flash,silverlight,html4",
                    browse_button: that.element,
                    url: that.options.url,
                    flash_swf_url: "/assets/swf/Moxie.swf",
                    silverlight_xap_url: "/assets/swf/Moxie.xap",
                    multi_selection: that.options.multi,
                    filters: {
                        max_file_size: that.options.max_file_size,
                        prevent_duplicates: that.options.prevent_duplicates,
                        mime_types: that.options.mime_types
                    },
                    multipart_params: that.options.data,
                    init: {
                        FilesAdded: function(a, b) {
                            return that.options.max_count && a.files.length > that.options.max_count ? (setting.msg.info("最多上传{0}个文件".format(that.options.max_count)), plupload.each(b, function(b) {
                                a.removeFile(b)
                            }), !1) : (that.options.FilesAdded(a, b), void a.start())
                        },
                        UploadProgress: function(a, b) {
                            that.options.UploadProgress(a, b)
                        },
                        UploadComplete: function(a, b) {
                            that.options.UploadComplete(a, b)
                        },
                        FileUploaded: function(a, c, d) {
                            that.options.FileUploaded(a, c, $.parseJSON(d.response))
                        },
                        Error: function(a, b) {
                            return setting.msg.info(b.message), !1
                        }
                    }
                });
                this.plupload.init()
            },
            update: function(a) {
                $.isPlainObject(a) && (this.options = $.extend(!0, this.options, a))
            },
            destroy: function() {
                this.plupload.destroy(), this.element.removeData("uploader")
            }
        };
        $.fn.uploader = function(c, e) {
            var f = this;
            require.async(["plupload"], function() {
                var a = typeof c;
                if ("string" === a) {
                    var g = Array.prototype.slice.call(arguments, 1);
                    f.each(function() {
                        var a = $.data(f, "uploader");
                        return a && $.isFunction(a[c]) && "_" !== c.charAt(0) ? void a[c].apply(a, g) : !1
                    })
                } else
                    f.each(function() {
                        var a = $.data(f, "uploader");
                        a ? a.update(c) : (a = new fileupload(c, e, f), $.data(f, "uploader", a))
                    })
            })
        }
    }(jQuery);
    //元素结尾插入内容
    $.fn.append2 = function(content, c) {
        var htmlLenght = $("body").html().length;
        this.append(content);
        var i = 1, timer = setInterval(function() {
            i++;
            var append2 = function() {
                clearInterval(timer), c()
            }, flag = htmlLenght != $("body").html().length || i > 1e3;
            flag && append2()
        }, 1)
    }
});