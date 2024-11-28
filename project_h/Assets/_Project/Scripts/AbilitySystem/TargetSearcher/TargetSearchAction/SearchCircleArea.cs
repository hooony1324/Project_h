using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SearchCircleArea: TargetSearchAction
{
    [Header("Data")]
    [Min(0f)]
    [SerializeField]
    private float range;
    [Range(0f, 360f)]
    [SerializeField]
    private float angle = 360f;
    // 검색을 요청한 Entity도 검색 대상에 포함할 것인가?
    [SerializeField]
    private bool isIncludeSelf;
    // Target이 검색을 요청한 Entity와 같은 Category를 가지고 있어야하는가?
    [SerializeField]
    private bool isSearchSameCategory;

    public override object Range => range;
    // SearchAction의 Scale에 따라 검색 범위가 달라짐
    public override object ScaledRange => range * Scale;
    public override float Angle => angle;

    public SearchCircleArea() { }

    public SearchCircleArea(SearchCircleArea copy)
        : base(copy)
    {
        range = copy.range;
        isIncludeSelf = copy.isIncludeSelf;
        isSearchSameCategory = copy.isSearchSameCategory;
    }

    public override TargetSearchResult Search(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, TargetSelectionResult selectResult)
    {
        var targets = new List<GameObject>();
        var circlePosition = selectResult.resultMessage == SearchResultMessage.FindTarget ?
            selectResult.selectedTarget.transform.position : selectResult.selectedPosition;

        circlePosition = requesterObject.transform.position;

        var colliders = Physics2D.OverlapCircleAll(circlePosition, (float)ProperRange);
        Vector3 requesterPosition = requesterObject.transform.position;
        var rushDirection = targetSearcher.SelectionResult.selectedPosition - requesterPosition;

        foreach (var collider in colliders)
        {
            var entity = collider.GetComponent<Entity>();
            // Entity가 null이거나, 이미 죽은 상태거나, 검색을 명령한 Entity인데 isIncludeSelf가 true가 아닐 경우 넘어감
            if (!entity || entity.IsDead || (entity == requesterEntity && !isIncludeSelf))
                continue;

            if (entity != requesterEntity)
            {
                // Requester와 Entity가 공유하는 Category가 있는지 확인
                var hasCategory = requesterEntity.Categories.Any(x => entity.HasCategory(x));
                // 공유하는 Category가 있지만 isSearchSameCategory가 false거나,
                // 공유하는 Category가 없지만 isSearchSameCategory가 true라면 넘어감
                if ((hasCategory && !isSearchSameCategory) || (!hasCategory && isSearchSameCategory))
                    continue;
            }

            Vector3 entityPosition = entity.transform.position;
            entityPosition.z = requesterPosition.z;

            var targetDirection = entityPosition - requesterPosition;
            
            // 수정, 공격하는 방향과의 각도 차이
            if (Vector3.Angle(rushDirection, targetDirection) < (angle * 0.5f))
                targets.Add(entity.gameObject);
        }
        return new(targets.ToArray());
    }

    protected override IReadOnlyDictionary<string, string> GetStringsByKeyword()
    {
        var dictionary = new Dictionary<string, string>() { { "range", range.ToString("0.##") } };
        return dictionary;
    }

    public override object Clone() => new SearchCircleArea(this);
}
