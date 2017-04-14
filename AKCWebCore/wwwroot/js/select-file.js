//JS-AJAX alternative for file "upload".
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
            var st;
            st = sessionStorage;
            st.setItem("AKCContent", this.result);

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
        }

        //Testing local storage
        try {
            var storTest = window['sessionStorage'];
            storTest.setItem("", ".");
            storTest.removeItem("");
        } catch (e) {
            alert("Failed to save content on local storage.");
        }

        //Getting parse content and preview
        reader.readAsText(selectedFile);

    } else {
        fileDisplayArea.innerHTML = "File not supported!"
    }
}