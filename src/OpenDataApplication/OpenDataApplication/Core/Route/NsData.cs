using System.Xml.Serialization;

namespace OpenDataApplication.Core.Route
{
    public sealed class NsData
    {
        [XmlElement("RitNummer")]
        public int Id { get; set; }

        [XmlElement("VertrekTijd")]
        public string DepartureTime { get; set; }

        [XmlElement("VertrekVertraging", IsNullable = true)]
        public string Delay { get; set; }

        [XmlElement("VertrekVertragingTekst ", IsNullable = true)]
        public string DelayInfo { get; set; }

        [XmlElement("EindBestemming")]
        public string Destination { get; set; }

        [XmlElement("TreinSoort")]
        public string TrainType { get; set; }

        [XmlElement("RouteTekst", IsNullable = true)]
        public string RouteText { get; set; }

        [XmlElement("Vervoerder")]
        public string Carrier;

        [XmlElement("VertrekSpoor")]
        public int Platform;

        [XmlElement("ReisTip", IsNullable = true)]
        public string TravelTip { get; set; }

        [XmlElement("Opmerkingen", IsNullable = true)]
        public string AdditionalInformation { get; set; }

        public override string ToString()
        {
            return $"{Carrier} {TrainType} from {DepartureTime}{(string.IsNullOrEmpty(Delay) ? string.Empty : $" (+{Delay})")} to {Destination} at platform {Platform}";
        }
    }
}