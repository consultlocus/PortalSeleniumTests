using System;
using System.Collections.Generic;
using System.Text;
using ConsultlocusSelenium.Tests.SuperAdmin.LogIn;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;

namespace ConsultlocusSelenium.Helpers
{
    public static class LogIn
    {
        private static IConfigurationRoot _configurationSA = new ConfigurationBuilder()
            .AddUserSecrets<LoginTests>()
            .Build();

        private static IConfigurationRoot _configurationLA = new ConfigurationBuilder()
            .AddUserSecrets<Tests.LocalAdmin.LogIn.LoginTests>()
            .Build();

        private static IConfigurationRoot _configurationCU = new ConfigurationBuilder()
            .AddUserSecrets<Tests.CompanyUser.LogIn.LoginTests>()
            .Build();

        public static TimeSpan WaitTime = TimeSpan.FromSeconds(5);

        public static IWebElement LogInWithSuperAdminSecret(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(@"https://test.consultlocus.com");

            //Filling the login form
            driver.FindElement(By.CssSelector("input[formcontrolname = 'email']")).SendKeys(_configurationSA["CredentialsSA:Login"]);
            var pass = driver.FindElement(By.CssSelector("input[type = 'password']"));
            pass.SendKeys(_configurationSA["CredentialsSA:Password"]);
            pass.Submit();

            //Checking if the avatar button on the upper right side has been loaded
            var avatarButton = Helpers.Waits.WaitUntilElementLoads(driver, WaitTime,
                By.CssSelector("button[class='HeaderNavMainLink dontCloseModal mr-2']"));

            return avatarButton;
        }

        public static IWebElement LogInWithLocalAdminSecret(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(@"https://test.consultlocus.com");

            //Filling the login form
            driver.FindElement(By.CssSelector("input[formcontrolname = 'email']")).SendKeys(_configurationLA["CredentialsLA:Login"]);
            var pass = driver.FindElement(By.CssSelector("input[type = 'password']"));
            pass.SendKeys(_configurationLA["CredentialsLA:Password"]);
            pass.Submit();

            //Checking if the avatar button on the upper right side has been loaded
            var avatarButton = Helpers.Waits.WaitUntilElementLoads(driver, WaitTime,
                By.CssSelector("button[class='HeaderNavMainLink dontCloseModal mr-2']"));

            return avatarButton;
        }

        public static IWebElement LogInWithCompanyUserSecret(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(@"https://test.consultlocus.com");

            //Filling the login form
            driver.FindElement(By.CssSelector("input[formcontrolname = 'email']")).SendKeys(_configurationCU["CredentialsCU:Login"]);
            var pass = driver.FindElement(By.CssSelector("input[type = 'password']"));
            pass.SendKeys(_configurationCU["CredentialsCU:Password"]);
            pass.Submit();

            //Checking if the avatar button on the upper right side has been loaded
            var avatarButton = Helpers.Waits.WaitUntilElementLoads(driver, WaitTime,
                By.CssSelector("button[class='HeaderNavMainLink dontCloseModal mr-2']"));

            return avatarButton;
        }
    }
}