using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField] ParticleSystem particles;

    float timePassed;
    bool active;

    private void Update()
    {
        if (active)
        {
            timePassed += Time.deltaTime;
            if(timePassed >= time)
            {
                Destroy(gameObject);
            }
        }
        
    }

    public void Activate()
    {
        active = true;
        particles.Play();
    }
}
