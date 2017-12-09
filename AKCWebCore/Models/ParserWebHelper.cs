using AKCCore;
using AKCWebCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AKCWebCore.Models {

    public class ParserWebHelper {
        public string language { get; set; }
        public bool reset { get; set; }
        public string content { get; set; }
        public string preview { get; set; }
        public List<Clipping> clippingData;

        public const string helperKey = "ParserHelper";

        //Sync
        public ParserWebHelper() {
            InitHelper();
        }

        public void InitHelper() {
            this.preview = "A preview of your text will appear here.";
            this.language = "English";
            this.reset = false;
            this.clippingData = new List<Clipping>();

            //Avoids serialization circular reference issue in CultureInfo
            Newtonsoft.Json.JsonConvert.DefaultSettings = () => new Newtonsoft.Json.JsonSerializerSettings() {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        //Session-aware helper
        public static ParserWebHelper GetHelper(IServiceProvider services) {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
            .HttpContext.Session;
            ParserWebHelper helper = session?.GetJson<ParserWebHelper>(helperKey)
            ?? new ParserWebHelper();
            helper.Session = session;
            return helper;
        }

        public void Save() {
            Session.SetJson(helperKey, this);
        }

        public void Reset() {
            Session.Remove(helperKey);
            InitHelper();
        }

        [JsonIgnore]
        public ISession Session { get; set; }
    }
}