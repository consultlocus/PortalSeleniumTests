using System;
using ConsultlocusSelenium.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace ConsultlocusSelenium.Tests.LocalAdmin.Design
{
    public class ApplicationsTests
    {
        private IWebDriver _driver;
        private IWebElement _avatarButton;

        private bool _stopTests;

        [OneTimeSetUp]
        public void OtSetup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--disable-notifications");

            //If you want to run the tests without opening the browser, uncomment this
            //If you want to run the tests with a browser gui, comment this
            options.AddArgument("headless");

            _driver = new ChromeDriver(options);

            _avatarButton = Helpers.LogIn.LogInWithLocalAdminSecret(_driver);

            if (_avatarButton == null)
            {
                _stopTests = true;
                Assume.That(_stopTests, Is.False, "Stopped testing because login has failed!");
            }
        }

        [OneTimeTearDown]
        public void OtTearDown()
        {
            _driver.Quit();
        }

        [SetUp]
        public void SetUp()
        {
            Assume.That(_stopTests, Is.False, "Stopped testing because the action before failed!");
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != NUnit.Framework.Interfaces.TestStatus.Passed)
            {
                _stopTests = true;
            }
        }

        [Order(1)]
        [Test]
        public void CanLocalAdminViewApplicationsTab()
        {
            _avatarButton.Click();

            var designButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.CssSelector("kendo-panelbar-item[ng-reflect-title = 'Design']"));
            if (designButton == null)
            {
                Assert.Fail("Could not load the 'Design' button in under 5 seconds!");
            }
            designButton.Click();

            var applicationsButton = Waits.WaitUntilElementVisible(_driver, TimeSpan.FromSeconds(5),
                By.CssSelector("kendo-panelbar-item[ng-reflect-title = 'Applications']"));
            if (applicationsButton == null)
            {
                Assert.Pass("Could not load the 'Applications' button in under 5 seconds!");
            }
            else
            {
                Assert.Fail("Local admin has access to the applications tab!");
            }
        }
    }
}