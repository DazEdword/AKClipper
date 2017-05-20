using AKCCore;
using System;
using AKCWebCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AKCWebCore.Models {

    public class ParserWebHelper  {

        public string language { get; set; }
        public bool reset { get; set; }
        public string content { get; set; }
        public string preview { get; set; }
        public List<Clipping> clippingData;

        public ParserController parserController;
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
            this.parserController = new ParserController();
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