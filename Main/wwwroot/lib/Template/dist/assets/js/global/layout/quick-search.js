"use strict";

var KTQuickSearch = function () {
    var target;
    var form;
    var input;
    var closeIcon;
    var resultWrapper;
    var resultDropdown;
    var resultDropdownToggle;
    var inputGroup;
    var query = '';

    var hasResult = false;
    var timeout = false;
    var isProcessing = false;
    var requestTimeout = 200; // ajax request fire timeout in milliseconds 
    var spinnerClass = 'kt-spinner kt-spinner--input kt-spinner--sm kt-spinner--brand kt-spinner--right';
    var resultClass = 'kt-quick-search--has-result';
    var minLength = 2;

    var showProgress = function () {
        isProcessing = true;
        KTUtil.addClass(inputGroup, spinnerClass);

        if (closeIcon) {
            KTUtil.hide(closeIcon);
        }
    }

    var hideProgress = function () {
        isProcessing = false;
        KTUtil.removeClass(inputGroup, spinnerClass);

        if (closeIcon) {
            if (input.value.length < minLength) {
                KTUtil.hide(closeIcon);
            } else {
                KTUtil.show(closeIcon, 'flex');
            }
        }
    }

    var showDropdown = function () {
        if (resultDropdownToggle && !KTUtil.hasClass(resultDropdown, 'show')) {
            $(resultDropdownToggle).dropdown('toggle');
            $(resultDropdownToggle).dropdown('update');
        }
    }

    var hideDropdown = function () {
        if (resultDropdownToggle && KTUtil.hasClass(resultDropdown, 'show')) {
            $(resultDropdownToggle).dropdown('toggle');
        }
    }

    var processSearch = function () {
        if (hasResult && query === input.value) {
            hideProgress();
            KTUtil.addClass(target, resultClass);
            showDropdown();
            KTUtil.scrollUpdate(resultWrapper);

            return;
        }

        query = input.value;

        KTUtil.removeClass(target, resultClass);
        showProgress();
        hideDropdown();

        setTimeout(function () {
            $.ajax({
                url: '/Dashboard/Search',
                data: {
                    query: query
                },
                dataType: 'json', // Mudado para json
                success: function (res) {
                    hasResult = true;
                    hideProgress();
                    KTUtil.addClass(target, resultClass);

                    // Construindo HTML a partir do resultado JSON
                    let html = '';
                    if (query.trim() === '') {
                        // Se a query estiver vazia, esvazie os resultados
                        html = 'Pesquise por alguem.';
                    } else if (res.length > 0) {
                        res.forEach(function (user, index) { // Adicionei o parâmetro index
                            // Adiciona a classe mt-3 apenas se o índice for maior que 0
                            const marginClass = index > 0 ? "mt-3" : ""; // Condição para margin
                            html += `
                                <div class="${marginClass}">
                                    <a href="/user/profile/${user.id}" class="kt-widget kt-widget--general-1">
                                        <div class="kt-media kt-media--brand kt-media--md kt-media--circle">
                                            <img src="${user.foto}" alt="Foto de ${user.nome}">
                                        </div>
                                        <div class="kt-widget__wrapper">
                                            <div class="kt-widget__label">
                                                <span class="kt-widget__title">
                                                    ${user.nome}
                                                </span>
                                                <span class="kt-widget__desc">
                                                    ${user.username}
                                                </span>
                                            </div>
                                        </div>
                                    </a>
                                </div>
                            `;
                        });

                    } else {
                        html = '<span class="kt-quick-search__message">Nenhum utilizador encontrado.</span>';
                    }

                    KTUtil.setHTML(resultWrapper, html);
                    showDropdown();
                    KTUtil.scrollUpdate(resultWrapper);
                },
                error: function (res) {
                    hasResult = false;
                    hideProgress();
                    KTUtil.addClass(target, resultClass);
                    KTUtil.setHTML(resultWrapper, '<span class="kt-quick-search__message">Erro ao pesquisar, por favor tente novamente mais tarde.</span>');
                    showDropdown();
                    KTUtil.scrollUpdate(resultWrapper);
                }
            });
        }, 1000);


    }

    var handleCancel = function (e) {
        input.value = '';
        query = '';
        hasResult = false;
        KTUtil.hide(closeIcon);
        KTUtil.removeClass(target, resultClass);
        hideDropdown();
    }

    var handleSearch = function () {
        if (input.value.length < minLength) {
            hideProgress();
            hideDropdown();

            return;
        }

        if (isProcessing == true) {
            return;
        }

        if (timeout) {
            clearTimeout(timeout);
        }

        timeout = setTimeout(function () {
            processSearch();
        }, requestTimeout);
    }

    return {
        init: function (element) {
            // Init
            target = element;
            form = KTUtil.find(target, '.kt-quick-search__form');
            input = KTUtil.find(target, '.kt-quick-search__input');
            closeIcon = KTUtil.find(target, '.kt-quick-search__close');
            resultWrapper = KTUtil.find(target, '.kt-quick-search__wrapper');
            resultDropdown = KTUtil.find(target, '.dropdown-menu');
            resultDropdownToggle = KTUtil.find(target, '[data-toggle="dropdown"]');
            inputGroup = KTUtil.find(target, '.input-group');

            // Attach input keyup handler
            KTUtil.addEvent(input, 'keyup', handleSearch);
            KTUtil.addEvent(input, 'focus', handleSearch);

            // Prevent enter click
            form.onkeypress = function (e) {
                var key = e.charCode || e.keyCode || 0;
                if (key == 13) {
                    e.preventDefault();
                }
            }

            KTUtil.addEvent(closeIcon, 'click', handleCancel);
        }
    };
};

var KTQuickSearchMobile = KTQuickSearch;

// Init on page load completed
KTUtil.ready(function () {
    if (KTUtil.get('kt_quick_search_dropdown')) {
        KTQuickSearch().init(KTUtil.get('kt_quick_search_dropdown'));
    }

    if (KTUtil.get('kt_quick_search_inline')) {
        KTQuickSearchMobile().init(KTUtil.get('kt_quick_search_inline'));
    }

    if (KTUtil.get('kt_quick_search_offcanvas')) {
        KTQuickSearchMobile().init(KTUtil.get('kt_quick_search_offcanvas'));
    }
});