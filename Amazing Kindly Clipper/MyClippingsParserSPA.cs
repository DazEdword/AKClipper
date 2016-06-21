using System;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace ClippingManager
{
    public class MyClippingsParserSPA : MyClippingsParser
    {
        public string[] typeEdPageKeys;
        public string[] typeEdLocationKeys;
        public string[] typeRubPageKeys;
        public string[] typeRubLocationKeys;
        public FormatType typeEd;
        public FormatType typeRub;
        public FormatType[] spaFormats;

        private static readonly MyClippingsParserSPA myParserSPA = new MyClippingsParserSPA(); //Singleton instantiation. 

        /* Following three methods: simple, thread safe singleton implementation*/

        private MyClippingsParserSPA()
        {
            defaultBookName = "Título desconocido";
            defaultAuthor = "Autor desconocido";
            defaultText = "";
            defaultLocation = "";
            defaultPage = "";
            defaultDateAdded = new DateTime();

            typeEdPageKeys = new string[] { " en la página " };   //Manually instancing an array of keys per type to be added to struct constructor. Modifyable. 
            typeEdLocationKeys = new string[] { " Posición " };

            typeRubPageKeys = new string[] { " en la página " };
            typeRubLocationKeys = new string[] { " Pos. " };

            typeEd = new FormatType("typeEd", typeEdPageKeys, typeEdLocationKeys, 2,
                new FormatType.KeyPositionLang[]
                {
                    new FormatType.KeyPositionLang("Mi", 2, "Spanish"),
                    new FormatType.KeyPositionLang("Tu", 2, "Spanish") 
                }, 8, 4, 6, 6, 9, 13);

            typeRub = new FormatType("typeRub", typeRubPageKeys, typeRubLocationKeys, 1,
                new FormatType.KeyPositionLang[]
                {
                    new FormatType.KeyPositionLang("-", 1, "Spanish"),
                }, 7, 3, 5, 5, 8, 13);

            spaFormats = new FormatType[] {typeEd, typeRub};
        }

        public static MyClippingsParserSPA MyParserSPA
        {
            get
            {
                return myParserSPA;
            }
        }

        // Explicit static constructor to tell C# compiler not to mark type as 'beforefieldinit'
        static MyClippingsParserSPA()
        {
        }

        public override void ParseLine2(string line, Clipping clipping, FormatType format)
        {
            var split = line.Split(' ');
            var fileType = "";

            bool hasPageNumber = false;
            bool hasLocation = false;

            //Detect type of file.

            try
            {
                if (!String.IsNullOrEmpty(format.ID))
                {
                    fileType = format.ID;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Can't identify TXT format.");
            }

            var clippingType = split[format.clippingTypePosition];

            switch (clippingType.ToLower())
            {
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

            hasPageNumber = format.pageWording.Any(line.Contains);
            hasLocation = format.locationWording.Any(line.Contains);

            var dateIndex = format.dateIndex;
            var locationIndex = format.locationIndex;
            var pageIndex = format.pageIndex;

            bool isSubtypeKyuni = false;

            if (split[1] == "Tu")
            {
                isSubtypeKyuni = true;
            }

            if (isSubtypeKyuni)
            {
                dateIndex = 10;
            }

            try
            {
                if (hasPageNumber)
                {
                    var pageNumber = split[pageIndex];
                    clipping.Page = pageNumber;

                    locationIndex = format.hasPageLocationIndex;
                    dateIndex = hasLocation ? format.hasPageHasLocationDateIndex : format.hasPageDateIndex;
                }
            }

            catch (Exception)
            {
                clipping.Page = defaultPage;
            }

            try
            {
                if (hasLocation)
                {
                    var location = split[locationIndex];
                    clipping.Location = location;
                }
            }
            catch (Exception)
            {
                clipping.Location = defaultLocation;
            }

            //Date detection.

            string[] formats = { "dddd d MMMM yyyy, HH:mm:ss", "dddd dd MMMM yyyy, HH:mm:ss", "dddd dd MMMM yyyy, hh:mm:ss", "dddd d MMMM yyyy, hh:mm:ss",
                "dddd d MMMM yyyy HH'H'mm", "dddd dd MMMM yyyy HH'H'mm", "dddd, dd MMMM yyyy HH:mm:ss", "dddd, d MMMM yyyy HH:mm:ss", "dddd dd MMMM yyyy, H:mm:ss", "dddd dd MMMM yyyy, h:mm:ss" };
            string dateAddedStringSPA = String.Join(" ", split[dateIndex], split[dateIndex + 1], split[dateIndex + 3], split[dateIndex + 5], split[dateIndex + 6]);
            string input = dateAddedStringSPA;  //domingo 2 septiembre 2012, 23:54:20 Case Edu //miércoles 29 abril 2015 15H08 case ruber

            try
            {

                /*Dates have to be parsed and converted to a dateTime format. TryParseExact should do the trick as long as
                 *the proper format is added to the formats array.  */

                DateTime dt;
                if (DateTime.TryParseExact(input, formats, Options.SpaCulture, DateTimeStyles.None, out dt))
                {
                    if (dt < DateTime.Now)
                    {
                        clipping.DateAdded = dt;
                    }
                }
            }

            catch (Exception ex)
            {
                clipping.DateAdded = defaultDateAdded;
                new Exception("Error encountered adding date: " + ex.Message, ex);
            }          
            }         
      }
  }

