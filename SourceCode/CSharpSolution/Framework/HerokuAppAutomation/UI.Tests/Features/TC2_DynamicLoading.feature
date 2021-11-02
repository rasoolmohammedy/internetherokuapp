Feature: TC2_DynamicLoading

TestCase-2
Step 1 - Select Dynamic Loading the click "Example 2: Element rendered after the fact"
Step 2 - Click Start
Step 3 - Validate the progress bar
Step 4 - Validate the final message once the progress gets complete. (Note: Avoid hard-coded wait)

@UI @DynamicLoading
Scenario: Test Case 2 - Dynamic Loading Demonstration
	Given the user landed into Homepage
	When the user clicks on Dynamic Loading Iteam
	And click on Example 2
	And click Start button
	And the user waits till progress bar to disappear
	Then user must see the final message