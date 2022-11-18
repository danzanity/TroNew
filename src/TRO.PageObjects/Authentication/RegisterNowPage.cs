using Auto.Core.Plus.UI.WaitHelpers;
using OpenQA.Selenium;
using IWebDriver = Auto.Core.Plus.UI.IWebDriver;

namespace TRO.PageObjects.Authentication
{
	public sealed partial class RegisterNowPage : BasePage<RegisterNowPage>
	{
		private readonly IWebDriver _driver;

		public RegisterNowPage(IWebDriver driver) : base(driver) => _driver = driver;

		public override RegisterNowPage WaitForPageToLoad()
		{
			base.WaitForPageToLoad();
			_driver.Wait().Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath("//re-captcha//iframe")));
			_driver.Wait().Until(ExpectedConditions.ElementIsVisible(By.Id("rc-anchor-container")));
			_driver.SwitchTo().DefaultContent();
			return this;
		}
	}
}