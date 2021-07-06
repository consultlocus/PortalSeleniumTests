using System;
using ConsultlocusSelenium.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ConsultlocusSelenium.Tests.Design.Grids
{
    public class GridsTests
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
        public void GridsListTest()
        {
            _avatarButton.Click();

            var designButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.CssSelector("kendo-panelbar-item[ng-reflect-title = 'Design']"));
            if (designButton == null)
            {
                Assert.Fail("Could not load the 'Design' button in under 5 seconds!");
            }

            designButton.Click();

            var gridsButton = Waits.WaitUntilElementVisible(_driver, TimeSpan.FromSeconds(5),
                By.CssSelector("kendo-panelbar-item[ng-reflect-title = 'Grids']"));
            if (gridsButton == null)
            {
                Assert.Fail("Could not load the 'Grids' button in under 5 seconds!");
            }

            gridsButton.Click();

            var newGridButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[contains(text(), 'New Grid')]"));
            if (newGridButton == null)
            {
                Assert.Fail("Could not load the 'New Grid' button in under 5 seconds!");
            }

            Console.WriteLine("List of Grids loaded successfully!");
            Assert.Pass("List of Grids loaded successfully!");
        }

        [Order(2)]
        [Test]
        public void GridsCreateTest()
        {
            var newGridButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[contains(text(), 'New Grid')]"));
            if (newGridButton == null)
            {
                Assert.Fail("Could not load the 'New Grid' button in under 5 seconds!");
            }

            newGridButton.Click();

            var gridNameInput = Waits.WaitUntilElementVisible(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//input[@id='modalAppName']"));
            if (gridNameInput == null)
            {
                Assert.Fail("Could not load the 'Grid Name' input in under 5 seconds!");
            }

            gridNameInput.SendKeys("seleniumTestGridName");

            var gridDescriptionTextArea = _driver.FindElement(By.XPath("//textarea[@id='modalAppDescription']"));

            gridDescriptionTextArea.SendKeys("seleniumTestGridDescription");

            var newFieldButton = _driver.FindElement(By.XPath("//button[contains(text(), 'New Field')]"));

            newFieldButton.Click();

            var textFieldNameInput = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//input[@id='fieldName-0']"));
            if (textFieldNameInput == null)
            {
                Assert.Fail("Could not load the Field 'Name' in under 5 seconds!");
            }

            textFieldNameInput.SendKeys("seleniumTestTextField");

            newFieldButton.Click();

            var numberFieldNameInput = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//input[@id='fieldName-1']"));
            if (textFieldNameInput == null)
            {
                Assert.Fail("Could not load the Field 'Name'  in under 5 seconds!");
            }

            numberFieldNameInput.SendKeys("seleniumTestNumberField");

            var numberFieldDataTypeSelect = _driver.FindElement(By.XPath("//select[@id='fieldDataType-1']"));

            numberFieldDataTypeSelect.Click();

            var numericOption = Helpers.Waits.WaitUntilElementVisible(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//select[@id='fieldDataType-1']/option[contains(text(), 'Numeric')]"));
            if (numericOption == null)
            {
                Assert.Fail("Could not load the Field DataType options  in under 5 seconds!");
            }

            numericOption.Click();

            var createButton = _driver.FindElement(By.XPath("//button[contains(text(), 'Create')]"));
            createButton.Click();

            var gridEntry = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//div[contains(text(), 'seleniumTestGridName')]"));
            if (gridEntry == null)
            {
                Assert.Fail("Could not create (or display on the list) a Grid!");
            }

            Console.WriteLine("Grid created successfully!");
            Assert.Pass("Grid created successfully!");
        }

        [Order(3)]
        [Test]
        public void GridsViewTest()
        {
            //TODO check if columns in the GridView are named as they were in create.
        }

        [Order(4)]
        [Test]
        public void GridsEditTest()
        {
            var gridEntry = _driver.FindElement(By.XPath("//div[contains(text(), 'seleniumTestGridName')]"));
            var dataSid = int.Parse(gridEntry.GetAttribute("data-sid").Replace("element", ""));

            var gridEditButton =
                _driver.FindElement(By.XPath($"//a[@data-sid='editButton{dataSid}']"));
            gridEditButton.Click();

            var gridNameInput = Waits.WaitUntilElementVisible(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//input[@id='modalAppName']"));
            if (gridNameInput == null)
            {
                Assert.Fail("Could not load the 'Grid Name' input in under 5 seconds!");
            }

            gridNameInput.SendKeys("EDITED");

            var gridDescriptionTextArea = _driver.FindElement(By.XPath("//textarea[@id='modalAppDescription']"));

            gridDescriptionTextArea.SendKeys("EDITED");

            var textFieldNameInput = Helpers.Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//input[@id='fieldName-0']"));
            if (textFieldNameInput == null)
            {
                Assert.Fail("Could not load the Field 'Name' in under 5 seconds!");
            }

            textFieldNameInput.SendKeys("EDITED");

            var numberFieldNameInput = Helpers.Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//input[@id='fieldName-1']"));
            if (textFieldNameInput == null)
            {
                Assert.Fail("Could not load the Field 'Name'  in under 5 seconds!");
            }

            numberFieldNameInput.SendKeys("EDITED");

            var createButton = _driver.FindElement(By.XPath("//button[contains(text(), 'Update')]"));
            createButton.Click();

            var editedGridEntry = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//div[contains(text(), 'seleniumTestGridNameEDITED')]"));
            if (gridEntry == null)
            {
                Assert.Fail("Could not edit (or display on the list) a Grid!");
            }

            //TODO check if column names are also edited within View.

            Console.WriteLine("Grid edited successfully!");
            Assert.Pass("Grid edited successfully!");
        }

        [Order(5)]
        [Test]
        public void GridsDeleteTest()
        {
            var gridEntry = _driver.FindElement(By.XPath("//div[contains(text(), 'seleniumTestGridNameEDITED')]"));
            var dataSid = int.Parse(gridEntry.GetAttribute("data-sid").Replace("element", ""));

            var gridDeleteButton =
                _driver.FindElement(By.XPath($"//a[@data-sid='deleteButton{dataSid}']"));
            gridDeleteButton.Click();

            var nextButton = Helpers.Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[text()='Delete']"));
            if (nextButton == null)
            {
                Assert.Fail("The 'Delete element' dialog did not appear!");
            }
            nextButton.Click();

            _driver.Navigate().Refresh();

            var newGridButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[contains(text(), 'New Grid')]"));
            if (newGridButton == null)
            {
                Assert.Fail("Could not load the 'New Grid' button in under 5 seconds!");
            }

            gridEntry = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//div[contains(text(), 'seleniumTestGridNameEDITED')]"));
            if (gridEntry != null)
            {
                Assert.Fail("Could not delete a Grid!");
            }
            Console.WriteLine("Grid deleted successfully!");
            Assert.Pass("Grid deleted successfully!");
        }
    }
}