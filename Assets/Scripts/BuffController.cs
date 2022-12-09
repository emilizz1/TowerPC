using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuffController : MonoSingleton<BuffController>
{
    public List<BuffInfo> allInfo;

    [Serializable]
    public struct BuffInfo
    {
        public string debuffName;
        public Sprite icon;
        public GameObject particles;
        public float effectAmount;
    }

    public BuffInfo GetInfo(string debuffName)
    {
        foreach (BuffInfo info in allInfo)
        {
            if (info.debuffName == debuffName)
            {
                return info;
            }
        }
        return new BuffInfo();
    }
}
