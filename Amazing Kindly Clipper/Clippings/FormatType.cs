namespace ClippingManager {

    public class FormatType {
        public string ID;
        public string[] pageWording;
        public string[] locationWording;
        public int clippingTypePosition;
        public KeyPositionLang[] keyPositions;
        public int dateIndex;
        public int locationIndex;
        public int pageIndex;
        public int hasPageDateIndex;
        public int hasPageLocationIndex;
        public int hasPageHasLocationDateIndex;

        /// <summary>
        /// This class defines FormatType, an object that gathers all the properties of every different type. That includes
        /// positions, indexes, keywords and others. A nested class KeyPositionLang is defined in order to store Keywords + Positions + Language
        /// objects, that will later be used to find entries in a dictionary. Each KeyPosition object must be unique, with an unique
        /// keyword + Position + Language combination.
        /// </summary>

        public FormatType(string name, string[] pageKeys, string[] locationKeys, int clippingTypeWhere, KeyPositionLang[] keyPositionPairs, int dateWhere, int locationWhere, int pageWhere, int altDateWithPage, int altLocation,
            int altDateWithPageAndLocation) {
            this.ID = name;
            this.pageWording = pageKeys;
            this.locationWording = locationKeys;
            this.clippingTypePosition = clippingTypeWhere;
            this.keyPositions = keyPositionPairs;
            this.dateIndex = dateWhere;
            this.locationIndex = locationWhere;
            this.pageIndex = pageWhere;
            this.hasPageDateIndex = altDateWithPage;
            this.hasPageLocationIndex = altLocation;
            this.hasPageHasLocationDateIndex = altDateWithPageAndLocation;
        }

        public class KeyPositionLang {
            public string Keyword;
            public int Position;
            public string Language;

            public KeyPositionLang(string keyword, int position, string language) {
                this.Keyword = keyword;
                this.Position = position;
                this.Language = language;
            }
        }
    }
}