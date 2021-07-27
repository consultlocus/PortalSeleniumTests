using System;
using ConsultlocusSelenium.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace ConsultlocusSelenium.Tests.CompanyUser.Design.Grids
{
    public class GridDataTypesTests
    {
        private IWebDriver _driver;
        private IWebElement _avatarButton;
        private IWebElement _addNewRowButton;
        private IWebElement _activeRow;

        private bool _stopTests;
        private bool _gridFound;

        [OneTimeSetUp]
        public void OtSetup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--disable-notifications");

            //If you want to run the tests without opening the browser, uncomment this
            //If you want to run the tests with a browser gui, comment this
            options.AddArgument("headless");

            _driver = new ChromeDriver(options);

            _avatarButton = Helpers.LogIn.LogInWithCompanyUserSecret(_driver);
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

            var gridEntry = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//div[contains(text(), 'SeleniumTestDataTypeGrid')]"));
            if (gridEntry == null)
            {
                _gridFound = false;
                Assert.Fail("Could not find the 'SeleniumTestDataTypeGrid' grid in under 5 seconds!");
            }
            _gridFound = true;

            var dataSid = int.Parse(gridEntry.GetAttribute("data-sid").Replace("element", ""));

            var gridViewButton =
                _driver.FindElement(By.XPath($"//a[@data-sid='viewButton{dataSid}']"));
            gridViewButton.Click();

            var gridViewRadioButton = Waits.WaitUntilElementVisible(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//input[@type='radio']"));
            if (gridViewRadioButton == null)
            {
                Assert.Fail("Could not load the 'SeleniumTestDataTypeGrid DefaultView' radio button in under 5 seconds!");
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
        }

        [OneTimeTearDown]
        public void OtTearDown()
        {
            _driver.Quit();
        }

        [SetUp]
        public void SetUp()
        {
            Assume.That(_stopTests, Is.False, "Stopped testing because login failed!");
            Assume.That(_gridFound, Is.True, "Stopped testing because could not locate the features grid!");
        }

        [TearDown]
        public void TearDown()
        {
            //Comment this if you dont want to delete any of the created rows
            RemoveRow(_driver, _activeRow);
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
            var typeInput = _driver.FindElement(By.XPath("//input[@ng-reflect-name='rowTypeCheckbox']"));
            typeInput.Click();
            createButton.Click();
            _driver.Navigate().Refresh();
            _addNewRowButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[text()='Add New']"));
            if (_addNewRowButton == null)
            {
                Assert.Fail("Could not load the 'Add New' button in under 5 seconds, after the default values had been used!");
            }

            _activeRow = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//tr[@kendogridlogicalrow and @role='row' and @data-kendo-grid-item-index='0' ]"));
            if (_activeRow == null)
            {
                Assert.Fail("Failed to create a row using default values!");
            }
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
            var typeInput = _driver.FindElement(By.XPath("//input[@ng-reflect-name='rowTypeCheckbox']"));
            typeInput.Click();

            var textInput = _driver.FindElement(By.XPath("//input[@ng-reflect-name='Text']"));
            textInput.SendKeys("TESTING");

            createButton.Click();
            _driver.Navigate().Refresh();
            _addNewRowButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[text()='Add New']"));
            if (_addNewRowButton == null)
            {
                Assert.Fail("Could not load the 'Add New' button in under 5 seconds, after the text value had been set!");
            }

            _activeRow = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//tr[@kendogridlogicalrow and @role='row' and @data-kendo-grid-item-index='0' ]"));
            if (_activeRow == null)
            {
                Assert.Fail("Failed to create a row using \"TESTING\" text value!");
            }

            var testCell = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//td[@kendogridlogicalcell and @role='gridcell' and text()='TESTING']"));
            if (testCell == null)
            {
                Assert.Fail("There is no cell with \"TESTING\" text value!");
            }
        }

        [Order(3)]
        [Test]
        public void IsNumberWorking()
        {
            _addNewRowButton.Click();
            var createButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[contains(text(), 'Create')]"));
            if (createButton == null)
            {
                Assert.Fail("Could not load the 'Create' button in under 5 seconds!");
            }
            var typeInput = _driver.FindElement(By.XPath("//input[@ng-reflect-name='rowTypeCheckbox']"));
            typeInput.Click();

            var textInput = _driver.FindElement(By.XPath("//span[@class='k-numeric-wrap']/input[@role='spinbutton']"));
            textInput.SendKeys("23.23");

            createButton.Click();
            _driver.Navigate().Refresh();
            _addNewRowButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[text()='Add New']"));
            if (_addNewRowButton == null)
            {
                Assert.Fail("Could not load the 'Add New' button in under 5 seconds, after the text value had been set!");
            }

            _activeRow = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//tr[@kendogridlogicalrow and @role='row' and @data-kendo-grid-item-index='0' ]"));
            if (_activeRow == null)
            {
                Assert.Fail("Failed to create a row using \"23.23\" numeric value!");
            }
            var testCell = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//td[@kendogridlogicalcell and @role='gridcell' and text()='23.23']"));
            if (testCell == null)
            {
                Assert.Fail("There is no cell with \"23.23\" numeric value!");
            }
        }

        [Order(4)]
        [Test]
        public void IsBooleanWorking()
        {
            _addNewRowButton.Click();
            var createButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[contains(text(), 'Create')]"));
            if (createButton == null)
            {
                Assert.Fail("Could not load the 'Create' button in under 5 seconds!");
            }
            var typeInput = _driver.FindElement(By.XPath("//input[@ng-reflect-name='rowTypeCheckbox']"));
            typeInput.Click();

            var textInput = _driver.FindElement(By.XPath("//kendo-switch[@ng-reflect-name='Boolean']"));
            textInput.Click();

            createButton.Click();
            _driver.Navigate().Refresh();
            _addNewRowButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[text()='Add New']"));
            if (_addNewRowButton == null)
            {
                Assert.Fail("Could not load the 'Add New' button in under 5 seconds, after the text value had been set!");
            }

            _activeRow = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//tr[@kendogridlogicalrow and @role='row' and @data-kendo-grid-item-index='0' ]"));
            if (_activeRow == null)
            {
                Assert.Fail("Failed to create a row using \"true\" boolean value!");
            }
            var testCell = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//td[@kendogridlogicalcell and @role='gridcell' and text()='true']"));
            if (testCell == null)
            {
                Assert.Fail("There is no cell with \"true\" boolean value!");
            }
        }

        [Order(5)]
        [Test]
        public void IsListWorking()
        {
            _addNewRowButton.Click();
            var createButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[contains(text(), 'Create')]"));
            if (createButton == null)
            {
                Assert.Fail("Could not load the 'Create' button in under 5 seconds!");
            }
            var typeInput = _driver.FindElement(By.XPath("//input[@ng-reflect-name='rowTypeCheckbox']"));
            typeInput.Click();

            var textInput = _driver.FindElement(By.XPath("//select[@ng-reflect-name='List']"));
            textInput.Click();

            var numericOption = Helpers.Waits.WaitUntilElementVisible(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//select[@ng-reflect-name='List']/option[contains(text(), 'Urgent')]"));
            if (numericOption == null)
            {
                Assert.Fail("Could not load the List options  in under 5 seconds!");
            }

            numericOption.Click();

            createButton.Click();
            _driver.Navigate().Refresh();
            _addNewRowButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[text()='Add New']"));
            if (_addNewRowButton == null)
            {
                Assert.Fail("Could not load the 'Add New' button in under 5 seconds, after the text value had been set!");
            }

            _activeRow = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//tr[@kendogridlogicalrow and @role='row' and @data-kendo-grid-item-index='0' ]"));
            if (_activeRow == null)
            {
                Assert.Fail("Failed to create a row using \"1. Highest - Urgent\" list value!");
            }
            var testCell = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//td[@kendogridlogicalcell and @role='gridcell']//select[@ng-reflect-model='0']"));
            if (testCell == null)
            {
                Assert.Fail("There is no cell with \"1. Highest - Urgent\" list value!");
            }
        }

        [Order(6)]
        [Test]
        public void IsDateWorking()
        {
            _addNewRowButton.Click();
            var createButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[contains(text(), 'Create')]"));
            if (createButton == null)
            {
                Assert.Fail("Could not load the 'Create' button in under 5 seconds!");
            }
            var typeInput = _driver.FindElement(By.XPath("//input[@ng-reflect-name='rowTypeCheckbox']"));
            typeInput.Click();

            var textInput = _driver.FindElement(By.XPath("//span[@class='k-dateinput-wrap']/input[@role='spinbutton']"));
            textInput.SendKeys(Keys.ArrowLeft);
            textInput.SendKeys(Keys.ArrowLeft);
            textInput.SendKeys(Keys.ArrowLeft);
            textInput.SendKeys(Keys.ArrowLeft);
            textInput.SendKeys(Keys.ArrowLeft);
            textInput.SendKeys(Keys.ArrowLeft);
            textInput.SendKeys("61520001200");

            createButton.Click();
            _driver.Navigate().Refresh();
            _addNewRowButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[text()='Add New']"));
            if (_addNewRowButton == null)
            {
                Assert.Fail("Could not load the 'Add New' button in under 5 seconds, after the date value had been set!");
            }

            _activeRow = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//tr[@kendogridlogicalrow and @role='row' and @data-kendo-grid-item-index='0' ]"));
            if (_activeRow == null)
            {
                Assert.Fail("Failed to create a row using \"6/15/2000 12:00 AM\" date value!");
            }
            var testCell = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//td[@kendogridlogicalcell and @role='gridcell' and text()='6/15/2000 12:00 AM']"));
            if (testCell == null)
            {
                Assert.Fail("There is no cell with \"6/15/2000 12:00 AM\" date value!");
            }
        }

        [Order(7)]
        [Test]
        public void IsCalculationalWorking()
        {
            _addNewRowButton.Click();
            var createButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[contains(text(), 'Create')]"));
            if (createButton == null)
            {
                Assert.Fail("Could not load the 'Create' button in under 5 seconds!");
            }
            var typeInput = _driver.FindElement(By.XPath("//input[@ng-reflect-name='rowTypeCheckbox']"));
            typeInput.Click();

            var numericInput = _driver.FindElement(By.XPath("//span[@class='k-numeric-wrap']/input[@role='spinbutton']"));
            numericInput.SendKeys("44");

            var calcInput = _driver.FindElement(By.XPath("//input[@ng-reflect-name='Calculational']"));
            calcInput.SendKeys("[2]+66");

            createButton.Click();
            _driver.Navigate().Refresh();
            _addNewRowButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[text()='Add New']"));
            if (_addNewRowButton == null)
            {
                Assert.Fail("Could not load the 'Add New' button in under 5 seconds, after the text value had been set!");
            }

            _activeRow = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//tr[@kendogridlogicalrow and @role='row' and @data-kendo-grid-item-index='0' ]"));
            if (_activeRow == null)
            {
                Assert.Fail("Failed to create a row using \"[2]+66\" calculational value!");
            }
            var testCell = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//td[@kendogridlogicalcell and @role='gridcell' and text()='110']"));
            if (testCell == null)
            {
                Assert.Fail("There is no cell with \"110\" numeric value!");
            }
        }

        [Order(8)]
        [Test]
        public void IsPictureAndFileWorking()
        {
            _addNewRowButton.Click();
            var createButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[contains(text(), 'Create')]"));
            if (createButton == null)
            {
                Assert.Fail("Could not load the 'Create' button in under 5 seconds!");
            }
            var typeInput = _driver.FindElement(By.XPath("//input[@ng-reflect-name='rowTypeCheckbox']"));
            typeInput.Click();

            var filesInput = _driver.FindElements(By.XPath("//input[@name='files']"));
            foreach (IWebElement element in filesInput)
            {
                element.SendKeys("C:/Selenium/001.jpg");
            }
            //Wait 2 seconds to finish upload
            Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(1), By.XPath("//input[@id='0']"));

            createButton.Click();
            _driver.Navigate().Refresh();
            _addNewRowButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[text()='Add New']"));
            if (_addNewRowButton == null)
            {
                Assert.Fail("Could not load the 'Add New' button in under 5 seconds, after the text value had been set!");
            }

            _activeRow = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//tr[@kendogridlogicalrow and @role='row' and @data-kendo-grid-item-index='0' ]"));
            if (_activeRow == null)
            {
                Assert.Fail("Failed to create a row using picture and file value!");
            }
            var testCell = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//td[@kendogridlogicalcell and @role='gridcell']/a[text()='001.jpg']"));
            if (testCell == null)
            {
                Assert.Fail("There is no cell with file set correctly!");
            }
            testCell = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//img"));
            if (testCell == null)
            {
                Assert.Fail("There is no cell with picture set correctly!");
            }
        }

        [Theory]
        public static bool RemoveRow(IWebDriver _driver, IWebElement row)
        {
            var rowIndex = row.GetAttribute("data-kendo-grid-item-index");
            var colIdCell = _driver.FindElement(By.XPath($"//td[@data-kendo-grid-column-index='0' and @ng-reflect-data-row-index='{rowIndex}']"));
            Actions actions = new Actions(_driver);
            actions.ContextClick(colIdCell).Perform();
            var deleteRowButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//li[contains(text(), 'Delete')]"));
            if (deleteRowButton == null)
            {
                Assert.Fail("Failed to delete a row!");
                return false;
            }

            actions.MoveToElement(deleteRowButton).Click().Perform();
            return true;
        }
    }
}