using Applitools.Selenium;
using TechTalk.SpecFlow;
using TRO.PageObjects.Project;

namespace TRO.Tests.Project
{
	[Binding]
	[Scope(Tag = "ui")]
	public sealed class RewardCostsUiSteps
	{
		private readonly RewardCostsPage _page;
		private readonly Eyes _eyes;
		private readonly UiLogin _login;
		private readonly ScenarioContext _sceCtx;

		public RewardCostsUiSteps(
			RewardCostsPage page,
			Eyes eyes,
			UiLogin login,
			ScenarioContext sceCtx)
		{
			_page = page;
			_eyes = eyes;
			_login = login;
			_sceCtx = sceCtx;
		}

		[When(@"Connie views reward costs")]
		public void WhenConnieViewsRewardCosts()
		{
			_login.AsConnie();
			_page.GoToUrl().WaitForPageToLoad();
		}

		[Then(@"the baseline reward costs is displayed")]
		public void ThenTheBaselineRewardCostsIsDisplayed()
		{
			_eyes.Check(_sceCtx.StepContext.StepInfo.Text, Target.Region(_page.MainBy).Ignore(_page.Footer));
		}
	}
}