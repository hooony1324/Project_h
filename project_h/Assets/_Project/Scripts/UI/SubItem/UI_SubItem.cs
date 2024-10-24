using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SubItem : UI_Base
{
    [SerializeField]
    protected ScrollRect _parentScrollRect;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        return true;
    }

    /// <summary> 
    /// Scroll Content의 Item을 눌러도 Scroll이 되도록
    /// ScrollView의 자식일 때 호출해야 유효함
    /// </summary>
    protected void SetParentScrollRect()
    {
        if (_parentScrollRect != null)
            return;

        _parentScrollRect = Util.FindAncestor<ScrollRect>(gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _parentScrollRect.OnBeginDrag(eventData);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        _parentScrollRect.OnDrag(eventData);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        _parentScrollRect.OnEndDrag(eventData);
    }
}
