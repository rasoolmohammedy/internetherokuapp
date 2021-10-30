Feature: F4_DeleteBooking
Feature-4
Positive & Negative Testcase for Delete Booking details API
In negative testcases, we validate the API with invalid inputs.
e.g: Invalid Date and invalid auth token

@API @DeleteBooking
Scenario: Test Case 08 - Delete Booking Postive scenario
	When the user tries to create a booking with valid input
	Then Booking must be created without any error
	And Store the created Booking ID back to Test Data
	When User requests a Auth token
	Then A valid Auth token must be granted and update the token into Test Data at "36" row
	When the user tries to delete a booking with valid booking id
	Then Booking information for that particular Booking ID must be delete successfully

@API @DeleteBooking
Scenario: Test Case 09 - Delete Booking Negative scenario with invalid Booking Id
	When User requests a Auth token
	Then A valid Auth token must be granted and update the token into Test Data at "41" row
	When the user tries to delete a booking with invalid Booking id
	Then Forbidden Error message should be returned to the user

@API @DeleteBooking
Scenario: Test Case 10 - Delete Booking Negative scenario with invalid Auth Token
	When the user tries to create a booking with valid input
	Then Booking must be created without any error
	And Store the created Booking ID back to Test Data
	When the user tries to delete a booking with invalid Auth Token with a valid Booking Id
	Then Forbidden Error message should be returned to the user