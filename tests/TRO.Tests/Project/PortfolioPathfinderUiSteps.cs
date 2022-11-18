using Applitools.Selenium;
using TechTalk.SpecFlow;
using TRO.PageObjects.Project;

namespace TRO.Tests.Project
{
	[Binding]
	[Scope(Tag = "ui")]
	public sealed class PortfolioPathfinderUiSteps
	{
		private readonly PortfolioPathfinderPage _page;
		private readonly Eyes _eyes;
		private readonly UiLogin _login;
		private readonly ScenarioContext _sceCtx;

		public PortfolioPathfinderUiSteps(
			PortfolioPathfinderPage page,
			Eyes eyes,
			UiLogin login,
			ScenarioContext sceCtx)
		{
			_page = page;
			_eyes = eyes;
			_login = login;
			_sceCtx = sceCtx;
		}

		[When(@"Connie views portfolio pathfinder")]
		public void WhenConnieViewsPortfolioPathfinder()
		{
			_login.AsConnie();
			_page.GoToUrl().WaitForPageToLoad();
		}

		[Then(@"the completed analyses is displayed")]
		public void ThenTheCompletedAnalysesIsDisplayed()
		{
			_eyes.Check(_sceCtx.StepContext.StepInfo.Text, Target.Region(_page.MainBy).Ignore(_page.Footer));
		}
	}
}