Feature: Portfolio Pathfinder
	As Connie
	I want a portfolio pathfinder
	So that it can identify the best portfolios for a given custom segment and cost profile

@smoke @connie @ui @visual
Scenario: View portfolio pathfinder
	Given test data "smokehouse"
	When Connie views portfolio pathfinder
	Then the completed analyses is displayed