// Translate functions
$(document).ready(function () {
    $('#English').on('input', function () {
        var englishText = $('#English').val();
        $.ajax({
            type: 'POST',
            url: '/Translate/UString',
            data: {estring: englishText},
            success: function (result) {
                $('#Japanese').html(result);
            }
        });
    });
});



// Kana functions.
function getKanaSound(clicked_id) {
    $.ajax({
        type: 'POST',
        url: '/Kana/KanaSounds',
        data: { kanastring: clicked_id },
        success: function (result) {
            document.getElementById(clicked_id).setAttribute("href", result);
            document.getElementById(clicked_id).setAttribute("onclick", "");
        }
    });
}





