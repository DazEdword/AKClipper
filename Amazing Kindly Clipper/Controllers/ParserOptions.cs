using System.Globalization;
using System.Text;

namespace AKCCore {
    /// <summary>
    /// Global options belong here.
    /// </summary>

    public class ParserOptions {
        private static string language = null;
        private static CultureInfo currentCulture;
        private static CultureInfo engCulture;
        private static CultureInfo spaCulture;
        private static string textToParsePath;
        private static Encoding fileEncoding;
        private static FormatType formatInUse;
        private static MyClippingsParser parserInUse;

        public ParserOptions() {
            language = null;
            engCulture = new CultureInfo("en-GB");
            spaCulture = new CultureInfo("es-ES");
            currentCulture = engCulture;
            fileEncoding = Encoding.UTF8;
            formatInUse = null;
        }

        public void SetCulture() {

        }

        public string Language {
            get {
                return language;
            }
            set {
                language = value;
            }
        }

        public CultureInfo CurrentCulture {
            get {
                return currentCulture;
            }
            set {
                currentCulture = value;
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

        public FormatType FormatInUse {
            get {
                return formatInUse;
            }
            set {
                formatInUse = value;
            }
        }
    }
}