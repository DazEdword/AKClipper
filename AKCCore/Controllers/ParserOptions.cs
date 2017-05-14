using System.Globalization;
using System.Text;

namespace AKCCore {
    /// <summary>
    /// Global options belong here.
    /// </summary>

    public class ParserOptions {
        //TODO perhaps Language/Culture/SthElse should be grouped into an object??
        private string language = null;
        private CultureInfo currentCulture;
        private CultureInfo engCulture;
        private CultureInfo spaCulture;
        private string textToParsePath;
        private Encoding fileEncoding;
        private FormatType formatInUse;
        private ClippingsParser parserInUse;

        public ParserOptions() {
            language = null;
            engCulture = new CultureInfo("en-GB");
            spaCulture = new CultureInfo("es-ES");
            currentCulture = engCulture;
            fileEncoding = Encoding.UTF8;
            formatInUse = null;
            parserInUse = null;
        }

        public string Language {
            get {
                return language;
            }
            set {
                language = value;
            }
        }

        public CultureInfo SpaCulture {
            get {
                return spaCulture;
            }
            set {
                SpaCulture = value;
            }
        }

        public CultureInfo EngCulture {
            get {
                return engCulture;
            }
            set {
                EngCulture = value;
            }
        }

        public string TextToParsePath {
            get {
                return textToParsePath;
            }
            set {
                textToParsePath = value;
            }
        }

        public Encoding FileEncoding {
            get {
                return fileEncoding;
            }
            set {
                fileEncoding = value;
            }
        }

        public ClippingsParser SelectedParser {
            get {
                return parserInUse;
            }
            set {
                parserInUse = value;
            }
        }

        public FormatType SelectedFormat {
            get {
                return formatInUse;
            }
            set {
                formatInUse = value;
            }
        }

        public CultureInfo SelectedCulture {
            get {
                return currentCulture;
            }
            set {
                currentCulture = value;
            }
        }

    }
}