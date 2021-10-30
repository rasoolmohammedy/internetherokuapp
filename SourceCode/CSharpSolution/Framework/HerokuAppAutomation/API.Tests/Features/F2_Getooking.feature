Feature: F2_Getooking
Feature-2
Positive & Negative Testcase for Get Booking API
In negative testcases, we validate the API with invalid inputs.
e.g: Invalid Booking ID

@API @GetBooking
Scenario: Test Case 4 - Get Booking Postive scenario
	When the user tries to get a booking with valid booking ID
	Then Booking information for that particular Booking ID must be returned to the user

@API @GetBooking
Scenario: Test Case 5 - Get Booking Negative scenario with invalid Booking ID
	When the user tries to get a booking with invalid booking ID
	Then Error message should be returned to the user