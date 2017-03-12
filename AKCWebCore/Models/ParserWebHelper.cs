using System;
using System.Threading.Tasks;

namespace AKCWebCore.Models {

    public interface IParserWeb {
        Task<String> GetPreview();
    }

    public class ParserWebHelper : IParserWeb {

        public string preview { get; set; }
        public string language { get; set; }


        //Sync
        public ParserWebHelper() {
            this.preview = "A preview of your text will appear here.";
            this.language = "English";
        }

        //Async
        public Task<String> GetPreview() {
            return Task.FromResult("Test test test I will be your preview");
        }
    }


}