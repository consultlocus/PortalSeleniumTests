using System;
using System.Collections.Generic;
using System.Text;
using ConsultlocusSelenium.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace ConsultlocusSelenium.Tests.Design.Grids
{
    public class GridDataTypesTests
    {
        private IWebDriver _driver;
        private IWebElement _avatarButton;
        private IWebElement _addNewRowButton;

        private bool _stopTests;
        private bool _gridFound;
        private bool _gridClean;

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
                return;
            }

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

            var gridEntry = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//span[contains(text(), 'Test Features')]"));
            var treeIndex = int.Parse(gridEntry.GetAttribute("data-treeindex"));

            var gridEditButton =
                _driver.FindElement(By.XPath($"//span[@data-treeindex='{treeIndex}']//a[text()='View']"));
            gridEditButton.Click();

            var gridViewRadioButton = Waits.WaitUntilElementVisible(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//input[@type='radio']"));
            if (gridViewRadioButton == null)
            {
                Assert.Fail("Could not load the 'Test Features DefaultView' radio button in under 5 seconds!");
            }
            gridViewRadioButton.Click();

            var gridViewSelectButton = _driver.FindElement(By.XPath("//button[text()='Select']"));
            gridViewSelectButton.Click();

            _addNewRowButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
               By.XPath("//button[text()='Add New']"));
            if (_addNewRowButton == null)
            {
                Assert.Fail("Could not load the 'Add New' button in under 5 seconds!");
            }
            _gridFound = true;
            _gridClean = true;
        }

        [OneTimeTearDown]
        public void OtTearDown()
        {
            //_driver.Quit();
        }

        [SetUp]
        public void SetUp()
        {
            Assume.That(_stopTests, Is.False, "Stopped testing because login failed!");
            Assume.That(_gridFound, Is.True, "Stopped testing because could not locate the features grid!");
            Assume.That(_gridClean, Is.True, "Stopped testing because grid is filled with data that were not removed!");
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Order(1)]
        [Test]
        public void IsDefaultValuesWorking()
        {
            _addNewRowButton.Click();
            var createButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[contains(text(), 'Create')]"));
            if (createButton == null)
            {
                Assert.Fail("Could not load the 'Create' button in under 5 seconds!");
            }
            createButton.Click();
            _driver.Navigate().Refresh();
            _addNewRowButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[text()='Add New']"));
            _gridClean = false;
            if (_addNewRowButton == null)
            {
                Assert.Fail("Could not load the 'Add New' button in under 5 seconds, after the default values had been used!");
            }

            var addedRow = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//tr[@kendogridlogicalrow and @role='row' and @data-kendo-grid-item-index='0' ]"));
            if (addedRow == null)
            {
                Assert.Fail("Failed to create a row using default values!");
            }

            Actions actions = new Actions(_driver);
            actions.ContextClick(addedRow).Perform();
            var deleteRowButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//li[contains(text(), 'Delete')]"));
            if (deleteRowButton == null)
            {
                Assert.Fail("Failed to delete a row!");
            }
            deleteRowButton.Click();
            _gridClean = true;
        }

        [Order(2)]
        [Test]
        public void IsTextWorking()
        {
            _addNewRowButton.Click();
            var createButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[contains(text(), 'Create')]"));
            if (createButton == null)
            {
                Assert.Fail("Could not load the 'Create' button in under 5 seconds!");
            }

            var textInput = _driver.FindElement(By.XPath("//input[@ng-reflect-name='Text']"));
            textInput.SendKeys("TESTING");

            createButton.Click();
            _driver.Navigate().Refresh();
            _addNewRowButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[text()='Add New']"));
            _gridClean = false;
            if (_addNewRowButton == null)
            {
                Assert.Fail("Could not load the 'Add New' button in under 5 seconds, after the text value had been set!");
            }

            var addedRow = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//tr[@kendogridlogicalrow and @role='row' and @data-kendo-grid-item-index='0' ]"));
            if (addedRow == null)
            {
                Assert.Fail("Failed to create a row using \"TESTING\" text value!");
            }

            var testCell = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//td[@kendogridlogicalcell and @role='gridcell' and text()='TESTING']"));
            if (testCell == null)
            {
                Assert.Fail("There is no cell with \"TESTING\" text value!");
            }

            Actions actions = new Actions(_driver);
            actions.ContextClick(addedRow).Perform();
            var deleteRowButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//li[contains(text(), 'Delete')]"));
            if (deleteRowButton == null)
            {
                Assert.Fail("Failed to delete a row!");
            }
            deleteRowButton.Click();
            _gridClean = true;
        }
    }
}