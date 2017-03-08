using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

namespace AKCWebCore.Extensions {

    public static class MyExtensions {

        public static ExpandoObject ToExpando(this object anonymousObject) {
            IDictionary<string, object> anonymousDictionary = new RouteValueDictionary(anonymousObject);
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var item in anonymousDictionary)
                expando.Add(item);
            return (ExpandoObject)expando;
        }
    }
}