using AKCCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKCWebCore.Models {

    public class ClippingsTable {
        public string Author { get; set; }
        public string BookName { get; set; }
        public ClippingTypeEnum ClippingType { get; set; }
        public DateTime DateAdded { get; set; }
        public string Page { get; set; }
        public string Location { get; set; }
        public string Text { get; set; }

        //List<Clipping> Clippings; //clippings = ClippingDatabase.finalClippingsList;
    }
}