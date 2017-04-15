//TODO 1) Update server with data from AJAX 2) Disable start parse button until update has occured
//JS-AJAX alternative for file "upload".
//http://stackoverflow.com/questions/4622499/disable-button-while-ajax-request
//https://en.wikipedia.org/wiki/Ajax_(programming)

var CONTENT_ID_NAME = Object.freeze("AKCContent");

function SelectFile() {
    var selectedFile = document.getElementById('file-select').files[0];
    var fileDisplayArea = document.getElementById("file-preview");
    fileDisplayArea.innerHTML = "";
    var textType = /text.*/;

    if (selectedFile.type.match(textType)) {
        disableParsingButton();
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

function updateServerContent() {
    var content = window['sessionStorage'].getItem(CONTENT_ID_NAME);
    
    //TODO localhost has to change in production
    //AJAX stub
    //$.ajax({
    //    url: 'http://localhost:60362/home/testajax',
    //    data: {
    //        content: content
    //    },
    //    success: function (data) {
    //        enableParsingButton();
    //    },
    //    error: function (dataerror) {
    //        alert("WE FAILED MATE");
    //    }
    //});
}