using System;
using System.IO;
using System.Collections;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Windows;
using OpenQA.Selenium.Support.UI;

namespace file// Note: actual namespace depends on the project name.
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var path = "./names.txt";
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);
            // this is the name list part
            List<string> myNameList = new List<string>();
            foreach (string line in lines)
            {
                myNameList.Add(line);
            }
            Random rnd = new Random();
            int r = rnd.Next(myNameList.Count());
            
            // this is the beginning of the bot part
            ChromeDriver driver1 = new ChromeDriver("./");
            
            string link2 = "https://mail.tm/en/";
            driver1.Navigate().GoToUrl(link2);
            Thread.Sleep(2500);
            driver1.FindElement(By.Id("DontUseWEBuseAPI")).Click();

            ChromeOptions options = new ChromeOptions();
            //options.AddExtension("C:\\Users\\BerkayPehlivan\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Extensions\\majdfhpaihoncoakbjgbdhglocklcgno\\2.5.0_0\\extension_2_5_0_0.crx");
            ChromeDriver driver2 = new ChromeDriver("./", options);

            string link = "https://www.instagram.com/accounts/emailsignup/?hl=en";
            driver2.Navigate().GoToUrl(link);
            Thread.Sleep(1500);

            try{
                driver2.FindElement(By.XPath("/html/body/div[4]/div/div/button[2]")).Click();
                Thread.Sleep(1000);
            }catch{
                System.Console.WriteLine("Bisi olmadi");
            }


            querySelector(driver2, "[aria-label=\"Mobile Number or Email\"]").SendKeys(Keys.LeftControl+'v');
            querySelector(driver2, "[aria-label=\"Full Name\"]").SendKeys(myNameList[r]);
            querySelector(driver2, "[aria-label=\"Username\"]").SendKeys(CreateRandomUsername(15));
            querySelector(driver2, "[aria-label=\"Password\"]").SendKeys(CreatePassword(12));
            //driver2.FindElement(By.XPath("//*[@id=\"react-root\"]/section/main/div/div/div[1]/div[2]/form/div[7]/div/button")).Click();
            Thread.Sleep(500);
            querySelector(driver2, "button[type=\"submit\"]").Click();
            //driver2.FindElement(By.XPath("//*[@id=\"react-root\"]/section/main/div/div/div[1]/div/div[4]/div/div/span/span[1]/select"));

            Thread.Sleep(1000);
            Random random = new Random();
            IWebElement month = querySelector(driver2, $"select[Title=\"Month:\"]");
            month.Click();
            for(int i=0; i<random.Next(1,12); i++){
                month.SendKeys(Keys.ArrowDown);
            }

            IWebElement day = querySelector(driver2, $"select[Title=\"Day:\"]");
            day.Click();
            for(int i=0; i<random.Next(1,28); i++){
                day.SendKeys(Keys.ArrowDown);
            }

            IWebElement year = querySelector(driver2, $"select[Title=\"Year:\"]");
            year.Click();
            for(int i=0; i<random.Next(22,42); i++){
                year.SendKeys(Keys.ArrowDown);
            }
            year.Click();
            Thread.Sleep(500);
            driver2.FindElement(By.XPath("//*[@id=\"react-root\"]/section/main/div/div/div[1]/div/div[6]/button")).Click();

            string code;
            int counter = 0;

            while(true){
                try{
                    code = driver1.FindElement(By.XPath("//*[@id=\"__layout\"]/div/div[2]/main/div/div[2]/ul/li/a/div/div[1]/div[2]/div[2]/div/div[1]")).Text;
                    break;
                }catch{
                    counter++;
                    if(counter > 10){
                        driver2.FindElement(By.XPath("//*[@id=\"react-root\"]/section/main/div/div/div[1]/div[1]/div[2]/div/button")).Click();
                        counter = 0;
                    }
                    Thread.Sleep(1000);
                }
            }
            code = code.Split(" ")[0];
            //driver1.Quit();
            querySelector(driver2, "input[aria-label=\"Confirmation Code\"]").SendKeys(code);
            Thread.Sleep(500);
            querySelector(driver2, "button[type=\"submit\"]").Click();
            /*
            Thread.Sleep(1500);
            runCommand(driver2, $"document.querySelector(`select[Title=\"Day:\"]`).value = {random.Next(1,28).ToString()}");
            Thread.Sleep(1500);
            querySelector(driver2, $"select[Title=\"Day:\"]").Click();
            
            Thread.Sleep(1500);
            runCommand(driver2, $"document.querySelector(`select[Title=\"Year:\"]`).value = {random.Next(1980, 2000).ToString()}");
            querySelector(driver2, $"select[Title=\"Year:\"]").Click();*/
            
            /*
            IWebElement element = driver2.FindElement(By.XPath("//*[@id=\"react-root\"]/section/main/div/div/div[1]/div/div[4]/div/div/span/span[1]/select"));
            element.SendKeys(Keys.ArrowDown);
            element.SendKeys(Keys.ArrowDown);
            element.SendKeys(Keys.ArrowDown);
            element.SendKeys(Keys.ArrowDown);
            element.SendKeys(Keys.ArrowDown);
            element.SendKeys(Keys.ArrowDown);
            SelectElement DateSelection = new SelectElement(element);
            Random random = new Random();
            int number = random.Next(1,12);
            DateSelection.SelectByValue($"{number}");*/
            
            //driver.FindElement(By.PartialLinkText("aria-label=\"Password\"")).SendKeys(CreatePassword(12));
            //driver.ExecuteScript("document.evaluate('//*[@id=\"accountDetailsNext\"]/div/button/div[3]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.click()");
            
        }

        private static void runCommand(ChromeDriver driver, string command){
            driver.ExecuteScript(command);
        }

        private static void queryValue(ChromeDriver driver, string selector, string value){
            string test = $"document.querySelector(`{selector}`).value = `{value}`";
            driver.ExecuteScript(test);
        }

        private static IWebElement querySelectorCount(ChromeDriver driver, string selector, int count){
            string test = $"return document.querySelector('{selector}')[{count}]";
            return (IWebElement) driver.ExecuteScript(test);
        }

        private static IWebElement querySelector(ChromeDriver driver, string selector){
            string test = $"return document.querySelector('{selector}')";
            return (IWebElement) driver.ExecuteScript(test);
        }
        private static string CreateRandomUsername(int length)
        {
            string alphabet = "abcdefghijklmnoprstuvyz";

            string validChars = alphabet + "0123456789";
            Random random = new Random();

            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }
        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}