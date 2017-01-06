using System;
using AKCCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AKCTests {
    [TestClass]
    public class DirectTest {

        [TestMethod]
        public void TestDirectParse() {
            MyClippingsParserSPA spaParser = MyClippingsParserSPA.MyParserSPA;
            FormatType format = spaParser.typeEd;
            string input = "A Dance With Dragons: Book 5 of A Song of Ice and Fire(Song of Ice & Fire 5)(Martin, George R.R.)\r\n- Mi subrayado en la página 41 | Posición 620 - 620 | Añadido el domingo 2 de septiembre de 2012, 23:54:20\r\n \r\n droll?”\r\n==========";

            IEnumerable<Clipping> output = spaParser.DirectParse(input, format);
            Assert.AreEqual(output.Cast<Clipping>().Count(), 1);
        }
    }
}

