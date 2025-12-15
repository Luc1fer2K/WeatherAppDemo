using System.Runtime.InteropServices;
using UnityEngine;

namespace MobileToastKit
{
    public static class MobileToastService
    {
        public static void Show(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                message = "(empty message)";

#if UNITY_ANDROID && !UNITY_EDITOR
            ShowAndroidToast(message);
#elif UNITY_IOS && !UNITY_EDITOR
            ShowiOSToast(message);
#else
            Debug.Log($"[MobileToastService] {message}");
#endif
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        private static AndroidJavaObject _currentToast;

        private static void ShowAndroidToast(string message)
        {
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    using (var toastClass = new AndroidJavaClass("android.widget.Toast"))
                    {
                        _currentToast?.Call("cancel");

                        _currentToast = toastClass.CallStatic<AndroidJavaObject>(
                            "makeText",
                            activity,
                            message,
                            toastClass.GetStatic<int>("LENGTH_SHORT")
                        );
                        _currentToast.Call("show");
                    }
                }));
            }
        }
#endif

#if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void _ShowNativeSnackbar(string message);

        private static void ShowiOSToast(string message)
        {
            try
            {
                _ShowNativeSnackbar(message);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[MobileToastService] iOS snackbar failed: {e}");
            }
        }
#endif
    }
}
