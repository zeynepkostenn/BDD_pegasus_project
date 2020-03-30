using BddSpecflow.Base;
using BddSpecflow.Util;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TechTalk.SpecFlow;

namespace BddSpecflow
{
    [Binding]
    public sealed class StepDefinition
    {

        public IWebDriver Driver { get; set; }
        public BasePage basePage;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ScenarioContext context;

        public StepDefinition(ScenarioContext injectedContext)
        {
            context = injectedContext;
        }

        [BeforeScenario] //Her senaryo öncesi çalışır.
        public void Setup()
        {
            Logging.Logger();
            // Optionns oluşturuyoruz
            ChromeOptions options = new ChromeOptions();
            // Setleme işlemini gerçekleştiriyoruz
            options.AddArgument("start-maximized");
            options.AddArgument("disable-popup-blocking");
            options.AddArgument("disable-notifications");
            options.AddArgument("test-type");
            // Driver'a setliyoruz options'ları
            Driver = new ChromeDriver(options);
            // Süreler
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            // Gidilen sayfa url'i
            Driver.Navigate().GoToUrl("https://www.flypgs.com/");
            log.Info("Driver ayağa kalktı...");
            basePage = new BasePage(Driver);
        }

        [BeforeStep]
        public void BeforeStep()
        {
            //stepin textini yazdırır
            log.Info("Step :" + context.StepContext.StepInfo.Text);
        }

        [Given("'(.*)' objesine tıklanır.")]
        public void LoginSteps(String obje)
        {
            log.Info("Parametre: " + obje);
            basePage.ClickElement(By.CssSelector(obje));
            new ScreenShot(Driver).TakeScreenShot(context.ScenarioInfo.Title);
        }

        [Given("'(.*)' alanına \"(.*)\" yazılır.")]
        public void SendKeys(String obje, String text)
        {
            log.Info("Parametre: " + obje);
            basePage.SendKeys(By.CssSelector(obje), text);
            new ScreenShot(Driver).TakeScreenShot(context.ScenarioInfo.Title);
        }

        [Given("'(.*)' saniye süre ile beklenir.")]
            public void TimeSeconds(int seconds)
        {
            log.Info(seconds + "saniye bekleniyor... ");
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            new ScreenShot(Driver).TakeScreenShot(context.ScenarioInfo.Title);
        }

        [Given("(.*) için (.*) tarihi girilir")]
        public void DatePickerEnter(string direction, string date)
        {
            string[] dates = date.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            log.Info(direction + " için " + "Tarih: " + date);
            basePage.SelectDates(direction, dates);
        }
        

        [AfterScenario]
        public void TearDown()
        {
            Driver.Quit();
        }
   
    }
}
