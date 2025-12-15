using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

[DisallowMultipleComponent]
public class UIButtonAnimatorDOTween : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Target (RectTransform)")]
    [SerializeField] private RectTransform _target; // defaults to self

    [Header("Press Scale")]
    [SerializeField, Range(0.8f, 1.2f)] private float _pressedScale = 0.94f;
    [SerializeField] private float _downDuration = 0.06f;
    [SerializeField] private float _upDuration = 0.08f;
    [SerializeField] private Ease _downEase = Ease.OutCubic;
    [SerializeField] private Ease _upEase = Ease.OutCubic;

    [Header("Click Punch (optional)")]
    [SerializeField] private bool _usePunch = true;
    [SerializeField, Range(0f, 0.25f)] private float _punchAmount = 0.06f;
    [SerializeField] private float _punchDuration = 0.10f;
    [SerializeField] private int _punchVibrato = 1;

    [Header("Safety")]
    [SerializeField] private bool _ignoreIfNotInteractable = true;

    private Vector3 _baseScale;
    private bool _isPointerDown;
    private Selectable _selectable;

    private const string TweenId = "UIButtonAnimatorDOTween";

    private void Awake()
    {
        if (_target == null) _target = transform as RectTransform;
        _baseScale = _target.localScale;
        _selectable = GetComponent<Selectable>();
    }

    private bool CanAnimate()
    {
        if (!_ignoreIfNotInteractable) return true;
        return _selectable == null || _selectable.IsInteractable();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CanAnimate()) return;
        _isPointerDown = true;
        ScaleTo(_baseScale * _pressedScale, _downDuration, _downEase);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!CanAnimate()) return;
        _isPointerDown = false;
        ScaleTo(_baseScale, _upDuration, _upEase);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!CanAnimate()) return;
        _isPointerDown = false;
        ScaleTo(_baseScale, _upDuration, _upEase);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!CanAnimate()) return;
        if (!_usePunch) return;

        // Kill any ongoing scale tween before punch.
        _target.DOKill(true);

        // Punch from current scale so it feels responsive.
        // Use SetUpdate(true) for UI even when timescale = 0.
        _target
            .DOPunchScale(Vector3.one * _punchAmount, _punchDuration, _punchVibrato, 0.9f)
            .SetId(TweenId)
            .SetUpdate(true)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                // Ensure we end in the correct state (pressed if still holding, else base).
                _target.localScale = _isPointerDown ? _baseScale * _pressedScale : _baseScale;
            });
    }

    private void ScaleTo(Vector3 target, float duration, Ease ease)
    {
        _target.DOKill(true);

        if (duration <= 0f)
        {
            _target.localScale = target;
            return;
        }

        _target
            .DOScale(target, duration)
            .SetEase(ease)
            .SetId(true)
            .SetUpdate(true); // important for UI / pause menus
    }

    private void OnDisable()
    {
        if (_target == null) return;
        _target.DOKill(true);
        _target.localScale = _baseScale;
        _isPointerDown = false;
    }
}
