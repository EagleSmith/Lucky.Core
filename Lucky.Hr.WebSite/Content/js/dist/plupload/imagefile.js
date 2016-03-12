/* 图片文件 */
define("dist/plupload/imagefile", ["$", "dist/application/app", "jquery_ui"], function(require, exports, module) {
    "use strict";
    var $ = require("$"), app = require("dist/application/app"), config = app.config, method = app.method;
    require("jquery_ui");
    var imageTemp = require("dist/plupload/imagetemplate"), progressTemp = require("dist/plupload/progresstemplate"), render_item = template.compile(imageTemp), render_progress = template.compile(progressTemp), uploadimte = null;
    $(".js_new_upload").each(function() {
        var $el = $(this), $form = $el.closest(".js_upload_container"), sname = $el.data("submitname"), upload_path = $el.data("uploadpath"), deletepath = $el.data("delpath"), count = $el.data("count"), datas = eval("(" + $el.data("data") + ")") || null;
        $el.uploader({multi: !0,url: upload_path,delete_path: deletepath,max_count: count - ($form.find("li.imgbox").length || 0),max_count_tips: count,data: datas,FilesAdded: function(a, b) {
            $.each(b, function(index, item) {
                $(".js_file_upload_queue", $form).append(render_progress({id: item.id,filename: item.name,filesize: Math.ceil(item.size / 1024)}))
            })
        },
        UploadProgress: function(uploader, file) {
            var uploadid = "upload{0}".format(file.id);
            if (uploadimte = $("#{0}".format(uploadid)), uploadimte.length) {
                var percent = file.percent + "%";
                $(".data", uploadimte).text(percent), $(".uploadify-progress-bar", uploadimte).width(percent)
            }
        },
        FileUploaded: function(uploader, file, result) {
            $.isPlainObject(result) ? (result.image.progressid = file.id, result.image.sname = sname, result.image.deletepath = deletepath, result.image.index = $form.find("li.imgbox").length, $(".js_fileList", $form).append(render_item(result.image)), uploadimte.remove()) : (uploadimte.addClass("uploadify-error"), uploadimte.remove(), uploader.files.removeFile(file), config.msg.info(result || config.lang.uplodError))
        },
        UploadComplete: function() {
            $(".uploadify-progress-bar", uploadimte).remove()
        }}, "picture"), $("ul.ipost-list", $form).sortable({opacity: .8})
    });
    $(document).on("click", "a.item_new_close", function(e) {
        var that = $(this), progressid = that.data("progressid"), delpath = that.data("delpath");
        $.post(delpath, {id: that.data("post-id"),url: that.data("path")});
        var imgbox = $(e.target).closest("li.imgbox");
        imgbox.fadeOut(function() {
            imgbox.remove(), $("#upload" + progressid).remove()
        })
    })
});