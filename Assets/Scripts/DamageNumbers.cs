using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageNumbers : MonoBehaviour
{
    [SerializeField] List<Text> damageNumbersText;
    [SerializeField] List<TweenAnimator> damageNumbersTween;

    int currentNumber;

    public void ShowDamage(float damage, Color color)
    {
        if(Mathf.RoundToInt(damage) == 0)
        {
            return;
        }

        damageNumbersText[currentNumber].text = Mathf.RoundToInt(damage).ToString();
        damageNumbersText[currentNumber].color = color;

        damageNumbersText[currentNumber].transform.localPosition = new Vector3(Random.Range(-75f, 75f), Random.Range(-100f, -135f));
        Vector3 newTarget = new Vector3(
            damageNumbersText[currentNumber].transform.localPosition.x,
            damageNumbersText[currentNumber].transform.localPosition.y + 100f);
        damageNumbersTween[currentNumber].data[0].position = newTarget;
        damageNumbersTween[currentNumber].PerformTween(0);

        currentNumber++;
        if (currentNumber == damageNumbersText.Count)
        {
            currentNumber = 0;
        }
    }
}
