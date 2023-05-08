using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Bestiary : MonoSingleton<Bestiary>
{
    [SerializeField] List<TweenAnimator> enemyButtonsAnimator;
    [SerializeField] List<Image> enemyButtonImages;
    [SerializeField] TweenAnimator animator;
    [SerializeField] GameObject bestiaryButton;

    [SerializeField] Image infoDisplay;
    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI armorText;
    [SerializeField] TextMeshProUGUI shieldText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI speedText;

    List<EnemyInfo> infoByIndex;

    private void Start()
    {
        FillEnemyButtons();
        if (infoByIndex.Count != 0)
        {
            ButtonPressed(0);
        }
    }

    public void FillEnemyButtons()
    {
        int used = 0;
        infoByIndex = new List<EnemyInfo>();

        if (SavedData.savesData.enemyHealth == 1)
        {
            enemyButtonsAnimator[used].transform.parent.gameObject.SetActive(true);
            infoByIndex.Add( EnemyAppearInfo.instance.GetInfoByType(ObjectPools.PoolNames.enemyHealth));
            enemyButtonImages[used].sprite = infoByIndex[used].image;
            used++;
        }
        if (SavedData.savesData.enemyArmor == 1)
        {
            enemyButtonsAnimator[used].transform.parent.gameObject.SetActive(true);
            infoByIndex.Add(EnemyAppearInfo.instance.GetInfoByType(ObjectPools.PoolNames.enemyArmor));
            enemyButtonImages[used].sprite = infoByIndex[used].image;
            used++;
        }
        if (SavedData.savesData.enemyBandit == 1)
        {
            enemyButtonsAnimator[used].transform.parent.gameObject.SetActive(true);
            infoByIndex.Add(EnemyAppearInfo.instance.GetInfoByType(ObjectPools.PoolNames.enemyBandit));
            enemyButtonImages[used].sprite = infoByIndex[used].image;
            used++;
        }
        if (SavedData.savesData.enemyBoss == 1)
        {
            enemyButtonsAnimator[used].transform.parent.gameObject.SetActive(true);
            infoByIndex.Add(EnemyAppearInfo.instance.GetInfoByType(ObjectPools.PoolNames.enemyBoss));
            enemyButtonImages[used].sprite = infoByIndex[used].image;
            used++;
        }
        if (SavedData.savesData.enemyHound == 1)
        {
            enemyButtonsAnimator[used].transform.parent.gameObject.SetActive(true);
            infoByIndex.Add(EnemyAppearInfo.instance.GetInfoByType(ObjectPools.PoolNames.enemyHound));
            enemyButtonImages[used].sprite = infoByIndex[used].image;
            used++;
        }
        if (SavedData.savesData.enemyShield == 1)
        {
            enemyButtonsAnimator[used].transform.parent.gameObject.SetActive(true);
            infoByIndex.Add(EnemyAppearInfo.instance.GetInfoByType(ObjectPools.PoolNames.enemyShield));
            enemyButtonImages[used].sprite = infoByIndex[used].image;
            used++;
        }

        bestiaryButton.SetActive(infoByIndex.Count > 0);
    }

    public void OpenBestiary()
    {
        animator.PerformTween(0);
        SoundsController.instance.PlayOneShot("Open");
        Time.timeScale = 0f;
    }

    public void CloseBestiary()
    {
        animator.PerformTween(1);
        SoundsController.instance.PlayOneShot("Close");
        SoundsController.instance.PlayOneShot("Click");
        Time.timeScale = SpeedButton.instance.currentSpeed;
    }

    public void ButtonPressed(int index)
    {
        for (int i = 0; i < infoByIndex.Count; i++)
        {
            if(i != index)
            {
                enemyButtonsAnimator[i].PerformTween(1);
            }
        }
        SoundsController.instance.PlayOneShot("Click");
        enemyButtonsAnimator[index].PerformTween(0);
        infoDisplay.sprite = infoByIndex[index].image;
        infoText.text = infoByIndex[index].enemyDescription;
        infoText.gameObject.SetActive(infoByIndex[index].enemyDescription.mTerm != "None");
        Enemy enemy = ObjectPools.instance.GetPool(infoByIndex[index].name).GetObject().GetComponent<Enemy>();
        healthText.text = enemy.maxHealth[0].ToString();
        armorText.text = enemy.maxHealth[1].ToString();
        shieldText.text = enemy.maxHealth[2].ToString();
        moneyText.text = enemy.moneyOnKill.ToString();
        damageText.text = enemy.damage.ToString();
        speedText.text = enemy.speed.ToString();
        ObjectPools.instance.GetPool(infoByIndex[index].name).ReturnObject(enemy.gameObject);
    }
}
