using Auto.Core.Plus.UI.WaitHelpers;
using OpenQA.Selenium;
using PageObjectSourceGenerator;
using IWebDriver = Auto.Core.Plus.UI.IWebDriver;

namespace TRO.PageObjects.Authentication
{
	public sealed partial class AuthenticationPage : BasePage<AuthenticationPage>
	{
		private readonly IWebDriver _driver;
		private readonly By _loginBy = By.XPath("//div[@class='login-button']/button[@type='submit']");

		public AuthenticationPage(IWebDriver driver) : base(driver) => _driver = driver;

		[PageObject]
		private IWebElement EmailEl =>
			_driver.Wait().Until(ExpectedConditions.ElementExists(By.Id("userName")));

		[PageObject]
		private IWebElement PasswordEl => _driver.FindElementOrDefault(By.Id("password"));

		[PageObject]
		private IWebElement LoginEl =>
			_driver.Wait().Until(ExpectedConditions.ElementToBeClickable(_loginBy));

		[PageObject]
		private IWebElement RegisterNowEl =>
			_driver.Wait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[@href='#/account/register']")));

		[PageObject]
		private IWebElement ForgotPasswordEl =>
			_driver.Wait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[@href='#/account/forgotpassword']")));

		public override AuthenticationPage WaitForPageToLoad()
		{
			base.WaitForPageToLoad();
			_driver.Wait().Until(ExpectedConditions.ElementIsVisible(_loginBy));
			return this;
		}

		public void WaitForPageToUnload() =>
			_driver.Wait().Until(ExpectedConditions.InvisibilityOfElementLocated(_loginBy));
	}
}