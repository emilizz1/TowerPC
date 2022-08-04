using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Sprite image;
    public string towerName;

    [Header("Components")]
    [SerializeField] Transform top;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] SphereCollider sphere;
    [SerializeField] Transform rangeSprite;

    [Header("Stats")]
    public float fireRate;
    public float range;
    public float damage;

    const float DEFAULT_RANGE_SPRITE_RADIUS = 4f;

    List<GameObject> reachableEnemies;
    GameObject currentTarget;
    float timeUntilShot;

    bool active;

    private void Start()
    {
        sphere.radius = range;
        reachableEnemies = new List<GameObject>();
        float rangeSpriteScale = range / DEFAULT_RANGE_SPRITE_RADIUS;
        rangeSprite.localScale = new Vector3(rangeSpriteScale, rangeSpriteScale, 1f);
    }

    private void Update()
    {
        if (active)
        {
            FindTarget();
            if (currentTarget != null)
            {
                top.LookAt(currentTarget.transform);
                top.transform.localEulerAngles = new Vector3(0f, top.transform.rotation.eulerAngles.y, 0f);
            }

            timeUntilShot -= Time.deltaTime;
            if (timeUntilShot < 0 && currentTarget != null)
            {
                GameObject newBullet = ObjectPools.instance.GetPool(ObjectPools.PoolNames.basicBullet).GetObject();
                newBullet.transform.parent = transform;
                newBullet.transform.position = top.transform.position;
                Bullet bullet = newBullet.GetComponent<Bullet>();
                bullet.damage = damage;
                bullet.target = currentTarget;
                timeUntilShot = fireRate;
            }
        }
    }

    void FindTarget()
    {
        while(reachableEnemies.Count > 0 && reachableEnemies[0] == null)
        {
            reachableEnemies.RemoveAt(0);
        }
        if (reachableEnemies.Count > 0)
        {
            currentTarget = reachableEnemies[0];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<EnemyMovement>() != null)
        {
            reachableEnemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EnemyMovement>() != null)
        {
            reachableEnemies.Remove(other.gameObject);
        }
    }

    public void Activate()
    {
        active = true;
        rangeSprite.gameObject.SetActive(false);   
    }

    private void OnMouseEnter()
    {
        Debug.Log("Entered");
    }
}
