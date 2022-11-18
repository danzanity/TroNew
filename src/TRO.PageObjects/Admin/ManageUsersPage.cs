using Auto.Core.Plus.UI.WaitHelpers;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using PageObjectSourceGenerator;
using System.Collections.Generic;
using System.Linq;
using IWebDriver = Auto.Core.Plus.UI.IWebDriver;

namespace TRO.PageObjects.Admin
{
	public sealed partial class ManageUsersPage : BasePage<ManageUsersPage>
	{
		private readonly IWebDriver _driver;
		private readonly string _baseUrl;

		public ManageUsersPage(IWebDriver driver, IConfiguration cfg) : base(driver)
		{
			_driver = driver;
			_baseUrl = cfg.GetValue<string>("baseUrl");
		}

		public ManageUsersPage GoToUrl()
		{
			_driver.Wait().Until(TroExpectedConditions.LocalStorageItemToBePresent("Data Imported"));
			base.GoToUrl($"{_baseUrl}/manageusers");
			return this;
		}

		[PageObject]
		private IWebElement CreateUserEl =>
			_driver.Wait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[text()='Create User']")));

		public CreateUserPage ClickCreateUser()
		{
			CreateUserEl.Click();
			return new CreateUserPage(_driver);
		}

		public IEnumerable<UserListItemPage> Users => _driver
			.FindElements(By.XPath("//table[contains(@class, 'table-users')]/tbody/tr"))
			.Select(el => new UserListItemPage(_driver, el));

		public override ManageUsersPage WaitForPageToLoad()
		{
			base.WaitForPageToLoad();
			_driver.Wait().Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//table[contains(@class, 'table-users')]/tbody/tr")));
			return this;
		}
	}

	public sealed partial class UserListItemPage : BasePage<UserListItemPage>
	{
		private readonly IWebElement _rowEl;

		public UserListItemPage(IWebDriver driver, IWebElement rowEl) : base(driver) => _rowEl = rowEl;

		[PageObject]
		private IWebElement EditEl => _rowEl.FindElement(By.XPath("//button[text()='Edit']"));
	}

	public sealed partial class CreateUserPage : BasePage<CreateUserPage>
	{
		private readonly IWebDriver _driver;

		public CreateUserPage(IWebDriver driver) : base(driver) => _driver = driver;

		public override CreateUserPage WaitForPageToLoad()
		{
			base.WaitForPageToLoad();
			_driver.Wait().Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@type='submit']")));
			return this;
		}
	}

	public sealed partial class EditUserPage : BasePage<EditUserPage>
	{
		private readonly IWebDriver _driver;

		public EditUserPage(IWebDriver driver) : base(driver) => _driver = driver;

		public override EditUserPage WaitForPageToLoad()
		{
			base.WaitForPageToLoad();
			_driver.Wait().Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@type='submit']")));
			return this;
		}
	}
}