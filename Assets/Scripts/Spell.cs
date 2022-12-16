using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public float range;
    public int snappingSize = 1;
    public int duration;

    [SerializeField] ParticleSystem startParticles;
    [SerializeField] Transform rangeSprite;
    [SerializeField] SphereCollider sphere;
    [SerializeField] AudioSource audioSource;

    const float DEFAULT_RANGE_SPRITE_RADIUS = 4f;

    internal float timePassed;

    bool active;
    List<GameObject> enemiesInside = new List<GameObject>();

    public virtual void Start()
    {
        if (rangeSprite != null)
        {
            float rangeSpriteScale = range / DEFAULT_RANGE_SPRITE_RADIUS;
            rangeSprite.localScale = new Vector3(rangeSpriteScale, rangeSpriteScale, 1f);
        }
        if (sphere != null)
        {
            sphere.radius = range;
        }
        active = false;
        enemiesInside = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            if (!active)
            {
                enemiesInside.Add(other.gameObject);
                return;
            }
            DoEffect(other.gameObject);
        }
    }

    public virtual void DoEffect(GameObject enemy)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            if (!active)
            {
                enemiesInside.Remove(other.gameObject);
                return;
            }
            StopEffect(other.gameObject);
        }
    }

    public virtual void StopEffect(GameObject enemy)
    {

    }

    public virtual void Activate()
    {
        active = true;
        audioSource.Play();
        if (rangeSprite != null)
        {
            rangeSprite.gameObject.SetActive(false);
        }
        if(startParticles != null)
        {
            startParticles.Play();
        }
        if(enemiesInside.Count > 0)
        {
            foreach(GameObject enemy in enemiesInside)
            {
                DoEffect(enemy);
            }
            enemiesInside = new List<GameObject>();
        }
    }

    public virtual void StopSpell()
    {
        duration--;
        if (duration > 0)
        {
            return;
        }
        //Later pool these objects
        Destroy(gameObject);
    }
}
