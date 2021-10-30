Feature: F2_Getooking
Feature-2
Positive & Negative Testcase for Get Booking API
In negative testcases, we validate the API with invalid inputs.
e.g: Invalid Booking ID

@API @GetBooking
Scenario: Test Case 04 - Get Booking Postive scenario
	Given a valid Booking ID generated
	When the user tries to get a booking with valid booking ID
	Then Booking information for that particular Booking ID must be returned to the user

@API @GetBooking
Scenario: Test Case 05 - Get Booking Negative scenario with invalid Booking ID
	When the user tries to get a booking with invalid booking ID
	Then Error message should be returned to the user