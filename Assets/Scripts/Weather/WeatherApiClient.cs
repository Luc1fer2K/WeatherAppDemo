using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

namespace WeatherApp
{
    public interface IWeatherApiClient
    {
        IEnumerator GetWeather(float latitude, float longitude, Action<WeatherApiResponse> onSuccess, Action<string> onError);
        string BuildUrl(float latitude, float longitude);
    }

    public class WeatherApiClient : IWeatherApiClient
    {
        private const string BaseUrl = "https://api.open-meteo.com/v1/forecast";

        public string BuildUrl(float latitude, float longitude)
        {
            var lat = latitude.ToString(CultureInfo.InvariantCulture);
            var lon = longitude.ToString(CultureInfo.InvariantCulture);
            return $"{BaseUrl}?latitude={lat}&longitude={lon}&timezone=auto&daily=temperature_2m_max";
        }

        public IEnumerator GetWeather(float latitude, float longitude, Action<WeatherApiResponse> onSuccess, Action<string> onError)
        {
            var url = BuildUrl(latitude, longitude);

            using (var req = UnityWebRequest.Get(url))
            {
                yield return req.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
                if (req.result != UnityWebRequest.Result.Success)
#else
                if (req.isNetworkError || req.isHttpError)
#endif
                {
                    onError?.Invoke(req.error);
                    yield break;
                }

                var json = req.downloadHandler.text;

                WeatherApiResponse data = null;
                try
                {
                    data = JsonUtility.FromJson<WeatherApiResponse>(json);
                }
                catch (Exception e)
                {
                    onError?.Invoke($"JSON parse error: {e.Message}");
                    yield break;
                }

                if (data?.daily?.temperature_2m_max == null || data.daily.temperature_2m_max.Length == 0)
                {
                    onError?.Invoke("Weather data missing daily.temperature_2m_max");
                    yield break;
                }

                onSuccess?.Invoke(data);
            }
        }
    }
}
