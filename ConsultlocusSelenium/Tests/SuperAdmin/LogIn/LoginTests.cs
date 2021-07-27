using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ConsultlocusSelenium.Tests.SuperAdmin.LogIn
{
    public class LoginTests
    {
        private IWebDriver _driver;
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
        public void CorrectCredentialsLoginTest()
        {
            var avatarButton = Helpers.LogIn.LogInWithSuperAdminSecret(_driver);

            if (avatarButton == null)
            {
                Assert.Fail($"Login failed - could not load avatar button in less than {Helpers.LogIn.WaitTime.Seconds} seconds!\nMaybe credentials are wrong?");
            }

            Console.WriteLine("Successfully logged in!");
            Assert.Pass("Successfully logged in!");
        }

        [Order(2)]
        [Test]
        public void LogoutTest()
        {
            //As we are logged in from the first test, the only thing needed is to navigate and click the log out button
            _driver.FindElement(By.CssSelector("button[class='HeaderNavMainLink dontCloseModal mr-2']")).Click();
            _driver.FindElement(By.Id("mectrl_body_signOut")).Click();

            //Waiting for sign in form to load
            var formEmail = Helpers.Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.CssSelector("input[formcontrolname = 'email']"));

            if (formEmail == null)
            {
                Assert.Fail("Logout failed - could not load sign in form in less than 5 seconds!");
            }

            Console.WriteLine("Successfully logged out!");
            Assert.Pass("Successfully logged out!");
        }

        [Order(3)]
        [Test]
        public void IncorrectCredentialsLoginTest()
        {
            //Because our 2nd test logged us out, we can now check for incorrect credentials behaviour

            //Credentials passed are:
            //login: bad
            //password: credentials
            _driver.FindElement(By.CssSelector("input[formcontrolname = 'email']")).SendKeys("bad");
            var pass = _driver.FindElement(By.CssSelector("input[type = 'password']"));
            pass.SendKeys("credentials");
            pass.Submit();

            //Waiting for invalid credentials dialog to pop up
            var failedLoginDialog = Helpers.Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.CssSelector("kendo-dialog[ng-reflect-title = 'Invalid Credentials']"));

            if (failedLoginDialog == null)
            {
                Assert.Fail("Could not load the invalid credentials alert in less than 5 seconds!");
            }

            //Checking if the text in the alert is the same as designed (can be ommited, could be just Assert.Pass(); )
            Assert.That(failedLoginDialog.Text.Equals("Invalid Credentials\r\nCould not log you in with the credentials you entered. Please check your entered email and password.\r\nOkay"),
                "Failed to log in with credentials: \nLogin: bad \nPassword: credentials");
            Console.WriteLine("Failed to log in with credentials: \nLogin: bad \nPassword: credentials");

            //Assert.Pass("Failed to log in with credentials: \nLogin: bad \nPassword: credentials");
        }
    }
}