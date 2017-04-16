using System.Collections.Generic;

namespace AKCWebCore.Models {
    public class RoutingHelper {
        public string Controller { get; set; }
        public string Action { get; set; }
        public IDictionary<string, object> Data { get; }
        = new Dictionary<string, object>();
    }
}
