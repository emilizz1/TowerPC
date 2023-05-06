using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] ParticleSystem placingParticles;
    [SerializeField] ParticleSystem sprungParticles;
    [SerializeField] AudioSource audioSource;
    [SerializeField] BoxCollider boxCollider;

    Trapper myTrapper;
    internal RoadSpot mySpot;
    
    public void Activate(Trapper trapper)
    {
        placingParticles.Play();
        audioSource.clip = SoundsController.instance.GetAudioClip("TrapPlaced");
        audioSource.Play();
        myTrapper = trapper;
        boxCollider.enabled = true;
        mySpot.taken = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Enemy>() != null)
        {
            TrapSprung(other.GetComponent<Enemy>());
        }
    }

    void TrapSprung(Enemy enemy)
    {
        if (enemy.GetComponent<StunDebuff>())
        {
            enemy.GetComponent<StunDebuff>().AddStack();
        }
        else
        {
            enemy.gameObject.AddComponent<StunDebuff>();
        }


        if (enemy.GetComponent<PoisonDebuff>())
        {
            enemy.GetComponent<PoisonDebuff>().AddStack();
        }
        else
        {
            enemy.gameObject.AddComponent<PoisonDebuff>();
        }

        audioSource.clip = SoundsController.instance.GetAudioClip("TrapSprung");
        audioSource.Play();
        sprungParticles.Play();
        myTrapper.TrapSprung(this);
        boxCollider.enabled = false;
        StartCoroutine(ReturnAfterTime());
        mySpot.taken = false;
    }

    IEnumerator ReturnAfterTime()
    {
        yield return new WaitForSeconds(2f);
        ObjectPools.instance.GetPool(ObjectPools.PoolNames.trap).ReturnObject(gameObject);
    }
}
