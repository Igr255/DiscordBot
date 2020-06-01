using DiscordBot.DriverSetUp;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace DiscordBot.BotFunctions
{
    class BotMain
    {
        private bool FollowUsers = false;
        private bool WalkAround = false;

        private Random random = new Random();
        private string[] cr = { "igorros.svk@gmail.com", "cabajka666" };
        public DriverInit d = new DriverInit();

        private int voiceChannelCount;
        private int startingVoiceChannelID;
        private int voiceChannelID;
        private int lastChannel;
        private int waitSpan;

        private Dictionary<int, int> channelList = new Dictionary<int, int>(); //channel ID, userCount


        public void SetVoiceChannelCount(int count) { voiceChannelCount = count; }
        public void SetStartingVoiceChannelID(int count) { startingVoiceChannelID = count; lastChannel = count; }
        public void SetFollowSequence(bool check) { FollowUsers = check; }
        public void SetWalkSequence(bool check, int wait) { WalkAround = check; waitSpan = wait; }


        public void LogIn()
        {
            d.Init();

            d.Driver.Navigate().GoToUrl("https://discord.com/login");
            d.Wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[name=\"email\"]"))).SendKeys(cr[0]);
            d.Wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[name=\"password\"]"))).SendKeys(cr[1]);
            d.Wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[type=\"submit\"]"))).Click();

        }

        public void SwitchChannel(string name)
        {
            d.Wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector($"[aria-label=\"{name}\"]"))).Click();

            EventFiringWebDriver efw = new EventFiringWebDriver(d.Driver);

            efw.ExecuteScript("document.querySelector('#app-mount > div.app-1q1i1E > div > div.layers-3iHuyZ.layers-3q14ss > div > div > div > div.content-98HsJk > div.sidebar-2K8pFh.hasNotice-1XRy4h > nav > div.scrollerWrap-2lJEkd.scrollerThemed-2oenus.themeGhostHairline-DBD-2d.scrollerFade-1Ijw5y > div').scrollTop=9999");

            d.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//*[@id=\"channels-{startingVoiceChannelID}\"]/div/div[1]"))).Click();

            d.Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("button-3zdF3z"))).Click(); //clicks popup after joining a channel
            Thread.Sleep(5000);
            efw.ExecuteScript("document.querySelector('#app-mount > div.app-1q1i1E > div > div.layers-3iHuyZ.layers-3q14ss > div > div > div > div.content-98HsJk > div.sidebar-2K8pFh.hasNotice-1XRy4h > nav > div.scrollerWrap-2lJEkd.scrollerThemed-2oenus.themeGhostHairline-DBD-2d.scrollerFade-1Ijw5y > div').scrollTop=9999");
            //d.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"channels-28\"]/div/div[1]"))).Click();



            if (FollowUsers)
            {
                Follow();
            }
            else if (WalkAround)
            {
                Walk();
            }
        }

        public void Follow()
        { //TODO walk around when noone's connected, walk smoothly towards the channel, follow specific person 
            while (FollowUsers)
            {
                voiceChannelID = startingVoiceChannelID;

                channelList.Clear();
                for (int i = 0; i < voiceChannelCount; i++)
                {
                    IReadOnlyList<IWebElement> userCount = d.Driver.FindElements(By.XPath($"//*[@class='containerDefault-1ZnADq' and div/@id='channels-{voiceChannelID}']/div/div/div/div"));

                    if (voiceChannelID == lastChannel) //does not count itself as a person (it's in the same channel)
                    {
                        channelList.Add(voiceChannelID, userCount.Count - 2);
                    }
                    else
                    {
                        channelList.Add(voiceChannelID, userCount.Count - 1);
                    }

                    /*if(channelList.All(d => d.Value <= 0)){
                        
                        waitSpan = 1000;
                        Walk();
                    }*/

                    voiceChannelID++;
                }

                var switchTo = channelList.FirstOrDefault(x => x.Value == channelList.Values.Max()).Key;
                lastChannel = switchTo;

                try
                {
                    d.WaitMusicChannel.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"app-mount\"]/div[4]/div[2]/div/form/div[2]/button"))).Click();
                }
                catch { }
                d.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//*[@id=\"channels-{switchTo}\"]/div/div[1]"))).Click();

                d.Sleep(100);
            }
        }

        public void Walk()
        {
            while (WalkAround)
            {
                int rnd = random.Next(startingVoiceChannelID, startingVoiceChannelID + voiceChannelCount);

                d.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//*[@id=\"channels-{rnd}\"]/div/div[1]"))).Click();
                try
                {
                    d.WaitMusicChannel.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"app-mount\"]/div[4]/div[2]/div/form/div[2]/button"))).Click();
                }
                catch { }

                d.Sleep(waitSpan);
            }
        }
    }
}
