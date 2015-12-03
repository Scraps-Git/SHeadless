using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using NUnit.Framework;
using System.Data;
using System.Data.OleDb;
using System.Xml.Serialization;
using MySql.Data;
using MySql.Fabric;
using MySql.Web;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Support.UI;

namespace Selenium_Headless
{
    [TestFixture()]

    public class Class1
    {

        private IWebDriver _driver;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {

            _driver = new PhantomJSDriver();

            _driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 15));
            _driver.Manage().Timeouts().SetPageLoadTimeout(new TimeSpan(0, 0, 15));
            _driver.Manage().Timeouts().SetScriptTimeout(new TimeSpan(0, 0, 15));

        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {

            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
            }
        }

        [Test()]

        public void TestLogin()
        {
            List<string> udb = this.ifDBhandle();
            int run = 0;
            this.setDriver(udb.ToArray(), run);
            /*List<string> udb = this.ifDBhandle();
            int run = 0;
            this.isetDriver(udb.ToArray(), run);*/
        }
        //
        public void setDriver(string[] db, int pass)
        {
            int index = 0;
            string[] q = new string[db.Length];
            String att = "";
            String att2 = "";
            int tab = 0;
            int antitab = 0;

            StreamWriter tto = new StreamWriter("C:\\Users\\jastaylor\\Desktop\\TimeOutLog.txt");
            StreamWriter err = new StreamWriter("C:\\Users\\jastaylor\\Desktop\\ErrorLog.txt");
            StreamWriter svg = new StreamWriter("C:\\Users\\jastaylor\\Desktop\\SVGlog.txt");
            PhantomJSDriverService service = PhantomJSDriverService.CreateDefaultService();
            service.IgnoreSslErrors = true;
            service.LoadImages = true;
            IWebDriver _driver = new PhantomJSDriver(service);
            _driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 40));
            _driver.Manage().Timeouts().SetPageLoadTimeout(new TimeSpan(0, 0, 40));
            _driver.Manage().Timeouts().SetScriptTimeout(new TimeSpan(0, 0, 40));
                         
            IJavaScriptExecutor j = (IJavaScriptExecutor)_driver;
           
            for (int i = 0; i <db.Length; i++)
            {
              
                string url = db[i];
                try
                {
                    _driver.Navigate().GoToUrl(url);

                    var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
                    wait.Until(ExpectedConditions.ElementIsVisible((By.TagName("svg-viewer"))));
                                      
                    IWebElement text = _driver.FindElement(By.TagName("svg-viewer"));
                  
                    att = text.GetAttribute("src");

                    if (att == "{{currentSVG}}")
                    {
                        q[index] = url;
                        err.WriteLine(url);
                        antitab++;

                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine("??? " + e);
                    err.WriteLine(e);
                    err.WriteLine(url);
                    string message = "error";
                    ifDBlogErr(url, message);

                    if (Regex.IsMatch(att, @"^(http:\/\/)(khovcache)*"))
                    {
                        Console.WriteLine("True");
                        svg.WriteLine(att);
                    }
                    else
                        Console.WriteLine("False");

                };
                tab++;
                Console.WriteLine(att);
                svg.WriteLine(att);
               
                if (Regex.IsMatch(att, @"^(http:\/\/)(khovcache)*")) { }
                ifDBupdate(url, att);
                if (att2 == att)
                {
                    var flg = "d";
                    ifDBsetFlag(url, flg);
                }
                att2 = att;
                index++;
            }
           
            svg.WriteLine(tab + " " + antitab);
            Console.WriteLine(tab + " " + antitab);

            _driver.Close();
            svg.Close();
            tto.Close();
            err.Close();
            pass++;
           
        }
        //
        public List<string> ifDBhandle()
        {
            MySqlConnection conn;
            MySqlCommand cmd;
            //private IAsyncResult asyncResult;
            int tab = 0;
            List<string> ar = new List<string>();

            string connStr = "server=localhost;uid=kHovFeedUotpf;pwd='$=b7r@KtZqU{';database=kHovFeed;database=kHovFeed";
            conn = new MySqlConnection(connStr);

            conn.Open();

            string sql = "SELECT PlanView FROM KHovFeed.ifps_db;";

            cmd = new MySqlCommand(sql, conn);

            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {

                ar.Add(rdr[0].ToString());

                tab++;
            }
            foreach (var value in ar)
            {

            }

            rdr.Close();
            conn.Close();

            return ar;

        }
        //
        public void ifDBupdate(string par, string child)
        {
            MySqlConnection conn;
            MySqlCommand cmd;

            string connStr = "server=localhost;uid=kHovFeedUotpf;pwd='$=b7r@KtZqU{';database=kHovFeed;";
            conn = new MySqlConnection(connStr);

            string sql = "UPDATE KHovFeed.ifps_db SET FloorPlan='" + child + "' WHERE PlanView='" + par + "';";

            cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
            }


            conn.Close();
        }
//
        public void ifDBlogErr(string par, string emsg)
        {
            MySqlConnection conn;
            MySqlCommand cmd;

            string connStr = "server=localhost;uid=kHovFeedUotpf;pwd='$=b7r@KtZqU{';database=kHovFeed;database=kHovFeed";
            conn = new MySqlConnection(connStr);

            string sql = "UPDATE KHovFeed.ifps_db SET SenFlt='" + emsg + "' WHERE PlanView='" + par + "';";

            cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
            }

            conn.Close();
        }

//
        public void ifDBsetFlag(string par, string flg)
        {
            MySqlConnection conn;
            MySqlCommand cmd;

            string connStr = "server=localhost;uid=kHovFeedUotpf;pwd='$=b7r@KtZqU{';database=kHovFeed;database=kHovFeed";
            conn = new MySqlConnection(connStr);

            string sql = "UPDATE KHovFeed.ifps_db SET Flag_d='" +flg + "' WHERE PlanView='" + par + "';";

            cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
            }

            conn.Close();
        }
//
        public void isetDriver(string[] db, int pass)
        {
            int index = 0;
            string[] q = new string[db.Length];
            String att = "";
            String att2 = "";
            int tab = 0;
            int antitab = 0;

            StreamWriter tto = new StreamWriter("C:\\Users\\jastaylor\\Desktop\\TimeOutLog.txt");
            StreamWriter err = new StreamWriter("C:\\Users\\jastaylor\\Desktop\\ErrorLog.txt");
            StreamWriter svg = new StreamWriter("C:\\Users\\jastaylor\\Desktop\\SVGlog.txt");
            PhantomJSDriverService service = PhantomJSDriverService.CreateDefaultService();
            service.IgnoreSslErrors = true;
            service.LoadImages = true;
            IWebDriver _driver = new PhantomJSDriver(service);
            _driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 40));
            _driver.Manage().Timeouts().SetPageLoadTimeout(new TimeSpan(0, 0, 40));
            _driver.Manage().Timeouts().SetScriptTimeout(new TimeSpan(0, 0, 40));

            IJavaScriptExecutor j = (IJavaScriptExecutor)_driver;

            for (int i = 1; i < 20; i++)///< db.Length; i++)
            {

                string url = db[i];
                string[] loc = url.Split('?');
                string newrl = loc[0].Replace("index.html#/floorplan", "LotMap/Details");
                Console.WriteLine(newrl);
                try
                {
                    _driver.Navigate().GoToUrl(newrl);
                    var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(60));                   
                    wait.Until(ExpectedConditions.ElementIsVisible((By.TagName("svg-viewer"))));

                    IWebElement text = _driver.FindElement(By.TagName("svg-viewer"));

                    att = text.GetAttribute("src");
                    Console.Write(att);
                    if (att == "{{currentSVG}}")
                    {
                        q[index] = url;
                        err.WriteLine(url);
                        antitab++;

                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine("??? " + e);
                    err.WriteLine(e);
                    err.WriteLine(url);
                    string message = "error";
                    isDBlogErr(newrl, message);

                    if (Regex.IsMatch(att, @"^(http:\/\/)(khovcache)*"))
                    {
                        Console.WriteLine("True");
                        svg.WriteLine(att);
                    }
                    else
                        Console.WriteLine("False");

                };
                tab++;
                Console.WriteLine(att);
                svg.WriteLine(att);

                if (Regex.IsMatch(att, @"^(http:\/\/)(khovcache)*")) { }
                isDBupdate(newrl, att);
                if (att2 == att)
                {
                    var flg = "d";
                    isDBsetFlag(newrl, flg);
                }
                att2 = att;
                index++;
            }

            svg.WriteLine(tab + " " + antitab);
            Console.WriteLine(tab + " " + antitab);

            _driver.Close();
            svg.Close();
            tto.Close();
            err.Close();
            pass++;

        }
//
        public List<string> isDBhandle()
        {
            MySqlConnection conn;
            MySqlCommand cmd;
            //private IAsyncResult asyncResult;
            int tab = 0;
            List<string> ar = new List<string>();

            string connStr = "server=localhost;uid=kHovFeedUotpf;pwd='$=b7r@KtZqU{';database=kHovFeed;database=kHovFeed";
            conn = new MySqlConnection(connStr);

            conn.Open();

            string sql = "SELECT ParentSite FROM KHovFeed.isps_db;";

            cmd = new MySqlCommand(sql, conn);

            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {

                ar.Add(rdr[0].ToString());

                tab++;
            }
            foreach (var value in ar)
            {

            }

            rdr.Close();
            conn.Close();

            return ar;

        }
//
        public void isDBsetFlag(string par, string flg)
        {
            MySqlConnection conn;
            MySqlCommand cmd;

            string connStr = "server=localhost;uid=kHovFeedUotpf;pwd='$=b7r@KtZqU{';database=kHovFeed;database=kHovFeed";
            conn = new MySqlConnection(connStr);

            string sql = "UPDATE KHovFeed.isps_db SET Flag_d='" + flg + "' WHERE ParentSite='" + par + "';";

            cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
            }

            conn.Close();
        }
//
        public void isDBupdate(string par, string child)
        {
            MySqlConnection conn;
            MySqlCommand cmd;

            string connStr = "server=localhost;uid=kHovFeedUotpf;pwd='$=b7r@KtZqU{';database=kHovFeed;";
            conn = new MySqlConnection(connStr);

            string sql = "UPDATE KHovFeed.isps_db SET SitePlan='" + child + "' WHERE ParentSite='" + par + "';";

            cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
            }


            conn.Close();
        }
//
        public void isDBlogErr(string par, string emsg)
        {
            MySqlConnection conn;
            MySqlCommand cmd;

            string connStr = "server=localhost;uid=kHovFeedUotpf;pwd='$=b7r@KtZqU{';database=kHovFeed;database=kHovFeed";
            conn = new MySqlConnection(connStr);

            string sql = "UPDATE KHovFeed.isps_db SET SenFlt='" + emsg + "' WHERE ParentSite='" + par + "';";

            cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
            }

            conn.Close();
        }
//
    }
}
    

//         
   
               
//
   