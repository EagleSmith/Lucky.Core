/* 产品 */
define("dist/goods/goods", ["$", "dist/application/app", "chosen"], function(require) {
    "use strict";
    var $ = require("$"), app = require("dist/application/app"), config = app.config, method = app.method;
    require("chosen");
    var attribute = $('[data-toggle="attribute"]');
    if(window.goods_data) {
        template.helper("attribute_selected", function(a, b) {
            return window.goods_data.attributes[parseInt(a)] == b ? 'checked="checked"' : config.empty
        });
        template.helper("attribute_checked", function(a, b) {
            return window.goods_data.attributes[parseInt(a)] == b ? 'class="active"' : config.empty
        });
        template.helper("attribute_disabled", function() {
            return window.goods_data.shelves ? 'disabled="disabled"' : config.empty
        });
        template.helper("specifications_checked", function(a, c) {
            return $.inArray(parseInt(c), window.goods_data.specifications[a]) > -1 ? 'checked="checked"' : config.empty
        })
    }
    else {
        template.helper("attribute_selected", function() {
            return config.empty
        });
        template.helper("specifications_checked", function() {
            return config.empty
        });
        template.helper("attribute_disabled", function() {
            return config.empty
        })
    }
    var initAttr = function(item) {
        //获取所有属性
        method.get(window.goods_setting.attribute_path, function(data) {
            data.shelves = window.goods_data.shelves;
            attribute.html(template("attribute_template", data));
            $(".js_custom_list_contair dl").each(function() {
                var that = $(this), first = that.find("ul:first"), id = first.attr("id"), title = first.find("li.active input[type='radio']:checked").data("title");
                $("#label_" + id).html(title)
            })
        }, config.lang.attributeError);
        item.customInput();
        //保存自定义属性值
        $(document).on("click", ".js_custom_list_save", function() {
            var customlist = $(this).parents(".js_custom_list").find(".js_custom_input"), name = $(this).data("name"), customdata = item.customdata(customlist);
            item.customAttrValvali(customlist) && customlist.val() && (item.getAttrValExist(customlist.val(), customdata.attrid) ? config.msg.info("已经存在的属性值") : item.addAttrVal(customdata, name, customlist, !1, $(this))), customlist.val() || config.msg.error("属性值为空！")
        });
        //取消添加自定义属性值
        $(document).on("click", ".js_custom_list_cancel", function() {
            $(this).parents(".js_custom_list").next(".js_custom_list").removeClass("hide"), $(this).parents(".js_custom_list").addClass("hide")
        });
        //添加自定义属性值
        $(document).on("click", ".js_custom_btn", function() {
            $(this).parents(".js_custom_list").prev(".js_custom_list").removeClass("hide"), $(this).parents(".js_custom_list").addClass("hide");
            var customselect = $(this).parents(".custom-select");
            customselect.scrollTop(customselect.get(0).scrollHeight - 0 + 60)
        });
        //删除自定义属性值
        $(document).on("click", ".js_custom_del", function() {
            var attr = $(this).prev("a").find("input[type='radio']"), attrvalid = attr.val();
            method.post(window.goods_setting.custom_attrvalue_del_path, function(data) {
                config.issucceed(data) ? (attr.prop("checked") ? $("#label_" + data.data.attrid).html("选择") : item.getAttrValExist($("#label_" + data.data.attrid).text(), data.data.attrid) || $("#label_" + data.data.attrid).html("选择"), attr.parents("li").remove(), config.msg.info(data.message || config.lang.removeSuccess)) : config.msg.error(data.message)
            }, config.lang.attributeError, !1, {attrid: attr.data("id"),attrvalid: attrvalid})
        });
        //保存自定义属性
        $(document).on("click", ".js_add_attr_save", function() {
            var that = $(this), addattrform = $(this).parents(".js_add_attr_form"), addattrinput = addattrform.find(".js_add_attr_input"), addattrvinput = addattrform.find(".js_add_attrv_input"), namevali = item.customAttrValvali(addattrinput), valuevali = item.customAttrValvali(addattrvinput);
            if (namevali && valuevali && addattrinput.val() && addattrvinput.val()) {
                var existattr = item.getAttrExist(addattrinput.val(), "custom-attr-dl-horizontal");
                if (existattr.flag)
                    if (item.getAttrValExist(addattrvinput.val(), existattr.id))
                        config.msg.info("已经存在的属性和属性值");
                    else {
                        var attrname = $("#" + existattr.id).find("input[type='radio']").attr("name");
                        item.addAttrVal({attrid: existattr.id,attrval: addattrvinput.val()}, attrname, addattrvinput, !0, $(this))
                    }
                else
                    that.data("loadingText", '<i class="fa fa-spinner fa-spin"></i>提交中'), that.button("loading"), $.ajax(window.goods_setting.custom_attr_add_path, {type: "post",data: {attrname: addattrinput.val(),attrvalname: addattrvinput.val()},dataType: "json"}).done(function(data) {
                        that.button("reset"), config.issucceed(data) ? (data.data.index = $(".js_custom_list_contair dl").length, item.attrresult(data.data, addattrvinput, "js_custom_list_contair"), config.msg.info(data.message)) : config.msg.error(data.message)
                    }).fail(function() {
                        that.button("reset"), config.msg.info(config.lang.attributeError)
                    })
            } else
                config.msg.error("请修改属性或属性值不正确的填写！");
            addattrinput.val() || addattrvinput.val() ? addattrinput.val() ? addattrvinput.val() || config.msg.error("属性值为空！") : config.msg.error("属性为空！") : config.msg.error("属性和属性值为空！")
        });
        //删除自定义属性
        $(document).on("click", ".js_custom_attr_del", function() {
            var that = $(this);
            config.msg.msgbox.confirm("确定删除整个分组吗？", function(result) {
                result && method.post(window.goods_setting.custom_attr_del_path, function(data) {
                    if (config.issucceed(data)) {
                        var attrdl = $("#" + data.data.attrid).parents("dl"), dlindex = attrdl.index(), nextdl = attrdl.nextAll("dl");
                        attrdl.remove(), nextdl.each(function(index) {
                            $(this).find("input[type='hidden']").attr("name", "attribute[" + (dlindex + index) + "].id"), $(this).find("input[type='radio']").attr("name", "attribute[" + (dlindex + index) + "].selected"), $(this).find(".js_custom_list_save").attr("data-name", "attribute[" + (dlindex + index) + "].selected")
                        }), config.msg.info(data.message || config.lang.removeSuccess)
                    } else
                        config.msg.error(data.message)
                }, config.lang.attributeError, !1, {attrid: that.data("id")})
            })
        });
        //添加自定义属性
        $(document).on("click", ".js_add_attr_btn", function() {
            $(".js_add_attr").addClass("hide"), $(".js_add_attr_form").removeClass("hide")
        });
        //取消添加自定义属性
        $(document).on("click", ".js_add_attr_cancel", function() {
            item.closeAttrForm()
        });
        //选中自定义下拉框
        $(document).on("click", "dd li", function() {
            $(this).find("input[type='radio']").length > 0 && ($(this).closest("li").show(), $(this).closest("dd").find(".js_custom_dropdown_label").removeClass("error"))
        });
        //触发自定义下拉框
        $(document).on("click", ".dropdown-toggle", function() {
            $(this).find(".js_custom_dropdown_label").trigger("click")
        });
        //点击自定义下拉框
        $(document).on("click", ".js_custom_dropdown_label", function() {
            var thatid = $(this).data("id"), thathtml = $(this).html();
            if ("选择" == thathtml)
                $(this).html(""), $("#" + thatid).find("li").show();
            else {
                var selected = 0;
                $("#" + thatid).find("input[type='radio']").each(function() {
                    $(this).prop("checked") && (selected = 1)
                }), selected && $("#" + thatid).find("li").show()
            }
            return "block" == $("#" + thatid).css("display") ? !1 : void 0
        });
        //自定义下拉框失去焦点
        $("body").on("blur", ".js_custom_dropdown_label", function() {
            var thatId = $(this).data("id"), thatName = $(this).html();
            if (thatName) {
                if ("选择" != thatName) {
                    var selected = 0;
                    $("#" + thatId).find("input[type='radio']").each(function() {
                        $(this).prop("checked") && ($(this).data("title") == thatName ? selected = 1 : ($(this).removeAttr("checked"), $(this).parent().parent().removeClass("active")))
                    }), selected ? $(this).removeClass("error") : $(this).addClass("error")
                }
            } else
                $(this).html("选择"), $("#" + thatId).find("input[type='radio']").each(function() {
                    $(this).removeAttr("checked"), $(this).parent().parent().show(), $(this).parent().parent().removeClass("active")
                }), $(this).removeClass("error")
        });
        //自定义下拉框输入搜索
        $(document).on("keyup", ".js_custom_dropdown_label", function() {
            var thatId = $(this).data("id"), thatText = $(this).text(), searchResult = $("#" + thatId).find(".js_search_result"), searchAttr = item.attrValSearch(thatText, thatId);
            searchAttr ? searchResult.addClass("hide") : (searchResult.find("a").html("未找到" + thatText), searchResult.removeClass("hide"))
        })
    };
    var spec = $('[data-toggle="specifications"]'), specTable = $("div.specificationstable"), noSpec = $("div.nospecifications");
    //类目规格控件
    var specControl = function() {
        this.specVals = null;
        this.SpecificationsList = null;
        this.TableData = {ths: []};
        this.specificationstable = $('[data-toggle="specificationstable"]');
        var that = this;
        //切换开启多规格
        $('[data-toggle="specification-enable"]').on("click", function() {
            var enable = $(this).data("enable");
            spec.toggle(enable), enable ? (specTable.show(), noSpec.hide()) : (specTable.hide(), noSpec.show())
        });
        //获取多规格数据
        var checkedEnable = $('[data-toggle="specification-enable"]:checked').data("enable");
        checkedEnable ? (spec.show(), specTable.show(), noSpec.hide()) : noSpec.show(), method.get(window.goods_setting.spec_path, function(data) {
            that.SpecificationsList = data.data;
            $.each(data.data, function(index, item) {
                that.TableData.ths.push(item)
            });
            spec.html(template("specifications_template", data));
            that.init();
            checkedEnable && that.acquire()
        }, config.lang.specificationsError)
    };
    //获取选中的规格
    specControl.prototype.acquire = function() {
        this.selectSpecifications_vals = [];
        var that = this;
        that.TableData.ths = [];
        $.each(this.SpecificationsList, function(index, item) {
            var id = item.id, checked = $("#specvals_" + id).find('[type="checkbox"]:checked'), specList = [];
            $.each(checked, function(index, item) {
                specList.push({val: item.title,key: item.value})
            });
            var specCount = specList.length > 0;
            specCount && that.TableData.ths.push(item), specCount && that.selectSpecifications_vals.push(specList)
        });
        this.showtable = this.selectSpecifications_vals.length;
        this.showtable && this.generate();
        this.showtable || this.specificationstable.html('<span class="help-block">请选择规格</span>')
    };
    //生成选中规格的sku
    specControl.prototype.generate = function() {
        this.res = this.combine(this.selectSpecifications_vals.reverse());
        this.rowspan();
        this.TableData.trs = [];
        var that = this;
        $.each(this.res, function(index, item) {
            var tds = [], keyList = [];
            $.each(item, function(subIndex, subItem) {
                var td = [];
                td.rowspan = that.row[subIndex];
                td.key = subItem.key;
                td.val = subItem.val;
                td.index = index;
                tds.push(td);
                keyList.push(subItem.key)
            });
            var key = keyList.join(":"), tr = {tds: tds,index: index,key: key};
            that.changeval(key);
            window.goods_data || (window.goods_data = {products: {}});
            if (window.goods_data && window.goods_data.products) {
                var product = window.goods_data.products[key];
                product && (tr = $.extend({}, tr, product))
            }
            that.TableData.trs.push(tr)
        });
        this.specificationstable.html(template("specifications_table_template", this.TableData));
        specTable.show()
    };
    //绑定sku行的输入事件
    specControl.prototype.changeval = function(key) {
        !window.goods_data.products[key] && (window.goods_data.products[key] = {});
        $(document).on("change", '[data-id="' + key + '"]', function() {
            var that = $(this), key = that.data("id"), thatName = that.data("name"), thatList = $('[data-id="' + key + '"]'), product = {};
            $.each(thatList, function() {
                var item = {};
                item[thatName] = that.val();
                product = $.extend({}, product, item)
            });
            var all = that.closest("tr").siblings().find('[data-name="' + thatName + '"]');
            that.valid() && $.each(all, function() {
                var item = $(this);
                item.val().length < 1 && (item.val(that.val()), item.valid())
            });
            window.goods_data.products[key] = product
        })
    };
    //创建sku行
    specControl.prototype.rowspan = function() {
        for (var row = [], resCount = this.res.length, valsCount = this.selectSpecifications_vals.length - 1; valsCount > -1; valsCount--)
            row[valsCount] = parseInt(resCount / this.selectSpecifications_vals[valsCount].length), resCount = row[valsCount];
        this.row = row.reverse()
    };
    //刷新规格数据
    specControl.prototype.refresh = function() {
        var that = this;
        that.TableData.ths = [];
        $.each(that.SpecificationsList, function(index, item) {
            that.TableData.ths.push(item)
        });
        this.acquire()
    };
    //规格列表
    specControl.prototype.SpecificationsListskey = function() {
        var that = this;
        return {
            delsp: function(id) {
                for (var list = [], i = 0, length = that.SpecificationsList.length; length > i; i++)
                    that.SpecificationsList[i].id != id && list.push(that.SpecificationsList[i]);
                that.SpecificationsList = list, that.refresh()
            },
            delspvals: function(id, list) {
                for (var i = 0, length = that.SpecificationsList.length; length > i; i++)
                    if (that.SpecificationsList[i].id == id) {
                        for (var allVal = [], x = 0, all_valLenght = that.SpecificationsList[i].all_val.length; all_valLenght > x; x++)
                            for (var y = 0, j = list.length; j > y; y++)
                                that.SpecificationsList[y].all_val[x].key != list[y] && allVal.push(that.SpecificationsList[y].all_val[x]);
                        that.SpecificationsList[i].all_val = allVal
                    }
                that.refresh()
            }
        }
    };
    //组合选择的规格
    specControl.prototype.combine = function(spec) {
        var sku = [];
        return function combo(item, spec, specLenght) {
            if (0 == specLenght)
                return sku.push(item);
            for (var i = 0; i < spec[specLenght - 1].length; i++)
                combo(item.concat(spec[specLenght - 1][i]), spec, specLenght - 1)
        }([], spec, spec.length), sku
    };
    specControl.prototype.init = function() {
        var that = this;
        //选中规格
        $(document).on("click", '[data-toggle="specvals"] input[type="checkbox"]', function() {
            that.acquire()
        });
        var attrItem = new customControl;
        //编辑自定义规格添加规格值
        $(document).on("click", ".js_add_speval", function() {
            var that = $(this).closest(".form-group").find("input"), thatId = $(this).data("id"), thatValue = that.val();
            attrItem.customAttrValvali(that) && thatValue ? attrItem.getSpevalExist(thatValue, thatId) ? $("#specvals_error_" + thatId).html("输入的规格值已存在") : ($("#specvals_error_" + thatId).html(""), $("#specvals_show_" + thatId).find("div").append(template("specv_show_tem", {val: thatValue,speid: thatId})), that.val(""), that.next(".js_limit").find("em").html(0)) : $("#specvals_error_" + thatId).html("请输入规格值，长度不要超过15个字")
        });
        //添加自定义规格值
        $(document).on("click", ".js_spe_speval", function() {
            var spevInput = $(".js_add_spev_input"), speValue = spevInput.val();
            attrItem.customAttrValvali(spevInput) && speValue ? attrItem.getnSpevalExist(speValue) ? $(".js_js_spe_spev_error").html("输入的规格值已存在") : ($(".js_js_spe_spev_error").html(""), $(".js_spe_spev_show").append(template("specv_show_tem", {val: speValue,speid: ""})), spevInput.val(""), spevInput.next(".js_limit").find("em").html(0)) : $(".js_js_spe_spev_error").html("请输入规格值，长度不要超过15个字")
        });
        //编辑自定义规格
        $(document).on("click", ".js_specifica_edit", function() {
            var thatId = $(this).data("id");
            attrItem.closeopen(thatId, !1), $(".js_add_spe_div,.js_specifica_edit,.js_specifica_del").addClass("hide")
        });
        //保存编辑自定义规格
        $(document).on("click", ".js_specvals_val_save", function() {
            var thatId = $(this).data("id"), e = $(this).closest(".form-group").prev(".form-group"), f = attrItem.getSpelist(e), g = f.arrval, h = f.idarr, i = {speid: thatId,speval: g};
            g.length > 0 && attrItem.saveSpeval(i, $(this)), h.length > 0 && attrItem.delSpeval({speid: thatId,idarr: h}, $(this)), g.length < 1 && h.length < 1 && config.msg.error("请添加规格值或者删除规格值")
        });
        //取消编辑自定义规格
        $(document).on("click", ".js_specvals_val_cancel", function() {
            var thatId = $(this).data("id");
            attrItem.closeopen(thatId, !0)
        });
        $(document).on("click", ".js_del_specvals_val_list", function() {
            var thatId = $(this).data("id");
            thatId ? $(this).parent().removeClass("js_specvals_result").addClass("hide js_specvals_del") : $(this).parent().remove()
        });
        //添加自定义规格
        $(document).on("click", ".js_add_spe_btn", function() {
            attrItem.specloseopen(!1)
        });
        //保存自定义规格
        $(document).on("click", ".js_add_spe_save", function() {
            var speName = $(".js_add_spe_input").val(), speValue = ($(".js_add_spev_input").val(), $(".js_spe_spev_show").find(".js_specvals_temp").length), speExist = attrItem.getSpeExist(speName);
            if ($(".js_js_spe_spev_error").html(""), speName)
                if (attrItem.customAttrValvali($(".js_add_spe_input")))
                    if (speExist.flag)
                        if (speValue > 0) {
                            var spevalExist = attrItem.spevalExist($(".js_spe_spev_show"), speExist.id);
                            if (spevalExist.length > 0)
                                $(".js_js_spe_spev_error").html("输入的规格值：" + spevalExist + "已存在");
                            else {
                                var id = speExist.id, formGroup = $(this).closest(".form-group").prev(".form-group"), speList = attrItem.getSpelist(formGroup), arrval = speList.arrval, speItem = (speList.idarr, {speid: id,speval: arrval});
                                arrval.length > 0 && (attrItem.saveSpeval(speItem, $(this)), $(".js_add_spe_input").val(""), $(".js_add_spe_input").next(".js_limit").find("em").html(0), $(".js_spe_spev_show").find(".js_specvals_temp").remove(), attrItem.specloseopen(!0))
                            }
                        } else
                            config.msg.error("请添加规格值");
                    else {
                        var arrval = attrItem.getSpelist($(".js_spe_spev_show")).arrval, speItem = {spe: speName,speval: arrval};
                        arrval.length > 0 ? attrItem.saveSpe(speItem, $(this)) : config.msg.error("请添加规格值")
                    }
                else
                    config.msg.error("规格只能输入5个字请修改规则");
            else
                config.msg.error("请添加规格")
        });
        //取消添加自定义规格
        $(document).on("click", ".js_add_spe_cancel", function() {
            attrItem.specloseopen(!0)
        });
        //删除类目自定义规格
        $(document).on("click", ".js_specifica_del", function() {
            var thatId = $(this).data("id"), thatName = $(this).data("name");
            config.msg.msgbox.confirm("确定删除规格：" + thatName + "，及其所属的规格吗？", function(result) {
                result && method.post(window.goods_setting.custom_spec_del_path, function(data) {
                    config.issucceed(data) ? (config.msg.info(data.message || config.lang.removeSuccess), $("#js_specifications_" + data.data.speid).remove(), $("#specvals_ed_" + data.data.speid).next(".line-dashed").remove(), $("#specvals_ed_" + data.data.speid).remove(), that.SpecificationsListskey().delsp(data.data.speid), $(".js_specifica").length < 1 && $(".js_nospe_tips").removeClass("hide")) : config.msg.error(data.message)
                }, config.lang.attributeError, !1, {speid: thatId})
            })
        })
    };
    //自定义控件
    var customControl = function() { };
    //验证自定义属性是否存在
    customControl.prototype.getAttrExist = function(attrTitle, attrClass) {
        var flag, titleIndex, titleList = [], id = "";
        return $("." + attrClass).find("dt").each(function() {
            $(this).data("title") && titleList.push($(this).data("title"))
        }), titleIndex = $.inArray(attrTitle, titleList), flag = 0 > titleIndex ? !1 : !0, id = $("." + attrClass).find("dt").eq(titleIndex).data("id"), {flag: flag,id: id}
    };
    //验证自定义属性值是否存在
    customControl.prototype.getAttrValExist = function(attrVal, attrId) {
        var flag, valIndex, valList = [];
        return $("#" + attrId).find("input[type='radio']").each(function() {
            $(this).val() && valList.push($(this).data("title"))
        }), valIndex = $.inArray(attrVal, valList), flag = 0 > valIndex ? !1 : !0
    };
    //自定义属性搜索
    customControl.prototype.attrValSearch = function(text, attrId) {
        var resultCount = 0;
        return $("#" + attrId).find("input[type='radio']").each(function() {
            var title = $(this).data("title") || "选择", selectElement = $(this).parent().parent();
            title && (title += "", title.indexOf(text) > -1 ? (resultCount++, selectElement.show()) : selectElement.hide())
        }), resultCount
    };
    //验证自定义规格值是否存在
    customControl.prototype.getSpevalExist = function(name, specId) {
        var flag, nameIndex, nameList = [];
        return $("#specvals_show_" + specId).find(".js_specvals_show").each(function() {
            nameList.push($(this).data("name"))
        }), nameIndex = $.inArray(name, nameList), flag = 0 > nameIndex ? !1 : !0
    };
    //是否已存在的自定义规格
    customControl.prototype.spevalExist = function(element, speId) {
        var flag, nameList = [], arrval = this.getSpelist(element).arrval, existItem = "";
        return $("#specvals_show_" + speId).find(".js_specvals_show").each(function() {
            nameList.push($(this).data("name"))
        }), $(arrval).each(function(index, item) {
            flag = $.inArray(item, nameList), flag > -1 && (existItem += item + ",")
        }), existItem
    };
    //判断是否存在自定义规格值
    customControl.prototype.getnSpevalExist = function(name) {
        var flag, nameIndex, nameList = [];
        return $(".js_spe_spev_show").find(".js_specvals_show").each(function() {
            nameList.push($(this).data("name"))
        }), nameIndex = $.inArray(name, nameList), flag = 0 > nameIndex ? !1 : !0
    };
    //添加自定义属性值
    customControl.prototype.addAttrVal = function(data, name, element, attrClass, btn) {
        var that = this;
        btn.data("loadingText", '<i class="fa fa-spinner fa-spin"></i>提交中'), btn.button("loading"), $.ajax(window.goods_setting.custom_attrvalue_add_path, {type: "post",data: data,dataType: "json"}).done(function(data) {
            btn.button("reset"), config.issucceed(data) ? (data.data.checked = !0, data.data.name = name, that.result(data.data, element, attrClass)) : config.msg.error(data.message)
        }).fail(function() {
            btn.button("reset"), config.msg.info(config.lang.attributeError)
        })
    };
    //关闭自定义属性框
    customControl.prototype.closeAttrForm = function() {
        $(".js_add_attr").removeClass("hide"), $(".js_add_attr_form").addClass("hide")
    };
    //自定义属性添加完成
    customControl.prototype.attrresult = function(data, element, attrClass) {
        var lastdl = $("." + attrClass).find("dl:last");
        lastdl.length ? $(template("customattr_tem", data)).insertAfter(lastdl) : $(template("customattr_tem", data)).insertBefore($("." + attrClass).find(".line-dashed:first")), element.val(""), element.next(".js_limit").find("em").html(0), $(".js_add_attr_input").val(""), $(".js_add_attr_input").next(".js_limit").find("em").html(0), this.closeAttrForm(), config.msg.info(config.lang.saveSuccess)
    };
    //自定义属性值添加完成
    customControl.prototype.result = function(data, element, attrClass) {
        var attrFirst = $("#" + data.attrid).find(".js_custom_list:first");
        $("#" + data.attrid).find("li").removeClass("active"), $("#" + data.attrid).find("li input[type='radio']").removeAttr("checked"), $(template("customattrval_tem", data)).insertBefore(attrFirst), $("#label_" + data.attrid).html(data.title), $("#label_" + data.attrid).removeClass("error"), $(document).trigger("click"), element.val(""), element.next(".js_limit").find("em").html(0), attrClass ? ($(".js_add_attr_input").val(""), $(".js_add_attr_input").next(".js_limit").find("em").html(0), this.closeAttrForm()) : (attrFirst.addClass("hide"), attrFirst.next(".js_custom_list").removeClass("hide")), $("#" + data.attrid).find(".js_search_result").addClass("hide"), config.msg.info(config.lang.saveSuccess)
    };
    //验证输入内容长度
    customControl.prototype.customAttrValvali = function(element) {
        var result = !0, limit = element.data("limit") - 0, len = this.getInputLen(element);
        return result = len > 2 * limit ? !1 : !0
    };
    //验证自定义属性内容
    customControl.prototype.customInput = function() {
        var that = this;
        //输入实时验证
        $(document).on("keyup", ".js_custom_input", function() {
            var inputlen = that.getInputLen($(this)), limit = $(this).data("limit") - 0, next = $(this).next(".js_limit");
            next.find("em").html(Math.ceil(inputlen / 2)), inputlen > 2 * limit ? next.addClass("error") : next.removeClass("error")
        })
    };
    //计算输入字符长度
    customControl.prototype.getInputLen = function(element) {
        return element.val().replace(/[^\x00-\xff]/g, "xx").length
    };
    //获取添加的自定义属性
    customControl.prototype.customdata = function(element) {
        return {attrid: element.data("id"),attrval: element.val()}
    };
    //获取自定义规格值列表
    customControl.prototype.getSpelist = function(element) {
        var valarr = [], idarr = [];
        element.find(".js_specvals_temp").each(function() {
            valarr.push($(this).data("name"))
        });
        element.find(".js_specvals_del .js_del_specvals_val_list").each(function() {
            idarr.push($(this).data("id"))
        });
        return {arrval: valarr,idarr: idarr}
    };
    //ajax保存自定义规格
    customControl.prototype.saveSpeval = function(spec, btn) {
        var that = this;
        btn.data("loadingText", '<i class="fa fa-spinner fa-spin"></i>提交中'), btn.button("loading"), $.ajax(window.goods_setting.custom_specvalue_add_path, {type: "post",data: spec,dataType: "json"}).done(function(data) {
            btn.button("reset"), config.issucceed(data) ? (that.resultSpeval(data.data), config.msg.info(data.message || config.lang.removeSuccess)) : config.msg.error(data.message)
        }).fail(function() {
            btn.button("reset"), config.msg.info(config.lang.attributeError)
        })
    };
    //添加类目自定义规格
    customControl.prototype.saveSpe = function(speItem, btn) {
        var that = this;
        btn.data("loadingText", '<i class="fa fa-spinner fa-spin"></i>提交中'), btn.button("loading"), $.ajax(window.goods_setting.custom_spec_add_path, {type: "post",data: speItem,dataType: "json"}).done(function(data) {
            btn.button("reset"), config.issucceed(data) ? (that.resultSpe(data.data), $(".js_nospe_tips").addClass("hide"), config.msg.info(data.message || config.lang.removeSuccess)) : config.msg.error(data.message)
        }).fail(function() {
            btn.button("reset"), config.msg.info(config.lang.attributeError)
        })
    };
    //删除类目自定义规格
    customControl.prototype.delSpeval = function(a) {
        var that = this;
        $.ajax(window.goods_setting.custom_specvalue_del_path, {type: "post",data: a,dataType: "json"}).done(function(data) {
            config.issucceed(data) ? (that.resultdelSpeval(data.data), specItem.SpecificationsListskey().delspvals(data.data.speid, data.data.spevalidList), config.msg.info(data.message || config.lang.removeSuccess)) : config.msg.error(data.message)
        }).fail(function() {
            config.msg.info(config.lang.attributeError)
        })
    };
    //编辑自定义规格保存返回
    customControl.prototype.resultSpeval = function(data) {
        var speid = data.speid, speElement = $("#specvals_" + speid), speDiv = $("#specvals_show_" + speid).find("div");
        speElement.append(template("specv_tem", data));
        this.closeopen(speid, !0);
        speDiv.find(".js_specvals_temp").remove();
        speDiv.append(template("specv_result_tem", data))
    };
    //自定义规格添加完成
    customControl.prototype.resultSpe = function(spec) {
        $(template("spe_result_tem", spec)).insertBefore($(".js_add_spe_div"));
        var specItem = {id: spec.id,name: spec.name,all_val: spec.spevalList};
        specItem.SpecificationsList.push(specItem), specItem.TableData.ths.push(specItem), specItem.acquire(), $(".js_add_spe_input").val(""), $(".js_add_spe_input").next(".js_limit").find("em").html(0), $(".js_spe_spev_show").find(".js_specvals_temp").remove(), this.specloseopen(!0)
    };
    //自定义规格删除完成
    customControl.prototype.resultdelSpeval = function(spec) {
        var speid = spec.speid, element = $("#specvals_" + speid), showDiv = $("#specvals_show_" + speid).find("div");
        this.closeopen(speid, !0), $(spec.spevalidList).each(function(index, item) {
            element.find("#spe_" + item).remove()
        }), showDiv.find(".js_specvals_del").remove()
    };
    //关闭打开编辑规格面板
    customControl.prototype.closeopen = function(a, c) {
        $("#specvals_ed_" + a).toggleClass("hide", c), $("#js_specifications_" + a).toggleClass("hide", !c), $(".js_add_spe_div,.js_specifica_edit,.js_specifica_del").toggleClass("hide", !c)
    };
    //关闭打开添加规格面板
    customControl.prototype.specloseopen = function(flag) {
        $(".js_add_spe_form").toggleClass("hide", flag), $(".js_add_spe_div,.js_specifica_edit,.js_specifica_del").toggleClass("hide", !flag)
    };
    //判断是否存在规则
    customControl.prototype.getSpeExist = function(name) {
        var flag, nameIndex, nameList = [], id = "";
        $("[data-toggle='specifications']").find(".js_specifica").each(function() {
            $(this).data("name") && nameList.push($(this).data("name"))
        });
        nameIndex = $.inArray(name, nameList);
        flag = 0 > nameIndex ? !1 : !0;
        id = $("[data-toggle='specifications']").find(".js_specifica").eq(nameIndex).data("id");
        return  {flag: flag,id: id}
    };
    attribute.length > 0 && initAttr(new customControl);
    var specItem;
    spec.length > 0 && (specItem = new specControl);
    var undertake = $(".js_undertake");
    if (undertake.length > 0) {
        var freightContainer = $(".js_freight_container"), freightType = $(".js_freight_type"), unifyContainer = $(".js_unify_container"), templateContainer = $(".js_template_container"), freightTemplate = ($(".js_freight_template_loading"), $(".js_freight_template")), freightTemplateRefresh = $(".js_freight_template_refresh"), freightItem = $(".js_freight_item");
        undertake.on("click", function() {
            freightContainer.toggle($(this).hasClass("js_freight_container_show"))
        });
        freightType.on("click", function() {
            var that = $(this);
            unifyContainer.toggle(that.hasClass("js_unify_container_show")), templateContainer.toggle(that.hasClass("js_template_container_show"))
        });
        freightItem.on("click", function() {
            var that = $(this), dl = that.closest("dl");
            $('input[type="text"]', dl).addAttr("disabled", !that.prop("checked"))
        });
        freightTemplateRefresh.on("click", function(e) {
            e && e.preventDefault();
            var that = $(this), remote = that.data("remote") || that.attr("href"), index = that.find("i");
            index.addClass("fa-spin");
            var option = ' <option value="{0}">{1}</option>';
            freightTemplate.empty();
            $.post(remote).done(function(data) {
                if(data.data) {
                    freightTemplate.append('<option value="">请选择</option>');
                    $.each(data.data, function(index, item) {
                        freightTemplate.append(option.format(item.id, item.name))
                    })
                }
                index.removeClass("fa-spin");
                freightTemplate.focus()
            }).fail(function() {
                index.removeClass("fa-spin");
                config.msg.info(config.lang.exception)
            })
        });
        //提交表单
        $(document).on("click", 'button[type="submit"]', function() {
            if (unifyContainer.is(":visible")) {
                var selectFreight = $(".js_freight_item:checked");
                if (selectFreight.length < 1)
                    return config.msg.info("至少选择一个运送方式"), !1
            }
        })
    }
    $(document).on("click", ".js_save_submit", function() {
        return $(".js_fileList li.imgbox").length < 1 ? (config.msg.error("至少选择一个商品图片"), !1) : void 0
    });
    $(document).on("click", ".js_save_submit", function() {
        var specPanel = $('[data-toggle="specifications"]'), selectSpec = specPanel.find("input[type='checkbox']:checked").length;
        return !specPanel.is(":hidden") && 1 > selectSpec ? (config.msg.error("至少选择一个商品规格"), !1) : void 0
    });
    //商品限购
    $(".js_quota").on("click", function() {
        var quotaShow = $(this).hasClass("js_quota_show"), container = $(".js_quota_container");
        container.toggleClass("hd", !quotaShow).toggleClass("inline", quotaShow)
    });
    //响应提交按钮提交表单
    $(document).on("click", ".js_submit", function() {
        $(this).closest("form").submit()
    });
    //按键事件响应回车提交
    $(document).on("keypress", function(e) {
        var item = e || event, keycode = item.keyCode || item.which || item.charCode;
        return 13 == keycode ? ($(".js_spe_speval").closest(".js_enter_div").hasClass("hide") || $(".js_spe_speval").trigger("click"), $(".js_add_speval").each(function() {
            $(this).closest(".js_enter_div").hasClass("hide") || $(this).trigger("click")
        }), !1) : void 0
    })
});
