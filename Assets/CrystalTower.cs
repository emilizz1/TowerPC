using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalTower : Tower
{
    [SerializeField] List<GameObject> activeBullets;
    [SerializeField] float shootingSpeed = 0.25f;
    [SerializeField] TweenAnimator spinningTween;
    [SerializeField] int activeBulletsCount;
    [SerializeField] int activeBulletsCountUpgraded;

    float timePassed;
    int preparedBullets;

    public override void Update()
    {
        if (active)
        {
            if(TurnController.currentPhase == TurnController.TurnPhase.EnemyWave)
            {
                timePassed += Time.deltaTime;
                if(timePassed >= GetTimeToNextShot())
                {
                    PrepareBullet();
                    timePassed = 0f;
                }
            }

            FindTarget();

            timeUntilShot -= Time.deltaTime;

            if (currentTarget != null && currentTarget.gameObject.activeInHierarchy)
            {

                if (timeUntilShot < 0 && preparedBullets > 0)
                {
                    Shoot();
                    timeUntilShot = shootingSpeed;
                }
            }
        }
    }

    void PrepareBullet()
    {
        if(preparedBullets >= (SecondTowerAbilityManager.instance.SecondSpecialUnlocked(towerType) == 1 ? activeBulletsCountUpgraded : activeBullets.Count))
        {
            return;
        }

        preparedBullets++;

        for (int i = 0; i < (SecondTowerAbilityManager.instance.SecondSpecialUnlocked(towerType) == 1 ? activeBulletsCountUpgraded : activeBullets.Count); i++)
        {
            if (!activeBullets[i].activeSelf)
            {
                activeBullets[i].SetActive(true);
                //animation
                return;
            }
        }
    }

    internal override void Shoot()
    {
        for (int i = 0; i < (SecondTowerAbilityManager.instance.SecondSpecialUnlocked(towerType) == 1 ? activeBulletsCountUpgraded : activeBullets.Count); i++)
        {
            if (activeBullets[i].activeSelf)
            {
                bulletSpawnPos = activeBullets[i].transform;
                preparedBullets--;
                activeBullets[i].SetActive(false);
                base.Shoot();
                return;
            }
        }

    }

    public override void Activate()
    {
        spinningTween.PerformTween(0);
        base.Activate();
    }
}
