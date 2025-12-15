using System;

namespace WeatherApp
{
    [Serializable]
    public class BigDataCloudReverseResponse
    {
        public string city;
        public string locality;
        public string principalSubdivision; // state
        public string countryName;
        public string countryCode;
    }
}
