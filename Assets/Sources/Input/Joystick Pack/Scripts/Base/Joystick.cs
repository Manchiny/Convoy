using Assets.Scripts.Units;
using Assets.Scripts.UserInputSystem;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : UserInput, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private float handleRange = 1;
    [SerializeField] private float deadZone = 0;
    [SerializeField] private AxisOptions axisOptions = AxisOptions.Both;
    [SerializeField] private bool snapX = false;
    [SerializeField] private bool snapY = false;

    [SerializeField] protected RectTransform background = null;
    [SerializeField] private RectTransform handle = null;

    private RectTransform _baseRect = null;

    private Canvas _canvas;
    private Camera _camera;

    private Vector2 _input = Vector2.zero;

    public override bool NeedActivate => SystemInfo.deviceType != DeviceType.Desktop;

    public AxisOptions AxisOptions { get { return AxisOptions; } set { axisOptions = value; } }
    public bool SnapX { get { return snapX; } set { snapX = value; } }
    public bool SnapY { get { return snapY; } set { snapY = value; } }

    protected override float Horizontal { get { return (snapX) ? SnapFloat(_input.x, AxisOptions.Horizontal) : _input.x; } }
    protected override float Vertical { get { return (snapY) ? SnapFloat(_input.y, AxisOptions.Vertical) : _input.y; } }

    public float HandleRange
    {
        get { return handleRange; }
        set { handleRange = Mathf.Abs(value); }
    }

    public float DeadZone
    {
        get { return deadZone; }
        set { deadZone = Mathf.Abs(value); }
    }

    protected override void Start()
    {
        HandleRange = handleRange;
        DeadZone = deadZone;

        _baseRect = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();

        if (_canvas == null)
            Debug.LogError("The Joystick is not placed inside a canvas");

        Vector2 center = new Vector2(0.5f, 0.5f);

        background.pivot = center;
        handle.anchorMin = center;
        handle.anchorMax = center;
        handle.pivot = center;
        handle.anchoredPosition = Vector2.zero;

        base.Start();
    }

    public override void Init(PlayerMovement character)
    {
        var canvasRect = _canvas.transform as RectTransform;
        _baseRect.sizeDelta = canvasRect.sizeDelta;

        base.Init(character);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _camera = null;
        if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            _camera = _canvas.worldCamera;

        Vector2 position = RectTransformUtility.WorldToScreenPoint(_camera, background.position);
        Vector2 radius = background.sizeDelta / 2;
        _input = (eventData.position - position) / (radius * _canvas.scaleFactor);
        FormatInput();
        HandleInput(_input.magnitude, _input.normalized, radius, _camera);
        handle.anchoredPosition = _input * radius * handleRange;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        _input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > deadZone)
        {
            if (magnitude > 1)
                _input = normalised;
        }
        else
            _input = Vector2.zero;
    }

    protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        Vector2 localPoint = Vector2.zero;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_baseRect, screenPosition, _camera, out localPoint))
        {
            Vector2 pivotOffset = _baseRect.pivot * _baseRect.sizeDelta;
            return localPoint - (background.anchorMax * _baseRect.sizeDelta) + pivotOffset;
        }
        return Vector2.zero;
    }

    protected override void GetInputVector() { }

    private void FormatInput()
    {
        if (axisOptions == AxisOptions.Horizontal)
            _input = new Vector2(_input.x, 0f);
        else if (axisOptions == AxisOptions.Vertical)
            _input = new Vector2(0f, _input.y);
    }

    private float SnapFloat(float value, AxisOptions snapAxis)
    {
        if (value == 0)
            return value;

        if (axisOptions == AxisOptions.Both)
        {
            float angle = Vector2.Angle(_input, Vector2.up);
            if (snapAxis == AxisOptions.Horizontal)
            {
                if (angle < 22.5f || angle > 157.5f)
                    return 0;
                else
                    return (value > 0) ? 1 : -1;
            }
            else if (snapAxis == AxisOptions.Vertical)
            {
                if (angle > 67.5f && angle < 112.5f)
                    return 0;
                else
                    return (value > 0) ? 1 : -1;
            }
            return value;
        }
        else
        {
            if (value > 0)
                return 1;
            if (value < 0)
                return -1;
        }
        return 0;
    }
}

public enum AxisOptions { Both, Horizontal, Vertical }