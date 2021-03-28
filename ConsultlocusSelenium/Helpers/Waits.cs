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
            catch (WebDriverTimeoutException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}