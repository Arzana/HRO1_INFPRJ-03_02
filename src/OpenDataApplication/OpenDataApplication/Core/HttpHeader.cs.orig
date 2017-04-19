namespace OpenDataApplication.Core
{
    using Properties;
    using Route;
    using System.Net;
    using System.Xml.Serialization;

    public static class HttpHeader
    {
<<<<<<< HEAD
        public int RitNummer;
        public DateTime VertrekTijd;
        public string EindBestemming;
        public string TreinSoort;
        public string Vervoerder;
        public string VertrekSpoor;
=======
        private const string username = "0916827@hr.nl";
        private const string password = "6qmg-XC7AH61Wz53i89ZC-bVSyab7QYTD6nS_Dx6wlLoMM_cFzzSXA";
        private static XmlSerializer serializer = new XmlSerializer(typeof(TrainRoutes));
>>>>>>> 103e164cda34bd04779f52eb3ef948946e5e0581

        public static TrainRoutes GetFromStation(string station)
        {
<<<<<<< HEAD
            return $"{{{RitNummer}, {VertrekTijd}, {EindBestemming ?? "NULL"}, {TreinSoort ?? "NULL"}, {Vervoerder ?? "NULL"}, {VertrekSpoor ?? "NULL"}}}";
        }
    }
=======
            HttpWebRequest req = GetReqCtor($"{Resources.NSApiUri}{station}");
>>>>>>> 103e164cda34bd04779f52eb3ef948946e5e0581

            HttpWebResponse resp;
            if (!TryGetResponse(req, out resp)) return new TrainRoutes();

            try
            {
                return (TrainRoutes)serializer.Deserialize(resp.GetResponseStream());
            }
            catch
            {
                return new TrainRoutes();
            }
        }

        private static bool TryGetResponse(HttpWebRequest req, out HttpWebResponse result)
        {
            try
            {
                result = (HttpWebResponse)req.GetResponse();
                return true;
            }
            catch(WebException we)
            {
                result = (HttpWebResponse)we.Response;
                return false;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        private static HttpWebRequest GetReqCtor(string uri)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
            req.Method = WebRequestMethods.Http.Get;
            req.ContentType = "text/xml; encoding='utf-8'";
            req.Credentials = new NetworkCredential(username, password);
            return req;
        }
    }
}
