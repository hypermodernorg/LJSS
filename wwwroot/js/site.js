
$(document).ready(function (){
    $('#English').on('input', function () {
        var englishText = $('#English').val();
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


