// getQuiz functions
function getQuiz(cnt) {
    var numQuestions = 2;

    if (cnt < numQuestions) {

        // check answers, bit not if the count is zero as zero indicates the first pass and the first pass 
        // of this script, and the first past should have no answers.
        if (cnt != 0) {
            var goodjob = false;

            // go through each radio element and determine if the checked element is the correct answer.
            for (var j = 0; j < 4; j++) {

                if (document.getElementById("optionsRadios" + j).checked == true && document.getElementById("optionsRadios" + j).value == "good") {
                    var goodjob = true;
                }
            }

            // If the correct answer was selected, increase the count of the hidden input's value. Also, do a little animation of the button
            // to indicate if the answer was correct or not.  
            if (goodjob == true) {
                var orig = document.getElementById("submitQuiz").style.backgroundColor;
                document.getElementById("submitQuiz").style.backgroundColor = "#00FF00";
                setTimeout(function () {
                    document.getElementById("submitQuiz").style.backgroundColor = orig;
                }, 1000);
                document.getElementById("numberCorrect").value = parseInt(document.getElementById("numberCorrect").value) + 1;
            }
            else {
                var orig = document.getElementById("submitQuiz").style.backgroundColor;
                document.getElementById("submitQuiz").style.backgroundColor = "#FF0000";
                setTimeout(function () {
                    document.getElementById("submitQuiz").style.backgroundColor = orig;
                }, 1000);
              
            }
        }
        // end check the answere

        var btncnt = document.getElementById("submitQuiz");
        btncnt.setAttribute("onclick", "getQuiz(" + (cnt + 1) + ")"); // determines how many questions have been answered.

        $.ajax({
            type: 'POST',
            url: '/Kana/GetQuizItem',

            // The data to pass into the function.
            //data: { estring: englishText },
            success: function (result) {

                var fieldset = document.getElementById("fg1");
                fieldset.innerHTML = "";

                if (document.getElementById("kqlegend") != null) {
                    document.getElementById("kqlegend").remove;
                }

                //legend not working
                var legend = document.createElement("LEGEND");
                legend.innerHTML = "Which Romaji Corresponds to the Above Kana";
                fieldset.appendChild(legend);

                var kanaquizdisplay = document.getElementById("kanaquizdisplay");
                kanaquizdisplay.innerHTML = result[0];

                let opts = result;
                let good = result[1];

                opts.splice(0, 1); // remove first item
                var rand = Math.floor(Math.random() * opts.length);

                for (var i = 0; i < 4; i++) {
                    rand = Math.floor(Math.random() * opts.length);
                    var opt = document.createElement("div");
                    opt.classList.add("form-check");

                    var inp = document.createElement("input");
                    inp.setAttribute("type", "radio");
                    inp.setAttribute("name", 'optionsRadios');
                    inp.setAttribute("id", 'optionsRadios' + i);
                    inp.classList.add("form-check-input");

                    if (opts[rand] == good) {

                        inp.setAttribute("value", "good");
                    }

                    var lbl = document.createElement("LABEL");
                    lbl.setAttribute("class", "form-check-label");
                    lbl.innerHTML = i + ' ' + rand + ' ' + opts[rand] + ' | result | ' + good;

                    opt.appendChild(inp);
                    opt.appendChild(lbl);
                    fieldset.appendChild(opt);

                    opts.splice(rand, 1);
                }
            }
        });
    }

    // if the number of questions are exhuasted, do the below.
    else {
        var fieldset = document.getElementById("fg1");
        var kanaquizdisplay = document.getElementById("kanaquizdisplay");
        var numberCorrect = document.getElementById("numberCorrect").value;
        fieldset.innerHTML = "";
        kanaquizdisplay.innerHTML = "You got " + numberCorrect + " correct out of a total of " + numQuestions + " For a score of " + (numberCorrect/numQuestions) * 100;


    }
}



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





