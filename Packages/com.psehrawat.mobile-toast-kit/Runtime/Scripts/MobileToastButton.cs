using UnityEngine;
using UnityEngine.UI;

namespace MobileToastKit
{
    [RequireComponent(typeof(Button))]
    public class MobileToastButton : MonoBehaviour
    {
        [SerializeField] private string _message = "Hello from MobileToastButton!";

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDestroy()
        {
            if (_button != null)
                _button.onClick.RemoveListener(OnClicked);
        }

        private void OnClicked()
        {
            MobileToastService.Show(_message);
        }

        public void SetMessage(string msg)
        {
            _message = msg;
        }
    }
}
