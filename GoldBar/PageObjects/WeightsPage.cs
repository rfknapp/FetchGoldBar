using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace GoldBar.PageObjects
{
    public class WeightsPage
    {
        protected WebDriver driver;

        public WebElement resetButton()
        {
            return (WebElement)driver.FindElement(By.XPath("//button[text()='Reset']"));
        }

        public WebElement weighButton()
        {
            return (WebElement)driver.FindElement(By.Id("weigh"));
        }

        public WeightsPage(WebDriver driver)
        {
            this.driver = driver;
        }

        public List<string> getBarNumbers()
        {
            var returnlist = new List<string>();
            var list = driver.FindElements(By.XPath("//div[@class='coins']/button"));
            returnlist.AddRange(from item in list select item.Text);

            return returnlist;
        }

        public void fillInValues(List<string> numbers)
        {
            var listCount = numbers.Count;
            var half = listCount / 2;

            for (int i = 0; i < listCount; i++)
            {
                if(i < half)
                {
                    driver.FindElement(By.Id($"left_{i}")).SendKeys(numbers[i]);
                } else
                {
                    driver.FindElement(By.Id($"right_{i % half}")).SendKeys(numbers[i]);
                }

            }
        }

        public List<string> firstWeigh(List<string> numbers)
        {
            var oddBar = new List<string>() { numbers[0] };
            numbers.Remove(numbers[0]);
            fillInValues(numbers);
            weighButton().Click();

            var weightResult = driver.FindElement(By.XPath("//div[@class='game-info']/ol/li")).Text;

            if (weightResult.Contains("="))
            {
                return oddBar;
            }

            var weightResultsSplit = weightResult.Replace("[", string.Empty).Replace("]", string.Empty).Split(' ');
            var leftResult = weightResultsSplit[0].Split(',').ToList();
            var rightResult = weightResultsSplit[2].Split(',').ToList();

            if (weightResult.Contains("<"))
            {
                return leftResult;
            }
            else
            {
                return rightResult;
            }
        }

        public List<string> performWeigh(List<string> numbers)
        {
            List<string> lighterBars = new List<string>();
            var numberOfWeighsDone = driver.FindElements(By.XPath("//div[@class='game-info']/ol/li")).Count;
            resetButton().Click();
            fillInValues(numbers);
            weighButton().Click();
            var weightResult = driver.FindElement(By.XPath($"//div[@class='game-info']/ol/li[{numberOfWeighsDone + 1}]")).Text;

            var weightResultsSplit = weightResult.Replace("[", string.Empty).Replace("]", string.Empty).Split(' ');
            var leftResult = weightResultsSplit[0].Split(',').ToList();
            var rightResult = weightResultsSplit[2].Split(',').ToList();

            if (weightResult.Contains("<"))
            {
                lighterBars = leftResult;
            }
            else
            {
                lighterBars = rightResult;
            }

            if (lighterBars.Count == 1)
                return lighterBars;
            else
            {
                return performWeigh(lighterBars);
            }
        }

        public void clickGoldBar(string number)
        {
            driver.FindElement(By.Id($"coin_{number}")).Click();
        }

        public void reportWeighings()
        {
            var weightResults = driver.FindElements(By.XPath("//ol/li"));
            foreach (var item in weightResults)
            {
                Console.WriteLine(item.Text);
            }
        }
    }
}
