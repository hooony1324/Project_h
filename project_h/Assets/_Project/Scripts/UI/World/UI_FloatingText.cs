using DG.Tweening;
using UnityEngine;
using static Define;

public class UI_FloatingText : UI_Base
{
    enum Texts
    {
        Text,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindTMPTexts(typeof(Texts));

        return true;
    }

    float distance = 3f;
    float duration = 2f;

    public void SetInfo(Vector2 spawnPosition, string message)
    {
        GetTMPText((int)Texts.Text).text = message;
        
        transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);

        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        //canvas.worldCamera = Managers.Game.Cam.UICamera;
        canvas.worldCamera = Camera.main;
        canvas.sortingOrder = SortingLayers.DAMAGE_FONT;
        
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMoveY(transform.position.y + distance, duration).SetEase(Ease.OutCirc))
        .OnComplete(() => { Managers.Resource.Destroy(gameObject);});
    }
}