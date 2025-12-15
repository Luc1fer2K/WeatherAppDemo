using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

namespace WeatherApp
{
    public class BigDataCloudReverseGeocodingClient : IReverseGeocodingClient
    {
        private const string BaseUrl = "https://api.bigdatacloud.net/data/reverse-geocode-client";

        public IEnumerator GetLocationName(float latitude, float longitude, Action<string> onSuccess, Action<string> onError)
        {
            var lat = latitude.ToString(CultureInfo.InvariantCulture);
            var lon = longitude.ToString(CultureInfo.InvariantCulture);

            var url = $"{BaseUrl}?latitude={lat}&longitude={lon}&localityLanguage=en";

            using (var req = UnityWebRequest.Get(url))
            {
                yield return req.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
                if (req.result != UnityWebRequest.Result.Success)
#else
                if (req.isNetworkError || req.isHttpError)
#endif
                {
                    onError?.Invoke($"Reverse geocode failed: HTTP {req.responseCode} - {req.error}");
                    yield break;
                }

                var json = req.downloadHandler.text;

                BigDataCloudReverseResponse data;
                try
                {
                    data = JsonUtility.FromJson<BigDataCloudReverseResponse>(json);
                }
                catch (Exception e)
                {
                    onError?.Invoke($"Reverse geocode JSON parse error: {e.Message}");
                    yield break;
                }

                var locality = FirstNonEmpty(data.city, data.locality);
                if (string.IsNullOrWhiteSpace(locality))
                {
                    onError?.Invoke("Reverse geocode returned no city/locality.");
                    yield break;
                }

                var country = data.countryName;
                var result = !string.IsNullOrWhiteSpace(country) ? $"{locality}, {country}" : locality;

                onSuccess?.Invoke(result);
            }
        }

        private static string FirstNonEmpty(params string[] values)
        {
            foreach (var v in values)
                if (!string.IsNullOrWhiteSpace(v))
                    return v;
            return null;
        }
    }
}
