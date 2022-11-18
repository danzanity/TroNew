Feature: Manage Users
	As Andie/Gina
	I want to be able to manage the users
	so that I can make some changes to their access

@smoke @gina @ui @visual
Scenario: View manage users
	Given test data "smokehouse"
	When Gina views manage users
	Then a list of users is displayed

@smoke @gina @ui @visual
Scenario: View create user form
	Given test data "smokehouse"
	When Gina wants to create a user
	Then a create user form is displayed
	
@smoke @gina @ui @visual
Scenario: View edit user form
	Given test data "smokehouse"
	When Gina wants to edit a user
	Then an edit user form is displayed