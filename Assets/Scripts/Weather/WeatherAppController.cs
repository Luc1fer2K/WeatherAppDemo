using System.Collections;
using MobileToastKit;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace WeatherApp
{
    public class WeatherAppController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private WeatherUI _ui;

        [Header("Package GameObject (optional)")]
        [SerializeField] private MobileToastButton _toastButton;

        private IWeatherApiClient _api;
        private ILocationServiceWrapper _location;
        private IReverseGeocodingClient _reverseGeocode;

        private Coroutine _refreshRoutine;


        private void Awake()
        {
            _api = new WeatherApiClient();
            _location = new LocationServiceWrapper();
            _reverseGeocode = new BigDataCloudReverseGeocodingClient();

            _ui?.Init();

            if (_ui != null && _ui.RefreshButton != null)
                _ui.RefreshButton.onClick.AddListener(OnRefresh);
        }

        private void Start()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (!AndroidLocationPermission.HasFineLocation())
            {
                _ui?.SetStatus("Requesting location permission...");
                AndroidLocationPermission.RequestFineLocation(
                    onGranted: () =>
                    {
                        _ui?.SetStatus("Permission granted. Refreshing...");
                        StartRefresh();
                    },
                    onDenied: () =>
                    {
                        _ui?.SetError("Location permission denied. Enable it in Settings and tap Refresh.");
                    });
                return;
            }
#endif
            StartRefresh();
        }

        private void StartRefresh()
        {
            if (_refreshRoutine != null) StopCoroutine(_refreshRoutine);
            _refreshRoutine = StartCoroutine(RefreshWeather());
        }


        private void OnDestroy()
        {
            if (_ui.RefreshButton != null)
                _ui.RefreshButton.onClick.RemoveListener(OnRefresh);
        }

        private void OnRefresh()
        {
            StartRefresh();
        }

        private IEnumerator RefreshWeather()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (!AndroidLocationPermission.HasFineLocation())
            {
                _ui?.SetStatus("Requesting location permission...");
                AndroidLocationPermission.RequestFineLocation(
                    onGranted: () => StartRefresh(),
                    onDenied: () => _ui?.SetError("Location permission denied.")
                );
                yield break;
            }
#endif
            _ui?.SetStatus("Getting location...");

            float lat = 0f, lon = 0f;
            bool gotLoc = false;
            string locErr = null;

            yield return _location.GetLocation(
                (a, b) => { lat = a; lon = b; gotLoc = true; },
                err => locErr = err
            );

            if (!gotLoc)
            {
                var msg = $"Location error: {locErr}";
                SetStatus(msg);
                MobileToastService.Show(msg);
                yield break;
            }

            _ui?.SetLocation(lat, lon);
            _ui?.SetStatus("Fetching weather...");

            string cityName = null;
            string cityErr = null;

            yield return _reverseGeocode.GetLocationName(
                lat, lon,
                name => cityName = name,
                err => cityErr = err
            );

            if (!string.IsNullOrWhiteSpace(cityName))
                _ui?.SetCity(cityName);
            else
                _ui?.SetCity("Unknown location"); // fallback

            WeatherApiResponse data = null;
            string apiErr = null;

            yield return _api.GetWeather(
                lat, lon,
                d => data = d,
                err => apiErr = err
            );

            if (data == null)
            {
                var msg = $"Weather error: {apiErr}";
                _ui?.SetError($"Weather error: {apiErr}");
                MobileToastService.Show(msg);
                yield break;
            }

            float todayMax = data.daily.temperature_2m_max[0];
            var toastMsg = WeatherToastFormatter.Format(todayMax, lat, lon);

            var unit = data.daily_units?.temperature_2m_max ?? "°C";
            _ui?.SetTemperature(todayMax, unit);

            _ui?.SetUpdated("Updated just now");
            _ui?.SetStatus("Ready");

            MobileToastService.Show(toastMsg);

            if (_toastButton != null)
                _toastButton.SetMessage(toastMsg);
        }

        private void SetStatus(string msg)
        {
            if (_ui != null) _ui.SetStatus(msg);
            Debug.Log($"[WeatherApp] {msg}");
        }
    }
}
