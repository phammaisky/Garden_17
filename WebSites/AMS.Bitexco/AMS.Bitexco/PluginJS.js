



(function ($) {
    var shade = "red";

    $.fn.greenify = function (options) {

        var setting = $.extend({
            color: "green", 
            backgroundColor: "white"
        }, options);

        return this.css({
            color: setting.color,
            backgroundColor: setting.backgroundColor
        });
    }

    $.fn.showLinkLocation = function () {
        this.filter("a").each(function () {
            
            var link = $(this);
            link.append("(" + link.attr("href") + ")");
            
        })
        return this;
    }

    // Plugin definition.
    $.fn.hilight = function (options) {

        // Iterate and reformat each matched element.
        return this.each(function () {

            var elem = $(this);

            // ...

            var markup = elem.html();

            // Call our format function.
            markup = $.fn.hilight.format(markup);

            elem.html(markup);

        });

    };

    // Define our format function.
    $.fn.hilight.format = function (txt) {
        return "<strong>" + txt + "</strong>";
    };
}(jQuery))