using Applitools.Selenium;
using TechTalk.SpecFlow;
using TRO.PageObjects.Project;

namespace TRO.Tests.Project
{
	[Binding]
	[Scope(Tag = "ui")]
	public sealed class ProjectsUiSteps
	{
		private readonly ProjectsPage _page;
		private readonly Eyes _eyes;
		private readonly ScenarioContext _sceCtx;

		public ProjectsUiSteps(ProjectsPage page, Eyes eyes, ScenarioContext sceCtx)
		{
			_page = page;
			_eyes = eyes;
			_sceCtx = sceCtx;
		}

		[Then(@"he gets redirected to a list of projects")]
		public void ThenHeGetsRedirectedToAListOfProjects()
		{
			_page.WaitForPageToLoad();
			_eyes.Check(_sceCtx.StepContext.StepInfo.Text, Target.Region(_page.MainBy).Ignore(_page.Header, _page.Footer).Layout());
		}

		[Then(@"she gets redirected to a list of projects")]
		public void ThenSheGetsRedirectedToAListOfProjects()
		{
			_page.WaitForPageToLoad();
			_eyes.Check(_sceCtx.StepContext.StepInfo.Text, Target.Region(_page.MainBy).Ignore(_page.Header, _page.Footer));
		}
	}
}