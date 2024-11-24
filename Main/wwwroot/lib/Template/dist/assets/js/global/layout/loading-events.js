
let logoutUrl = null;
var noLoader = false;
var $loadingElement = $('.inner-controllerbody');

function setLoading(value) {

    if (!value) {
        //$('.loading.ajax-event').toggleClass('loading ajax-event', value);
        //swal.close();
    }
    else if ($loadingElement) {
        //$loadingElement.toggleClass('loading ajax-event', value);
        //swal.fire({
        //    title: 'Aguarde',
        //    text: 'A Carregar...',
        //    timer: 3000,
        //    onOpen: function () {
        //        swal.showLoading();
        //    }
        //});
    }
}

// Coloca a janela em estado loading quando "window.location" tem alterações: *@
window.onbeforeunload = function () {

    let $activeElement = $(event.target.activeElement);
    let ignoreOnbeforeunload =
        $activeElement &&
        ($activeElement.is("a[href^='mailto:']") || $activeElement.is("a[href^='tel:']"));

    if (!ignoreOnbeforeunload) setLoading(true);
    return null;
}

$(window).bind('hashchange', function () {
    // console.log('Layout :: window.bind(haschage) :: popstate');
    $loadingElement.html('');
    $loadingElement.css('background-color', 'gray');
});


$(document).ajaxSend(function (event, request, settings) {
    setLoading(true);
});

$(document).ajaxComplete(function (event, request, settings) {
    setLoading(false);
});

$(document).ajaxError(function (event, request, settings, thrownError) {
    if (thrownError !== "abort") {

        console.error('Ajax Error status code: ' + settings.statusCode);

        if (settings.statusCode == 401)
            window.location.assign(logoutUrl);

        console.log(event);
        console.log(request);
        console.log(settings);
        console.log(thrownError);

        $("#ajaxErrorMessageTitle").text(thrownError);
        $("#ajaxErrorMessageDescription")
            .html(request.responseText
                // Remove o elemento <style> da resposta: *@
                .replace(/<style.*?<\/style>/g, '')
            );
        $("#ajaxErrorMessage").show();
    }
});
