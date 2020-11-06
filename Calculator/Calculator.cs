using NUnit.Framework;
using System.Collections.Generic;
using OpenQA.Selenium.Appium.Windows;
using System;
using OpenQA.Selenium.Appium;
using System.Linq;

namespace Calculator
{
    enum Actions { plusButton, minusButton, multiplyButton, divideButton, x2powerButton, squareRootButton }

    public class Tests
    {
        private const string appName = "Calculator";
        private const string clearEntry = "Clear entry";

        private const string ipAddress = "http://127.0.0.1:4723/wd/hub";

        private static WindowsDriver<WindowsElement> _driver;

        private string numberButtons(int number) => $"num{number}Button";

        private AppiumOptions appCapabilities;

        private int nine = 9;
        private int zero = 0;

        #region Positive Tests

        /// <summary>
        /// Open windows calculator application 
        /// </summary>
        [OneTimeSetUp]
        public void OpenCalculator()
        {
            appCapabilities = new AppiumOptions();
            appCapabilities.AddAdditionalCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            appCapabilities.AddAdditionalCapability("deviceName", "WIN10KIEVVM02");
            _driver = new WindowsDriver<WindowsElement>(new Uri(ipAddress), appCapabilities);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        /// <summary>
        /// Check adding two numbers
        /// </summary>
        [Test, Order(1)]
        public void VerifyAdding()
        {
            Assert.AreEqual($"Display is 18", GetCalculationResults(Actions.plusButton.ToString(), nine, nine));
        }

        /// <summary>
        /// Check substraction of two numbers
        /// </summary>
        [Test, Order(2)]
        public void VerifySubstraction()
        {
            Assert.AreEqual($"Display is 0", GetCalculationResults(Actions.minusButton.ToString(), nine, nine));
        }

        /// <summary>
        /// Check power of two numbers
        /// </summary>
        [Test, Order(3)]
        public void VerifyMultiply()
        {
            Assert.AreEqual($"Display is 81", GetCalculationResults(Actions.multiplyButton.ToString(), nine, nine));
        }

        /// <summary>
        /// Check divide of two numbers
        /// </summary>
        [Test, Order(4)]
        public void VerifyDividing()
        {
            Assert.AreEqual($"Display is 1", GetCalculationResults(Actions.divideButton.ToString(), nine, nine));
        }

        /// <summary>
        /// Check square from given number 
        /// </summary>
        [Test, Order(6)]
        public void VerifySquareFrom()
        {
            Assert.AreEqual($"Display is 81", GetCalculationResults(Actions.squareRootButton.ToString(), 9));
        }

        /// <summary>
        /// Check power from given number
        /// </summary>
        [Test, Order(7)]
        public void VerifyPower()
        {
            Assert.AreEqual($"Display is {nine}", GetCalculationResults(Actions.x2powerButton.ToString(), nine));
            _driver.CloseApp();
        }

        #endregion

        #region Negative Test

        /// <summary>
        /// Check divide by zero
        /// </summary>
        [Test, Order(5)]
        public void VerifyDivideByZero()
        {
            Assert.AreEqual("Display is Cannot divide by zero", GetCalculationResults(Actions.divideButton.ToString(), nine, zero));
        }

        #endregion

        /// <summary>
        /// Make calculations of given numbers
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

        /// <summary>
        /// Clear screen 
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            _driver.FindElementByName(clearEntry).Click();
        }
    }
}