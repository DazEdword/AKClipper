using AKCCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AKCTests.Clippings {

    [TestClass]
    public class Clippings_Tests {

        public Clipping clipping;
        public int validBeginning;
        public int validEnd;
        public string location;
        public int validPage;

        public void InitDefaultClipping(Clipping clipping) {
            clipping.Location = "25-27";
            validBeginning = 25;
            validEnd = 27;

            clipping.Page = "50";
            validPage = 50;

        }

        [TestInitialize]
        public void Init() {
            clipping = new Clipping();
            InitDefaultClipping(clipping);
        }

        [TestMethod]
        public void BeginningOfRange() {
            int testBeginning = (int)clipping.BeginningLocation;
            Assert.AreEqual(testBeginning, validBeginning);
        }

        [TestMethod]
        public void BeginningOfPageRange() {
            int testBeginning = (int)clipping.BeginningPage;
            Assert.AreEqual(testBeginning, validPage);
        }

        [TestMethod]
        public void IllegalRange() {
            clipping.Location = "2T5-27";
            Assert.AreEqual(null, clipping.BeginningLocation);
        }
    }
}