using System;
using System.IO;
using ConsultlocusSelenium.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ConsultlocusSelenium.Tests.SuperAdmin.Design
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
            // options.AddArgument("headless");

            _driver = new ChromeDriver(options);

            _avatarButton = Helpers.LogIn.LogInWithSuperAdminSecret(_driver);

            if (_avatarButton == null)
            {
                _stopTests = true;
                Assume.That(_stopTests, Is.False, "Stopped testing because login has failed!");
            }

            //create log file
            Logger.CreateLog(this.GetType().Name);
        }

        [OneTimeTearDown]
        public void OtTearDown()
        {
            //sending email with problems
            if (!Logger.IsLogFileEmpty(this.GetType().Name)) Emailer.SendEmail("Problems with " + this.GetType().Name, Logger.GetLinesFromLogFile(this.GetType().Name));

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
                Logger.WriteLinesToLog(this.GetType().Name, TestContext.CurrentContext.Result.Message);
            }
        }

        [Order(1)]
        [Test]
        public void TextTemplatesListTest()
        {
            _avatarButton.Click();

            var designButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//div/a[contains(text(),'Design')]/.."));
            if (designButton == null)
            {
                Assert.Fail("Could not load the 'Design' button in under 5 seconds!");
            }
            designButton.Click();

            var textTemplatesButton = Waits.WaitUntilElementVisible(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//div/a[contains(text(),'Text Templates')]/.."));
            if (textTemplatesButton == null)
            {
                Assert.Fail("Could not load the 'Text Templates' button in under 5 seconds!");
            }
            textTemplatesButton.Click();

            var newTextTemplateButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.CssSelector("a[routerlink = 'create']"));
            if (newTextTemplateButton == null)
            {
                Assert.Fail("Could not load the 'New Text Templates' button in under 5 seconds!");
            }
            Console.WriteLine("List of Text Templates loaded successfully!");
            Assert.Pass("List of Text Templates loaded successfully!");
        }

        [Order(2)]
        [Test]
        public void TextTemplatesCreateTest()
        {
            _driver.FindElement(By.CssSelector("a[routerlink = 'create']")).Click();
            var newTextTemplateNameInput = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//input[@name='nameButton']"));
            if (newTextTemplateNameInput == null)
            {
                Assert.Fail("Could not load the 'Name' input in under 5 seconds!");
            }

            newTextTemplateNameInput.SendKeys("Selenium Test Name");

            var textAreaFrame = _driver.FindElement(By.CssSelector("iframe[class = 'k-iframe']"));
            _driver.SwitchTo().Frame(textAreaFrame);

            var textArea = _driver.FindElement(By.CssSelector("div[contenteditable = 'true']"));
            textArea.Click();
            textArea.SendKeys("Selenium Test Content");

            _driver.SwitchTo().DefaultContent();
            Waits.ImplicitWait(TimeSpan.FromSeconds(0.5));
            _driver.FindElement(By.XPath("//button[@data-sid='createButton']")).Click();

            var newTextTemplateButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.CssSelector("a[routerlink = 'create']"));
            if (newTextTemplateButton == null)
            {
                Assert.Fail("Could not load the 'New Text Templates' button in under 5 seconds!");
            }

            var textTemplateEntry = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//div[contains(text(), 'Selenium Test Name')]"));
            if (textTemplateEntry == null)
            {
                Assert.Fail("Could not create (or display on the list) a Text Template!");
            }

            Console.WriteLine("Text Template created successfully!");
            Assert.Pass("Text Template created successfully!");
        }

        [Order(3)]
        [Test]
        public void TextTemplatesViewTest()
        {
            var textTemplateEntry = _driver.FindElement(By.XPath("//div[contains(text(), 'Selenium Test Name')]"));
            var dataSid = int.Parse(textTemplateEntry.GetAttribute("data-sid").Replace("element", ""));

            var textTemplateViewButton =
                _driver.FindElement(By.XPath($"//a[@data-sid='viewButton{dataSid}']"));
            textTemplateViewButton.Click();
            var paragraph = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//p[text()='Selenium Test Content']"));

            if (paragraph == null)
            {
                Assert.Fail("Could not load the content of the Text Template in under 5 seconds (or the content is incorrect)!");
            }

            _driver.Navigate().Back();
            var newTextTemplateButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.CssSelector("a[routerlink = 'create']"));
            if (newTextTemplateButton == null)
            {
                Assert.Fail("Could not load the 'New Text Templates' button in under 5 seconds!");
            }
            textTemplateEntry = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//div[contains(text(), 'Selenium Test Name')]"));
            if (textTemplateEntry == null)
            {
                Assert.Fail("Could not create (or display on the list) a Text Template!");
            }
            Console.WriteLine("Text Template viewed successfully!");
            Assert.Pass("Text Template viewed successfully!");
        }

        [Order(4)]
        [Test]
        public void TextTemplatesEditTest()
        {
            var textTemplateEntry = _driver.FindElement(By.XPath("//div[contains(text(), 'Selenium Test Name')]"));
            var dataSid = int.Parse(textTemplateEntry.GetAttribute("data-sid").Replace("element", ""));

            var textTemplateEditButton =
                _driver.FindElement(By.XPath($"//a[@data-sid='editButton{dataSid}']"));
            textTemplateEditButton.Click();

            var nextButton = Helpers.Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[text()='Next']"));
            if (nextButton == null)
            {
                Assert.Fail("The 'Choose language' dialog did not appear!");
            }
            nextButton.Click();
            Waits.ImplicitWait(TimeSpan.FromSeconds(0.5));
            var editTextTemplateNameInput = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.CssSelector("input[name = 'nameButton']"));
            if (editTextTemplateNameInput == null)
            {
                Assert.Fail("Could not load the 'Name' input in under 5 seconds!");
            }
            editTextTemplateNameInput.SendKeys(" EDITED");

            var textAreaFrame = _driver.FindElement(By.CssSelector("iframe[class = 'k-iframe']"));
            _driver.SwitchTo().Frame(textAreaFrame);

            var textArea = _driver.FindElement(By.XPath("//div[@contenteditable='true']"));
            textArea.Click();

            textArea.SendKeys("\nSelenium Test Content Edited");

            _driver.SwitchTo().DefaultContent();
            Waits.ImplicitWait(TimeSpan.FromSeconds(0.5));
            _driver.FindElement(By.XPath("//*[text()='Update']")).Click();

            var newTextTemplateButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.CssSelector("a[routerlink = 'create']"));
            if (newTextTemplateButton == null)
            {
                Assert.Fail("Could not load the 'New Text Templates' button in under 5 seconds!");
            }

            textTemplateEntry = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//div[contains(text(), 'Selenium Test Name EDITED')]"));
            if (textTemplateEntry == null)
            {
                Assert.Fail("Could not edit (or display on the list) the name of a Text Template!");
            }

            dataSid = int.Parse(textTemplateEntry.GetAttribute("data-sid").Replace("element", ""));

            var textTemplateViewButton =
                _driver.FindElement(By.XPath($"//a[@data-sid='viewButton{dataSid}']"));
            textTemplateViewButton.Click();
            var paragraph = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//p[text()='Selenium Test Content Edited']"));

            if (paragraph == null)
            {
                Assert.Fail("Could not load the content of the Text Template in under 5 seconds (or the content is incorrectly edited)!");
            }

            _driver.Navigate().Back();

            newTextTemplateButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.CssSelector("a[routerlink = 'create']"));
            if (newTextTemplateButton == null)
            {
                Assert.Fail("Could not load the 'New Text Templates' button in under 5 seconds!");
            }
            textTemplateEntry = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//div[contains(text(), 'Selenium Test Name EDITED')]"));
            if (textTemplateEntry == null)
            {
                Assert.Fail("Could not edit (or display on the list) a Text Template!");
            }
            Console.WriteLine("Text Template edited successfully!");
            Assert.Pass("Text Template edited successfully!");
        }

        [Order(4)]
        [Test]
        public void TextTemplatesDeleteTest()
        {
            var textTemplateEntry = _driver.FindElement(By.XPath("//div[contains(text(), 'Selenium Test Name EDITED')]"));
            var dataSid = int.Parse(textTemplateEntry.GetAttribute("data-sid").Replace("element", ""));

            var textTemplateDeleteButton =
                _driver.FindElement(By.XPath($"//a[@data-sid='deteButton{dataSid}']"));
            textTemplateDeleteButton.Click();
            var nextButton = Helpers.Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.XPath("//button[text()='Delete']"));
            if (nextButton == null)
            {
                Assert.Fail("The 'Delete element' dialog did not appear!");
            }
            nextButton.Click();

            _driver.Navigate().Refresh();

            var newTextTemplateButton = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5),
                By.CssSelector("a[routerlink = 'create']"));
            if (newTextTemplateButton == null)
            {
                Assert.Fail("Could not load the 'New Text Templates' button in under 5 seconds!");
            }
            textTemplateEntry = Waits.WaitUntilElementLoads(_driver, TimeSpan.FromSeconds(5), By.XPath("//div[contains(text(), 'Selenium Test Name EDITED')]"));
            if (textTemplateEntry != null)
            {
                Assert.Fail("Could not delete a Text Template!");
            }
            Console.WriteLine("Text Template deleted successfully!");
            Assert.Pass("Text Template deleted successfully!");
        }
    }
}