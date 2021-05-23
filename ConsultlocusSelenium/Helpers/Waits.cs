using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ConsultlocusSelenium.Helpers
{
    public static class Waits
    {
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

        public static bool WaitUntilElementVisible(IWebDriver driver, TimeSpan timeSpan, IWebElement element)
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