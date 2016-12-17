using System;
using TechTalk.SpecFlow;
using NUnit.Framework;

using AKCCore;

namespace AKCTests.ClippingTests
{
    [Binding]
    public class Parsing_EntrySteps
    {
        //MyClippingsParserENG parser = new MyClippingsParserENG();
        ParserENG parser = new ParserENG();

        [Given(@"a text to parse")]
        public void GivenATextToParse(string entry)
        {
            parser.Sample = entry;
        }
        
        [When(@"I parse it")]
        public void WhenIParseIt()
        {
            parser.Parse();
        }
        
        [Then(@"the book name should be ""(.*)""")]
        public void ThenTheBookNameShouldBe(string expected)
        {
            Assert.AreEqual(expected, parser.Result.BookName);                
        }
    }
}
