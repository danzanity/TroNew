using Auto.Core.Plus.UI.WaitHelpers;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using IWebDriver = Auto.Core.Plus.UI.IWebDriver;

namespace TRO.PageObjects.Admin
{
	public sealed class ManageProjectPage : BasePage<ManageProjectPage>
	{
		private readonly IWebDriver _driver;
		private readonly string _baseUrl;

		public ManageProjectPage(IWebDriver driver, IConfiguration cfg) : base(driver)
		{
			_driver = driver;
			_baseUrl = cfg.GetValue<string>("baseUrl");
		}

		public ManageProjectPage GoToUrl()
		{
			_driver.Wait().Until(TroExpectedConditions.LocalStorageItemToBePresent("Data Imported"));
			base.GoToUrl($"{_baseUrl}/manageproject");
			return this;
		}

		public override ManageProjectPage WaitForPageToLoad()
		{
			base.WaitForPageToLoad();
			_driver.Wait().Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@title='Edit']")));
			return this;
		}
	}
}