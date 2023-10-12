using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoldBar.PageObjects
{
    public class WeightsPage
    {
        protected WebDriver driver;

        public WeightsPage(WebDriver driver)
        {
            this.driver = driver;
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
    }
}
