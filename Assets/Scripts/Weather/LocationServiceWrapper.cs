using System.Collections;
using UnityEngine;

namespace WeatherApp
{
    public interface ILocationServiceWrapper
    {
        IEnumerator GetLocation(System.Action<float, float> onSuccess, System.Action<string> onError);
    }

    public class LocationServiceWrapper : ILocationServiceWrapper
    {
        private readonly float _timeoutSeconds;

        public LocationServiceWrapper(float timeoutSeconds = 20f)
        {
            _timeoutSeconds = timeoutSeconds;
        }

        public IEnumerator GetLocation(System.Action<float, float> onSuccess, System.Action<string> onError)
        {
#if UNITY_EDITOR
            onSuccess?.Invoke(19.07f, 72.87f); // Mumbai sample coords
            yield break;
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            if (!AndroidLocationPermission.Ensure())
            {
                onError?.Invoke("Requesting location permission... tap Allow and retry.");
                yield break;
            }
#endif

            if (!Input.location.isEnabledByUser)
            {
                onError?.Invoke("Location service disabled by user.");
                yield break;
            }

            Input.location.Start();

            float elapsed = 0f;
            while (Input.location.status == LocationServiceStatus.Initializing && elapsed < _timeoutSeconds)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (Input.location.status != LocationServiceStatus.Running)
            {
                onError?.Invoke($"Location service failed: {Input.location.status}");
                yield break;
            }

            var last = Input.location.lastData;
            onSuccess?.Invoke(last.latitude, last.longitude);

            Input.location.Stop();
        }
    }
}
