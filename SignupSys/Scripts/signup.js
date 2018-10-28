$(document).ready(function () {
    var editbox = $('#ftbPage');
    if (editbox.size()) { // 编辑模式
        $('form').attr('disabled', true);
    }
    $(':submit').attr('disabled', false);
});