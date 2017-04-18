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
    //[XmlRoot("VertrekkendeTrein")]
    public class NsData
    {
        public int RitNummer;
        public string VertrekTijd;
        public string EindBestemming;
        public string TreinSoort;
        public string Vervoerder;
        public string VertrekSpoor;

        public override string ToString()
        {
            return $"{{{RitNummer}, {VertrekTijd ?? "NULL"}, {EindBestemming ?? "NULL"}, {TreinSoort ?? "NULL"}, {Vervoerder ?? "NULL"}, {VertrekSpoor ?? "NULL"}}}";
        }
    }

    [XmlRoot("ActueleVertrekTijden")]
    public class ActueleVertrekTijden
    {
        [XmlElement("VertrekkendeTrein")]
        public List<NsData> data { get; set; }

        public ActueleVertrekTijden()
        {
            data = new List<NsData>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{Count=");
            sb.Append(data.Count);
            sb.Append(", Data=");
            for (int i = 0; i < data.Count; i++)
            {
                sb.Append(data[i]);
            }
            sb.Append("}");
            return sb.ToString();
        }
    }

    public class HttpHeader
    {
        //const string URL = "https://webservices.ns.nl/ns-api-avt?station=ut";
        const string username = "0916827@hr.nl";
        const string password = "6qmg-XC7AH61Wz53i89ZC-bVSyab7QYTD6nS_Dx6wlLoMM_cFzzSXA";

        public static XmlDocument webRequest(string URL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);

            request.Method = WebRequestMethods.Http.Get;
            request.Credentials = new NetworkCredential(username, password);
            request.ContentType = "text/xml; encoding='utf-8'";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            XmlDocument xmlDocu = new XmlDocument();

            XmlSerializer serial = new XmlSerializer(typeof(ActueleVertrekTijden));
            ActueleVertrekTijden resp = (ActueleVertrekTijden)serial.Deserialize(response.GetResponseStream());

            Console.WriteLine(resp);

            return (xmlDocu);
        }
    }
}
