Feature: TC1_FormAuthentication

TestCase-1
Step 1 - Launch the site then click on Form Authentication
Step 2 - Get the username and password from the given text and store them in an external file.
Step 3 - Read that username and password from the external file and log in with those credentials
Step 4 - Validate the login success.
Step 5 - Enter any invalid credentials and validate the failure scenario as well.

@UI @FormAuthentication
Scenario: Test Case 1 - Form Authentication Demonstration
	When I launch the website and click on Form Authentication
	Then Get the username and password displayed on screen and store them in test data file
	When I read those stored username and password file from test data file and try to login with those credentials
	Then User must be logged in successfully
	When I enter any invalid credentials
	Then I must see failure error message