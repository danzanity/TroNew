using Applitools.Selenium;
using System.Linq;
using TechTalk.SpecFlow;
using TRO.PageObjects.Admin;

namespace TRO.Tests.Admin
{
	[Binding]
	[Scope(Tag = "ui")]
	public sealed class ManageUsersUiSteps
	{
		private readonly ManageUsersPage _page;
		private readonly Eyes _eyes;
		private readonly UiLogin _login;
		private readonly ScenarioContext _sceCtx;

		public ManageUsersUiSteps(
			ManageUsersPage page,
			Eyes eyes,
			UiLogin login,
			ScenarioContext sceCtx)
		{
			_page = page;
			_eyes = eyes;
			_login = login;
			_sceCtx = sceCtx;
		}

		[When(@"Gina views manage users")]
		public void WhenGinaViewsManageUsers()
		{
			_login.AsGina();
			_page.GoToUrl().WaitForPageToLoad();
		}

		[Then(@"a list of users is displayed")]
		public void ThenAListOfUsersIsDisplayed()
		{
			_eyes.Check(_sceCtx.StepContext.StepInfo.Text, Target.Region(_page.MainBy).Ignore(_page.Header, _page.Footer).Layout());
		}

		[When(@"Gina wants to create a user")]
		public void WhenGinaWantsToCreateAUser()
		{
			_login.AsGina();
			_page.GoToUrl().WaitForPageToLoad().ClickCreateUser().WaitForPageToLoad();
		}

		[Then(@"a create user form is displayed")]
		public void ThenACreateUserFormIsDisplayed()
		{
			_eyes.Check(_sceCtx.StepContext.StepInfo.Text, Target.Region(_page.MainBy).Ignore(_page.Header, _page.Footer));
		}

		[When(@"Gina wants to edit a user")]
		public void WhenGinaWantsToEditAUser()
		{
			_login.AsGina();
			_page.GoToUrl().WaitForPageToLoad().Users.First().ClickEdit().WaitForPageToLoad();
		}

		[Then(@"an edit user form is displayed")]
		public void ThenAnEditUserFormIsDisplayed()
		{
			_eyes.Check(_sceCtx.StepContext.StepInfo.Text, Target.Region(_page.MainBy).Ignore(_page.Header, _page.Footer));
		}
	}
}