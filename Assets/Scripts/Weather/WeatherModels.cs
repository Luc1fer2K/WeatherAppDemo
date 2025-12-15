using System;

namespace WeatherApp
{
    [Serializable]
    public class DailyUnits
    {
        public string time;
        public string temperature_2m_max;
    }

    [Serializable]
    public class DailyData
    {
        public string[] time;
        public float[] temperature_2m_max;
    }

    [Serializable]
    public class WeatherApiResponse
    {
        public float latitude;
        public float longitude;
        public float generationtime_ms;
        public int utc_offset_seconds;
        public string timezone;
        public string timezone_abbreviation;
        public float elevation;
        public DailyUnits daily_units;
        public DailyData daily;
    }
}
