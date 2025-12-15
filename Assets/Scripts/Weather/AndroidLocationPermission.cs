using UnityEngine;

namespace WeatherApp
{
    public static class AndroidLocationPermission
    {
        public static bool Ensure()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation))
            {
                UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.FineLocation);
                return false; // request started, wait a frame / retry later
            }
#endif
            return true;
        }
    }
}
