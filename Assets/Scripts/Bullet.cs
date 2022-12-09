using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;
    [SerializeField] GameObject model;
    [SerializeField] GameObject endParticles;
    [SerializeField] float speed;

    internal GameObject target;
    internal List<float> damage;

    GameObject endParticleInstance;
    bool returning;

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
                target.GetComponent<Enemy>().DealDamage(damage);
                if (endParticles != null)
                {
                    endParticleInstance = Instantiate(endParticles, target.transform);
                }
                StartCoroutine(ReturnAfterTimer());
            }
        }
        else
        {
            StartCoroutine(ReturnAfterTimer());
        }

    }

    public void SetTarget(GameObject setTarget)
    {
        returning = false;
        target = setTarget;
        particles.Play();
    }

    IEnumerator ReturnAfterTimer()
    {
        particles.Stop();
        returning = true;
        yield return new WaitForSeconds(1f);
        if(endParticleInstance!= null)
        {
            Destroy(endParticleInstance);
        }
        ObjectPools.instance.GetPool(ObjectPools.PoolNames.basicBullet).ReturnObject(gameObject);
    }
}
