var li = $("nav li .active");
if (li.length > 0) {
    selected(li);
} else {
    //使用面包屑遍历可被识别的导航，并展开其父级导航。
    $(function () {
        var lis = $(".breadcrumb > li");
        var length = lis.length;
        for (; length > 0; length--) {
            var li = lis.eq(length - 1);
            var liA = li.find("a");
            var text, href = null;
            if (liA.length > 0) {
                text = liA.text();
                href = liA.attr("href");
            } else {
                text = li.text();
            }
            var navLis = $("nav li");
            $.each(navLis, function () {
                var item = $(this);
                var t = item.find(".menu-text").text();
                var h = item.find("a").attr("href");
                if (t == text && h == href) {
                    selected(item);
                    return false;
                }
                return true;
            });
        }
    });
}

//通过选中的导航来展开父级导航。
function selected(li) {
    var ul = li.parents("ul .submenu");
    if (ul.length > 0) {
        if (!li.hasClass("active")) {
            li.addClass("active");
        }
        ul.show();
        ul.parent().addClass("active open");
    }
}