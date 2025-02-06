using DG.Tweening;
using UnityEngine;

public class SquareIndicator : Indicator
{
    public struct SquareArea : IAreaData
    {
        public Vector2 scale;

        public bool isArrowType;
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public override void Setup(Entity owner, IAreaData shape, float fillAmount = 0, Transform traceTarget = null, bool isTransparent = false)
    {
        _owner = owner;

        SquareArea squareShape = (SquareArea)shape;

        TraceTarget = traceTarget;
        Area.transform.localScale = new Vector3(squareShape.scale.x, squareShape.scale.y, 1);

        if (squareShape.isArrowType)
        {
            _borderImage.sprite = Managers.Resource.Load<Sprite>("arrow_border");
            _fillImage.sprite = Managers.Resource.Load<Sprite>("arrow_fill");
        }
        else
        {
            _borderImage.sprite = Managers.Resource.Load<Sprite>("square_border");
            _fillImage.sprite = Managers.Resource.Load<Sprite>("square_fill");
        }

        IsTransparent = isTransparent;
        FillAmount = fillAmount;


        SetIndicatorColor();
        StartIndicatorSpreading();
    }

    public override float FillAmount
    {
        get => _fillAmount;
        set
        {
            _fillAmount = Mathf.Clamp01(value);
            FillImage.fillAmount = _fillAmount;
        }
    }

    protected override void StartIndicatorSpreading()
    {
        transform.localScale = new Vector3(1, 0, 1);
        transform.DOScale(Vector3.one, 0.2f);
    }


}