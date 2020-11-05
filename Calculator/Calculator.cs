using NUnit.Framework;
using System.Collections.Generic;
using OpenQA.Selenium.Appium.Windows;
using System;
using OpenQA.Selenium.Appium;
using System.Linq;

namespace Calculator
{
    enum Actions { plusButton, minusButton, multiplyButton, divideButton, x2power, squareRootButton, invertButton }

    public class Tests
    {
        private const string appName = "Calculator";
        private const string clearEntry = "Clear entry";

        private const string ipAddress = "http://127.0.0.1:4723/wd/hub";

        private static WindowsDriver<WindowsElement> _driver;

        private string numberButtons(int number) => $"num{number}Button";

        private AppiumOptions appCapabilities;

        private int number = 9;
        private int zero = 0;

        #region Positive Tests

        /// <summary>
        /// 
        /// </summary>
        [OneTimeSetUp]
        public void OpenCalculator()
        {
            appCapabilities = new AppiumOptions();
            appCapabilities.AddAdditionalCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            appCapabilities.AddAdditionalCapability("deviceName", "WIN10KIEVVM02");
            _driver = new WindowsDriver<WindowsElement>(new Uri(ipAddress), appCapabilities);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test, Order(1)]
        public void VerifyAdding()
        {
            Assert.AreEqual($"Display is 18", GetCalculationResults(Actions.plusButton.ToString(), number, number));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test, Order(2)]
        public void VerifySubstraction()
        {
            Assert.AreEqual($"Display is 0", GetCalculationResults(Actions.minusButton.ToString(), number, number));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test, Order(3)]
        public void VerifyMultiply()
        {
            Assert.AreEqual($"Display is 81", GetCalculationResults(Actions.multiplyButton.ToString(), number, number));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test, Order(4)]
        public void VerifyDividing()
        {
            Assert.AreEqual($"Display is 1", GetCalculationResults(Actions.divideButton.ToString(), number, number));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test, Order(6)]
        public void VerifySquareFrom()
        {
            Assert.AreEqual($"Display is 1", GetCalculationResults(Actions.squareRootButton.ToString(), 91, number));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test, Order(7)]
        public void VerifyDividingByModule()
        {
            Assert.AreEqual($"Display is {number}", GetCalculationResults(Actions.divideButton.ToString(), number, number));
            _driver.CloseApp();
        }

        #endregion

        #region Negative Test

        /// <summary>
        /// 
        /// </summary>
        [Test, Order(5)]
        public void VerifyDivideByZero()
        {
            Assert.AreEqual("Display is Cannot divide by zero", GetCalculationResults(Actions.divideButton.ToString(), number, zero));
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action">Actions to perform</param>
        /// <param name="first_argument">First number for calculations</param>
        /// <param name="second_argument">Second number for calculations</param>
        /// <returns>Result on display</returns>
        public string GetCalculationResults(string action, int first_argument = 0, int second_argument = 0)
        {
            _driver.FindElementByName(appName);
            _driver.FindElementByAccessibilityId(numberButtons(first_argument)).Click();
            _driver.FindElementByAccessibilityId(action).Click();
            _driver.FindElementByAccessibilityId(numberButtons(second_argument)).Click();
            _driver.FindElementByName("Equals").Click();

            return _driver.FindElementByAccessibilityId("CalculatorResults").Text;
        }

        [TearDown]
        public void TearDown()
        {
            _driver.FindElementByName(clearEntry).Click();
        }
    }
}