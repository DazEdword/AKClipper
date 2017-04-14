using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace AKCWebCore.Models {

    public interface IParserWeb {
        Task<String> GetPreview();
    }

    public class ParserWebHelper  {

        public RoutingHelper routing { get; set; }
        public ParserClientContent parserClientContent { get; set; }

        public ParserWebHelper() {
            this.routing = new RoutingHelper();
            this.parserClientContent = new ParserClientContent();
        }

        public class ParserClientContent : IParserWeb {
            public string preview { get; set; }
            public string textSample { get; set; }
            public string language { get; set; }
            public string content { get; set; }
            public bool parsed { get; set; }

            //Sync
            public ParserClientContent() {
                this.preview = "A preview of your text will appear here.";
                this.language = "English";
                this.parsed = false; 
            }

            //Async
            public Task<String> GetPreview() {
                return Task.FromResult("Test test test I will be your preview");
            }
        }

        public class RoutingHelper {
            public string Controller { get; set; }
            public string Action { get; set; }
            public IDictionary<string, object> Data { get; }
            = new Dictionary<string, object>();
        }
    }


}