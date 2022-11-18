using Auto.Core.Plus.UI.WaitHelpers;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using IWebDriver = Auto.Core.Plus.UI.IWebDriver;

namespace TRO.PageObjects.Project
{
	public sealed partial class RewardCostsPage : BasePage<RewardCostsPage>
	{
		private readonly IWebDriver _driver;
		private readonly string _baseUrl;

		public RewardCostsPage(IWebDriver driver, IConfiguration cfg) : base(driver)
		{
			_driver = driver;
			_baseUrl = cfg.GetValue<string>("baseUrl");
		}

		public RewardCostsPage GoToUrl()
		{
			_driver.Wait().Until(TroExpectedConditions.LocalStorageItemToBePresent("Data Imported"));
			base.GoToUrl($"{_baseUrl}/rewardcost");
			return this;
		}

		public override RewardCostsPage WaitForPageToLoad()
		{
			base.WaitForPageToLoad();
			_driver.Wait().Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//mat-icon[text()='info']")));
			return this;
		}
	}
}