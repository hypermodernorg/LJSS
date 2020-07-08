// Translation function
function getTranslation() {
    var englishText = $('#English').val();
    englishText = englishText.replace(/[\n\r]+/g, '');
    $.ajax({
        type: 'POST',
        url: '/Translate/UString',
        data: { estring: englishText },
        success: function (result) {
            if (document.getElementById("removereplace")) {
                document.getElementById("removereplace").remove();
            }
       
            $('#Japanese').html(result);
            var audio = document.createElement("audio");
            audio.controls = true;
            audio.src = '/assets/sounds/translations/' + decodeURIComponent(result) + '.mp3';
            audio.id = 'removereplace';
            
            document.getElementById("audiohere").appendChild(audio);
        }
    });
}

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

// Vocabulary functions.
function getVocabularySound(clicked_id) {
    $.ajax({
        type: 'POST',
        url: '/Vocabulary/VocabularySounds',
        data: { vocabularystring: clicked_id },
        success: function (result) {
            document.getElementById(clicked_id).setAttribute("href", result);
            document.getElementById(clicked_id).setAttribute("onclick", "");
        }
    });
}

// Sort function for tables
function sortTable(n) {
    var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;
    table = document.getElementById("kanatable");
    switching = true;
    // Set the sorting direction to ascending:
    dir = "asc";
    /* Make a loop that will continue until
    no switching has been done: */
    while (switching) {
        // Start by saying: no switching is done:
        switching = false;
        rows = table.rows;
        /* Loop through all table rows (except the
        first, which contains table headers): */
        for (i = 1; i < (rows.length - 1); i++) {
            // Start by saying there should be no switching:
            shouldSwitch = false;
            /* Get the two elements you want to compare,
            one from current row and one from the next: */
            x = rows[i].getElementsByTagName("TD")[n];
            y = rows[i + 1].getElementsByTagName("TD")[n];
            /* Check if the two rows should switch place,
            based on the direction, asc or desc: */
            if (dir == "asc") {
                if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
                    // If so, mark as a switch and break the loop:
                    shouldSwitch = true;
                    break;
                }
            } else if (dir == "desc") {
                if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
                    // If so, mark as a switch and break the loop:
                    shouldSwitch = true;
                    break;
                }
            }
        }
        if (shouldSwitch) {
            /* If a switch has been marked, make the switch
            and mark that a switch has been done: */
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
            // Each time a switch is done, increase this count by 1:
            switchcount++;
        } else {
            /* If no switching has been done AND the direction is "asc",
            set the direction to "desc" and run the while loop again. */
            if (switchcount == 0 && dir == "asc") {
                dir = "desc";
                switching = true;
            }
        }
    }
}





