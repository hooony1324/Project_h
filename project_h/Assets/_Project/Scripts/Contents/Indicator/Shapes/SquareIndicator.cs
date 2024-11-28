using UnityEngine;

public class SquareIndicator : Indicator
{
    public struct SquareArea : IAreaData
    {
        public Vector2 scale;

        // 시전자와의 거리 float -> area.posY
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public override void Setup(IAreaData shape, float fillAmount = 0, Transform traceTarget = null)
    {
        SquareArea squareShape = (SquareArea)shape;

        TraceTarget = traceTarget;
        Area.transform.localScale = new Vector3(squareShape.scale.x, squareShape.scale.y, 1);

        FillAmount = fillAmount;
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


}