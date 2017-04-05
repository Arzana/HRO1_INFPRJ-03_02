namespace OpenDataApplication.Core
{
    using GMap.NET;

    public sealed class Station
    {
        public short Id { get; set; }
        public string Code { get; set; }
        public int UIC { get; set; }
        public string FullName { get; set; }
        public string MiddleName { get; set; }
        public string ShortName { get; set; }
        public string FriendlyName { get; set; }
        public string CountryCode { get; set; }
        public StationType Type { get; set; }
        public GPoint Position { get; set; }
    }
}