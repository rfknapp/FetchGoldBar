using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using GoldBar.PageObjects;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace GoldBar
{
    public class Tests
    {
        WebDriver driverChrome;
        WebDriver driverFireFox;
        WebDriver driverEdge;
        WebDriver driver;
        WeightsPage weights;

        [OneTimeSetUp]
        public void Setup()
        {
            var path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            driverChrome = new ChromeDriver(path + @"\drivers\");
            //driverFireFox = new FirefoxDriver(path + @"\drivers\");
            //driverEdge = new EdgeDriver(path + @"\drivers\");

            driver = driverChrome;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl("http://sdetchallenge.fetch.com/");
            weights = new WeightsPage(driver);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            driver.Close();
        }

        [Test]
        public void findFakeGoldBar()
        {
            string correctBar;

            var startingNumbers = weights.getBarNumbers();
			var bars = weights.firstWeigh(startingNumbers);

            if (bars.Count > 1)
            {
                bars = weights.performWeigh(bars);
            }

            correctBar = bars[0];

            weights.clickGoldBar(correctBar);
            var alertText = driver.SwitchTo().Alert().Text;
            Console.WriteLine(alertText);
            Assert.AreEqual("Yay! You find it!", alertText);
            driver.SwitchTo().Alert().Accept();

            weights.reportWeighings();
            Console.WriteLine($"Correct bar number is {correctBar}");
        }
    }
}