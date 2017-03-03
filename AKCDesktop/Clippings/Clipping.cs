using System;
using System.Linq;

namespace AKCDesktop {

    public class Clipping {
        public string BookName { get; set; }
        public string Author { get; set; }
        public ClippingTypeEnum ClippingType { get; set; }
        public string Page { get; set; }
        public string Location { get; set; }
        public DateTime DateAdded { get; set; }
        public string Text { get; set; }

        public int? BeginningPage => GetBeginningOfRange(Page);

        public int? BeginningLocation {
            get { return GetBeginningOfRange(Location); }
        }

        private int? GetBeginningOfRange(string range) {
            if ((String.IsNullOrEmpty(range)) || (range.All(c => char.IsDigit(c))) == false) {
                return null;
            }

            var hIndex = range.IndexOf('-');

            string first;

            first = (hIndex >= 0) ? range.Substring(0, hIndex) : range;

            try { return int.Parse(first); }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message, "Error parsing range.");
                return null;
            }
        }

        public static bool IsNullOrEmpty(Clipping item) {
            return item == null || string.IsNullOrEmpty(item.Text);
        }
    }
}