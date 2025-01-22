using UnityEngine;
using UnityEngine.UI;
using static Define;

public abstract class Indicator : InitOnce
{
    public interface IAreaData {}

    private Canvas _canvas;
    public Canvas Canvas => _canvas;
    private RectTransform _area;
    public RectTransform Area => _area;

    private Image _borderImage;
    public Image BorderImage => _borderImage;
    private Image _fillImage;
    public Image FillImage => _fillImage;

    protected float _fillAmount;
    protected float _rotation;

    public Transform TraceTarget
    {
        get => transform.parent;
        set
        {
            transform.parent = value;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }

    public bool IsTransparent
    {
        set
        {
            if (value)
            {
                Color transparent = new Color(1, 1, 1, 0);
                BorderImage.color = transparent;
                FillImage.color = transparent;
            }
            else
            {
                BorderImage.color = Color.white;
                FillImage.color = Color.white;
            }

        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _area = Util.FindChild(gameObject, "Area", true).GetComponent<RectTransform>();
        _canvas = Util.FindChild(gameObject, "Canvas", true).GetComponent<Canvas>();
        _canvas.sortingOrder = SortingLayers.SPELL_INDICATOR;

        _borderImage = Util.FindChild(gameObject, "BorderImage", true).GetComponent<Image>();
        _fillImage = Util.FindChild(gameObject, "FillImage", true).GetComponent<Image>();

        return true;
    }

    public abstract void Setup(IAreaData shape, float fillAmount = 0, Transform traceTarget = null, bool isTransparent = false);

    public abstract float FillAmount { get; set; }
}
