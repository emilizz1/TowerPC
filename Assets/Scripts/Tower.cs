using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using I2.Loc;
using System;

public class Tower : MonoBehaviour
{
    public Sprite image;
    public string towerName;
    public LocalizedString specialText;
    public LocalizedString specialFirstText;
    public LocalizedString specialSecondText;
    public TowerType towerType;

    [Header("Components")]
    public Transform rangeSprite;
    [SerializeField] Transform top;
    [SerializeField] SphereCollider sphere;
    [SerializeField] ParticleSystem levelUp;
    [SerializeField] ParticleSystem shootingParticles;
    [SerializeField] List<GameObject> levelUpRings;
    [SerializeField] GameObject canvas;
    [SerializeField] AudioSource audioSource;
    public Transform bulletSpawnPos;

    [Header("Stats")]
    public List<TowerStats> towerStats;
    public List<int> experienceNeeded;
    public List<DamageTypes> damageTypes;
    [SerializeField] ObjectPools.PoolNames bulletType;
    [SerializeField] AudioClip shootSound;

    public enum TargetSelectOptions
    {
        First,
        Last,
        HighestHealth,
        HighestArmor,
        HighestShield,
        LowestHealth,
        LowestArmor,
        LowestShield
    }

    const float DEFAULT_RANGE_SPRITE_RADIUS = 4f;
    const int EXP_PER_SHOT = 1;

    internal TargetSelectOptions targeting = TargetSelectOptions.First;
    internal TowerStats statsMultiplayers = new TowerStats();
    internal int experience;
    internal int currentLevel;
    internal int additionalGoldPerKill = 0;
    
    List<EnemyMovement> reachableEnemies = new List<EnemyMovement>();
    internal GameObject currentTarget;
    internal float timeUntilShot;

    Spot mySpot;

    internal bool active;

    public virtual void PrepareTower(Spot spot)
    {
        mySpot = spot;
        statsMultiplayers.fireRate = 1f;
        statsMultiplayers.range = 1f;
        statsMultiplayers.damage = new List<float>();
        for (int i = 0; i < 3; i++)
        {
            statsMultiplayers.damage.Add(1f);
        }

        statsMultiplayers.CombineStats(PasiveTowerStatsController.GetStats(damageTypes));

        reachableEnemies = new List<EnemyMovement>();
        SetupRange();
    }

    public virtual void SetupRange(float additionalRange = 0)
    {
        sphere.radius = towerStats[currentLevel].range * (statsMultiplayers.range + additionalRange);
        float rangeSpriteScale = (towerStats[currentLevel].range * (statsMultiplayers.range + additionalRange)) / DEFAULT_RANGE_SPRITE_RADIUS;
        rangeSprite.localScale = new Vector3(rangeSpriteScale, rangeSpriteScale, 1f);
    }

    public void ResetTargets()
    {
        reachableEnemies = new List<EnemyMovement>();
        currentTarget = null;
    }

    public virtual void Update()
    {
        if (active)
        {
            FindTarget();

            timeUntilShot -= Time.deltaTime;
            
            if (currentTarget != null && currentTarget.gameObject.activeInHierarchy)
            {
                top.LookAt(currentTarget.transform);
                top.transform.localEulerAngles = new Vector3(0f, top.transform.rotation.eulerAngles.y, 0f);

                if (timeUntilShot < 0)
                {
                    Shoot();
                    timeUntilShot = GetTimeToNextShot();
                }
            }
        }
    }

    internal virtual float GetTimeToNextShot()
    {
        return towerStats[currentLevel].fireRate * (statsMultiplayers.fireRate);
    }

    internal virtual void Shoot()
    {
        GameObject newBullet = ObjectPools.instance.GetPool(bulletType).GetObject();
        newBullet.transform.parent = transform;
        newBullet.transform.position = bulletSpawnPos.position;
        Bullet bullet = newBullet.GetComponent<Bullet>();
        bullet.damage = GetDamageMultiplied();
        bullet.additionalGoldOnKill = additionalGoldPerKill;
        bullet.SetTarget(currentTarget);
        if (shootingParticles != null)
        {
            shootingParticles.Play();
        }
        audioSource.clip = shootSound;
        audioSource.Play();
        AddExperience();
    }

    internal virtual List<float> GetDamageMultiplied()
    {
        List<float> finalDamage = new List<float>();
        for (int i = 0; i < towerStats[currentLevel].damage.Count; i++)
        {
            finalDamage.Add(towerStats[currentLevel].damage[i] * statsMultiplayers.damage[i]);
        }
        return finalDamage;
    }

    public virtual void AddExperience()
    {
        if(MaxLevel())
        {
            return;
        }

        experience += EXP_PER_SHOT + PasiveTowerStatsController.extraExperience;
        if (experience >= experienceNeeded[currentLevel])
        {
            LevelUp();
        }
        TowerInfoWindow.instance.UpdateInfo(this);
    }

    public virtual void LevelUp()
    {
        experience -= experienceNeeded[currentLevel];
        levelUpRings[currentLevel].SetActive(true);
        currentLevel++;
        levelUp.Play();

        TowerInfoWindow.instance.UpdateInfo(this);

        audioSource.PlayOneShot( SoundsController.instance.GetAudioClip("LevelUp"));
        audioSource.Play();
        SetupRange();
    }

    public void FindTarget()
    {
        currentTarget = null;

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
            case (TargetSelectOptions.LowestArmor):
                currentTarget = GetTargetMinArmor().gameObject;
                return;
            case (TargetSelectOptions.LowestHealth):
                currentTarget = GetTargetMinHealth().gameObject;
                return;
            case (TargetSelectOptions.LowestShield):
                currentTarget = GetTargetMinShield().gameObject;
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

    EnemyMovement GetTargetMinArmor()
    {
        float maxValue = 9999999;
        EnemyMovement target = reachableEnemies[0];
        foreach (EnemyMovement enemy in reachableEnemies)
        {
            if (enemy.enemy.currentHealth[1] < maxValue)
            {
                maxValue = enemy.enemy.currentHealth[1];
                target = enemy;
            }
        }
        return target;
    }

    EnemyMovement GetTargetMinHealth()
    {
        float maxValue = 9999999;
        EnemyMovement target = reachableEnemies[0];
        foreach (EnemyMovement enemy in reachableEnemies)
        {
            if (enemy.enemy.currentHealth[0] < maxValue)
            {
                maxValue = enemy.enemy.currentHealth[0];
                target = enemy;
            }
        }
        return target;
    }

    EnemyMovement GetTargetMinShield()
    {
        float maxValue = 9999999;
        EnemyMovement target = reachableEnemies[0];
        foreach (EnemyMovement enemy in reachableEnemies)
        {
            if (enemy.enemy.currentHealth[2] < maxValue)
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

    public virtual void Activate()
    {
        for (int i = 0; i < currentLevel; i++)
        {
            levelUpRings[i].SetActive(true);
        }
        audioSource.clip = SoundsController.instance.GetAudioClip("TowerPlaced");
        audioSource.Play();
        SetupRange();
        if (canvas != null)
        {
            canvas.SetActive(true);
        }
        active = true;
        rangeSprite.gameObject.SetActive(false);
    }

    public void EnemyDestroyed(Enemy enemy)
    {
        if(currentTarget == enemy)
        {
            currentTarget = null;
        }

        if (reachableEnemies.Contains(enemy.movement))
        {
            reachableEnemies.Remove(enemy.movement);
        }
    }

    public void UpgradeTower()
    {
        int experienceToGive = experienceNeeded[currentLevel] - experience;
        for (int i = 0; i < experienceToGive; i++)
        {
            AddExperience();
        }
    }

    public bool MaxLevel() 
    {
        return currentLevel == experienceNeeded.Count;
    }
}


[Serializable]
public struct TowerStats
{
    public float fireRate;
    public float range;
    public List<float> damage;

    public void CombineStats(TowerStats newStats)
    {
        fireRate -= newStats.fireRate;
        range += newStats.range;

        if (newStats.damage == null)
        {
            return;
        }

        if (damage == null)
        {
            damage = new List<float>();
            damage.Add(0);
            damage.Add(0);
            damage.Add(0);
        }

        damage[0] += newStats.damage.Count > 0 ? newStats.damage[0] : 0;
        damage[1] += newStats.damage.Count > 1 ? newStats.damage[1] : 0;
        damage[2] += newStats.damage.Count > 2 ? newStats.damage[2] : 0;
    }

    public void RemoveStats(TowerStats statsToRemove)
    {
        fireRate += statsToRemove.fireRate;
        range -= statsToRemove.range;

        damage[0] -= statsToRemove.damage.Count > 0 ? statsToRemove.damage[0] : 0;
        damage[1] -= statsToRemove.damage.Count > 1 ? statsToRemove.damage[1] : 0;
        damage[2] -= statsToRemove.damage.Count > 2 ? statsToRemove.damage[2] : 0;
    }
}