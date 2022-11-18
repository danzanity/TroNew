Feature: Import Survey Data
	As Andie/Gina
	I want to be able to import a survey data
	so that I can make some changes to a project

@smoke @gina @ui @visual
Scenario: View import survey data
	Given test data "smokehouse"
	When Gina wants to import a survey data
	Then an import survey data form is displayed