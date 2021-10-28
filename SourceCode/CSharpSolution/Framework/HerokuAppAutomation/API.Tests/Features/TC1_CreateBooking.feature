﻿Feature: TC1_CreateBooking
TestCase-1
Positive Testcase for Create Booking

@API
Scenario: Test Case 1 - Create Booking Postive scenario
	When the user tries to create a booking with valid input
	Then Booking must be created without any error
	And Store the created Booking ID back to Test Data

@API
Scenario: Test Case 2 - Create Booking Negative scenario Null Check
	When the user tries to create a booking with invalid input with null value
	Then Booking must not be created error must be thrown

@API
Scenario: Test Case 3 - Create Booking Negative scenario Invalid Date Check
	When the user tries to create a booking with invalid input with invalid date value
	Then Booking must not be created error must be thrown stating Invalid Date