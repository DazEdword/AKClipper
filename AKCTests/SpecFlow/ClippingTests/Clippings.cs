using AKCDesktop;
using System;
using TechTalk.SpecFlow;

namespace AKCTests.ClippingTests
{
    [Binding]
    public class ClippingsSteps
    {
        //Beware, we are testing static method of the class. 
        Clipping clipping = new Clipping();
        Clipping clip = null;

        [When(@"I use a null clipping item in IsNullOrEmpty method\.")]
        public void WhenIUseANullClippingItem_(Clipping clip) {
            bool result = Clipping.IsNullOrEmpty(clip);
        }

        [Then(@"Class has to return ""(.*)""")]
        public void ThenClassHasToReturn(Clipping clip) {
            bool result = Clipping.IsNullOrEmpty(clip);
        }
    }
}
