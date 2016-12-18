using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKCCore{
    public class ParserENG:MyClippingsParser{
        public string Sample { get; set; }


        public void Parse(){

            //ParseLine1(line, clipping);
            //Parse(Sample);
        }

        public override void ParseLine2(string a, Clipping b, FormatType c) {
           
        }

        public Clipping Result = new Clipping();

    }
}
