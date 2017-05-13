using System;
using System.Globalization;
using System.Linq;

namespace AKCCore {

    public class MyClippingsParserSPA : MyClippingsParser {
        public string[] typeEdPageKeys;
        public string[] typeEdLocationKeys;
        public string[] typeRubPageKeys;
        public string[] typeRubLocationKeys;
        public FormatType typeEd;
        public FormatType typeRub;
        public FormatType[] spaFormats;
        private CultureInfo spaCulture;
        private string[] dateFormats;

        //Singleton instantiation.
        private static readonly MyClippingsParserSPA myParserSPA = new MyClippingsParserSPA();

        /* Following three methods: simple, thread safe singleton implementation*/

        // Note the two different initialisation methods. 
        // We are using it to initialise the parser, but even if there's nothing to initialise
        // the init method needs to exist if we want this singleton pattern to work properly 

        private MyClippingsParserSPA() {
            spaCulture = new CultureInfo("es-ES");
            InitDefaults();
            InitFormats();
        }

        // Explicit static constructor to tell C# compiler not to mark type as 'beforefieldinit'
        static MyClippingsParserSPA() {

        }

        public static MyClippingsParserSPA MyParserSPA {
            get {
                return myParserSPA;
            }
        }

        protected override void InitFormats(){
            //Manually instancing an array of keys per type to be added to struct constructor. Modifyable.
            typeEdPageKeys = new string[] { " en la página " };
            typeEdLocationKeys = new string[] { " Posición ", " posición ", " Location " };

            typeRubPageKeys = new string[] { " en la página " };
            typeRubLocationKeys = new string[] { " Pos. " };

            typeEd = new FormatType("typeEd", typeEdPageKeys, typeEdLocationKeys, 2,
                new FormatType.KeyPositionLang[] {
                    new FormatType.KeyPositionLang("Mi", 2, "Spanish"),
                    new FormatType.KeyPositionLang("Tu", 2, "Spanish")
                }, 8, 4, 6, 6, 9, 13);

            typeRub = new FormatType("typeRub", typeRubPageKeys, typeRubLocationKeys, 1,
                new FormatType.KeyPositionLang[] {
                    new FormatType.KeyPositionLang("-", 1, "Spanish"),
                }, 8, 3, 5, 5, 8, 13);

            spaFormats = new FormatType[] { typeEd, typeRub };

            //Formatting help: https://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx
            dateFormats = new string[] {
                "dddd d MMMM yyyy, HH:mm:ss",
                "dddd dd MMMM yyyy, HH:mm:ss",
                "dddd dd MMMM yyyy, hh:mm:ss",
                "dddd d MMMM yyyy, hh:mm:ss",
                "dddd d MMMM yyyy H'H'mm",
                "dddd dd MMMM yyyy",
                "dddd, dd MMMM yyyy HH:mm:ss",
                "dddd, d MMMM yyyy HH:mm:ss",
                "dddd, d MMMM yyyy H:mm:ss",
                "dddd dd MMMM yyyy, H:mm:ss",
                "dddd dd MMMM yyyy, h:mm:ss",
                "dddd d MMMM yyyy, h:mm:ss" };
            }

        protected override void InitDefaults() {
            Defaults = new Clipping();
            Defaults.BookName = "Título desconocido";
            Defaults.Author = "Autor desconocido";
            Defaults.Text = "";
            Defaults.Location = "";
            Defaults.Page = "";
            Defaults.DateAdded = new DateTime();
        }

        protected override void ParseLine2(string line, Clipping clipping, FormatType format) {
            var split = line.Split(' ');
            var fileType = "";

            bool hasPageNumber = false;
            bool hasLocation = false;

            //Detect type of file.
            try {
                if (!String.IsNullOrEmpty(format.ID)) {
                    fileType = format.ID;
                }
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message, "Can't identify TXT format.");
            }

            string clippingType = split[format.clippingTypePosition];

            SetClippingType(clippingType, clipping);

            hasPageNumber = format.pageWording.Any(line.Contains);
            hasLocation = format.locationWording.Any(line.Contains);

            var dateIndex = format.dateIndex;
            var locationIndex = format.locationIndex;
            var pageIndex = format.pageIndex;
            var hasPageDateIndex = format.hasPageDateIndex;
            var hasPageHasLocationDateIndex = format.hasPageHasLocationDateIndex;
            var hasPageLocationIndex = format.hasPageLocationIndex;

            bool isSubtypeKyuni = false;

            if (split[1] == "Tu") {
                isSubtypeKyuni = true;
            }

            if (isSubtypeKyuni) {
                dateIndex = 10;
                locationIndex = 6;
                hasPageDateIndex = 10;
            }

            try {
                if (hasPageNumber) {
                    var pageNumber = split[pageIndex];
                    clipping.Page = pageNumber;

                    locationIndex = hasPageLocationIndex;
                    dateIndex = hasLocation ? hasPageHasLocationDateIndex : hasPageDateIndex;
                }
            }
            catch (Exception) {
                clipping.Page = Defaults.Page;
            }

            try {
                if (hasLocation) {
                    var location = split[locationIndex];
                    clipping.Location = location;
                }
            }
            catch (Exception) {
                clipping.Location = Defaults.Location;
            }

            ParseDateExact(split, clipping, dateIndex, dateFormats, spaCulture);
        }

        private void SetClippingType(string clippingType, Clipping clipping) {
            switch (clippingType.ToLower()) {
                case "subrayado":
                    clipping.ClippingType = ClippingTypeEnum.Subrayado;
                    break;

                case "nota":
                    clipping.ClippingType = ClippingTypeEnum.Notas;
                    break;

                case "marcador":
                    clipping.ClippingType = ClippingTypeEnum.Marcador;
                    break;

                default:
                    clipping.ClippingType = ClippingTypeEnum.NoReconocido;
                    break;
            }
        }
    }
}