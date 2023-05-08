using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInfoWindow : MonoBehaviour
{
    [SerializeField] TweenAnimator animator;

    bool open;

    void Start()
    {
        if (SavedData.savesData.openedCardIntro == 1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            SavedData.savesData.openedCardIntro = 1;
            SavedData.Save();
            Cover.cover = true;
            Time.timeScale = 0f;
            open = true;
        }
    }

    private void Update()
    {
        if (open)
        {
            if (Input.GetMouseButtonDown(0))
            {
                open = false;
                animator.PerformTween(0);
                Cover.cover = false;
                if(Time.timeScale == 0f)
                {
                    Time.timeScale = SpeedButton.instance.currentSpeed;
                }
            }
        }
    }

    public void Open()
    {
        if (!open)
        {
            open = true;
            Cover.cover = true;
            gameObject.SetActive(true);
            animator.PerformTween(1);

            if (Time.timeScale == 0f)
            {
                Time.timeScale = SpeedButton.instance.currentSpeed;
            }
        }
    }
}
