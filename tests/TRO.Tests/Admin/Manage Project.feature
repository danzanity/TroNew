Feature: Manage Project
	As Andie/Gina
	I want to be able to manage a project
	so that I can make some changes to it

@smoke @gina @ui @visual
Scenario: View manage project
	Given test data "smokehouse"
	When Gina views manage project
	Then a manage project form is displayed