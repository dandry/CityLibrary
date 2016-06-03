// change chevron buttons on pages with expandable lists (which use _BookList partialview)
$('.collapse-toggle').click(function () {
    var sender = $(this);
    sender.find('span').toggleClass('glyphicon-chevron-up glyphicon-chevron-down')
});

$(document).ready(function () {

    var locationPath = this.location.pathname.split("/")[1];

    $('#navbar-links').children('li').children('a[href="/' + locationPath + '"]').parent().addClass('active'); 
});

// a confirmation popup for returning/prolonging a book
var confirmAction = function () {
    return confirm('Czy chcesz wykonać daną akcję?');
};

// a confirmation popup for deleting a book
var confirmDelete = function () {
    var confirmation = prompt('Czy na pewno chcesz usunąć wybraną pozycję? \n\nWpisz TAK aby potwierdzić.');

    if (confirmation == 'TAK') {
        return true;
    }
    
    return false;
};

// ajax call to load authors based on searchbox
$('#addCopySearchBtn').click(function () {

    var searchString = $('#addCopySearchBox').val();

    $.get('/Books/AddCopy_LoadAuthors?searchString=' + searchString, function (data) {

        $('#authorsPlaceHolder').html(data);
        $('#authorsPlaceHolder').slideDown();

    });
});

// ajax call to load books based on author dropdownlist
$('#authorsPlaceHolder').on('change', '#AuthorsDropDown', function () {

    var authorName = $(this).val();

    $.get('/Books/AddCopy_LoadAuthorBooks?AuthorName=' + authorName, function (data) {

        $('#authorBooksPlaceHolder').html(data);
        $('#authorBooksPlaceHolder').slideDown();
    });
});

// ajax call to load book details based on book dropdownlist
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
        // if the server returned a JSON object containing an URL
        // property, redirection to that URL
        window.location.href = result.url;
    }
};

var animateContentLoad = function (element) {

    $(element).slideDown("slow");

    // reset autocomplete form submit
    $('form').children('#autocompletesource').val(false);
};

var animateListContentLoad = function (element) {

    $(element).effect("highlight");
    $('form').children('#autocompletesource').val(false);
};

// autocomplete

// this function autosubmits the search form when suggested option is clicked
var submitAutocompleteForm = function (event, ui) {

    var $input = $(this);
    $input.val(ui.item.value);

    var $form = $input.parents("form:first");

    $form.children('#autocompletesource').val(true);
    $form.submit();
};


var createAutoComplete = function () {

    var $input = $(this);

    var options = {
        source: $input.attr("data-source-autocomplete"),
        select: submitAutocompleteForm,
    };

    $input.autocomplete(options);
};
$("input[data-source-autocomplete]").each(createAutoComplete);

// show tooltip
$(document).ready(function () {
    $('[data-tooltip="tooltip"]').tooltip();
});

// Modal
$("#myModal").on("show.bs.modal", function (e) {
    var link = $(e.relatedTarget);
    $(this).find(".modal-content").load(link.attr("href"), function () {
        // create autocomplete on ajax load into Modal
        $("input[data-source-autocomplete]").each(createAutoComplete);
    });
});


// pagedlist navigation
var getPage = function () {
    var $a = $(this);

    var options = {
        url: $a.attr("href"),
        data: $("form").serialize(),
        type: "get"
    };

    $.ajax(options).done(function (data) {
        var target = $a.parents("div.pagedList").attr("data-pl-target");
        $newHtml = $(data);
        $(target).replaceWith($newHtml);
        $newHtml.effect("highlight");
    });
    return false;
};

$(".body-content").on("click", ".pagedList a", getPage);