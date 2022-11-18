Feature: Dynamic Text Replacement
	As Andie/Gina
	I want to be able to dynamically replace a default text
	so that I can replace it with a custom one

@smoke @gina @ui @visual
Scenario: View dynamic text replacement
	Given test data "smokehouse"
	When Gina views dynamic text replacement
	Then a list of default text with a corresponding custom text is displayed