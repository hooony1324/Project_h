using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterWaveController : InitOnce
{
    public delegate void WavesClearedHandler();
    public event WavesClearedHandler onWavesCleared = () => { };

    private int _currentWaveIndex = 0;
    private float _waveTimer = 0f;
    private bool _isWaveInProgress = false;


    [Header("Wave Settings")]
    [HelpBox("ForceSpawnAfterDuration\n false : 웨이브 처리할 때까지 기다림\n true : 웨이브 처리 못해도 진행")]
    [SerializeField] private bool _forceSpawnAfterDuration = false;

    
    private int _killCount = 0;
    private int _targetKillCount = 0;
    private int _totalKillCount = 0;
    private int _totalMonsterCount = 0;

    [Space(20)]
    [SerializeField] private List<float> _waveDurationList;
    
    private class SpawnData
    {
        public int monsterID;
        public Vector3 position;
    }

    private List<List<SpawnData>> _spawnDataList;

    public void InitWaveDatas(DungeonRoom owner)
    {
        GameObject monsterWavesObj = Util.FindChild(gameObject, "MonsterWaves");
        _spawnDataList = new ();
        _totalMonsterCount = 0;

        int waveCount = monsterWavesObj.transform.childCount;
        for (int i = 0; i < waveCount; i++)
        {
            Tilemap tilemap = monsterWavesObj.transform.GetChild(i).GetComponent<Tilemap>();

            if (tilemap == null)
                continue;

            var bounds = tilemap.cellBounds;
            var size = bounds.size;

            _spawnDataList.Add(new List<SpawnData>());

            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    Vector3Int tilePos = new Vector3Int(x, y, 0);
                    var tile = tilemap.GetTile(tilePos) as EntityTile;

                    if (tile != null)
                    {
                        int index = (x - bounds.min.x) * size.y + (y - bounds.min.y);
                        _spawnDataList[i].Add(new SpawnData()
                        {
                            monsterID = tile.ID,
                            position = tilemap.CellToWorld(tilePos),
                        });
                        _totalMonsterCount++;
                    }
                }
            }

            tilemap.gameObject.SetActive(false);
        }
    }


    private void Update()
    {
        if (!_isWaveInProgress)
            return;

        // 웨이브 처리 못해도 진행하는 경우
        if (_forceSpawnAfterDuration)
        {
            if (Time.time >= _waveTimer)
            {
                StartNextWave();
            }
        }
    }

    public void StartWave(int waveIndex = 0)
    {
        _currentWaveIndex = waveIndex;
        _killCount = 0;
        _isWaveInProgress = true;
        _targetKillCount = 0;

        if (_forceSpawnAfterDuration)
        {
            _waveTimer = Time.time + _waveDurationList[waveIndex];
        }

        foreach (var spawnData in _spawnDataList[waveIndex])
        {
            if (spawnData == null)
                continue;

            SpawnMonster(spawnData.monsterID, spawnData.position);
            _targetKillCount++;
        }

    }

    private void SpawnMonster(int monsterID, Vector3 position)
    {
        Monster monster = Managers.Object.Spawn<Monster>(position);
        monster.SetData(Managers.Data.GetMonsterData(monsterID));
        monster.onDead -= OnMonsterDeath;
        monster.onDead += OnMonsterDeath;
    }

    private void OnMonsterDeath(Entity entity)
    {
        _killCount++;
        _totalKillCount++;
        
        if (_killCount >= _targetKillCount)
        {
            if (_totalKillCount >= _totalMonsterCount)
            {
                onWavesCleared?.Invoke();
                onWavesCleared = null;
            }
            else
            {
                StartNextWave();
            }
        }
    }

    private void StartNextWave()
    {
        _isWaveInProgress = false;
        StartWave(_currentWaveIndex + 1);
    }

}