


$(document).ready(function (){
    $('#English').on('input', function () {
        var englishText = $('#English').val();
        //document.getElementById('Japanese').textContent = englishText;
        // The above works, but it was only to test if I could update the Japanese textarea,
        // now lets pass englishText to a script to query the db.
       //$.post("lib/translate.php"), {englishText} });
        //var t = new XMLHttpRequest();
        //t.open("GET", "/Translate/UString");
        //t.send();

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


