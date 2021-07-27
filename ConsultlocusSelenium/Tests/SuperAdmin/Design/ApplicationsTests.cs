using System;
using ConsultlocusSelenium.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace ConsultlocusSelenium.Tests.SuperAdmin.Design
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

            _avatarButton = Helpers.LogIn.LogInWithSuperAdminSecret(_driver);

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
        public void ApplicationsListTest()
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
                Assert.Fail("Could not load the 'Applications' button in under 5 seconds!");
            }
            applicationsButton.Click();

            var newApplicationButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[contains(text(), 'Create New Application')]"));
            if (newApplicationButton == null)
            {
                Assert.Fail("Could not load the 'Create New Application' button in under 5 seconds!");
            }
            Console.WriteLine("List of Applications loaded successfully!");
            Assert.Pass("List of Applications loaded successfully!");
        }

        [Order(2)]
        [Test]
        public void ApplicationCreateTest()
        {
            _driver.FindElement(By.XPath("//button[contains(text(), 'Create New Application')]")).Click();
            var newApplicationNameInput = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//input[@data-sid = 'applicationNameInput']"));
            if (newApplicationNameInput == null)
            {
                Assert.Fail("Could not load the 'Name' input in under 5 seconds!");
            }

            newApplicationNameInput.SendKeys("Selenium Test Name");

            var functionalityToDragAndDrop = _driver.FindElement(By.XPath("//span[contains(text(),' SeleniumTestDataTypeGrid ')]"));
            var whereToDrop = _driver.FindElement(By.XPath("//span[contains(text(),' [Place new item] ')]"));

            Actions actions = new Actions(_driver);
            actions.DragAndDrop(functionalityToDragAndDrop, whereToDrop).Build().Perform();

            _driver.FindElement(By.XPath("//button[contains(text(),'Create')]")).Click();

            var newApplicationButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[contains(text(), 'Create New Application')]"));
            if (newApplicationButton == null)
            {
                Assert.Fail("Could not load the 'Create New Application' button in under 5 seconds!");
            }

            var applicationEntry = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//li[contains(text(), 'Selenium Test Name')]"));
            if (applicationEntry == null)
            {
                Assert.Fail("Could not create (or display on the list) an Application!");
            }
        }

        [Order(3)]
        [Test]
        public void ApplicationViewTest()
        {
            _driver.FindElement(By.XPath("//button[@data-sid='openApplicationLauncherButton']")).Click();
            var applicationChoice = Waits.WaitUntilElementVisible(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//span[contains(text(), 'Selenium Test Name')]"));
            if (applicationChoice == null)
            {
                Assert.Fail("Could not load the 'Selenium Test Name' choice in under 5 seconds!");
            }
            Waits.ImplicitWait(TimeSpan.FromSeconds(1));
            applicationChoice.Click();

            var dropdownRoot = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//a[contains(text(),' SeleniumTestDataTypeGrid ')]"));

            Actions actions = new Actions(_driver);
            actions.MoveToElement(dropdownRoot).Build().Perform();

            var gridViewFromDropdown = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//a[contains(text(),'SeleniumTestDataTypeGrid DefaultView')]"));
            gridViewFromDropdown.Click();

            var addNewRowButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[text()='Add New']"));
            if (addNewRowButton == null)
            {
                Assert.Fail("Could not load the 'Add New' button in under 5 seconds!");
            }
        }

        [Order(4)]
        [Test]
        public void ApplicationEditTest()
        {
            _avatarButton.Click();

            var applicationsButton = Waits.WaitUntilElementVisible(_driver, TimeSpan.FromSeconds(5),
                By.CssSelector("kendo-panelbar-item[ng-reflect-title = 'Applications']"));
            if (applicationsButton == null)
            {
                Assert.Fail("Could not load the 'Applications' button in under 5 seconds!");
            }
            applicationsButton.Click();

            var newApplicationButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[contains(text(), 'Create New Application')]"));
            if (newApplicationButton == null)
            {
                Assert.Fail("Could not load the 'Create New Application' button in under 5 seconds!");
            }
            Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//li[contains(text(), 'Selenium Test Name')]/button[contains(text(), 'Edit')]")).Click();
            var newApplicationNameInput = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//input[@data-sid = 'applicationNameInput']"));
            if (newApplicationNameInput == null)
            {
                Assert.Fail("Could not load the 'Name' input in under 5 seconds!");
            }

            newApplicationNameInput.SendKeys(" EDITED");

            var functionalityToDragAndDrop = _driver.FindElement(By.XPath("//span[contains(text(),' Selenium App Test Template ')]"));
            var whereToDrop = _driver.FindElement(By.XPath("//div[@class='col']/kendo-textbox-container/../kendo-treeview"));

            Actions actions = new Actions(_driver);
            actions.DragAndDrop(functionalityToDragAndDrop, whereToDrop).Build().Perform();

            _driver.FindElement(By.XPath("//button[contains(text(),'Update')]")).Click();

            newApplicationButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[contains(text(), 'Create New Application')]"));
            if (newApplicationButton == null)
            {
                Assert.Fail("Could not load the 'Create New Application' button in under 5 seconds!");
            }

            var applicationEntry = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//li[contains(text(), 'Selenium Test Name EDITED')]"));
            if (applicationEntry == null)
            {
                Assert.Fail("Could not edit (or display on the list) an Application!");
            }

            _driver.Navigate().Refresh();

            var dropdownRoot = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//a[contains(text(),' SeleniumTestDataTypeGrid ')]"));

            actions = new Actions(_driver);
            actions.MoveToElement(dropdownRoot).Build().Perform();

            var textTemplateFromDropdown = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//a[contains(text(),'Selenium App Test Template')]"));
            textTemplateFromDropdown.Click();
            var paragraph = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//p[text()='Selenium Content']"));

            if (paragraph == null)
            {
                Assert.Fail("Could not load the content of the Text Template in under 5 seconds (or the content is incorrect)!");
            }

            _driver.Navigate().Back();
        }

        [Order(5)]
        [Test]
        public void ApplicationDeleteTest()
        {
            Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//li[contains(text(), 'Selenium Test Name')]/button[contains(text(), 'Delete')]")).Click();
            Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//div[@role='dialog']//button[contains(text(), 'Delete')]")).Click();
            _driver.Navigate().Refresh();
            var newApplicationButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[contains(text(), 'Create New Application')]"));
            if (newApplicationButton == null)
            {
                Assert.Fail("Could not load the 'Create New Application' button in under 5 seconds!");
            }
            var applicationEntry = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//li[contains(text(), 'Selenium Test Name EDITED')]"));
            if (applicationEntry != null)
            {
                Assert.Fail("Could not edit (or display on the list) an Application!");
            }
        }
    }
}