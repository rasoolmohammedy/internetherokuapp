Feature: TC5_Frames

TestCase-5
Step 1 - Select Frames then click iFrame
Step 2 - Clear the predefined text and Enter some text
Step 3 - Apply Bold style for the newly entered text
Step 4 - Take the screenshot of the same

@UI @Frames
Scenario: Test Case 5 - Frames Demonstration
	When User launch the website and click on Frames item
	Then User should be able to see iFrame link
	When User clicks on iFrame
	Then User should be able to see the predefined text
	When User clears current text and enter new text and apply bold style
	Then Take screenshot of the current window