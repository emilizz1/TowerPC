using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedButton : MonoBehaviour
{
    [SerializeField] GameObject normalSpeedObj;
    [SerializeField] GameObject fasterSpeedObj;
    [SerializeField] GameObject fastestSpeedObj;

    [SerializeField] float normalSpeed;
    [SerializeField] float fasterSpeed;
    [SerializeField] float fastestSpeed;

    int currentSpeed = 0;

    public void SpeedUp()
    {
        switch (currentSpeed)
        {
            case (0):
                Time.timeScale = fasterSpeed;
                currentSpeed = 1;
                normalSpeedObj.SetActive(false);
                fasterSpeedObj.SetActive(true);
                fastestSpeedObj.SetActive(false);
                break;
            case (1):
                Time.timeScale = fastestSpeed;
                currentSpeed = 2;
                normalSpeedObj.SetActive(false);
                fasterSpeedObj.SetActive(false);
                fastestSpeedObj.SetActive(true);
                break;
            case (2):
                Time.timeScale = normalSpeed;
                currentSpeed = 0;
                normalSpeedObj.SetActive(true);
                fasterSpeedObj.SetActive(false);
                fastestSpeedObj.SetActive(false);
                break;
        }
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
