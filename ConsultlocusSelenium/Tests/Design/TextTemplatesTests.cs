using System;
using ConsultlocusSelenium.Helpers;
using ConsultlocusSelenium.Tests.LogIn;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ConsultlocusSelenium.Tests.Design
{
    public class TextTemplatesTests
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
            //options.AddArgument("headless");

            _driver = new ChromeDriver(options);

            _avatarButton = Helpers.LogIn.LogInWithSecret(_driver);

            if (_avatarButton == null)
            {
                _stopTests = true;
                Assume.That(_stopTests, Is.False, "Stopped testing because login has failed!");
            }
        }

        [OneTimeTearDown]
        public void OtTearDown()
        {
            //_driver.Close();
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
        public void TextTemplatesListTest()
        {
            _avatarButton.Click();
            _driver.FindElement(By.CssSelector("kendo-panelbar-item[ng-reflect-title = 'Design']")).Click();
            _driver.FindElement(By.CssSelector("kendo-panelbar-item[ng-reflect-title = 'Text Templates']")).Click();
            var newTextTemplateButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.CssSelector("a[routerlink = 'create']"));
            if (newTextTemplateButton == null)
            {
                Assert.Fail("Could not load the 'New Text Templates' button in under 5 seconds!");
            }
            Console.WriteLine("List of Text Templates loaded successfully!");
            Assert.Pass("List of Text Templates loaded successfully!");
        }
    }
}