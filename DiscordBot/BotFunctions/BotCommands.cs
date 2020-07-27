using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.BotFunctions
{

   
    public class BotCommands
    {
        private string lastID;
        private string msgID = "";

        private IWebElement command;
        private string ChannelName = "commands-test";
        public void SetCommandChannel(string Cname) { ChannelName = Cname; }


        public void Listen() {

            Utils.d.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//*[@class=\"name-3_Dsmg\" and text()=\"{ChannelName}\"]"))).Click(); //read commands in a channel
            while (true)
            {
                try {
                    command = Utils.d.Driver.FindElement(By.XPath("//*[text()='cheese']"));
                    if (command != null)
                    {
                        msgID = command.GetAttribute("id"); //5
                    }
                }
                catch(Exception ex) { }

                

                /*if (msgID != lastID) {

                    if (command.Text == "cheese") {
                        Utils.d.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//*[@role=\"textbox\" and @aria-label=\"Message #general\"]"))).SendKeys("Mnam do pici");
                        Utils.d.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//*[@role=\"textbox\" and @aria-label=\"Message #general\"]"))).SendKeys(Keys.Enter);
                    }

                    lastID = msgID;
                }*/
            }



        }

    }
}
