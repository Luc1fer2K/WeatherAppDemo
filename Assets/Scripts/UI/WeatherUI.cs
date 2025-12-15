using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WeatherApp
{
    public class WeatherUI : MonoBehaviour
    {
        [Header("Texts")]
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _subtitleText;
        [SerializeField] private TMP_Text _locationText;
        [SerializeField] private TMP_Text _cityText;
        [SerializeField] private TMP_Text _tempText;
        [SerializeField] private TMP_Text _tempUnitText;
        [SerializeField] private TMP_Text _updatedText;
        [SerializeField] private TMP_Text _statusChipText;

        [Header("Buttons")]
        [SerializeField] private Button _refreshButton;

        public Button RefreshButton => _refreshButton;

        public void Init()
        {
            if (_titleText) _titleText.text = "Weather";
            if (_subtitleText) _subtitleText.text = "Open-Meteo • Current Location";
            SetStatus("Idle");
        }

        public void SetLocation(float lat, float lon)
        {
            if (_locationText) _locationText.text = $"Lat {lat:F4}  •  Lon {lon:F4}";
        }

        public void SetCity(string city)
        {
            if (_cityText) _cityText.text = city;
        }

        public void SetTemperature(float value, string unit)
        {
            if (_tempText) _tempText.text = value.ToString("0.0");
            if (_tempUnitText) _tempUnitText.text = unit;
        }

        public void SetUpdated(string text)
        {
            if (_updatedText) _updatedText.text = text;
        }

        public void SetStatus(string status)
        {
            if (_statusChipText) _statusChipText.text = status;
        }

        public void SetError(string message)
        {
            SetStatus("Error");
            if (_updatedText) _updatedText.text = message;
        }
    }
}
