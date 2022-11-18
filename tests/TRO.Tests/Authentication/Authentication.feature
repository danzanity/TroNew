Feature: Authentication
	As a user
	I want to be able to login
	so that I can use the app

@smoke @ui @visual
Scenario: View login
	When a user visits the total rewards optimization page
	Then the user gets redirects to the login page

@smoke @ui @visual
Scenario: View register now
	When a user wants to register now
	Then a register now form is displayed

@smoke @ui @visual
Scenario: View forgot password
	When a user forgots their password
	Then a forgot password form is displayed

@smoke @andie @ui @visual
Scenario: Login as Andie
	When Andie logs in
	Then he gets redirected to a list of projects

@smoke @gina @ui @visual
Scenario: Login as Gina assigned to multiple projects
	When Gina logs in
	Then she gets redirected to a list of projects

@smoke @connie @ui @visual
Scenario: Login as Connie assigned to multiple projects
	When Connie logs in
	Then she gets redirected to a list of projects

@smoke @connie @ui @visual
Scenario: Login as Connie assigned to one project only
	Given test data "smokehouse"
	When Connie logs in
	Then she gets redirected to the project home