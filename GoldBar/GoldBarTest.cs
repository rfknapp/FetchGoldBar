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

namespace GoldBar
{
    public class Tests
    {
        WebDriver driverChrome;
        WebDriver driverFireFox;
        WebDriver driverEdge;

        [OneTimeSetUp]
        public void Setup()
        {
            var path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            driverChrome = new ChromeDriver(path + @"\drivers\");
            driverChrome.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            //driverFireFox = new FirefoxDriver(path + @"\drivers\");
            //driverEdge = new EdgeDriver(path + @"\drivers\");
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            driverChrome.Close();
        }

        [Test]
        public void findFakeGoldBar()
        {
            string correctBar;
            driverChrome.Navigate().GoToUrl("http://sdetchallenge.fetch.com/");
            Assert.IsTrue(driverChrome.FindElement(By.Id("reset")).Displayed);
            var weights = new WeightsPage(driverChrome);

            var startingNumbers = new List<string>() { "0", "1", "2", "3", "5", "6", "7", "8" };
            weights.fillInValues(startingNumbers);

            driverChrome.FindElement(By.Id("weigh")).Click();
            var weightResult = driverChrome.FindElement(By.XPath("//div[@class='game-info']/ol/li")).Text;

            if (weightResult.Contains("="))
            {
                driverChrome.FindElement(By.Id("coin_4")).Click();
                correctBar = "4";
            } else
            {
                var weightResultsSplit = weightResult.Replace("[", string.Empty).Replace("]", string.Empty).Split(' ');
                var leftResult = weightResultsSplit[0].Split(',').ToList();
                var rightResult = weightResultsSplit[2].Split(',').ToList();

                if (weightResultsSplit[1].Equals("<"))
                {
                    driverChrome.FindElement(By.XPath("//button[text()='Reset']")).Click();
                    weights.fillInValues(leftResult);
                }
                else
                {
                    driverChrome.FindElement(By.XPath("//button[text()='Reset']")).Click();
                    weights.fillInValues(rightResult);
                }
                driverChrome.FindElement(By.Id("weigh")).Click();
                
                weightResult = driverChrome.FindElement(By.XPath("//div[@class='game-info']/ol/li/following-sibling::li")).Text;
                weightResultsSplit = weightResult.Replace("[", string.Empty).Replace("]", string.Empty).Split(' ');
                leftResult = weightResultsSplit[0].Split(',').ToList();
                rightResult = weightResultsSplit[2].Split(',').ToList();

                if (weightResult.Contains("<"))
                {
                    driverChrome.FindElement(By.XPath("//button[text()='Reset']")).Click();
                    weights.fillInValues(leftResult);
                }
                else
                {
                    driverChrome.FindElement(By.XPath("//button[text()='Reset']")).Click();
                    weights.fillInValues(rightResult);
                }
                driverChrome.FindElement(By.Id("weigh")).Click();
                
                weightResult = driverChrome.FindElement(By.XPath("//div[@class='game-info']/ol/li/following-sibling::li[2]")).Text;
                weightResultsSplit = weightResult.Replace("[", string.Empty).Replace("]", string.Empty).Split(' ');
                leftResult = weightResultsSplit[0].Split(',').ToList();
                rightResult = weightResultsSplit[2].Split(',').ToList();

                if (weightResult.Contains("<"))
                {
                    driverChrome.FindElement(By.Id($"coin_{leftResult[0]}")).Click();
                    correctBar = leftResult[0];
                }
                else
                {
                    driverChrome.FindElement(By.Id($"coin_{rightResult[0]}")).Click();
                    correctBar = rightResult[0];
                }
            }

            var alertText = driverChrome.SwitchTo().Alert().Text;
            Console.WriteLine(alertText);
            Assert.AreEqual("Yay! You find it!", alertText);
            driverChrome.SwitchTo().Alert().Accept();

            var weightResults = driverChrome.FindElements(By.XPath("//ol/li"));
            foreach (var item in weightResults)
            {
                Console.WriteLine(item.Text);
            }
            Console.WriteLine($"Correct bar number is {correctBar}");
        }
    }
}