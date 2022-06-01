using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;

    internal GameObject target;
    internal float damage;

    void Update()
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            }
            else
            {
                target.GetComponent<Enemy>().DealDamage(damage);
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
