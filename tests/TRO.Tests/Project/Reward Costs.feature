Feature: Reward Costs
	As Connie
	I want a reward costs
	So that I can create a costs profile

@smoke @connie @ui @visual
Scenario: View reward costs
	Given test data "smokehouse"
	When Connie views reward costs
	Then the baseline reward costs is displayed