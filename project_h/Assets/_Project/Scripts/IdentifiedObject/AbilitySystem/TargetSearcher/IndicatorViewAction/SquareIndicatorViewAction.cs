using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SquareIndicatorViewAction : IndicatorViewAction
{
    [SerializeField]
    private GameObject indicatorPrefab;

    [SerializeField]
    private bool isArrowType;

    // Indicator의 속을 채우는 fillAmount Property를 사용할 것인가?
    [SerializeField]
    private bool isUseIndicatorFillAmount;
    
    // Indicator를 requsterObject의 자식 Object로 만들 것인가?
    [SerializeField]
    private bool isAttachIndicatorToRequester;

    // ShowIndicator 함수로 생성한 Indicator
    private Indicator spawnedRangeIndicator;

    public override void ShowIndicator(TargetSearcher targetSearcher, GameObject requesterObject,
        object range, float angle, float fillAmount, bool isTransparent)
    {
        Debug.Assert(range is Vector2, "SquareIndicatorViewAction::ShowIndicator - range는 null 또는 Vector2형만 허용됩니다.");

        // 이미 Indicator를 보여주고 있다면 먼저 Hide 처리를 해줌
        HideIndicator();

        fillAmount = isUseIndicatorFillAmount ? fillAmount : 0f;

        var attachTarget = isAttachIndicatorToRequester ? requesterObject.transform : null;

        Vector2 rangeScale = (Vector2)range;
        // Indicator를 생성하고, Setup 함수로 위에서 정한 값들을 Setting해줌
        Entity owner = requesterObject.GetComponent<Entity>();
        spawnedRangeIndicator = GameObject.Instantiate(indicatorPrefab).GetComponent<Indicator>();
        spawnedRangeIndicator.Setup(owner, new SquareIndicator.SquareArea() { scale = new Vector2(rangeScale.x, rangeScale.y), isArrowType = this.isArrowType}, fillAmount, traceTarget:attachTarget, isTransparent);

        // Rotate Indicator to target position
        var targetPos = targetSearcher.SelectionResult.selectedPosition;
        var dir = targetPos - requesterObject.transform.position;
        spawnedRangeIndicator.transform.localRotation = Quaternion.identity;
        float rotateAngle = Vector2.SignedAngle(Vector2.up, new Vector2(dir.x, dir.y));
        spawnedRangeIndicator.transform.Rotate(Vector3.forward, rotateAngle);
    }

    public override void HideIndicator()
    {
        if (!spawnedRangeIndicator)
            return;

        GameObject.Destroy(spawnedRangeIndicator.gameObject);
    }

    public override void SetFillAmount(float fillAmount)
    {
        if (!isUseIndicatorFillAmount || spawnedRangeIndicator == null)
            return;

        spawnedRangeIndicator.FillAmount = fillAmount;
    }

    public override object Clone()
    {
        return new SquareIndicatorViewAction()
        {
            indicatorPrefab = indicatorPrefab,
            isUseIndicatorFillAmount = isUseIndicatorFillAmount,
            isAttachIndicatorToRequester = isAttachIndicatorToRequester
        };
    }
}
