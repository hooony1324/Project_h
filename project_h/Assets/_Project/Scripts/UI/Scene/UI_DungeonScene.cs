public class UI_DungeonScene : UI_Scene
{
    #region Events
    EventBinding<HeroDeadEvent> heroDeadEventBinding;
    EventBinding<HeroRevialEvent> heroRevialEventBinding;
    void OnEnable()
    {
        heroDeadEventBinding = new EventBinding<HeroDeadEvent>(HandleHeroDead);
        EventBus<HeroDeadEvent>.Register(heroDeadEventBinding);

        heroRevialEventBinding = new EventBinding<HeroRevialEvent>(HandleHeroRevial);
        EventBus<HeroRevialEvent>.Register(heroRevialEventBinding);
    }

    void OnDisable()
    {
        EventBus<HeroDeadEvent>.Deregister(heroDeadEventBinding);
        EventBus<HeroRevialEvent>.Deregister(heroRevialEventBinding);
    }
    #endregion

    enum GameObjects
    {
        CharacterStatus,
        PlayContinue,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
            
        BindGameObjects(typeof(GameObjects));
        GetGameObject((int)GameObjects.PlayContinue).SetActive(false);
        return true;
    }

    public void Setup(Entity mainHero)
    {
        GetGameObject((int)GameObjects.CharacterStatus).GetComponent<CharacterStatus>().Setup(mainHero);
    }

    void HandleHeroDead()
    {
        GetGameObject((int)GameObjects.PlayContinue).SetActive(true);
    }

    void HandleHeroRevial()
    {
        GetGameObject((int)GameObjects.PlayContinue).SetActive(false);
    }
}
