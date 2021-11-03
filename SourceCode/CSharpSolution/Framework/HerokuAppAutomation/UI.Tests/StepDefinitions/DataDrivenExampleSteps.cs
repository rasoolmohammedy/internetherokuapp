using System;
using TechTalk.SpecFlow;
using Utilities;

namespace UI.Tests.StepDefinitions
{
    [Binding]
    public class DataDrivenExampleSteps
    {
        int parama;
        int paramb;
        int result;
        int expected;

        [Given]
        public void GivenTheFirstNumberIs_P0(int p0)
        {
            parama = p0;
        }
        
        [Given]
        public void GivenTheSecondNumberIs_P0(int p0)
        {
            paramb = p0;
        }
        
        [When]
        public void WhenTheTwoNumbersAreAdded()
        {
            result = parama + paramb;
        }
        
        [Then]
        public void ThenTheResultShouldBe_P0(int p0)
        {
            expected = p0;
            if (result == expected)
                ExtentReportsHelper.SetStepStatusPass($"Adding parama {parama} with paramb {paramb} is equal to {result} which is equal to expected output {expected}");
            else
                ExtentReportsHelper.SetTestStatusFail($"Adding parama {parama} with paramb {paramb} is equal to {result} which is not equal to expected output {expected}");
        }
    }
}
