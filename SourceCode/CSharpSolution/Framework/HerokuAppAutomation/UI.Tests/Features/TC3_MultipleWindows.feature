Feature: TC3_MultipleWindows
TestCase-3
Step 1 - Select Multiple Windows
Step 2 - Click on "Click Here"
Step 3 - Log the URL of the newly opened tab
Step 4 - Close the newly opened tab
Step 5 - Log the title of the current page

@UI @MultipleWindows
Scenario: Test Case 3 - Multiple Windows Demonstration
	Given the user landed into Homepage
	When the user clicks on Multiple Windows Link
	And the user clicks on Click Here Link
	Then log the URL of the newly opened tab
	And close the newly opened tab
	And log the title of the current page