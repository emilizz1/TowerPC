using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tower : MonoBehaviour
{
    public Sprite image;
    public string towerName;

    [Header("Components")]
    [SerializeField] Transform top;
    [SerializeField] SphereCollider sphere;
    [SerializeField] Transform rangeSprite;
    [SerializeField] ParticleSystem levelUp;
    [SerializeField] ParticleSystem shootingParticles;

    [Header("Stats")]
    public List<TowerStats> towerStats;
    public List<int> experienceNeeded;
    public List<TowerTypes> towerTypes;
    [SerializeField] ObjectPools.PoolNames bulletType;

    [Serializable]
    public struct TowerStats
    {
        public float fireRate;
        public float range;
        public List<float> damage;

        public void CombineStats(TowerStats newStats)
        {
            fireRate += newStats.fireRate;
            range += newStats.range;

            if(newStats.damage == null)
            {
                return;
            }

            damage[0] += newStats.damage.Count > 0? newStats.damage[0]: 0;
            damage[1] += newStats.damage.Count > 1 ? newStats.damage[1] : 0;
            damage[2] += newStats.damage.Count > 2 ? newStats.damage[2] : 0;
        }
    }

    public enum TargetSelectOptions
    {
        First,
        Last,
        HighestHealth,
        HighestArmor,
        HighestShield
    }


    public enum TowerTypes
    {
        Arrow,
        Magic,
        All
    }

    const float DEFAULT_RANGE_SPRITE_RADIUS = 4f;

    const int EXP_PER_SHOT = 1;

    List<EnemyMovement> reachableEnemies = new List<EnemyMovement>();
    GameObject currentTarget;
    internal TargetSelectOptions targeting = TargetSelectOptions.First;
    float timeUntilShot;
    internal int experience;
    internal int currentLevel;

    internal TowerStats statsMultiplayers= new TowerStats();

    bool active;

    private void Start()
    {
        statsMultiplayers.fireRate = 1f;
        statsMultiplayers.range = 1f;
        statsMultiplayers.damage = new List<float>();
        for (int i = 0; i < 3; i++)
        {
            statsMultiplayers.damage.Add(1f);
        }

        statsMultiplayers.CombineStats(PasiveTowerStatsController.GetStats(towerTypes));

        reachableEnemies = new List<EnemyMovement>();
        SetupRange();
    }

    public void SetupRange()
    {
        sphere.radius = towerStats[currentLevel].range * statsMultiplayers.range;
        float rangeSpriteScale = (towerStats[currentLevel].range * statsMultiplayers.range) / DEFAULT_RANGE_SPRITE_RADIUS;
        rangeSprite.localScale = new Vector3(rangeSpriteScale, rangeSpriteScale, 1f);
    }

    private void Update()
    {
        if (active)
        {
            FindTarget();

            timeUntilShot -= Time.deltaTime;
            
            if (currentTarget!= null && currentTarget.gameObject.activeInHierarchy)
            {
                top.LookAt(currentTarget.transform);
                top.transform.localEulerAngles = new Vector3(0f, top.transform.rotation.eulerAngles.y, 0f);


                if (timeUntilShot < 0)
                {
                    Shoot();
                    timeUntilShot = towerStats[currentLevel].fireRate * statsMultiplayers.fireRate;
                }
            }
        }
    }

    private void Shoot()
    {
        GameObject newBullet = ObjectPools.instance.GetPool(bulletType).GetObject();
        newBullet.transform.parent = transform;
        newBullet.transform.position = top.transform.position;
        Bullet bullet = newBullet.GetComponent<Bullet>();
        bullet.damage = GetDamageMultiplied();
        bullet.target = currentTarget;
        if (shootingParticles != null)
        {
            shootingParticles.Play();
        }
        AddExperience();
    }

    List<float> GetDamageMultiplied()
    {
        List<float> finalDamage = new List<float>();
        for (int i = 0; i < towerStats[currentLevel].damage.Count; i++)
        {
            finalDamage.Add(towerStats[currentLevel].damage[i] * statsMultiplayers.damage[i]);
        }
        return finalDamage;
    }

    void AddExperience()
    {
        if(currentLevel == experienceNeeded.Count)
        {
            return;
        }

        experience += EXP_PER_SHOT + PasiveTowerStatsController.extraExperience;
        if(experience >= experienceNeeded[currentLevel])
        {
            experience -= experienceNeeded[currentLevel];
            currentLevel++;
            levelUp.Play();
            SetupRange();
        }
        TowerInfoWindow.instance.UpdateInfo(this);
    }

    void FindTarget()
    {
        if (reachableEnemies.Count > 0)
        {
            if (!reachableEnemies[0].gameObject.activeInHierarchy)
            {
                reachableEnemies.RemoveAt(0);
                return;
            }
        }

        if (reachableEnemies.Count == 0)
        {
            return;
        }

        switch (targeting)
        {
            case (TargetSelectOptions.First):
                currentTarget = GetTargetFirst().gameObject;
                return;
            case (TargetSelectOptions.Last):
                currentTarget = GetTargetLast().gameObject;
                return;
            case (TargetSelectOptions.HighestArmor):
                currentTarget = GetTargetMaxArmor().gameObject;
                return;
            case (TargetSelectOptions.HighestHealth):
                currentTarget = GetTargetMaxHealth().gameObject;
                return;
            case (TargetSelectOptions.HighestShield):
                currentTarget = GetTargetMaxShield().gameObject;
                return;
        }
    }

    EnemyMovement GetTargetFirst()
    {
        float minValue = reachableEnemies[0].GetPathRemaining();
        EnemyMovement target = reachableEnemies[0];
        foreach (EnemyMovement enemy in reachableEnemies)
        {
            if(enemy.GetPathRemaining() < minValue)
            {
                minValue = enemy.GetPathRemaining();
                target = enemy;
            }
        }
        return target;
    }

    EnemyMovement GetTargetLast()
    {
        float maxValue = reachableEnemies[0].GetPathRemaining();
        EnemyMovement target = reachableEnemies[0];
        foreach (EnemyMovement enemy in reachableEnemies)
        {
            if (enemy.GetPathRemaining() > maxValue)
            {
                maxValue = enemy.GetPathRemaining();
                target = enemy;
            }
        }
        return target;
    }

    EnemyMovement GetTargetMaxArmor()
    {
        float maxValue = 0;
        EnemyMovement target = reachableEnemies[0];
        foreach (EnemyMovement enemy in reachableEnemies)
        {
            if (enemy.enemy.currentHealth[1] > maxValue)
            {
                maxValue = enemy.enemy.currentHealth[1];
                target = enemy;
            }
        }
        return target;
    }

    EnemyMovement GetTargetMaxHealth()
    {
        float maxValue = 0;
        EnemyMovement target = reachableEnemies[0];
        foreach (EnemyMovement enemy in reachableEnemies)
        {
            if (enemy.enemy.currentHealth[0] > maxValue)
            {
                maxValue = enemy.enemy.currentHealth[0];
                target = enemy;
            }
        }
        return target;
    }

    EnemyMovement GetTargetMaxShield ()
    {
        float maxValue = 0;
        EnemyMovement target = reachableEnemies[0];
        foreach (EnemyMovement enemy in reachableEnemies)
        {
            if (enemy.enemy.currentHealth[2] > maxValue)
            {
                maxValue = enemy.enemy.currentHealth[2];
                target = enemy;
            }
        }
        return target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<EnemyMovement>() != null)
        {
            reachableEnemies.Add(other.GetComponent<EnemyMovement>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EnemyMovement>() != null)
        {
            reachableEnemies.Remove(other.GetComponent<EnemyMovement>());
        }
    }

    public void Activate()
    {
        active = true;
        rangeSprite.gameObject.SetActive(false);   
    }
}
