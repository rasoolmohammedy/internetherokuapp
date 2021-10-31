Feature: TC6_JavaScriptAlerts
TestCase-6
Step 1 - Select JavaScript Alerts
Step 2 - Click on "Click for JS confirm"
Step 3 - Cancel the alert
Step 4 - Validate the alert canceled message

@UI
Scenario: Test Case 6 - JavaScript Alerts Demonstration
	When User launch the website and click on JavaScript link
	Then User should be able to see Click for JS Confirm button
	When User clicks on JS Confirm button
	Then JavaScript Alert must be shown
	When User clicks cancel button
	Then User should be able to see Cancelled validation text message on screen