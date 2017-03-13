$(document).ready(function(){

    $("#home-banner-computer").swiperight(function() {
        $("#home-banner-computer").carousel('prev');
    });
    $("#home-banner-computer").swipeleft(function() {
        $("#home-banner-computer").carousel('next');
    });
    $("#home-banner-tablet").swiperight(function() {
        $("#home-banner-tablet").carousel('prev');
    });
    $("#home-banner-tablet").swipeleft(function() {
        $("#home-banner-tablet").carousel('next');
    });
    $("#home-banner-phone").swiperight(function() {
        $("#home-banner-phone").carousel('prev');
    });
    $("#home-banner-phone").swipeleft(function() {
        $("#home-banner-phone").carousel('next');
    });

    //BOOTSTRAP TOOLTIP
    $('[data-toggle="tooltip"]').tooltip();

    //LIST COURSE WISHLIST
    var whislist = 0;
    $(".row-list-course-each-wishlist").click(function(e){
        e.stopPropagation();
        if(whislist == 0){
            $(this).html('<img src="image/course/icon-wishlist-active.png" alt="Wishlist Icon">');
            whislist = 1;
        }else{
            $(this).html('<img src="image/course/icon-wishlist.png" alt="Wishlist Icon">');
            whislist = 0;
        }
    });

    //BOOTSTRAP TOOLTIP DISABLED CLICK
    $(".share-row-list-course-5-only-each-infoicon-each").click(function(e){
        e.stopPropagation();
    });

});
