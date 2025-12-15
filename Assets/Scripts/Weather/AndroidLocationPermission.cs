#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine.Android;
#endif
using System;

namespace WeatherApp
{
    public static class AndroidLocationPermission
    {
        public static bool HasFineLocation()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return Permission.HasUserAuthorizedPermission(Permission.FineLocation);
#else
            return true;
#endif
        }

        public static void RequestFineLocation(Action onGranted, Action onDenied)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            var callbacks = new PermissionCallbacks();
            callbacks.PermissionGranted += _ => onGranted?.Invoke();
            callbacks.PermissionDenied += _ => onDenied?.Invoke();
            callbacks.PermissionDeniedAndDontAskAgain += _ => onDenied?.Invoke();

            Permission.RequestUserPermission(Permission.FineLocation, callbacks);
#else
            onGranted?.Invoke();
#endif
        }
    }
}
