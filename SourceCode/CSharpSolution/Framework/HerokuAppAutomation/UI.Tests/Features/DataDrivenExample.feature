Feature: DataDrivenExample
	Simple Data Driven Example Test case

@mytag
Scenario: Add two numbers
	Given the first number is 50
	And the second number is 70
	When the two numbers are added
	Then the result should be 120
	Examples:
	| param1 | param2 | result |
	| 10     | 20     | 30     |
	| 15     | 10     | 25     |
	| 350    | 50     | 400    |
	| 456    | 70     | 526    |
	| 20     | 40     | 60     |
	| 30     | 80     | 110    |