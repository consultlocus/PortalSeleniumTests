using System;
using System.Collections.Generic;
using System.Text;
using ConsultlocusSelenium.Tests.LogIn;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;

namespace ConsultlocusSelenium.Helpers
{
    public static class LogIn
    {
        private static IConfigurationRoot _configuration = new ConfigurationBuilder()
            .AddUserSecrets<LoginTests>()
            .Build();

        public static TimeSpan WaitTime = TimeSpan.FromSeconds(5);

        public static IWebElement LogInWithSecret(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(@"https://app.consultlocus.com");

            //Filling the login form
            driver.FindElement(By.CssSelector("input[formcontrolname = 'email']")).SendKeys(_configuration["Credentials:Login"]);
            var pass = driver.FindElement(By.CssSelector("input[type = 'password']"));
            pass.SendKeys(_configuration["Credentials:Password"]);
            pass.Submit();

            //Checking if the avatar button on the upper right side has been loaded
            var avatarButton = Helpers.Waits.WaitUntilElementLoads(driver, WaitTime,
                By.CssSelector("button[class='HeaderNavMainLink dontCloseModal mr-2']"));

            return avatarButton;
        }
    }
}