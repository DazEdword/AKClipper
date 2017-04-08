using System;
using System.Linq;

namespace AKCCore {

    public sealed class MyClippingsParserENG : MyClippingsParser {
        public string[] typeBasePageKeys;
        public string[] typeBaseLocationKeys;
        public string[] typeRickPageKeys;
        public string[] typeRickLocationKeys;
        public FormatType typeBase;
        public FormatType typeRick;
        public FormatType[] engFormats;

        //Singleton instantiation.
        public static readonly MyClippingsParserENG myParserENG = new MyClippingsParserENG();

        /* First three methods below: simple, thread safe singleton implementation. */
        private MyClippingsParserENG() {
            InitDefaults();
            InitFormats();
        }

        public static MyClippingsParserENG MyParserENG {
            get {
                return myParserENG;
            }
        }

        // Explicit static constructor to tell C# compiler not to mark type as 'beforefieldinit'
        static MyClippingsParserENG() {
        }

        protected override void InitFormats() {
            //TODO If we keep using the FormatType approach to parsing, we have to create proper constructors for this.
            //But perhaps we should move towards regexp and keep a really small database with the languages.

            //Manually instancing an array of keys per type to be added to struct constructor. Modifyable.
            typeBasePageKeys = new string[] { " on Page ", " on page " };
            typeBaseLocationKeys = new string[] { " Loc. ", " Location " };

            typeRickPageKeys = new string[] { " on page " };
            typeRickLocationKeys = new string[] { " on Location ", " Location " };

            typeBase = new FormatType("typeBase", typeBasePageKeys, typeBaseLocationKeys, 1,
                new FormatType.KeyPositionLang[] {
                    new FormatType.KeyPositionLang("-", 1, "English")
                }, 8, 3, 4, 7, 9, 12);

            typeRick = new FormatType("typeRick", typeRickPageKeys, typeRickLocationKeys, 2,
                new FormatType.KeyPositionLang[] {
                    new FormatType.KeyPositionLang("Your", 2, "English"),
                    new FormatType.KeyPositionLang("Clip", 2, "English")
                }, 9, 5, 5, 9, 8, 12);

            //Gathering all formats in an array, easy foreach iteration.
            engFormats = new FormatType[] {typeBase, typeRick};
        }

        protected override void InitDefaults() {
            Defaults = new Clipping();
            Defaults.BookName = "Unknown book";
            Defaults.Author = "Unknown author";
            Defaults.Location = "";
            Defaults.Text = "";
            Defaults.Page = "";
            Defaults.DateAdded = new DateTime();
        }

        protected override void ParseLine2(string line, Clipping clipping, FormatType format) {
            var split = line.Split(' ');
            string fileType = null;

            bool hasPageNumber = false;
            bool hasLocation = false;
            bool hasInstapaper = false;

            try {
                if (!String.IsNullOrEmpty(format.ID)) {
                    fileType = format.ID;
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message, "Can't identify TXT format.");
            }

            var clippingType = split[format.clippingTypePosition];

            switch (clippingType.ToLower()) {
                case "highlight":
                    clipping.ClippingType = ClippingTypeEnum.Highlight;
                    break;

                case "note":
                    clipping.ClippingType = ClippingTypeEnum.Note;
                    break;

                case "bookmark":
                    clipping.ClippingType = ClippingTypeEnum.Bookmark;
                    break;

                default:
                    clipping.ClippingType = ClippingTypeEnum.NotRecognized;
                    break;
            }

            //Check if line contains any of the critical strings stored in keywords arrays.

            hasPageNumber = format.pageWording.Any(line.Contains);
            hasLocation = format.locationWording.Any(line.Contains);

            /*Indexes are different in Spanish and English version (answers "where to cut" for the different variables).
            It also depends on particular formats for each language.*/

            var dateIndex = format.dateIndex;
            var locationIndex = format.locationIndex;
            var pageIndex = format.pageIndex;

            try {
                if (hasPageNumber) {
                    var pageNumber = split[pageIndex];
                    clipping.Page = pageNumber;

                    locationIndex = format.hasPageLocationIndex;
                    dateIndex = hasLocation ? format.hasPageHasLocationDateIndex : format.hasPageDateIndex;
                }
            } catch (Exception) {
                clipping.Page = Defaults.Page;
            }

            try {
                if (hasLocation) {
                    var location = split[locationIndex];
                    clipping.Location = location;
                }
            } catch (Exception) {
                clipping.Location = Defaults.Location;
            }

            try {
                //Date detection.
                string dateAddedString = String.Join(" ", split[dateIndex], split[dateIndex + 1], split[dateIndex + 2], split[dateIndex + 3], split[dateIndex + 4], split[dateIndex + 5]);
                DateTime dateAdded = DateTime.Parse(dateAddedString);
                clipping.DateAdded = dateAdded;
            } catch (Exception ex) {
                clipping.DateAdded = Defaults.DateAdded;
                new Exception("Error encountered adding date: " + ex.Message, ex);
            }

            /*Indexes are different in Spanish and English version (answers "where to cut" for the different variables).
            It also depends on particular formats for each language. If any format exceptions occur or it is better to
            manually look for something for any reason, add logic below. */

            if (fileType == "typeBase") {
            }

            if (fileType == "typeRick") {
                if (split[1] == "Clip") {
                    hasInstapaper = true;
                }

                if (hasInstapaper) {
                    locationIndex = 6;
                    dateIndex = 10;

                    if (hasLocation) {
                        var location = split[locationIndex];
                        clipping.Location = location;
                    }
                }
            }
        }
    }
}