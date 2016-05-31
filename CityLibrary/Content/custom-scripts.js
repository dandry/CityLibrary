$('.collapse-toggle').click(function () {
    var sender = $(this);
    sender.find('span').toggleClass('glyphicon-chevron-up glyphicon-chevron-down')
});

$(document).ready(function () {
    var locationPath = this.location.pathname.split("/")[1];


    $('#navbar-links').children('li').children('a[href="/' + locationPath + '"]').parent().addClass('active');
});

var confirmReturn = function () {
    return confirm('Czy chcesz zwrócić tę książkę?');
};

var confirmDelete = function () {
    var confirmation = prompt('Czy na pewno chcesz usunąć wybraną pozycję? \n\nWpisz TAK aby potwierdzić.');

    if (confirmation == 'TAK') {
        return true;
    }
    
    return false;
};

$('#addCopySearchBtn').click(function () {

    var searchString = $('#addCopySearchBox').val();

    $.get('/Books/AddCopy_LoadAuthors?searchString=' + searchString, function (data) {

        $('#authorsPlaceHolder').html(data);
        $('#authorsPlaceHolder').slideDown();

    });

});

$('#authorsPlaceHolder').on('change', '#AuthorsDropDown', function () {

    var authorName = $(this).val();

    $.get('/Books/AddCopy_LoadAuthorBooks?AuthorName=' + authorName, function (data) {

        /* data is the pure html returned from action method, load it to your page */
        $('#authorBooksPlaceHolder').html(data);
        /* little fade in effect */
        $('#authorBooksPlaceHolder').slideDown();
    });

});

$('#authorBooksPlaceHolder').on('change', '#AuthorBooksDropDown', function () {

    var bookId = $(this).val();

    $.get('/Books/AddCopy_RenderDetails/' + bookId, function (data) {

        $('#bookDetailsPlaceHolder').html(data);
        $('#bookDetailsPlaceHolder').slideDown();
        $.validator.unobtrusive.parse(document);
    });

    
});

var onSuccess = function (result) {
    if (result.url) {
        // if the server returned a JSON object containing an url 
        // property we redirect the browser to that url
        window.location.href = result.url;
    }
};

var animateBookLoad = function () {

    $('#bookDetailsPlaceHolder').slideDown("slow");
};

var submitAutocompleteForm = function (event, ui) {

    var $input = $(this);
    $input.val(ui.item.value);

    var $form = $input.parents("form:first");
    $form.submit();
};

var createAutoComplete = function () {

    var $input = $(this);

    var options = {
        source: $input.attr("data-source-autocomplete"),
        select: submitAutocompleteForm
    };

    $input.autocomplete(options);
};
$("input[data-source-autocomplete]").each(createAutoComplete);

$(document).ready(function () {
    $('[data-tooltip="tooltip"]').tooltip();
});

// Fill modal with content from link href
$("#myModal").on("show.bs.modal", function (e) {
    var link = $(e.relatedTarget);
    $(this).find(".modal-content").load(link.attr("href"));
});