using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace BddSpecflow.Util
{
    public class ScreenShot
    {
        public IWebDriver Driver { get; set; }
        public ScreenShot(IWebDriver driver)
        {
            this.Driver = driver;
        }
        public void TakeScreenShot(String stepName)
        {
            Screenshot ss = ((ITakesScreenshot)Driver).GetScreenshot();
            string title = stepName;
            string Runname = title + DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss");
            string screenshotfilename = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin")) + @"Screenshots/" + Runname + ".png";
            ss.SaveAsFile(screenshotfilename, ScreenshotImageFormat.Png);
            
        }
    }
}
