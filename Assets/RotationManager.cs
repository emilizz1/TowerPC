using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoSingleton<RotationManager>
{
    List<Quaternion> rotations;

    int rotationIndex;

    protected override void Awake()
    {
        base.Awake();
        rotations = new List<Quaternion>();
        rotations.Add(Quaternion.Euler(0f, 0f, 0f));
        rotations.Add(Quaternion.Euler(0f, 90f, 0f));
        rotations.Add(Quaternion.Euler(0f, 180f, 0f));
        rotations.Add(Quaternion.Euler(0f, 270f, 0f));
    }

    public Quaternion GetRotation()
    {
        switch (rotationIndex)
        {
            case (0):
                return rotations[0];
            case (1):
                return rotations[1];
            case (2):
                return rotations[2];
            case (3):
                return rotations[3];
        }
        return Quaternion.identity;
    }

    public void Rotated()
    {
        rotationIndex++;
        if(rotationIndex >= 4)
        {
            rotationIndex = 0;
        }
    }
}
