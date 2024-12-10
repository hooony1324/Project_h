using System.Linq;
using System.Text;
using UnityEngine;

public class UI_HeroInfo : UI_Base
{
    enum TMPTexts
    {
        HeroNameText,
        HeroInfoText,
    }

    enum GameObjects
    {
        FlexibleLayout,
        StartButton,
    }

    EventBinding<HeroSelectEvent> _heroSelectionEventBinding;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindTMPTexts(typeof(TMPTexts));
        BindGameObjects(typeof(GameObjects));

        GetGameObject((int)GameObjects.StartButton).BindEvent(OnClickStartButton);

        GetGameObject((int)GameObjects.FlexibleLayout).SetActive(false);

        return true;
    }

    void OnEnable()
    {
        _heroSelectionEventBinding = new EventBinding<HeroSelectEvent>(HandleHeroSelectEvent);
        EventBus<HeroSelectEvent>.Register(_heroSelectionEventBinding);
    }

    void OnDisable()
    {
        EventBus<HeroSelectEvent>.Deregister(_heroSelectionEventBinding);
    }

    private void HandleHeroSelectEvent(HeroSelectEvent selectEvent)
    {
        Managers.Game.SetHeroData(selectEvent.heroDataName);

        HeroData heroData = Managers.Data.GetHeroData(selectEvent.heroDataName);
        Stat[] stats = heroData.StatOverrides.Select(x => x.CreateStat()).ToArray();

        StringBuilder sb = new StringBuilder();
        foreach (Stat stat in stats)
        {
            sb.AppendLine($"{stat.CodeName}: {stat.Value}");
        }

        sb.AppendLine("부연 설명...");

        GetTMPText((int)TMPTexts.HeroNameText).text = heroData.entityId;
        GetTMPText((int)TMPTexts.HeroInfoText).text = sb.ToString();

        GetGameObject((int)GameObjects.FlexibleLayout).SetActive(true);
    }

    void OnClickStartButton()
    {
        Managers.Scene.LoadScene(EScene.GameScene);
    }

}