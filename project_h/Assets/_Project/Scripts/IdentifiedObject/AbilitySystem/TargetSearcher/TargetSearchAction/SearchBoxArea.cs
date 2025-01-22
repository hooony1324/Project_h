using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SearchBoxArea : TargetSearchAction
{
    [Header("Data")]
    [SerializeField]
    private Vector2 range;
    
    // 검색을 요청한 Entity도 검색 대상에 포함할 것인가?
    [SerializeField]
    private bool isIncludeSelf;
    // Target이 검색을 요청한 Entity와 같은 Category를 가지고 있어야하는가?
    [SerializeField]
    private bool isSearchSameCategory;

    public override object Range => range;
    // SearchAction의 Scale에 따라 검색 범위가 달라짐
    public override object ScaledRange => range * Scale;
    public override float Angle => 0;

    public SearchBoxArea() { }

    public SearchBoxArea(SearchBoxArea copy)
        : base(copy)
    {
        isIncludeSelf = copy.isIncludeSelf;
        isSearchSameCategory = copy.isSearchSameCategory;
    }

    public override TargetSearchResult Search(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, TargetSelectionResult selectResult)
    {
        var targets = new List<GameObject>();
        var targetPoint = selectResult.resultMessage == SearchResultMessage.FindTarget ?
            selectResult.selectedTarget.transform.position : selectResult.selectedPosition;

        Vector2 boxSize = (Vector2)ProperRange;
        Vector3 requesterPosition = requesterObject.transform.position;
        Vector3 rushDirection = targetPoint - requesterPosition;
        rushDirection = rushDirection.With(z:0).normalized * boxSize.y;

        Vector2 boxPosition = requesterPosition + rushDirection * 0.5f;
        float angle = Mathf.Atan2(rushDirection.y, rushDirection.x) * Mathf.Rad2Deg + 90f;
        
        var colliders = Physics2D.OverlapBoxAll(boxPosition, boxSize, angle);

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
                // 공유 Category가 있지만 isSearchSameCategory가 false거나,
                // 공유하는 Category가 없지만 isSearchSameCategory가 true라면 넘어감
                if ((hasCategory && !isSearchSameCategory) || (!hasCategory && isSearchSameCategory))
                    continue;
            }

            targets.Add(entity.gameObject);
        }

        #if UNITY_EDITOR
        GizmoSpawner.Instance.DrawDebugBox(boxPosition, boxSize, angle, 4f);
        #endif

        return new(targets.ToArray());
    }

    protected override IReadOnlyDictionary<string, string> GetStringsByKeyword()
    {
        var dictionary = new Dictionary<string, string>() { { "range", range.ToString("0.##") } };
        return dictionary;
    }

    public override object Clone() => new SearchBoxArea(this);
}
