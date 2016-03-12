/* 消息通知控件 */
define("dist/tips/notify", ["jquery"], function(require) {
    "use strict";
    require("jquery");
    !function($) {
        var notify = function(element, option) {
            if (this.$element = $(element), this.$note = $('<div class="msg alert"></div>'), this.options = $.extend(!0, {}, $.fn.notify.defaults, option), this.options.transition ? "fade" == this.options.transition ? this.$note.addClass("in").addClass(this.options.transition) : this.$note.addClass(this.options.transition) : this.$note.addClass("fade").addClass("in"), this.$note.addClass(this.options.type ? "alert-" + this.options.type : "alert-success"), this.options.message || "" === this.$element.data("message") ? "object" == typeof this.options.message && (this.options.message.html ? this.$note.html(this.options.message.html) : this.options.message.text ? this.$note.text(this.options.message.text) : this.$note.html(this.options.message)) : this.$note.html(this.$element.data("message")), this.options.closable)
                var pull = $('<a class="close pull-right" href="javascript:;">&times;</a>');
            return $(pull).on("click", $.proxy(close, this)), this.$note.prepend(pull), this
        },
        close = function() {
            return this.options.onClose && this.options.onClose(), $(this.$note).remove(), this.options.onClosed(), !1
        };
        notify.prototype.show = function() {
            this.options.fadeOut.enabled && this.$note.delay(this.options.fadeOut.delay || 3e3).fadeOut("slow", $.proxy(close, this)), this.$element.empty(), this.$element.append(this.$note), this.$note.alert()
        };
        notify.prototype.hide = function() {
            this.options.fadeOut.enabled ? this.$note.delay(this.options.fadeOut.delay || 3e3).fadeOut("slow", $.proxy(close, this)) : close.call(this)
        };
        notify.prototype.close = function() {
            close.call(this)
        };
        $.fn.notify = function(a) {
            return new notify(this, a)
        };
        $.fn.notify.defaults = {
            type: "success",
            closable: !1,
            transition: "fade",
            fadeOut: {enabled: !0,delay: 2e3},
            message: null,
            onClose: function() {},
            onClosed: function() {}
        }
    }(window.jQuery)
});