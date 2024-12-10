using UnityEditor.Animations;
using UnityEngine;

public class UI_HeroSelectSlot : UI_Base
{
    enum GameObjects
    {
        HeroSprite,
    }

    EventBinding<HeroSelectEvent> _heroSelectionEventBinding;

    // dataName, AnimatorController'
    private string _heroDataName;
    private string _animatorControllerName;
    private Animator _animator;

    private int _idleHash = Animator.StringToHash("Idle");
    private int _walkHash = Animator.StringToHash("Walk");

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindGameObjects(typeof(GameObjects));

        _animator = GetGameObject((int)GameObjects.HeroSprite).GetComponent<Animator>();

        gameObject.BindEvent(OnClick);

        return true;
    }
    public void Setup(string heroDataName, string animatorControllerName)
    {
        _heroDataName = heroDataName;
        _animatorControllerName = animatorControllerName;

        string controllerName = $"{_animatorControllerName}_Selection";
        _animator.runtimeAnimatorController = Managers.Resource.Load<AnimatorOverrideController>(controllerName);
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

    void OnClick()
    {
        EventBus<HeroSelectEvent>.Raise(new HeroSelectEvent
        {
            heroDataName = _heroDataName,
            selectPosition = transform.position,        // for SelectionGlow Position
        });
    }

    void HandleHeroSelectEvent(HeroSelectEvent selectEvent)
    {
        if (selectEvent.heroDataName == _heroDataName)
        {
            // 선택됨
            _animator.Play(_walkHash);
        }
        else
        {
            // 선택 해제
            _animator.Play(_idleHash);
        }
    }

}