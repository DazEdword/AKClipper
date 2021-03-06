﻿using System;
using System.Linq;

namespace AKCCore {

    public class Clipping {

        public string Author { get; set; }
        public string BookName { get; set; }
        public ClippingTypeEnum ClippingType { get; set; }
        public DateTime DateAdded { get; set; }
        public string Page { get; set; }
        public string Location { get; set; }
        public string Text { get; set; }

        public int? BeginningPage => GetBeginningOfRange(Page);
        public int? BeginningLocation => GetBeginningOfRange(Location);

        private int? GetBeginningOfRange(string range) {
            if ((String.IsNullOrEmpty(range)) || (range.All(c => CharIsDigitOrHyphen(c))) == false) {
                return null;
            }

            int hIndex = range.IndexOf('-');
            string first;

            first = (hIndex >= 0) ? range.Substring(0, hIndex) : range;

            try {
                return int.Parse(first);
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message, "Error parsing range.");
                return null;
            }
        }

        private static bool CharIsDigitOrHyphen(char c) {
            if (char.IsDigit(c) == true || c.ToString() == "-") {
                return true;
            } else {
                return false;
            }
        }

        public static bool IsNullOrEmpty(Clipping item) {
            return item == null || string.IsNullOrEmpty(item.Text);
        }

        //TODO quick hardcoded solution in order to exclude bookmarks from empty clipping removal. 
        //Quick and dirty, can be way better.
        public static bool IsBookMark(Clipping item) {
            if (item.ClippingType == ClippingTypeEnum.Bookmark || 
                item.ClippingType == ClippingTypeEnum.Marcador) {
                return true;
            } else {
                return false;
            }
        }
    }
}