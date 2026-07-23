
window.initVuexy = function () {

    if (window.feather) {
        feather.replace();
    }

    if ($.app && $.app.menu && !window.menuInitialized) {
        $.app.menu.init(false);
        window.menuInitialized = true;
    }

    if ($.app && $.app.nav && !window.navInitialized) {
        $.app.nav.init();
        window.navInitialized = true;
    }
}

window.menuFunctions = {

    updateActiveMenu: function (path) {

        $('.navigation li').removeClass('active');

        var activeLink = $('.navigation a[href="' + path + '"]');

        activeLink.parent('li').addClass('active');

        activeLink.parents('.has-sub')
            .addClass('open')
            .children('.menu-content')
            .show();

        if (window.feather) {
            feather.replace();
        }

        // IMPORTANT
        if ($.app && $.app.menu) {
            $.app.menu.update();
        }
    }
};

window.hideBootstrapModal = (modalId) => {
    const modalElement = document.getElementById(modalId);
    if (modalElement) {
        const modalInstance = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
        modalInstance.hide();
    }
};

