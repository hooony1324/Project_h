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
    private int _killedMonsterCount = 0;
    private float _waveTimer = 0f;
    private bool _isWaveInProgress = false;


    [Header("Wave Settings")]
    [HelpBox("ForceSpawnAfterDuration\n false : 웨이브 처리할 때까지 기다림\n true : 웨이브 처리 못해도 진행")]
    [SerializeField] private bool _forceSpawnAfterDuration = false;

    
    private int _totalMonsterCount = 0;

    [Space(20)]
    [SerializeField] private List<float> _waveDurationList;
    
    private class SpawnData
    {
        public string monsterDataName;
        public Vector3 position;
    }

    private SpawnData[][] _spawnDataList;

    public void InitWaveDatas()
    {
        _spawnDataList = new SpawnData[transform.childCount][];
        for (int i = 0; i < transform.childCount; i++)
        {
            Tilemap tilemap = transform.GetChild(i).GetComponent<Tilemap>();

            if (tilemap == null)
                continue;

            var bounds = tilemap.cellBounds;
            var size = bounds.size;
            _spawnDataList[i] = new SpawnData[size.x * size.y];

            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    Vector3Int tilePos = new Vector3Int(x, y, 0);
                    var tile = tilemap.GetTile(tilePos) as EntityTile;

                    if (tile != null)
                    {
                        int index = (x - bounds.min.x) * size.y + (y - bounds.min.y);
                        _spawnDataList[i][index] = new SpawnData()
                        {
                            monsterDataName = tile.entityDataName,
                            position = tilemap.CellToWorld(tilePos),
                        };
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
        if (waveIndex >= _spawnDataList.Length)
        {
            onWavesCleared.Invoke();
            return;
        }

        _currentWaveIndex = waveIndex;
        _killedMonsterCount = 0;
        _waveTimer = Time.time + _waveDurationList[waveIndex];
        _isWaveInProgress = true;
        _totalMonsterCount = 0;

        foreach (var spawnData in _spawnDataList[waveIndex])
        {
            if (spawnData == null)
                continue;



            SpawnMonster(spawnData.monsterDataName, spawnData.position);
            _totalMonsterCount++;
        }

    }

    private void SpawnMonster(string monsterDataName, Vector3 position)
    {
        Monster monster = Managers.Object.Spawn<Monster>(position);
        monster.SetData(Managers.Data.GetMonsterData(monsterDataName));
        monster.onDead += OnMonsterDeath;
    }

    private void OnMonsterDeath(Entity entity)
    {
        _killedMonsterCount++;
        
        if (_killedMonsterCount >= _totalMonsterCount)
        {
            StartNextWave();
        }
    }

    private void StartNextWave()
    {
        _isWaveInProgress = false;
        StartWave(_currentWaveIndex + 1);
    }

}