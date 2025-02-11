using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class GameStatusPanel : UI_Base 
{

    enum Texts
    {
        TimerText,
        FPSText,
        StatsText,
    }

    TMP_Text timerText;
    TMP_Text fpsText;

    float ms;
    int sec;
    int min;

    Dictionary<string, float> statInfos = new();

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindTMPTexts(typeof(Texts));
        
        timerText = GetTMPText((int)Texts.TimerText);
        fpsText = GetTMPText((int)Texts.FPSText);

        return true;   
    }

    EventBinding<TEST_HeroSpawnEvent> _heroSpawnEventBinding;
    void OnEnable()
    {
        _heroSpawnEventBinding = new EventBinding<TEST_HeroSpawnEvent>(HandleHeroSpawnEvent);
        EventBus<TEST_HeroSpawnEvent>.Register(_heroSpawnEventBinding);
    }

    void OnDisable()
    {
        EventBus<TEST_HeroSpawnEvent>.Deregister(_heroSpawnEventBinding);
    }
    private void HandleHeroSpawnEvent()
    {
        // Hero의 Stats 확인 가능
        Hero hero = Managers.Hero.MainHero;

        statInfos.Clear();
        foreach (var stat in hero.StatsComponent.Stats)
        {
            statInfos.Add(stat.CodeName, stat.Value);

            stat.onValueChanged -= HandleStatsChanged;
            stat.onValueChanged += HandleStatsChanged;
        }
        PrintStats();
    }

    void Start()
    {
        timerText.text = "00:00";
    }

    void Update()
    {
        ms += Time.deltaTime;

        if (sec >= 60.00f)
        {
            min += 1;
            sec = 0;
        }

        if (ms >= 1.00f)
        {
            sec += 1;
            ms %= 1.00f;

            timerText.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);
            fpsText.text = string.Format("{0:N1}FPS", 1.0f / Time.deltaTime);
        }
    }

    void HandleStatsChanged(Stat stat, float currentValue, float prevValue)
    {
        statInfos[stat.CodeName] = currentValue;
        PrintStats();
    }

    void PrintStats()
    {
        StringBuilder statsString = new();

        foreach (var statInfo in statInfos)
        {
            statsString.AppendLine($"{statInfo.Key}: {statInfo.Value}");
        }

        GetTMPText((int)Texts.StatsText).text = statsString.ToString();
    }
}