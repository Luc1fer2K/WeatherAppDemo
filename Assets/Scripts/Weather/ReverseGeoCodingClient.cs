using System; 
using System.Collections; 

namespace WeatherApp { 
    public interface IReverseGeocodingClient { 
        IEnumerator GetLocationName(float latitude, float longitude, Action<string> onSuccess, Action<string> onError); 
    } 
}