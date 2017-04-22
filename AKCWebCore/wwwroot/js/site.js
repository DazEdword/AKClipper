// Write your Javascript code.

function highlightSelectedNavItem() {
    $('#nav span a').click(function () {

        //'unselect' all the rest

        $('#nav span a').each(function (index, element) {
            $(this).removeClass("current-page-item"); // set all links' style to unselected first
        })

        // $(this) is now the link user has clicked on
        $(this).addClass('current-page-item'); // class you want to use for the link user clicked
    });
}