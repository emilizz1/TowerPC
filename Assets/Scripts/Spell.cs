using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public float range;

    [SerializeField] ParticleSystem startParticles;
    [SerializeField] Transform rangeSprite;
    [SerializeField] SphereCollider sphere;


    const float DEFAULT_RANGE_SPRITE_RADIUS = 4f;

    float timePassed;
    bool active;

    public virtual void Start()
    {
        float rangeSpriteScale = range / DEFAULT_RANGE_SPRITE_RADIUS;
        rangeSprite.localScale = new Vector3(rangeSpriteScale, rangeSpriteScale, 1f);
        sphere.radius = range;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            DoEffect(other.gameObject);
        }
    }

    public virtual void DoEffect(GameObject enemy)
    {
        Debug.Log("Entered");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            StopEffect(other.gameObject);
        }
    }

    public virtual void StopEffect(GameObject enemy)
    {

        Debug.Log("exited");
    }

    public virtual void Activate()
    {
        active = true;
        rangeSprite.gameObject.SetActive(false);
        if(startParticles != null)
        {
            startParticles.Play();
        }
    }

    public virtual void StopSpell()
    {
        //Later pool these objects
        Destroy(gameObject);
    }
}
