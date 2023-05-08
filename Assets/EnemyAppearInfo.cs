using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using I2.Loc;
using TMPro;

public class EnemyAppearInfo : MonoSingleton<EnemyAppearInfo>
{
    public List<EnemyInfo> enemyInfos;

    [SerializeField] List<EnemyNotificationButton> notificationButtons;

    [SerializeField] TweenAnimator animator;
    [SerializeField] Image infoDisplay;
    [SerializeField] TextMeshProUGUI infoText; 
    [SerializeField] TextMeshProUGUI healthText; 
    [SerializeField] TextMeshProUGUI armorText; 
    [SerializeField] TextMeshProUGUI shieldText; 
    [SerializeField] TextMeshProUGUI moneyText; 
    [SerializeField] TextMeshProUGUI damageText; 
    [SerializeField] TextMeshProUGUI speedText; 

    internal int used;

    public void EnemySpawned(ObjectPools.PoolNames enemyType)
    {
        switch (enemyType)
        {
            case (ObjectPools.PoolNames.enemyArmor):
                if(SavedData.savesData.enemyArmor == 0)
                {
                    SavedData.savesData.enemyArmor = 1;
                    SavedData.Save();
                    DisplayEnemyNotification(enemyType);
                }
                break;
            case (ObjectPools.PoolNames.enemyBandit):
                if (SavedData.savesData.enemyBandit == 0)
                {
                    SavedData.savesData.enemyBandit = 1;
                    SavedData.Save();
                    DisplayEnemyNotification(enemyType);
                }
                break;
            case (ObjectPools.PoolNames.enemyBoss):
                if (SavedData.savesData.enemyBoss == 0)
                {
                    SavedData.savesData.enemyBoss = 1;
                    SavedData.Save();
                    DisplayEnemyNotification(enemyType);
                }
                break;
            case (ObjectPools.PoolNames.enemyHealth):
                if (SavedData.savesData.enemyHealth == 0)
                {
                    SavedData.savesData.enemyHealth = 1;
                    SavedData.Save();
                    DisplayEnemyNotification(enemyType);
                }
                break;
            case (ObjectPools.PoolNames.enemyHound):
                if (SavedData.savesData.enemyHound == 0)
                {
                    SavedData.savesData.enemyHound = 1;
                    SavedData.Save();
                    DisplayEnemyNotification(enemyType);
                }
                break;
            case (ObjectPools.PoolNames.enemyShield):
                if (SavedData.savesData.enemyShield == 0)
                {
                    SavedData.savesData.enemyShield = 1;
                    SavedData.Save();
                    DisplayEnemyNotification(enemyType);
                }
                break;
        }

    }

    void DisplayEnemyNotification(ObjectPools.PoolNames enemyType)
    {
        foreach (EnemyInfo info in enemyInfos)
        {
            if (info.name == enemyType)
            {
                notificationButtons[used].gameObject.SetActive(true);
                notificationButtons[used].DislayNotification(info);
                used++;
                return;
            }
        }
    }

    public void DisplayEnemyFullInfo(EnemyInfo info)
    {
        Cover.cover = true;
        animator.PerformTween(0);
        SoundsController.instance.PlayOneShot("Open");
        infoDisplay.sprite = info.image;
        infoText.text = info.enemyDescription;
        infoText.gameObject.SetActive(info.enemyDescription.mTerm != "None");
        Enemy enemy = ObjectPools.instance.GetPool(info.name).GetObject().GetComponent<Enemy>();
        healthText.text = enemy.maxHealth[0].ToString();
        armorText.text = enemy.maxHealth[1].ToString();
        shieldText.text = enemy.maxHealth[2].ToString();
        moneyText.text = enemy.moneyOnKill.ToString();
        damageText.text = enemy.damage.ToString();
        speedText.text = enemy.speed.ToString();
        ObjectPools.instance.GetPool(info.name).ReturnObject(enemy.gameObject);
        Bestiary.instance.FillEnemyButtons();
    }

    public void CloseEnemyInfo()
    {
        Cover.cover = false;
        animator.PerformTween(1);
        SoundsController.instance.PlayOneShot("Close");
        SoundsController.instance.PlayOneShot("Click");
    }

    public Sprite GetEnemyImage(ObjectPools.PoolNames poolName)
    {
        foreach (EnemyInfo enemyImage in enemyInfos)
        {
            if (enemyImage.name == poolName)
            {
                return enemyImage.image;
            }
        }
        return null;
    }

    public EnemyInfo GetInfoByType(ObjectPools.PoolNames enemy)
    {
        foreach(EnemyInfo info in enemyInfos)
        {
            if(enemy == info.name)
            {
                return info;
            }
        }
        return new EnemyInfo();
    }
}


[Serializable]
public struct EnemyInfo
{
    public ObjectPools.PoolNames name;
    public Sprite image;
    public LocalizedString enemyDescription;

}