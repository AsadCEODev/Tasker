
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


window.initSelect2 = function (element, dotNetHelper, isMultiple) {
    if (!element) return;

    var $element = $(element);

    if ($element.hasClass("select2-hidden-accessible")) {
        $element.select2('destroy');
    }

    if (isMultiple) {
        $element.attr('multiple', 'multiple');
    } else {
        $element.removeAttr('multiple');
    }

    setTimeout(function () {
        $element.select2({
            theme: "bootstrap-5",
            multiple: isMultiple,
            width: '100%',
            dropdownParent: $element.closest('.offcanvas, .modal, body'),
            minimumResultsForSearch: 0,
            closeOnSelect: false // Multi-select mein option select karne par dropdown band na ho taake mazeed select kar sakein
        });
    }, 50);

    $element.off('change.blazor').on('change.blazor', function (e) {
        var val = $element.val();
        dotNetHelper.invokeMethodAsync('OnSelectChanged', val);
    });
};

window.updateSelect2Value = function (element, value) {
    if (!element) return;
    var $element = $(element);
    var currentValue = $element.val();

    if (JSON.stringify(currentValue) !== JSON.stringify(value)) {
        $element.val(value).trigger('change.select2');
    }
};


// window.initFeatherObserver = function () {

//     if (window.featherObserver)
//         return;

//     window.featherObserver = new MutationObserver(function () {
//         if (window.feather) {
//             feather.replace();
//         }
//     });

//     window.featherObserver.observe(document.body, {
//         childList: true,
//         subtree: true
//     });
// }

window.initFeatherObserver = function () {
    if (window.featherObserver) return;

    window.featherObserver = new MutationObserver(function (mutations, observer) {
        // Ensure feather library exists and the replace method is available
        if (window.feather && typeof window.feather.replace === 'function') {
            try {
                window.feather.replace();
            } catch (error) {
                console.warn("Feather replacement failed:", error);
            }
        }
    });

    // Start observing the body for added/modified nodes
    window.featherObserver.observe(document.body, {
        childList: true,
        subtree: true
    });
};