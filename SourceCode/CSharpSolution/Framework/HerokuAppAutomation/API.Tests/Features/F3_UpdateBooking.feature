Feature: F3_UpdateBooking
Feature-3
Positive & Negative Testcase for Update Booking details API
In negative testcases, we validate the API with invalid inputs.
e.g: Invalid Date

@API @UpdateBooking
Scenario: Test Case 06 - Update Booking Postive scenario
	Given a valid Booking ID generated
	When User requests a Auth token
	Then A valid Auth token must be granted and update the token into Test Data at "24" row
	When the user tries to update a booking with valid inputs
	Then Booking information for that particular Booking ID must be updated successfully

@API @UpdateBooking
Scenario: Test Case 07 - Update Booking Negative scenario with invalid Date
	Given a valid Booking ID generated
	When User requests a Auth token
	Then A valid Auth token must be granted and update the token into Test Data at "30" row
	When the user tries to update a booking with invalid Date
	Then Error message should be returned to the user for updating with Invalid Date