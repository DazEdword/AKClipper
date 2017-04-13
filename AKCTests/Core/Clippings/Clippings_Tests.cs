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

        [TestMethod]
        public void TestCharIsDigit() {
            char c = (char)'1';
            bool isDigit = Clipping.CharIsDigitOrHyphen(c);
            Assert.AreEqual(true, isDigit);
        }

        [TestMethod]
        public void TestCharIsHyphen() {
            char c = (char)'-';
            bool isHyphen = Clipping.CharIsDigitOrHyphen(c);
            Assert.AreEqual(true, isHyphen);
        }

        [TestMethod]
        public void TestOtherCharsNotDigitOrHyphenDetected() {
            char c = (char)'r';
            bool isDigitOrHyphen = Clipping.CharIsDigitOrHyphen(c);
            Assert.AreEqual(false, isDigitOrHyphen);
        }

        [TestMethod]
        public void TestClippingNull() {
            Clipping myNullClipping = null;
            bool isNull = Clipping.IsNullOrEmpty(myNullClipping);
            Assert.AreEqual(true, isNull);
        }

        [TestMethod]
        public void TestIsBookmark() {
            clipping.ClippingType = ClippingTypeEnum.Bookmark;
            bool isBk = Clipping.IsBookMark(clipping);
            Assert.AreEqual(true, isBk);
        }
    }
}