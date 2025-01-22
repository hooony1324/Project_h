using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonManager
{
    public Dungeon CurrentDungeon { get; private set; }

    public DungeonData CurrentDungeonData => _currentDungeonData;
    private DungeonData _currentDungeonData;
    public int CurrentDungeonId => _currentDungeonData ? _currentDungeonData.Id : 0;

    public void Setup(Dungeon dungeon)
    {
        CurrentDungeon = dungeon;
    }

    public void SetFirstDungeon(DungeonData firstDungeonData)
    {
        _currentDungeonData = firstDungeonData;
    }

    public void EnterFirstDungeon()
    {
        if (_currentDungeonData == null)
            return;

        Managers.Map.SetMap(_currentDungeonData.PrefabName);
        Managers.Scene.LoadScene(EScene.DungeonScene);
    }

    public void TryEnterNextDungeon()
    {
        if (_currentDungeonData == null)
            return;

        if (!_currentDungeonData.HasNextDungeon)
        {
            // 마지막 던전 -> 마을
            if  (_currentDungeonData.IsFinalDungeon)
            {
                
                Managers.Map.SetMap("BaseMap");
                Managers.Scene.LoadScene(EScene.GameScene);
            }

            return;
        }

        // 스탯 저장 -> 다음 던전 시작 시 적용
        //_heroStats = Managers.Hero.MainHero.Stats;
        
        DungeonData nextDungeonData = Managers.Data.GetDungeonData(_currentDungeonData.NextDungeonId);
        if (nextDungeonData == null)
        {
            Debug.Log("다음 던전 데이터를 설정하지 않았습니다.");
            return;
        }
        
        // 재입장 할 경우의 상황 저장
        _currentDungeonData = nextDungeonData;
        // HeroInfo = asdfasdfsda;

        // 다음 던전으로 입장
        Managers.Map.SetMap(nextDungeonData.PrefabName);
        Managers.Scene.LoadScene(EScene.DungeonScene);

        return;
    }

    public void Clear()
    {
        CurrentDungeon = null;
        _currentDungeonData = null;
    }

    public async Awaitable GenerateDungeon()
    {
        await CurrentDungeon?.GenerateDungeon();
    }
}