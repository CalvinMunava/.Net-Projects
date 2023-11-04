function printContent(el) {
    var restorepage = $('body').html();
    var printcontent = $('#' + el);
    $('body').empty().html(printcontent);
    window.print();
    $('body').html(restorepage);
}