$(document).ready(function() {
    // scroll body to 0px on click
    $('#back-to-top').click(function() {
        (function smoothscroll() {
            var currentScroll = document.documentElement.scrollTop || document.body.scrollTop;
            if (currentScroll > 0) {
                window.requestAnimationFrame(smoothscroll);
                window.scrollTo(0, currentScroll - (currentScroll / 15));
            }
        })();
    });
});