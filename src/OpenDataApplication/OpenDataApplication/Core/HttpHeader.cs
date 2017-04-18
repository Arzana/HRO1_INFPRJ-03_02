using System;
using System.Xml.Serialization;
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
    [XmlRoot("temp")] //name of the file as it is stored??????
    class HttpHeader
    {
        [XmlAttribute("id")] // The attribute you wish to filter on?
        public int Id { get; set; }
        //const string URL = "https://webservices.ns.nl/ns-api-avt?station=ut";
        const string username = "0916827@hr.nl";
        const string password = "6qmg-XC7AH61Wz53i89ZC-bVSyab7QYTD6nS_Dx6wlLoMM_cFzzSXA";


        public static string serialiser(XmlDocument x, string destination)
        {
            
            object obj;
            using (XmlReader reader = XmlReader.Create(new StringReader(destination)))
            {
                reader.MoveToContent();
                switch (reader.Name)
                {
                    case "temp":
                        obj = new XmlSerializer(typeof(HttpHeader)).Deserialize(reader);
                        break;
                    default:
                        throw new NotSupportedException("unexpected" + reader.Name);
                }
            }

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
