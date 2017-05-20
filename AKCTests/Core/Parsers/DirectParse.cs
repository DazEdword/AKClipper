using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AKCCore;

/* TODO learn how to implement MSTest in .NET Core/ .NETStandard or swap to Xunit/Nunit*/

namespace AKCTests {
    [TestClass]
    public class DirectParse {

        [TestMethod]
        public void TestDirectParse() {
            ParserSPA spaParser = ParserSPA.MyParserSPA;
            FormatType format = spaParser.typeEd;
            string input = "A Dance With Dragons: Book 5 of A Song of Ice and Fire(Song of Ice & Fire 5)(Martin, George R.R.)\r\n- Mi subrayado en la página 41 | Posición 620 - 620 | Añadido el domingo 2 de septiembre de 2012, 23:54:20\r\n \r\n droll?”\r\n==========";

            IEnumerable<Clipping> output = spaParser.DirectParse(input, format);
            Assert.AreEqual(output.Cast<Clipping>().Count(), 1);
        }
    }
}

