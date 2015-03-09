(function ($) {
    $.fn.extend({
        emulator: function (opt) {
            //组合配置信息。
            var options = $.extend({ defaultUrl: "/", url: null }, opt);
            //初始化辅助方法。
            var t = controller($(this));
            var iframe = t.find("iframe");
            //仿真器与document top的间距
            var spacing;
            //当前Url。
            var currentUrl;
            (function () {
                var url = options.url || ($.cookie("emulator_url") || options.defaultUrl);
                if (url.indexOf("about:") != -1) {
                    url = options.defaultUrl;
                }

                /*设置位置信息*/
                var position = $.cookie("emulator_position");
                if (position) {
                    var temp = position.split(",");
                    position = { top: temp[0], left: temp[1] };
                } else {
                    position = { top: 80, left: $(document).width() - t.width() - 50 };
                }
                t.css({ top: position.top, left: position.left });
                /*位置信息设置结束*/

                //设置url（不立即加载）
                currentUrl = url;

                /*可拖动*/
                t.draggable({
                    cursor: "move",
                    stop: function () {
                        $.cookie("emulator_position", t.css("top") + "," + t.css("left"));
                    }
                });
                /*可拖动结束*/

                iframe.load(function () {
                    $.cookie("emulator_url", t.getUrl());
                });
                t.find("#btnHome").click(function () {
                    t.setUrl(options.url);
                });
                t.find(".glyphicon-remove-circle").click(function () {
                    t.hide();
                });
                if ($.cookie("emulator_isShow") == "true") {
                    t.show();
                }

                /*跟随滚动条*/
                var timeout;
                $(window).scroll(function () {
                    if (timeout)
                        clearTimeout(timeout);
                    timeout = setTimeout(function () {
                        t.css({ top: $(window).scrollTop() - spacing });
                    }, 30);
                });
                /*跟随滚动条结束*/
            })();
            return t;

            function controller(e) {
                e.show = function () {
                    if (t.has(":hidden")) {
                        t.setUrl(currentUrl);
                        $.cookie("emulator_isShow", true);
                        t.fadeIn(t.getUrl() == currentUrl ? "fast" : "slow", function () {
                            spacing = $(document).scrollTop() - t.position().top;
                        });
                    }
                    return e;
                };
                e.hide = function () {
                    if (t.has(":visible")) {
                        $.cookie("emulator_isShow", null);
                        t.fadeOut("fast");
                    }
                    return e;
                };
                e.getUrl = function () {
                    return iframe.contents()[0].URL;
                };
                e.setUrl = function (url) {
                    url = url || options.defaultUrl;
                    if (url == e.getUrl()) {
                        e.reload();
                    } else {
                        iframe.attr("src", url);
                    }
                };
                e.reload = function () {
                    window[iframe[0].id].window.location.reload();
                };
                return e;
            }
        }
    });
})(jQuery);