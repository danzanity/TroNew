using Applitools.Selenium;
using TechTalk.SpecFlow;
using TRO.PageObjects.Admin;

namespace TRO.Tests.Admin
{
	[Binding]
	[Scope(Tag = "ui")]
	public sealed class ManageProjectUiSteps
	{
		private readonly ManageProjectPage _page;
		private readonly Eyes _eyes;
		private readonly UiLogin _login;
		private readonly ScenarioContext _sceCtx;

		public ManageProjectUiSteps(
			ManageProjectPage page,
			Eyes eyes,
			UiLogin login,
			ScenarioContext sceCtx)
		{
			_page = page;
			_eyes = eyes;
			_login = login;
			_sceCtx = sceCtx;
		}

		[When(@"Gina views manage project")]
		public void WhenGinaViewsManageProject()
		{
			_login.AsGina();
			_page.GoToUrl().WaitForPageToLoad();
		}

		[Then(@"a manage project form is displayed")]
		public void ThenAManageProjectFormIsDisplayed()
		{
			_eyes.Check(_sceCtx.StepContext.StepInfo.Text, Target.Region(_page.MainBy).Ignore(_page.Header, _page.Footer));
		}
	}
}