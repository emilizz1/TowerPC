using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Color damageColor;
    [SerializeField] TrailRenderer trail;
    [SerializeField] GameObject model;
    [SerializeField] GameObject endParticles;
    [SerializeField] float speed;
    [SerializeField] AudioSource audioSource;

    internal GameObject target;
    internal List<float> damage;
    internal int additionalGoldOnKill;

    GameObject endParticleInstance;
    bool returning;

    private void Start()
    {
    }

    void Update()
    {
        if (returning)
        {
            return;
        }

        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
                transform.LookAt(target.transform.position, Vector3.up);
            }
            else
            {
                TargetReached();
            }
        }
        else
        {
            StartCoroutine(ReturnAfterTimer());
        }

    }

    internal virtual void TargetReached()
    {
        target.GetComponent<Enemy>().DealDamage(damage, damageColor, additionalGoldOnKill);
        if (endParticles != null)
        {
            endParticleInstance = Instantiate(endParticles, target.transform);
        }
        if(audioSource != null)
        {
            audioSource.Play();
        }
        StartCoroutine(ReturnAfterTimer());
    }

    public void SetTarget(GameObject setTarget)
    {
        returning = false;
        target = setTarget;
        trail.emitting = true;
    }

    IEnumerator ReturnAfterTimer()
    {
        trail.emitting = false;
        returning = true;
        yield return new WaitForSeconds(1f);
        if(endParticleInstance!= null)
        {
            Destroy(endParticleInstance);
        }
        additionalGoldOnKill = 0;
        ObjectPools.instance.GetPool(ObjectPools.PoolNames.basicBullet).ReturnObject(gameObject);
    }
}
