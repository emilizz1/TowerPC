using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebuffController : MonoSingleton<DebuffController>
{
    public List<DebuffInfo> allInfo;

    [Serializable]
    public struct DebuffInfo
    {
        public string debuffName;
        public Sprite icon;
        public float debuffTimer;
        public int maxDebuffs;
        public float effectAmount;

        public void IncreaseMaxDebuffs()
        {
            maxDebuffs++;
        }
    }

    public DebuffInfo GetInfo(string debuffName)
    {
        foreach(DebuffInfo info in allInfo)
        {
            if(info.debuffName == debuffName)
            {
                return info;
            }
        }
        return new DebuffInfo();
    }

    public void AddAdditionalTime()
    {
        foreach (DebuffInfo debuff in allInfo)
        {
            debuff.IncreaseMaxDebuffs();
        }
    }
}
