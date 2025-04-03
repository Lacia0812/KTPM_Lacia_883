(function ($) {
    $(function () {

        $('.sidenav').sidenav();
        $('.parallax').parallax();

        // Prevent browser back and forward buttons
        if (window.history && window.history.pushState) {
            window.history.replaceState(null, '', window.location.href); // Đảm bảo trạng thái đầu tiên không thể back
            window.history.pushState(null, '', window.location.href);

            $(window).on('popstate', function (e) {
                window.history.pushState(null, '', window.location.href);
                e.preventDefault();
            });
        }

        // Prevent right-click on entire window
        $(window).on("contextmenu", function () {
            return false;
        });

    }); // end of document ready
})(jQuery); // end of jQuery namespace
