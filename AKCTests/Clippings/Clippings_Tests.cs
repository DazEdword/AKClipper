using AKCCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AKCTests.Clippings {

    [TestClass]
    public class Clippings_Tests {

        public Clipping clipping;
        public int validBeginning;
        public int validEnd;
        public string location;


        [TestInitialize]
        public void Init() {
            clipping = new Clipping();
            clipping.Location = "25-27";
            validBeginning = 25;
            validEnd = 27;
        }

        [TestMethod]
        public void BeginningOfRange() {
            int testBeginning = (int)clipping.BeginningLocation;
            Assert.AreEqual(testBeginning, validBeginning);
        }
    }
}