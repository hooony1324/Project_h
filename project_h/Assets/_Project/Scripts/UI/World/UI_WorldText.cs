using DG.Tweening;
using UnityEngine;
using static Define;

public class UI_WorldText : UI_Base
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

        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        canvas.sortingOrder = SortingLayers.WORLD_FONT;

        return true;
    }

    public void SetInfo(string message, Vector3 spawnPosition)
    {
        GetTMPText((int)Texts.Text).text = message;
        transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);
    }

    public void SetInfo(string message)
    {
        GetTMPText((int)Texts.Text).text = message;
    }
}