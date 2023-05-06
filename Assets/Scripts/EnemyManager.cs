using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    [SerializeField] float waveSpawnTime = 0.1f;
    [SerializeField] float spawnTime = 0.1f;
    [SerializeField] List<EnemyWave> enemyWaves;

    internal List<Enemy> aliveEnemies;

    internal int enemiesKilled;
    internal int wavesTarget;
    internal int currentPlay;

    Dictionary<Lane, List<ObjectPools.PoolNames>> nextWave;

    void Start()
    {
        aliveEnemies = new List<Enemy>();
        int wins = CharacterSelector.difficulty == 0 ? SavedData.savesData.wins :
            CharacterSelector.difficulty == 1 ? SavedData.savesData.normalWins :
            CharacterSelector.difficulty == 2 ? SavedData.savesData.hardWins :
            CharacterSelector.difficulty == 3 ? SavedData.savesData.nightmareWins: 0;
        wavesTarget = Mathf.Clamp( 10 + (wins * 2), 10, 30);
        WaveCounter.instance.DisplayCounter();
        Analytics.instance.StartedMatch();
        currentPlay = SavedData.savesData.gamesPlayed;
    }

    public void PrepareNextWave()
    {
        EnemyWave wave = enemyWaves[TurnController.currentTurn - 1];
        int enemiesToSkip = wave.enemiesSkippedForFirstPlays.Count > currentPlay ? wave.enemiesSkippedForFirstPlays[currentPlay] : 0;

        List<ObjectPools.PoolNames> myWave = new List<ObjectPools.PoolNames>();

        foreach (EnemyWave.EnemyWaveData enemyWave in wave.enemies)
        {
            for (int i = 0; i < enemyWave.amount; i++)
            {
                myWave.Add(enemyWave.type);
            }
        }

        List<Lane> openLanes = TileManager.instance.GetAllOpenLanes();
        nextWave = new Dictionary<Lane, List<ObjectPools.PoolNames>>();
        List<ObjectPools.PoolNames> temp;

        while (myWave.Count - enemiesToSkip > 0)
        {
            foreach (Lane lane in openLanes)
            {
                if (myWave.Count - enemiesToSkip > 0)
                {
                    if (!nextWave.TryGetValue(lane, out temp))
                    {
                        nextWave.Add(lane, new List<ObjectPools.PoolNames>());
                    }
                    int randomEnemyIndex = UnityEngine.Random.Range(0, myWave.Count);
                    nextWave[lane].Add(myWave[randomEnemyIndex]);
                    myWave.RemoveAt(randomEnemyIndex);
                }
            }
        }

        if (GlobalConditionHolder.banditsAppear)
        {
            foreach (Lane lane in openLanes)
            {
                nextWave[lane].Add(ObjectPools.PoolNames.enemyBandit);
            }

        }

        if (GlobalConditionHolder.showUpcomingEnemies)
        {
            ShowUpcomingEnemyInfo();
        }
    }

    void ShowUpcomingEnemyInfo()
    {

        foreach (KeyValuePair<Lane, List<ObjectPools.PoolNames>> nextWave in nextWave)
        {
            Dictionary<ObjectPools.PoolNames, int> enemiesCount = new Dictionary<ObjectPools.PoolNames, int>();
            foreach (ObjectPools.PoolNames enemy in nextWave.Value)
            {
                if (enemiesCount.ContainsKey(enemy))
                {
                    enemiesCount[enemy]++;
                }
                else
                {
                    enemiesCount.Add(enemy, 1);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                nextWave.Key.enemiesAmount[i].transform.parent.gameObject.SetActive(enemiesCount.Count < i);
            }

            int index = 0;
            foreach (KeyValuePair<ObjectPools.PoolNames, int> enemy in enemiesCount)
            {
                nextWave.Key.enemiesAmount[index].text = enemy.Value.ToString();
                nextWave.Key.enemiesSprites[index].sprite = EnemyAppearInfo.instance.GetEnemyImage(enemy.Key);

                index++;
            }
        }
    }


    public void NewTileAdded(Tile tile, Lane lane)
    {
        List<Lane> newLanes = new List<Lane>( tile.lanes);
        int lastLane = newLanes.Count;

        foreach(Lane newLane in newLanes)
        {
            nextWave.Add(newLane, new List<ObjectPools.PoolNames>());
        }

        foreach (ObjectPools.PoolNames poolName in nextWave[lane])
        {
            if(lastLane == newLanes.Count)
            {
                lastLane = 0;
            }
            nextWave[newLanes[lastLane]].Add(poolName);
            lastLane++;
        }

        nextWave.Remove(lane);
    }

    public void SpawnNextWave()
    {
        TurnController.StartedEnemyWave();
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        int maxAmount = 0;

        foreach (KeyValuePair<Lane, List<ObjectPools.PoolNames>> pools in nextWave)
        {
            if(maxAmount < pools.Value.Count)
            {
                maxAmount = pools.Value.Count;
            }
        }

        for (int i = 0; i < maxAmount; i++)
        {
            yield return new WaitForSeconds(waveSpawnTime);
            foreach (KeyValuePair<Lane, List<ObjectPools.PoolNames>> pools in nextWave)
            {
                if (pools.Value.Count > i)
                {
                    GameObject newEnemyObj = ObjectPools.instance.GetPool(pools.Value[i]).GetObject();
                    Enemy newEnemy = SpawnEnemy(newEnemyObj, pools.Key.spawnPoint.transform.position);
                    if (GlobalConditionHolder.increasedHealth)
                    {
                        newEnemy.tempMaxHealth[0] += newEnemy.maxHealth[0] * 0.25f;
                        newEnemy.currentHealth[0] += newEnemy.maxHealth[0] * 0.25f;
                        newEnemy.tempMaxHealth[1] += newEnemy.maxHealth[1] * 0.25f;
                        newEnemy.currentHealth[1] += newEnemy.maxHealth[1] * 0.25f;
                        newEnemy.tempMaxHealth[2] += newEnemy.maxHealth[2] * 0.25f;
                        newEnemy.currentHealth[2] += newEnemy.maxHealth[2] * 0.25f;
                        newEnemy.currentMoneyOnKill++;
                    }
                    FillMovementWithLanePath(newEnemy.movement, pools.Key);
                    yield return new WaitForSeconds(spawnTime);
                }
            }
        }
    }

    public Enemy SpawnEnemy(GameObject enemy, Vector3 spawnPos)
    {
        enemy.transform.parent = transform;
        enemy.transform.position = spawnPos;
        Enemy newEnemy = enemy.GetComponent<Enemy>();
        aliveEnemies.Add(newEnemy);
        newEnemy.ResetEnemy();
        return newEnemy;
    }

    void FillMovementWithLanePath(EnemyMovement enemyMovement, Lane lane)
    {
        while (lane != null)
        {
            foreach (Spot pathStop in lane.path)
            {
                enemyMovement.movementPath.Add(pathStop.transform.position);
            }
            lane = lane.prevLane;
        }
    }

    public void EnemyRemoved(Enemy enemy)
    {
        if (aliveEnemies.Contains(enemy))
        {
            foreach(Tower tower in TowerPlacer.allTowers)
            {
                tower.EnemyDestroyed(enemy);
            }

            aliveEnemies.Remove(enemy);
            if(aliveEnemies.Count <= 0)
            {
                StartCoroutine(FinishEnemyWaveAfterPlayerFinishesAction());
            }
        }
    }

    public void CheckIfGameWon()
    {
        if(wavesTarget == TurnController.currentTurn)
        {
            EndScreen.instance.Appear("Win!");
        }
    }

    IEnumerator FinishEnemyWaveAfterPlayerFinishesAction()
    {
        while (Input.GetMouseButton(0))
        {
            yield return null;
        }
        TurnController.FinishedEnemyWave();

    }
}
