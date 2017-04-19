namespace OpenDataApplication.Core.DataTypes
{
    using System.Runtime.Serialization;

    public sealed class RetStop : ISerializable
    {
        public int TransferStop { get; set; }
        public string Code { get; set; }
        public int TransferTime { get; set; }
        private int NOT_USED1 { get; set; }
        public string CountryCode { get; set; }
        public int TimeZone { get; set; }
        private int NOT_USED2 { get; set; }
        public int XCoord { get; set; }
        public int YCoord { get; set; }
        public string Name { get; set; }
        public int Zone1 { get; set; }
        public int Zone2 { get; set; }
        public int Zone3 { get; set; }
        public int Zone4 { get; set; }

        public RetStop()
        {
            Code = string.Empty;
            CountryCode = string.Empty;
            Name = string.Empty;
        }

        public RetStop(SerializationInfo info, StreamingContext context)
        {
            TransferStop = info.GetInt32("Overstaphalte");
            Code = info.GetString("Haltecode");
            TransferTime = info.GetInt32("Overstaptijd");
            NOT_USED1 = info.GetInt32("Maximale overstaptijd");
            CountryCode = info.GetString("Landcode");
            TimeZone = info.GetInt32("Tijdzone");
            NOT_USED2 = info.GetInt32("Attribuut");
            XCoord = info.GetInt32("x-coördinaat");
            YCoord = info.GetInt32("y-coördinaat");
            Name = info.GetString("Naam");
            Zone1 = info.GetInt32("Zone1");
            Zone2 = info.GetInt32("Zone2");
            Zone3 = info.GetInt32("Zone3");
            Zone4 = info.GetInt32("Zone4");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Overstaphalte", TransferStop);
            info.AddValue("Haltecode", Code);
            info.AddValue("Overstaptijd", TransferTime);
            info.AddValue("Maximale overstaptijd", NOT_USED1);
            info.AddValue("Landcode", CountryCode);
            info.AddValue("Tijdzone", TimeZone);
            info.AddValue("Attribuut", NOT_USED2);
            info.AddValue("x-coördinaat", XCoord);
            info.AddValue("y-coördinaat", YCoord);
            info.AddValue("Naam", Name);
            info.AddValue("Zone1", Zone1);
            info.AddValue("Zone2", Zone2);
            info.AddValue("Zone3", Zone3);
            info.AddValue("Zone4", Zone4);
        }
    }
}
