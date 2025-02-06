using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public abstract class Indicator : InitOnce
{
    public interface IAreaData {}
    protected Entity _owner;
    private Canvas _canvas;
    public Canvas Canvas => _canvas;
    private RectTransform _area;
    public RectTransform Area => _area;

    protected Image _borderImage;
    public Image BorderImage => _borderImage;
    protected Image _fillImage;
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

    bool _isTransparent;
    public bool IsTransparent
    {
        get => _isTransparent;
        set
        {
            _isTransparent = value;

            if (_isTransparent)
            {
                Color transparent = new Color(1, 1, 1, 0);
                IndicatorColor = transparent;
            }
            else
                IndicatorColor = Color.white;
        }
    }

    protected Color IndicatorColor
    {
        set
        {
            BorderImage.color = value;
            FillImage.color = value;
        }
    }

    protected Color IndicatorRed = new Color(0.86f, 0.2f, 0.27f);
    protected Color IndicatorBlue = new Color(0.2f, 0.9f, 0.9f);

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

    public abstract void Setup(Entity owner, IAreaData shape, float fillAmount = 0, Transform traceTarget = null, bool isTransparent = false);

    public abstract float FillAmount { get; set; }

    protected virtual void SetIndicatorColor()
    {
        if (!IsTransparent)
        {
            if (_owner.HasCategory("RELATIONSHIP_ENEMY"))
                IndicatorColor = IndicatorRed;
            else
                IndicatorColor = IndicatorBlue;
        }
    }

    protected virtual void StartIndicatorSpreading()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.2f);
    }
}
