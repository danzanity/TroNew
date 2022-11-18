using Auto.Core.Plus.UI.WaitHelpers;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using IWebDriver = Auto.Core.Plus.UI.IWebDriver;

namespace TRO.PageObjects.Admin
{
	public sealed class DynamicTextReplacementPage : BasePage<DynamicTextReplacementPage>
	{
		private readonly IWebDriver _driver;
		private readonly string _baseUrl;

		public DynamicTextReplacementPage(IWebDriver driver, IConfiguration cfg) : base(driver)
		{
			_driver = driver;
			_baseUrl = cfg.GetValue<string>("baseUrl");
		}

		public DynamicTextReplacementPage GoToUrl()
		{
			_driver.Wait().Until(TroExpectedConditions.LocalStorageItemToBePresent("Data Imported"));
			base.GoToUrl($"{_baseUrl}/dynamictextreplacement");
			return this;
		}

		public override DynamicTextReplacementPage WaitForPageToLoad()
		{
			base.WaitForPageToLoad();
			_driver.Wait().Until(ExpectedConditions.ElementIsVisible(By.XPath("//table[contains(@class,'mat-table')]")));
			return this;
		}
	}
}