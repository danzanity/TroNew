using Auto.Core.Plus.UI.WaitHelpers;
using OpenQA.Selenium;
using PageObjectSourceGenerator;
using IWebDriver = Auto.Core.Plus.UI.IWebDriver;

namespace TRO.PageObjects
{
	public sealed partial class CommonPage : BasePage<CommonPage>
	{
		public CommonPage(IWebDriver driver) : base(driver) { }
	}

	public sealed partial class CookieNotificationPage
	{
		private readonly IWebDriver _driver;
		public readonly By NotificationBy = By.CssSelector(".cc-banner");

		public CookieNotificationPage(IWebDriver driver) => _driver = driver;

		[PageObject]
		private IWebElement LearnMoreEl =>
			_driver.Wait().Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".cc-link")));

		[PageObject]
		private IWebElement OkEl =>
			_driver.Wait().Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".cc-btn.cc-dismiss")));
	}
}