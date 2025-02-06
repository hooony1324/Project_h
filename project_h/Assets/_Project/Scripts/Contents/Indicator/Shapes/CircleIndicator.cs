using Unity.VisualScripting;
using UnityEngine;

public class CircleIndicator : Indicator
{
    public struct CircleArea : IAreaData
    {
        public float angle;
        public float radius;
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

        CircleArea circleShape = (CircleArea)shape;

        TraceTarget = traceTarget;
        Area.transform.localScale = new Vector3(circleShape.radius * 2, circleShape.radius * 2, 1);

        float angle = Mathf.Clamp(circleShape.angle, 0f, 360f);
        BorderImage.fillAmount = angle / 360f;
        FillImage.fillAmount = BorderImage.fillAmount; 
        IsTransparent = isTransparent;

        // Y벡터를 향한 부채꼴
        Canvas.transform.eulerAngles = new Vector3(0, 0, angle * 0.5f);

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
            FillImage.transform.localScale = new Vector3(_fillAmount, _fillAmount, 1);
        }
    }

}