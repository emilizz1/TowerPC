using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    public static bool cover;

    [SerializeField] GameObject coverImage;

    void Update()
    {
        if(cover && !coverImage.activeSelf)
        {
            coverImage.SetActive(true);
        }
        else if(!cover && coverImage.activeSelf)
        {
            coverImage.SetActive(false);
        }
    }
}
