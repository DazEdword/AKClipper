using AKCCore;
using System.Collections.Generic;


namespace AKCWebCore.Models {

    public class ParserWebHelper  {

        public string language { get; set; }
        public bool reset { get; set; }
        public string content { get; set; }
        public string preview { get; set; }
        public List<Clipping> clippingData;


        //Sync
        public ParserWebHelper() {
                this.preview = "A preview of your text will appear here.";
                this.language = "English";
                this.reset = false;
                this.clippingData = new List<Clipping>();
        }
    }


}