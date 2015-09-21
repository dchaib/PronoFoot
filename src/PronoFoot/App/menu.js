$(document).ready(function () {
    if (matchMedia) {
        var mq = window.matchMedia('(min-width: 768px)');
        mq.addListener(WidthChange);
        WidthChange(mq);
    }

    function WidthChange(mq) {
        if (mq.matches) {
            $subNav = $('ul.navbar-nav > li.active.dropdown > .dropdown-menu').clone();
            if ($subNav.length > 0) {
                $subNav.removeClass('dropdown-menu')
                    .addClass('nav navbar-nav');
                $('#secondaryNav > .container-fluid').html($subNav.get(0).outerHTML);
                $('#secondaryNav').show();
            }
            else {
                $('#secondaryNav').hide();
            }
            // Restore "clickable parent links" in navbar
            $('ul.navbar-nav > li.dropdown > a').on('click', handleClick);
        } else {
            $('ul.navbar-nav > li.dropdown > a').off('click', handleClick);
        }
    };

    function handleClick() {
        window.location = this.href;
    }
});