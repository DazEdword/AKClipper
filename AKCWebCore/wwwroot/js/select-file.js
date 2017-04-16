//TODO 1) Update server with data from AJAX 2) Disable start parse button until update has occured
//JS-AJAX alternative for file "upload".
//http://stackoverflow.com/questions/4622499/disable-button-while-ajax-request
//https://en.wikipedia.org/wiki/Ajax_(programming)

window.onload = function () {
    //TODO We are not using JQuery where we could, fix that. 
    //Selector logic. 
    document.getElementById("languageSelector").onchange = function() {
        updateServerContent(null, this.value);
    };

    //Start parsing logic.
    document.getElementById('startParsingButton').onclick = function() {
        //document.location = '@Url.Action("Parse","Home")';
        $.ajax({
            url: 'http://localhost:60362/home/parse',
            type: "get",
            success: function (response) {
                var component = document.getElementById('akc-container').innerHTML = response;
            },
            error: function (dataerror) {
                alert("Failed to parse. Please refresh your browser and try again.");
            }
        });
    };
};

var CONTENT_ID_NAME = Object.freeze("AKCContent");

function SelectFile() {
    var selectedFile = document.getElementById('file-select').files[0];
    var fileDisplayArea = document.getElementById("file-preview");
    fileDisplayArea.innerHTML = "";
    var textType = /text.*/;

    if (selectedFile.type.match(textType)) {
        //disableParsingButton();
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

            //AJAX call here
            updateServerContent();
        };

        //Getting parse content and preview
        reader.readAsText(selectedFile);   

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

function updateServerContent(content, language) {
    //TODO at the moment selecting language also re-sends content, that has to change.
    if (!content) {
        content = window['sessionStorage'].getItem(CONTENT_ID_NAME);
    }

    if (!language) {
        language = document.getElementById("languageSelector").value;
    }
   
    disableParsingButton();

    //TODO localhost has to change in production
    $.ajax({
        url: 'http://localhost:60362/home/updatecontent',
        type: "post",
        data: {
            content: content,
            language: language
        },
        success: function () {
            enableParsingButton();
        },
        error: function (dataerror) {
            alert("Failed to send content. Please refresh your browser and try again.");
        }
    });
}