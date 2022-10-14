using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject endParticles;
    [SerializeField] float speed;

    internal GameObject target;
    internal List<float> damage;

    void Update()
    {
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
                if(endParticles != null)
                {
                    Instantiate(endParticles, target.transform);
                }
                ObjectPools.instance.GetPool(ObjectPools.PoolNames.basicBullet).ReturnObject(gameObject);
            }
        }
        else
        {
            ObjectPools.instance.GetPool(ObjectPools.PoolNames.basicBullet).ReturnObject(gameObject);
        }
        
    }
}
