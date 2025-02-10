using DG.Tweening;
using TMPro;
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

    public void SetInfo(string message, Vector2 spawnPosition, EFloatingTextType type = EFloatingTextType.Damage)
    {
        TMP_Text floatingText = GetTMPText((int)Texts.Text);
        floatingText.text = message;

        switch (type)
        {
            case EFloatingTextType.Damage:
                floatingText.color = Color.white;
                break;
            case EFloatingTextType.Heal:
                floatingText.color = Color.green;
                break;
            case EFloatingTextType.Buff:
                floatingText.color = Color.yellow;
                break;

            case EFloatingTextType.Debuff:
                floatingText.color = Color.blue;
                break;
            case EFloatingTextType.CC:
                floatingText.color = Color.red;
                break;
        }

        transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);

        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        canvas.sortingOrder = SortingLayers.WORLD_FONT;
        
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMoveY(transform.position.y + distance, duration).SetEase(Ease.OutCirc))
        .OnComplete(() => { Managers.Resource.Destroy(gameObject);});
    }
}