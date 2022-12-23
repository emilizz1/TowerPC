using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    [SerializeField] float waveSpawnTime = 0.1f;
    [SerializeField] float spawnTime = 0.1f;
    [SerializeField] List<EnemyWave> enemyWaves;

    internal List<Enemy> aliveEnemies;

    void Start()
    {
        aliveEnemies = new List<Enemy>();
    }

    public void SpawnNextWave()
    {
        TurnController.StartedEnemyWave();
        StartCoroutine(SpawnEnemies(enemyWaves[TurnController.currentTurn]));
    }

    IEnumerator SpawnEnemies(EnemyWave wave)
    {
        EnemyWave myWave = Instantiate(wave);
        List<Lane> openLanes = TileManager.instance.GetAllOpenLanes();

        while (myWave.enemies.Count > 0)
        {
            yield return new WaitForSeconds(waveSpawnTime);

            foreach (Lane lane in openLanes)
            {
                if (myWave.enemies.Count > 0)
                {
                    int randomEnemyIndex = Random.Range(0, myWave.enemies.Count);
                    GameObject newEnemyObj = ObjectPools.instance.GetPool(myWave.enemies[randomEnemyIndex]).GetObject();
                    myWave.enemies.RemoveAt(randomEnemyIndex);
                    Enemy newEnemy =  SpawnEnemy(newEnemyObj, lane.spawnPoint.transform.position);
                    FillMovementWithLanePath(newEnemy.movement, lane);
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
                TurnController.FinishedEnemyWave();
            }
        }
    }

    public void CheckIfGameWon()
    {
        if(enemyWaves.Count == TurnController.currentTurn)
        {
            SceneManager.LoadScene(SceneManager.WIN);
        }
    }
}
