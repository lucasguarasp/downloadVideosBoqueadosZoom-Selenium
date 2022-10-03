using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace VideoZomDownload
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string url = "https://zoom.us/";
            string linkVideo = "";
            string src = "";
            ChromeOptions options = new ChromeOptions();
            IWebDriver driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), options);


            Thread.Sleep(2000); /*2seg*/
            // Clear the Console
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Cole o link do video aqui, depois aperte ENTER: ");
            linkVideo = Console.ReadLine();


            driver.Navigate().GoToUrl(linkVideo);
            Thread.Sleep(1000); /*1seg*/
            driver.Manage().Window.Maximize();


            //tenta achar a src do video
            if (IsElementPresent(By.TagName("video")))
            {
                src = driver.FindElement(By.TagName("video")).GetAttribute("src");
            }
            else
            {
                Console.WriteLine("Nao achar source do video");
                return;
            }

            var botaoDownload = $"<a href=\"{src}\"><download class=\"hoverZoomLink\">Clique botão direito, salvar link como</a>";


            //abre o site do zoom
            driver.FindElement(By.CssSelector("body")).SendKeys(Keys.Control + "t");
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            driver.Navigate().GoToUrl(url);
            Thread.Sleep(1000); /*1seg*/

            //aceitar cookies
            if (IsElementPresent(By.Id("onetrust-accept-btn-handler")))
            {
                driver.FindElement(By.Id("onetrust-accept-btn-handler")).Click();
            }


            //IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            //js.ExecuteScript($"alert('{botaoDownload}');");



            //tenta criar botao de download
            if (IsElementPresent(By.ClassName("imglink")))
            {
                //driver.FindElement(By.ClassName("imglink")).SendKeys(botaoDownload);
                //IWebElement outer = driver.FindElement(By.ClassName("imglink"));
                //driver.FindElement(By.ClassName("imglink")).SendKeys($"\"{botaoDownload}\"");
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                if (js != null)
                {

                    //string title = (string)js.ExecuteScript("return document.title");

                    String code =
                        "body = document.querySelector('body');" +
                        "elementClass = document.body.getElementsByClassName('imglink');" +
                        "element = document.createElement('a');" +
                        "elementDownload = document.createElement('download');" +
                        "elementDownload.setAttribute('class', 'hoverZoomLink');" +
                        "element.append(elementDownload);" +
                        "text = document.createTextNode('Clique botão direito, salvar link como');" +
                        "element.appendChild(text);" +
                        $"element.setAttribute('href', '{src}');" +
                        "element.setAttribute('id', 'downloadLink');" +
                        "elementClass[0].append(element);";

                    js.ExecuteScript(code);
                }

                //Console.Clear();

                //Console.WriteLine(botaoDownload);
            }
            else
            {
                Console.WriteLine("Nao conseguir criar o botão de download");
                return;
            }

            //Thread.Sleep(1000); /*1seg*/
            if (IsElementPresent(By.Id("downloadLink")))
            {

                var download = driver.FindElement(By.Id("downloadLink"));
                Actions action = new Actions(driver);
                action.ContextClick(download).Build().Perform();
                //action.KeyDown(Keys.ArrowDown);
                //action.KeyDown(Keys.ArrowDown);
                //action.KeyDown(Keys.ArrowDown);
                //action.KeyDown(Keys.Enter); 
                //InputSimulator sim = new InputSimulator();
                //sim.Keyboard.KeyPress(VirtualKeyCode.DOWN);
                //Thread.Sleep(2000);
                //sim.Keyboard.KeyPress(VirtualKeyCode.DOWN);
                //Thread.Sleep(2000);
                //sim.Keyboard.KeyPress(VirtualKeyCode.DOWN);
                //Thread.Sleep(2000);
                //sim.Keyboard.KeyPress(VirtualKeyCode.DOWN);
                //Thread.Sleep(2000);
                //sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

            }
            else
            {
                Console.WriteLine("Nao clicar para fazer download");
                return;
            }


            bool IsElementPresent(By by)
            {
                try
                {
                    driver.FindElement(by);
                    return true;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            }



        }
    }
}
