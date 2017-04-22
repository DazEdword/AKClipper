//TODO 1) Update server with data from AJAX 2) Disable start parse button until update has occured
//JS-AJAX alternative for file "upload".
//http://stackoverflow.com/questions/4622499/disable-button-while-ajax-request
//https://en.wikipedia.org/wiki/Ajax_(programming)

//TODO This file's name must not be "select-file" anymore

var CONTENT_ID_NAME = Object.freeze("AKCContent");

window.onload = function () {
    //TODO We are not using JQuery where we could (get elemns by id), fix that. 
    document.getElementById('startParsingButton').onclick = (function() {
        sendParseRequest(undefined, undefined);
    })

    highlightSelectedNavItem();
};

function sendParseRequest(content, language) {
    //TODO at the moment selecting language also re-sends content, that has to change.
    if (!content) {
        content = window['sessionStorage'].getItem(CONTENT_ID_NAME);
    }

    if (!language) {
        language = document.getElementById("languageSelector").value;
    }

    //TODO localhost has to change in production
    $.ajax({
        url: 'http://localhost:60362/home/parse',
        data: {
            content: content,
            language: language
        },
        type: "post",
        success: function (response) {
            var component = document.getElementById('akc-container').innerHTML = response;
        },
        error: function (dataerror) {
            alert("Failed to parse. Please refresh your browser and try again.");
        }
    });
}

function SelectFile() {
    var selectedFile = document.getElementById('file-select').files[0];
    var fileDisplayArea = document.getElementById("file-preview");
    fileDisplayArea.innerHTML = "";
    var textType = /text.*/;

    if (selectedFile.type.match(textType)) {
        var reader = new FileReader();

        reader.onload = function (e) {
            var lines = this.result.split('\n');

            //Saving in local storage
            try {
                var session = window['sessionStorage'];
                session.setItem("AKCContent", this.result);

            } catch (e) {
                alert("Failed to save content on local storage.");
            }

            //Generating preview
            for (var line = 0; line < 39; line++) {
                if (lines[line] == undefined) break;
                var newLine = document.createElement("span");
                var lineBreak = document.createElement("br");
                newLine.innerHTML = lines[line];
                fileDisplayArea.appendChild(newLine);
                fileDisplayArea.appendChild(lineBreak);
                //fileDisplayArea.innerHTML += (lines[line] + '\n');
            }
        };

        //Getting parse content and preview
        reader.readAsText(selectedFile);
        enableParsingButton();

    } else {
        fileDisplayArea.innerHTML = "File not supported!";
    }
}

function enableParsingButton() {
    document.getElementById("startParsingButton").disabled = false;
}

function disableParsingButton() {
    document.getElementById("startParsingButton").disabled = true;
}

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