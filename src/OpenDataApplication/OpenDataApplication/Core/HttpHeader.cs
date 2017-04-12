using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using static System.Console;
using static System.ConsoleKey;


namespace OpenDataApplication.Core
{
    class HttpHeader
    {
        //const string URL = "https://webservices.ns.nl/ns-api-avt?station=ut";
        const string username = "0916827@hr.nl";
        const string password = "6qmg-XC7AH61Wz53i89ZC-bVSyab7QYTD6nS_Dx6wlLoMM_cFzzSXA";


        public static string XmLcon(XmlDocument x)
        {
            Console.WriteLine(x.InnerText);
            return x.InnerText;
        }





        public static XmlDocument webRequest(string URL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = WebRequestMethods.Http.Get;
            request.Credentials = new NetworkCredential(username, password);
            request.ContentType = "text/xml; encoding='utf-8'";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            XmlDocument xmlDocu = new XmlDocument();

            XmlTextReader reader = new XmlTextReader(response.GetResponseStream());
            xmlDocu.Load(reader);

            return (xmlDocu);
        }

    }
}
