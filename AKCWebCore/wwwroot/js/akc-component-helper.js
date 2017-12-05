var ENVIRONMENT = "DEV";
var LIVE_ENV_URL = "http://akc-gbg.azurewebsites.net";
var DEV_ENV_URL = "http://localhost:60362";
var CONTENT_ID_NAME = Object.freeze("AKCContent");
var body = $body = $("body");

window.onload = function () {
    //TODO We are not using JQuery where we could (get elemns by id), fix that.
    document.getElementById('startParsingButton').onclick = function() {
        sendParseRequest(undefined, undefined);
    };
};

function getEnvironment() {
    $.ajax({
        url: url.concat('/home/parse'),
        data: {
            content: content,
            language: language
        },
        type: "post",
        success: function (response) {
            stop_loading();
            window.location.reload();
            var component = document.getElementById('akc-container').innerHTML = response;
            $('.mvc-grid').mvcgrid();
        },
        error: function (dataerror) {
            stop_loading();
            alert("Failed to parse. Please refresh your browser and try again.");
        }
    });
}

function sendParseRequest(content, language) {
    //TODO at the moment selecting language also re-sends content, that has to change.
    if (!content) {
        content = window['sessionStorage'].getItem(CONTENT_ID_NAME);
    }

    if (!language) {
        language = document.getElementById("languageSelector").value;
    }

    var url = (ENVIRONMENT === "LIVE") ? LIVE_ENV_URL : DEV_ENV_URL;

    show_loading();

    $.ajax({
        url: url.concat('/home/parse'),
        data: {
            content: content,
            language: language
        },
        type: "post",
        success: function (response) {
            stop_loading();
            window.location.reload();
            var component = document.getElementById('akc-container').innerHTML = response;
            $('.mvc-grid').mvcgrid();
        },
        error: function (dataerror) {
            stop_loading();
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

                //Pick first line for language detection. 
                if (line == 1) {
                    language = detect_language_from_sample(lines[line])
                    if (language) {
                        select_detected_language(language)
                    }
                }
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

function show_loading() {
    body.addClass("loading");
}

function stop_loading() {
    body.removeClass("loading");
}

function detect_language_from_sample(sample) {
    list_sample = sample.split(" ")
    if (list_sample.indexOf("Added") != -1) {
        return "English";
    }
        
    if (list_sample.indexOf("Añadido") != -1) {
        return "Spanish";
    }

    return undefined;
}

function select_detected_language(language) {
    document.getElementById("languageSelector").value = language;
}