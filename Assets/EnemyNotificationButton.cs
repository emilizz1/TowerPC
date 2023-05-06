using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyNotificationButton : MonoBehaviour
{
    [SerializeField] Image enemyImage;

    EnemyInfo myInfo;

    public void DislayNotification(EnemyInfo info)
    {
        myInfo = info;

        enemyImage.sprite = info.image;
    }

    public void ButtonPressed()
    {
        SoundsController.instance.PlayOneShot("Click");
        EnemyAppearInfo.instance.DisplayEnemyFullInfo(myInfo);
        gameObject.SetActive(false);
    }
}
