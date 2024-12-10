using System;
using UnityEngine;

public class UI_HeroSelector : UI_Base
{
    // EventBus??
    // - Raise는 Slot이 발생시킴

    enum GameObjects
    {
        HeroSelection,
        SelectionGlow,
    }

    EventBinding<HeroSelectEvent> _heroSelectionEventBinding;
    private GameObject _heroSelection;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindGameObjects(typeof(GameObjects));

      
        _heroSelection = GetGameObject((int)GameObjects.HeroSelection);

        foreach (Transform child in _heroSelection.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var data in Managers.Data.HeroDatas)
        {
            var heroInfo = (data.Key, data.Value.AnimatorControllerName);

            UI_HeroSelectSlot heroSelectSlot = Managers.Resource.Instantiate(nameof(UI_HeroSelectSlot), _heroSelection.transform).GetComponent<UI_HeroSelectSlot>();
            heroSelectSlot?.Setup(heroInfo.Item1, heroInfo.Item2);
        }

        return true;
    }

    void OnEnable()
    {
        _heroSelectionEventBinding = new EventBinding<HeroSelectEvent>(HandleHeroSelectEvent);
        EventBus<HeroSelectEvent>.Register(_heroSelectionEventBinding);

        GameObject glowObject = GetGameObject((int)GameObjects.SelectionGlow);
        glowObject.SetActive(false);
    }

    void OnDisable()
    {
        EventBus<HeroSelectEvent>.Deregister(_heroSelectionEventBinding);
    }

    void HandleHeroSelectEvent(HeroSelectEvent selectEvent)
    {
        GameObject glowObject = GetGameObject((int)GameObjects.SelectionGlow);
        glowObject.SetActive(true);
        glowObject.transform.position = selectEvent.selectPosition;
        

        // 게임 진행할 Hero를 Manager에 설정 > GameScene입장
    }

}