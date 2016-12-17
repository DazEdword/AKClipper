using System.Globalization;
using System.Text;

namespace AKCCore {
    /// <summary>
    /// Global options belong here.
    /// </summary>

    public static class OptionsDeprecate {
        private static string language = null;
        private static CultureInfo currentCulture;
        private static CultureInfo engCulture;
        private static CultureInfo spaCulture;
        private static string textToParsePath;
        private static Encoding fileEncoding;
        private static bool filtersActive;
        private static FormatType formatInUse;

        static OptionsDeprecate() {
            language = null;
            engCulture = new CultureInfo("en-GB");
            spaCulture = new CultureInfo("es-ES");
            currentCulture = engCulture;
            fileEncoding = Encoding.UTF8;
            filtersActive = true;
            formatInUse = null;
        }

        public static void SetCulture() {

        }

        public static string Language {
            get {
                return language;
            }
            set {
                language = value;
            }
        }

        public static CultureInfo CurrentCulture {
            get {
                return currentCulture;
            }
            set {
                currentCulture = value;
            }
        }

        public static CultureInfo SpaCulture {
            get {
                return spaCulture;
            }
            set {
                SpaCulture = value;
            }
        }

        public static CultureInfo EngCulture {
            get {
                return engCulture;
            }
            set {
                EngCulture = value;
            }
        }

        public static string TextToParsePath {
            get {
                return textToParsePath;
            }
            set {
                textToParsePath = value;
            }
        }

        public static Encoding FileEncoding {
            get {
                return fileEncoding;
            }
            set {
                fileEncoding = value;
            }
        }

        public static bool FiltersActive {
            get {
                return filtersActive;
            }
            set {
                filtersActive = value;
            }
        }

        public static FormatType FormatInUse {
            get {
                return formatInUse;
            }
            set {
                formatInUse = value;
            }
        }
    }
}