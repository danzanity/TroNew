using Applitools.Selenium;
using TechTalk.SpecFlow;
using TRO.PageObjects.Project;

namespace TRO.Tests.Project
{
	[Binding]
	[Scope(Tag = "ui")]
	public sealed class HomeUiSteps
	{
		private readonly HomePage _page;
		private readonly Eyes _eyes;
		private readonly ScenarioContext _sceCtx;

		public HomeUiSteps(HomePage page, Eyes eyes, ScenarioContext sceCtx)
		{
			_page = page;
			_eyes = eyes;
			_sceCtx = sceCtx;
		}

		[Then(@"she gets redirected to the project home")]
		public void ThenSheGetsRedirectedToTheProjectHome()
		{
			_page.WaitForPageToLoad();
			_eyes.Check(_sceCtx.StepContext.StepInfo.Text, Target.Region(_page.MainBy).Ignore(_page.Footer));
		}
	}
}