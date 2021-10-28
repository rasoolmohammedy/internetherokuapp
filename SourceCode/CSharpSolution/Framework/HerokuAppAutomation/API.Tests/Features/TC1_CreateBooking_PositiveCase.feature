Feature: TC1_CreateBooking_PositiveCase

TestCase-1
Positive Testcase for Create Booking

@API
Scenario: Test Case 1 - Create Booking Postive scenario
	When the user tries to create a booking with valid input
	Then Booking must be created without any error