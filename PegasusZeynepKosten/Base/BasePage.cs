using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BddSpecflow.Base
{
    public class BasePage
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IWebDriver driver;
        WebDriverWait wait;
        IJavaScriptExecutor scriptExecutor;
        IReadOnlyList<IWebElement> dayList;
        string day = "";
        string month = "";
        string year = "";
        string yearDeparture = "//*[@id='search-flight-datepicker-departure']/div/div[1]/div/div/span[2]";
        string yearArrival = "//*[@id='search-flight-datepicker-arrival']/div/div[2]/div/div/span[2]";

        string monthDeparture = "//*[@id='search-flight-datepicker-departure']/div/div[1]/div/div/span[1]";
        string monthArrival = "//*[@id='search-flight-datepicker-arrival']/div/div[2]/div/div/span[1]";

        string nextMonthDeparture = "//*[@id='search-flight-datepicker-departure']/div/div[2]/div/a";
        string nextMonthArrival = "//*[@id='search-flight-datepicker-arrival']/div/div[2]/div/a";

        string dayDeparture = "//*[@id='search-flight-datepicker-departure']/div/div[1]//tbody//a";
        string dayArrival = "//*[@id='search-flight-datepicker-arrival']/div/div[2]//tbody//a";

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));   
        }


        public IWebElement FindElement(By by)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(by));
            HighLightElement(by);
            log.Info("Aranılan element: " + by);
            return driver.FindElement(by);
        }

        public void ClickElement(By by)
        {
            FindElement(by).Click();
        }

        public void SendKeys(By by, String value)
        {
            FindElement(by).SendKeys(value);
            FindElement(by).SendKeys(Keys.Enter);
        }

        public string GetText(By by)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
            string elementText = FindElement(by).Text;
            log.Info("Element Text :" + elementText);
            return elementText;
        }

        public bool SelectYear(string _year,string yearXpath, string nextButton)
        {
            string year = GetText(By.XPath(yearXpath));
            if (_year != year)
            {
                ClickElement(By.XPath(nextButton));
                SelectYear(_year, yearXpath, nextButton);
            }
            else {
                //SelectMonth(month, monthDepartureXpath, nextMonthDepartureXpath);
            }
            
                
            return true;

        }
        public bool SelectMonth(string _month, string monthXpath, string nextButton)
        {
            string month = GetText(By.XPath(monthXpath));
            if (_month != month)
            {
                ClickElement(By.XPath(nextButton));
                SelectMonth(_month, monthXpath, nextButton);

            }
            else {
                //SelectDay(day, dayDepartureXpath);
            }
            return true;

        }
        public bool SelectDay(string _day, string dayXpath)
        {
            dayList = driver.FindElements(By.XPath(dayXpath));
            foreach (var days in dayList)
            {
                if (days.Text.Equals(_day))
                {
                    days.Click();
                    break;
                }
            }
            return true;

        }
       
        public void SelectDates(string direction, string[] dates)
        {
            day = dates[0];
            month = dates[1];
            year = dates[2];

            if (direction.Equals("Gidiş"))
            {
                SelectYear(year, yearDeparture, nextMonthDeparture);
                SelectMonth(month, monthDeparture, nextMonthDeparture);
                SelectDay(day, dayDeparture);
            }
            else if (direction.Equals("Dönüş"))
            {
                SelectYear(year, yearArrival, nextMonthArrival);
                SelectMonth(month, monthArrival, nextMonthArrival);
                SelectDay(day, dayArrival);
            }
        }
     
        public void HighLightElement(By by)
        {
            scriptExecutor = (IJavaScriptExecutor)driver;
            scriptExecutor.ExecuteScript("arguments[0].setAttribute('style', 'background: yellow; border: 2px solid red;');", driver.FindElement(by));
            Thread.Sleep(TimeSpan.FromMilliseconds(700));
        }
    }
}
