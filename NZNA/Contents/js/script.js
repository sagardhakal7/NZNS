(function($) {

    //"use strict";

    var mainwindow = $(window);

    // rev-slider
    if (jQuery("#slider1").length) {
        jQuery("#slider1").revolution({
            sliderType: "standard",
            sliderLayout: "fullwidth",
            delay: 5000,
            navigation: {
                keyboardNavigation: "on",
                keyboard_direction: "horizontal",
                mouseScrollNavigation: "off",
                onHoverStop: "on",
                arrows: {
                    style: 'zeus',
                    tmp: '<div class="tp-title-wrap"><div class="tp-arr-imgholder"></div></div>',
                    enable: true,
                    rtl: false,
                    hide_onmobile: false,
                    hide_onleave: false,
                    hide_delay: 200,
                    hide_delay_mobile: 1200,
                    hide_under: 0,
                    hide_over: 9999,
                    tmp: ''
                }
            },
            parallax: {
                type: "scroll",
                origo: "slidercenter",
                speed: 1000,
                levels: [5, 10, 15, 20, 25, 30, 35, 40, 45, 46, 47, 48, 49, 50, 100, 55],
                type: "scroll",
            },
            gridwidth: 1170,
            gridheight: 580
        });
    };

    //Hide Loading Box (Preloader)
    function stylePreloader() {
        if ($('.preloader').length) {
            $('.preloader').delay(200).fadeOut(500);
        }
    }

    //Update header style + Scroll to Top
    function headerStyle() {
        if ($('.site-header').length) {
            var windowpos = mainwindow.scrollTop();
            if (windowpos >= 250) {
                $('.site-header').addClass('fixed-header');
                $('.scroll-to-top').fadeIn(300);
            } else {
                $('.site-header').removeClass('fixed-header');
                $('.scroll-to-top').fadeOut(300);
            }
        }
    }

    //Submenu Dropdown Toggle
    if ($('.site-header li.dropdown ul').length) {
        $('.site-header li.dropdown').append('<div class="dropdown-btn"><span class="fa fa-angle-down"></span></div>');

        //Dropdown Button
        $('.site-header li.dropdown .dropdown-btn').on('click', function() {
            $(this).prev('ul').slideToggle(500);
        });

        //Disable dropdown parent link
        $('.navigation li.dropdown > a').on('click', function(e) {
            e.preventDefault();
        });
    }

    //show hide search box

    $('.bz_search_bar').on("click", function(e) {
        $('.bz_search_box').slideToggle();
        e.stopPropagation();
    });

    $(document).on("click", function(e) {
        if (!(e.target.closest('.bz_search_box'))) {
            $(".bz_search_box").slideUp();
        }
    });

    //Mixitup Gallery
    if ($('.filter-list').length) {
        $('.filter-list').mixItUp({});
    }

    //Accordion Box
    if ($('.accordion-box').length) {
        $(".accordion-box").on('click', '.acc-btn', function() {

            var outerBox = $(this).parents('.accordion-box');
            var target = $(this).parents('.accordion');

            if ($(this).hasClass('active') !== true) {
                $('.accordion .acc-btn').removeClass('active');

            }

            if ($(this).next('.acc-content').is(':visible')) {
                return false;
            } else {
                $(this).addClass('active');
                $(outerBox).children('.accordion').removeClass('active-block');
                $(outerBox).children('.accordion').children('.acc-content').slideUp(300);
                target.addClass('active-block');
                $(this).next('.acc-content').slideDown(300);
            }
        });
    }

    if ($('#accordion > .panel').length) {
        $('#accordion > .panel').on('show.bs.collapse', function(e) {
            var heading = $(this).find('.panel-heading');
            heading.addClass("active-panel");

        });
        $('#accordion > .panel').on('hidden.bs.collapse', function(e) {
            var heading = $(this).find('.panel-heading');
            heading.removeClass("active-panel");
        });
    }

    //Sponsors Slider
    if ($('.sowbox').length) {
        $('.sowbox').owlCarousel({
            loop: true,
            margin: 30,
            nav: true,
            smartSpeed: 500,
            autoplay: 4000,
            items: 1,
            dots: false,
            navText: ['<span class="fa fa-angle-left"></span>', '<span class="fa fa-angle-right"></span>'],
            responsive: {
                0: {
                    items: 1
                },
                600: {
                    items: 3
                },
                800: {
                    items: 3
                },
                1024: {
                    items: 4
                },
                1200: {
                    items: 4
                }
            }
        });
    }

    //Testimonial Slider
    if ($('.testmonrow').length) {
        $('.testmonrow').owlCarousel({
            loop: true,
            nav: false,
            dots: false,
            items: 1,
            smartSpeed: 500,
            autoplay: 2000,
            navText: ['<span class="fa fa-angle-left"></span>', '<span class="fa fa-angle-right"></span>'],
            responsive: {
                0: {
                    items: 1
                },
                600: {
                    items: 1
                },
                800: {
                    items: 2
                },
                1024: {
                    items: 2
                },
                1200: {
                    items: 2
                }
            }
        });
    }

    //Sponsors Slider
    if ($('.partener-slider').length) {
        $('.partener-slider').owlCarousel({
            loop: true,
            nav: false,
            smartSpeed: 500,
            autoplay: 2000,
            navText: ['<span class="fa fa-angle-left"></span>', '<span class="fa fa-angle-right"></span>'],
            responsive: {
                0: {
                    items: 2
                },
                600: {
                    items: 3
                },
                800: {
                    items: 4
                },
                1024: {
                    items: 5
                },
                1200: {
                    items: 6
                }
            }
        });
    }

    //Gallery Carousel Slider
    if ($('.carousel-outer').length) {
        $('.carousel-outer').owlCarousel({
            loop: true,
            margin: 0,
            nav: true,
            autoplayHoverPause: false,
            autoplay: true,
            smartSpeed: 700,
            navText: ['<span class="fa fa-angle-left"></span>', '<span class="fa fa-angle-right"></span>'],
            responsive: {
                0: {
                    items: 1
                },
                600: {
                    items: 1
                },
                760: {
                    items: 2
                },
                1024: {
                    items: 3
                },
                1100: {
                    items: 4
                }
            }
        });
    }

    //Testimonial Carousel Slider
    if ($('.testm-wrp').length) {
        $('.testm-wrp').owlCarousel({
            loop: true,
            margin: 30,
            nav: false,
            smartSpeed: 1000,
            autoplay: 4000,
            items: 1,
            navText: ['<span class="fa fa-angle-left"></span>', '<span class="fa fa-angle-right"></span>'],
            responsive: {
                0: {
                    items: 1
                },
                480: {
                    items: 1
                },
                600: {
                    items: 1
                },
                800: {
                    items: 1
                },
                1024: {
                    items: 1
                }
            }
        });
    }

    //sidebar testimonial Slider
    if ($('.sidebar-testimonial-widge').length) {
        $('.sidebar-testimonial-widge').owlCarousel({
            loop: true,
            margin: 0,
            nav: false,
            items: 1,
            smartSpeed: 500,
            autoplay: 4000,
            navText: ['<span class="fa fa-angle-left"></span>', '<span class="fa fa-angle-right"></span>'],
            responsive: {
                0: {
                    items: 1
                },
                480: {
                    items: 1
                },
                600: {
                    items: 1
                },
                800: {
                    items: 1
                },
                1024: {
                    items: 1
                }
            }
        });
    }

    //sidebar contact widget Slider
    if ($('.sidecontact').length) {
        $('.sidecontact').owlCarousel({
            loop: true,
            margin: 0,
            nav: false,
            items: 1,
            smartSpeed: 1000,
            autoplay: 8000,
            navText: ['<span class="fa fa-angle-left"></span>', '<span class="fa fa-angle-right"></span>'],
            responsive: {
                0: {
                    items: 1
                },
                480: {
                    items: 1
                },
                600: {
                    items: 1
                },
                800: {
                    items: 1
                },
                1024: {
                    items: 1
                }
            }
        });
    }

    //related-products-carouse
    if ($('.related-products-carousel2').length) {
        $('.related-products-carousel2').owlCarousel({
            loop: true,
            margin: 30,
            nav: true,
            smartSpeed: 500,
            dots: false,
            autoplay: 4000,
            navText: ['<span class="fa fa-angle-left"></span>', '<span class="fa fa-angle-right"></span>'],
            responsive: {
                0: {
                    items: 1
                },
                480: {
                    items: 1
                },
                600: {
                    items: 2
                },
                800: {
                    items: 2
                },
                1024: {
                    items: 3
                }
            }
        });
    }

    //LightBox / Fancybox
    if ($('.lightbox-image').length) {
        $('.lightbox-image').fancybox({
            openEffect: 'fade',
            closeEffect: 'fade',
            helpers: {
                media: {}
            }
        });
    }

    //Contact Form Validation
    if ($('#contact-form').length) {
        $('#contact-form').validate({
            rules: {
                username: {
                    required: true
                },
                email: {
                    required: true,
                    email: true
                },
                phone: {
                    required: true
                },
                message: {
                    required: true
                }
            }
        });
    }

    // Scroll to a Specific Div
    if ($('.scroll-to-target').length) {
        $(".scroll-to-target").on('click', function() {
            var target = $(this).attr('data-target');
            // animate
            $('html, body').animate({
                scrollTop: $(target).offset().top
            }, 1000);

        });
    }

    //Input Quantity Up & Down
    function quantity_changer() {
        $('#quantity-holder').on('click', '.quantity-plus', function() {
            var $holder = $(this).parents('.quantity-holder');
            var $target = $holder.find('input.quantity-input');
            var $quantity = parseInt($target.val(), 10);
            if ($.isNumeric($quantity) && $quantity > 0) {
                $quantity = $quantity + 1;
                $target.val($quantity);
            } else {
                $target.val($quantity);
            }
        }).on('click', '.quantity-minus', function() {
            var $holder = $(this).parents('.quantity-holder');
            var $target = $holder.find('input.quantity-input');
            var $quantity = parseInt($target.val(), 10);
            if ($.isNumeric($quantity) && $quantity >= 2) {
                $quantity = $quantity - 1;
                $target.val($quantity);
            } else {
                $target.val(1);
            }
        });

    }

    //counter number changer
    function counter_number() {
        var timer = $('.timer');
        if (timer.length) {
            timer.appear(function() {
                timer.countTo();
            })
        }
    }

    /* ==========================================================================
       When document is ready, do
       ========================================================================== */

    $(document).on('ready', function() {
        counter_number();
        quantity_changer();
    });

    /* ==========================================================================
       When document is Scrollig, do
       ========================================================================== */

    mainwindow.on('scroll', function() {
        headerStyle();

    });

    /* ==========================================================================
       When document is loading, do
       ========================================================================== */

    mainwindow.on('load', function() {
        stylePreloader();
    });

    /* ==========================================================================
       When Window is resizing, do
       ========================================================================== */

    mainwindow.on('resize', function() {});

})(window.jQuery);