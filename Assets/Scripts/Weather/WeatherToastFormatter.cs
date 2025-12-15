namespace WeatherApp
{
    public static class WeatherToastFormatter
    {
        public static string Format(float tempMax, float lat, float lon)
        {
            return $"Max temp today: {tempMax:0.0}°C @ ({lat:F2}, {lon:F2})";
        }
    }
}
