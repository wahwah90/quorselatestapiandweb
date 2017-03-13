$(document).ready(function () {

    //RESIZE WINDOW
    $(window).resize(function () {
        var win = $(this);
        //1300
        if (win.width() >= 1280) {
            $("#global-header-mobile-nav").css({ 'display': 'none' });
        }
        //1024
        if (win.width() >= 1004) {
            $("#main-body").show();
            $("#mobile-body").hide();
            $("#mobile-body-2").hide();
        }
        //800
        if (win.width() >= 780) {
            $("#global-header-mobile").css({ 'display': 'none' });
        }
    });

    //JQUERY MOBILE HIDE "LOADING" TEXT ON BOTTOM OF THE PAGE
    $.mobile.loading().hide();

    //GLOBAL HEADER TABLET & PHONE ICON
    $("#global-header-mobile-icon-search").click(function () {
        $("#global-header-mobile").toggle();
        $("#global-header-mobile-nav").hide();
    });
    $("#global-header-mobile-icon-nav").click(function () {
        $("#global-header-mobile-nav").toggle();
        $("#global-header-mobile").hide();
    });
    //GLOBAL HEADER SEARCH NAVIGATION
    $("#global-header-search-nav-btn, #global-header-search-nav-dropdown").mouseenter(function () {
        $("#global-header-mobile-nav").hide();
        $("#global-header-search-nav-dropdown").css({ 'display': 'block' });
    }).mouseleave(function () {
        $("#global-header-search-nav-dropdown").css({ 'display': 'none' });
    });
    $("#global-header-search-nav-dropdown > ul > li").mouseenter(function () {
        $(this).css({ "background": "#0092D1" });
        $(this).children().css({ "color": "white" });
    });
    $("#global-header-search-nav-dropdown li").mouseleave(function () {
        $(this).css({ "background": "white" });
        $(this).children().css({ "color": "black" });
    });
    $("#global-header-search-nav-dropdown > ul > li ul > li").mouseenter(function () {
        $(this).children().css({ "color": "#0092D1" });
    });

    //GLOBAL FOOTER ICON HOVER
    $("#global-footer-partnership").mouseenter(function () {
        $("#global-footer-partnership-icon-image").attr('src', 'image/footer/icon-call-for-partnership-active.png');
    }).mouseleave(function () {
        $("#global-footer-partnership-icon-image").attr('src', 'image/footer/icon-call-for-partnership.png');
    });
    $("#global-footer-follow-social-media-facebook").mouseenter(function () {
        $(this).attr('src', 'image/footer/icon-social-media-facebook-active.png');
    }).mouseout(function () {
        $(this).attr('src', 'image/footer/icon-social-media-facebook.png');
    });
    $("#global-footer-follow-social-media-twitter").mouseenter(function () {
        $(this).attr('src', 'image/footer/icon-social-media-twitter-active.png');
    }).mouseout(function () {
        $(this).attr('src', 'image/footer/icon-social-media-twitter.png');
    });
    $("#global-footer-follow-social-media-youtube").mouseenter(function () {
        $(this).attr('src', 'image/footer/icon-social-media-youtube-active.png');
    }).mouseout(function () {
        $(this).attr('src', 'image/footer/icon-social-media-youtube.png');
    });
    $("#global-footer-follow-social-media-linkedin").mouseenter(function () {
        $(this).attr('src', 'image/footer/icon-social-media-linkedin-active.png');
    }).mouseout(function () {
        $(this).attr('src', 'image/footer/icon-social-media-linkedin.png');
    });

    //GLOBAL FOOTER FIXED BACKGROUND
    $("#global-footer-fixed-background").css({ 'bottom': '0px !important' });
    $(window).scroll(function () {
        if ($(window).scrollTop() > 100) {
            $("#global-footer-fixed-background").css({ 'margin': '0px' });
        }
        else {
            $("#global-footer-fixed-background").css({ 'margin': '3000px' });
        }
    });

});







