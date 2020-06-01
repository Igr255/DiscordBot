using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace DiscordBot.DriverSetUp
{
    public class DriverInit
    {
        private TimeSpan TimeSpan = TimeSpan.FromSeconds(10);
        private IWebDriver driver;
        private WebDriverWait wait;
        private WebDriverWait waitMusicChannel;


        public IWebDriver Driver { get { return driver; } set { driver = value; } }
        public WebDriverWait Wait { get { return wait; } set { wait = value; } }
        public WebDriverWait WaitMusicChannel { get { return waitMusicChannel; } set { waitMusicChannel = value; } }

        public void Sleep(int amount)
        {
            Thread.Sleep(amount);
        }

        public void Init()
        {
            var opts = new ChromeOptions();
            opts.AddArguments("--use-fake-ui-for-media-stream");

            driver = new ChromeDriver(opts);
            wait = new WebDriverWait(driver, TimeSpan);
            waitMusicChannel = new WebDriverWait(driver, TimeSpan.FromMilliseconds(800));
        }

    }
}
