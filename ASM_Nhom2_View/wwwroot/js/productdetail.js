$(document).ready(function () {
    var pro_width = 0;
    var pro_height = 0;
    $(".big-img").css("background", "url('" + $(".small-img").attr("src") + "') no-repeat");

    $(".magnifier").mousemove(function (e) {
        if (!pro_width && !pro_height) {
            var img_obj = new Image();
            img_obj.src = $(".small-img").attr("src");
            pro_width = img_obj.width;
            pro_height = img_obj.height;
        } else {
            var img_offset = $(this).offset();
            var mx = e.pageX - img_offset.left;
            var my = e.pageY - img_offset.top;
            if (mx < $(this).width() && my < $(this).height() && mx > 0 && my > 0) {
                $(".big-img").fadeIn(100);
            } else {
                $(".big-img").fadeOut(100);
            }
            if ($(".big-img").is(":visible")) {
                var rx = Math.round(mx / $(".small-img").width() * pro_width - $(".big-img").width() / 2) * -1;
                var ry = Math.round(my / $(".small-img").height() * pro_height - $(".big-img").height() / 2) * -1;
                var bgp = rx + "px " + ry + "px";
                var px = mx - $(".big-img").width() / 2;
                var py = my - $(".big-img").height() / 2;
                $(".big-img").css({ left: px, top: py, backgroundPosition: bgp });
            }
        }
    });

    // Initial highlight of the first thumbnail and update magnifier background
    $(".owl-carousel .item:first").addClass("selected");

    // Update main image and add border to selected thumbnail
    $(".owl-carousel .item").click(function () {
        var imgSrc = $(this).find("img").attr("src");
        $("#main-image").attr("src", imgSrc);
        $(".owl-carousel .item").removeClass("selected");
        $(this).addClass("selected");

        // Update magnifier background
        $(".big-img").css("background", "url('" + imgSrc + "') no-repeat");
    });
});

$(document).ready(function () {
    $('.owl-carousel').owlCarousel({
        loop: false,
        margin: 10,
        nav: true,
        navText: [
            "<i class='fa fa-caret-left'></i>",
            "<i class='fa fa-caret-right'></i>"
        ],
        autoplay: false,
        autoplayHoverPause: false,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 3
            },
            1000: {
                items: 5
            }
        }
    });
});

document.querySelector('#refund').addEventListener('click', function () {
    var myModal = new bootstrap.Modal(document.getElementById('popupModal'));
    myModal.show();
});
$(document).ready(function () {
    $(window).scroll(function () {
        if ($(this).scrollTop() > 50) {
            $('.navbar').removeClass('sticky-top').addClass('fixed-top');
        } else {
            $('.navbar').removeClass('fixed-top').addClass('sticky-top');
        }
    });
});