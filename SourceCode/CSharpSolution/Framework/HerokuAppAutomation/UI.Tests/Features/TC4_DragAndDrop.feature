Feature: TC4_DragAndDrop

TestCase-4
Step 1 - Select Drag and Drop
Step 2 - Drag box A and drop it into B
Step 3 - Validate the drop success
Step 4 - Take the screenshot of the same

@UI @DragNDrop
Scenario: Test Case 4 - Drag And Drop Demonstration
	When User launch the website and click on Drag And Drop item
	Then User should be able to see Box A first followed by Box B
	And Take screenshot of the current window
	When User drag Box A and drop inot Box B
	Then Box B should be present first followed by Box A
	Then Take screenshot of the current window