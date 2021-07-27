using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ConsultlocusSelenium.Helpers
{
    public static class Waits
    {
        public static void ImplicitWait(TimeSpan timeSpan)
        {
            Thread.Sleep(timeSpan);
        }

        public static IWebElement WaitUntilElementLoads(IWebDriver driver, TimeSpan timeSpan, By by)
        {
            try
            {
                var element = new WebDriverWait(driver, timeSpan)
                    .Until(drv => drv.FindElement(by));
                return element;
            }
            catch (WebDriverTimeoutException)
            {
                return null;
            }
        }

        public static bool WaitUntilElementClickable(IWebDriver driver, TimeSpan timeSpan, IWebElement element)
        {
            try
            {
                var wait = new WebDriverWait(driver, timeSpan);
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public static IWebElement WaitUntilElementVisible(IWebDriver driver, TimeSpan timeSpan, By by)
        {
            try
            {
                var wait = new WebDriverWait(driver, timeSpan);
                var obj = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
                return obj;
            }
            catch (WebDriverTimeoutException)
            {
                return null;
            }
        }

        public static IWebElement WaitUntilElementClickable(IWebDriver driver, TimeSpan timeSpan, By by)
        {
            try
            {
                var wait = new WebDriverWait(driver, timeSpan);
                var obj = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(by));
                return obj;
            }
            catch (WebDriverTimeoutException)
            {
                return null;
            }
        }
    }
}