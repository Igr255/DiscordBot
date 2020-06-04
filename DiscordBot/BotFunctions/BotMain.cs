using DiscordBot.DriverSetUp;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
using System.Xml;

namespace DiscordBot.BotFunctions
{
    public class BotMain
    {
        private bool followUsers = false;
        private bool walkAround = false;
        private bool smoothFollow = false;

        private Random random = new Random();
        private string[] cr = { "igorros.svk@gmail.com", "cabajka666" };
        public DriverInit d = new DriverInit();

        private int voiceChannelCount;
        private int startingVoiceChannelID;
        private int voiceChannelID;
        private int lastChannel;
        private int waitSpan;
        private int currentChannel;

        private string UserName;
        private string ChannelName;

        private Dictionary<int, int> channelList = new Dictionary<int, int>(); //channel ID, userCount


        public void SetVoiceChannelCount(int count) { voiceChannelCount = count; }
        public void SetStartingVoiceChannelID(int count) { startingVoiceChannelID = count; lastChannel = count; }
        public void SetFollowSequence(bool checkFollow) { followUsers = checkFollow; }
        public void SetSmoothFollow(bool checkSmoothFollow) { smoothFollow = checkSmoothFollow; }
        public void SetWalkSequence(bool checkWalk, int wait=1000) { walkAround = checkWalk; waitSpan = wait; }
        public void SetCustomName(string Uname) { UserName = Uname; }
        public void SetCommandChannel(string Cname) { ChannelName = Cname; }


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
            d.Wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector($"[aria-label=\"{name}\"]"))).Click(); //switch to a server by its name

            EventFiringWebDriver efw = new EventFiringWebDriver(d.Driver);

            efw.ExecuteScript("document.querySelector('#app-mount > div.app-1q1i1E > div > div.layers-3iHuyZ.layers-3q14ss > div > div > div > div.content-98HsJk > div.sidebar-2K8pFh.hasNotice-1XRy4h > nav > div.scrollerWrap-2lJEkd.scrollerThemed-2oenus.themeGhostHairline-DBD-2d.scrollerFade-1Ijw5y > div').scrollTop=9999");

            d.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//*[@id=\"channels-{startingVoiceChannelID}\"]/div/div[1]"))).Click();

            d.Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("button-3zdF3z"))).Click(); //clicks popup after joining a channel

            d.Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("close-relY5R"))).Click(); //popup on top
            
            efw.ExecuteScript("document.querySelector('#app-mount > div.app-1q1i1E > div > div.layers-3iHuyZ.layers-3q14ss > div > div > div > div.content-98HsJk > div.sidebar-2K8pFh.hasNotice-1XRy4h > nav > div.scrollerWrap-2lJEkd.scrollerThemed-2oenus.themeGhostHairline-DBD-2d.scrollerFade-1Ijw5y > div').scrollTop=9999");

            //d.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//*[@class=\"name-3_Dsmg\" and text()=\"{ChannelName}\"]"))).Click(); //read commands in a channel
            
            ///
            if (UserName != null && UserName != "")
            {
                ChangeName(UserName);
            }
            ///
            CheckIfChannelsAreEmpty();
            ///
        }

        public void ChangeName(string name) {

            IJavaScriptExecutor ex = (IJavaScriptExecutor)d.Driver;

            d.Wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[aria-label='User Settings']"))).Click();
            d.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@class=\"userInfoViewingButton-2-jbH9 button-38aScr lookFilled-1Gx00P colorBrand-3pXr91 sizeSmall-2cSMqn grow-q77ONN\"]"))).Click();            
            
            ///NameSet///
            d.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@name=\"username\"]")));
            IWebElement NameElement = d.Driver.FindElement(By.XPath("//*[@class=\"inputDefault-_djjkz input-cIJ7To multiInputField-1x_Zdx\"]"));
            ex.ExecuteScript($"arguments[0].value='{UserName}';", NameElement);

            d.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@name=\"username\"]"))).SendKeys(Keys.Space); //these need to be used for some reason
            d.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@name=\"username\"]"))).SendKeys(Keys.Backspace);

            ///PasswordSet///
            d.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@name=\"password\"]"))).SendKeys(cr[1]);

            ///Save///
            d.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@class=\"button-38aScr lookFilled-1Gx00P colorGreen-29iAKY sizeSmall-2cSMqn grow-q77ONN\"]"))).Click();
            d.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@class=\"closeButton-1tv5uR\"]"))).Click();
            

        }

        public void CheckIfChannelsAreEmpty()
        {
            while (true)
            {
                voiceChannelID = startingVoiceChannelID; //count reset

                channelList.Clear(); //dict reset

                for (int i = 0; i < voiceChannelCount; i++) //counting number of users in each channel
                {
                    IReadOnlyList<IWebElement> userCount = d.Driver.FindElements(By.XPath($"//*[@class='containerDefault-1ZnADq' and div/@id='channels-{voiceChannelID}']/div/div/div/div"));

                    if (voiceChannelID == lastChannel) //does not count itself as a person (it's in the same channel)
                    {
                        channelList.Add(voiceChannelID, userCount.Count - 2);
                    }
                    else
                    {
                        channelList.Add(voiceChannelID, userCount.Count - 1); // -1 cause of some random extra div 
                    }     

                    voiceChannelID++;                   
                }

                //// gets max value and the channel it belongs to
                var maxPair = channelList.FirstOrDefault(x => x.Value == channelList.Values.Max());
                var value = maxPair.Value;
                var maxChannel = maxPair.Key;
                ////

                currentChannel = lastChannel;

                lastChannel = maxChannel; //setting the last channel number, also indicates next channel at this moment

                if (maxChannel >= startingVoiceChannelID && value > 0 && followUsers) //if bot finds users
                {
                    if (smoothFollow)
                    {
                        SmoothFollow();
                    }
                    else
                    {
                        Follow(maxChannel);
                    }
                }
                else if ((maxChannel >= startingVoiceChannelID && value <= 0) && walkAround) //if all channels are empty
                {
                    Walk();
                }


                d.Sleep(100);
            }


        }

        public void Follow(int switchTo)
        { //TODO walk around when noone's connected(done), walk smoothly towards the channel(done), follow specific person
            try
            {
                d.WaitMusicChannel.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"app-mount\"]/div[4]/div[2]/div/form/div[2]/button"))).Click();
            }
            catch { }
            d.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//*[@id=\"channels-{switchTo}\"]/div/div[1]"))).Click();          
            
        }
        
        public void SmoothFollow() {
            int waitTime = 500;
            int waitTimeSpan;      

            if (lastChannel >= currentChannel)
            {
                waitTimeSpan = (int)((waitTime / lastChannel-currentChannel) + 100);
                for (int i = currentChannel+1; i < lastChannel+1; i++)
                {
                    Follow(i);
                    d.Sleep(waitTime);

                    waitTime -= waitTimeSpan;
                    if (waitTime <= 0) {
                        waitTime = 100;
                    }
                }
            }

            else if (lastChannel < currentChannel)
            {
                waitTimeSpan = (int)((waitTime / currentChannel - lastChannel) + 100);
                for (int i = currentChannel-1; i > lastChannel-1; i--)
                {
                    Follow(i);
                    d.Sleep(waitTime);

                    waitTime -= waitTimeSpan; //check in case it does a oopsie
                    if (waitTime <= 0)
                    {
                        waitTime = 100;
                    }
                }
            }
        }

        public void Walk()
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
