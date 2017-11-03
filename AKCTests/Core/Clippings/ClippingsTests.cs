using AKCCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AKCTests.Clippings {

    [TestClass]
    public class ClippingsTests {

        public Clipping clipping;
        public int validBeginning;
        public string location;
        public int validPage;

        public void InitDefaultClipping(Clipping clipping) {
            clipping.Location = "25-27";
            clipping.Page = "50";
        }

        [TestInitialize]
        public void Init() {
            clipping = new Clipping();
            InitDefaultClipping(clipping);
        }

        [TestMethod]
        public void TestGetBeginningOfRangeReturnsNumberBeforeHyphen() {
            int expected = 25;
            int actual = (int)clipping.BeginningLocation;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestGetBeginningOfRangeReturnsNullIfCharacterIsAnyOtherThanDigitOrHyphen() {
            clipping.Location = "2T5-27";
            int? actual = (int?)clipping.BeginningLocation;
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void TestBeginningOfPageRange() {
            int validPage = 50;
            int testBeginning = (int)clipping.BeginningPage;
            Assert.AreEqual(testBeginning, validPage);
        }

        [TestMethod]
        public void TestCharIsDigitOrHyphenReturnsTrueForDigits() {
            char c = (char)'1';
            bool isDigit = Clipping.CharIsDigitOrHyphen(c);
            Assert.IsTrue(isDigit);
        }

        [TestMethod]
        public void TestCharIsDigitOrHyphenReturnsTrueForHyphens() {
            char c = (char)'-';
            bool isHyphen = Clipping.CharIsDigitOrHyphen(c);
            Assert.IsTrue(isHyphen);
        }

        [TestMethod]
        public void TestOtherCharsNotDigitOrHyphenDetected() {
            char c = (char)'r';
            bool isDigitOrHyphen = Clipping.CharIsDigitOrHyphen(c);
            Assert.AreEqual(false, isDigitOrHyphen);
        }

        [TestMethod]
        public void IsNullOrEmptyDetectsNullClippings() {
            Clipping myNullClipping = null;
            bool isNull = Clipping.IsNullOrEmpty(myNullClipping);
            Assert.IsTrue(isNull);
        }

        [TestMethod]
        public void TestIsBookmark() {
            clipping.ClippingType = ClippingTypeEnum.Bookmark;
            bool isBk = Clipping.IsBookMark(clipping);
            Assert.IsTrue(isBk);
        }
    }
}