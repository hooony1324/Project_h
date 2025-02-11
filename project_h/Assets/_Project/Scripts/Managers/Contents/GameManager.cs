using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameManager
{
    private Vector2 _moveDir = Vector2.zero;
    public Vector2 MoveDir 
    {
        get { return _moveDir; }
        set 
        {
            _moveDir = value;
            OnMoveDirChanged?.Invoke(value);
        }
    }

    private EJoystickState _joystickState;
    public EJoystickState JoystickState
    {
        get => _joystickState;
        set
        {
            _joystickState = value;
            OnJoystickStateChanged?.Invoke(_joystickState);
        }
    }

    public float TriggerTab
    {
        set
        {
            OnTabTriggered?.Invoke(value);
        }
    }

    public event Action<float> OnTabTriggered;
    public event Action<Vector2> OnMoveDirChanged;
    public event Action<EJoystickState> OnJoystickStateChanged;

    private CameraController _cam;

    public CameraController Cam
    {
        get
        {
            if (_cam == null)
            {
                _cam = Object.FindObjectOfType<CameraController>();
            }

            return _cam;
        }
    }

    private PlayerController playerController;

    public PlayerController PlayerController
    {
        get
        {
            if (playerController == null)
                playerController = Object.FindObjectOfType<PlayerController>();
            
            return playerController;
        }
    }

    public void GoToLobby()
    {
        ResumeGame();
        Managers.SaveLoad.RemovePlayData();
        Managers.Scene.LoadScene(EScene.SelectHeroScene);
    }

    public void ReviveHero()
    {
        // 체력 올려서 부활
        Stat hpStat = Managers.Hero.MainHero.StatsComponent.HPStat;
        hpStat.DefaultValue = hpStat.MaxValue;

        EventBus<HeroRevialEvent>.Raise(new HeroRevialEvent());
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

}

