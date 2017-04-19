namespace OpenDataApplication.Core.Route
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot("ActueleVertrekTijden")]
    public sealed class TrainRoutes
    {
        [XmlElement("VertrekkendeTrein")]
        public List<NsData> Data { get; set; }

        public TrainRoutes()
        {
            Data = new List<NsData>();
        }
    }
}
