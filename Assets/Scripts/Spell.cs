using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] Transform rangeSprite;
    [SerializeField] SphereCollider sphere;
    [SerializeField] float range;


    const float DEFAULT_RANGE_SPRITE_RADIUS = 4f;

    float timePassed;
    bool active;

    public virtual void Start()
    {
        float rangeSpriteScale = range / DEFAULT_RANGE_SPRITE_RADIUS;
        rangeSprite.localScale = new Vector3(rangeSpriteScale, rangeSpriteScale, 1f);
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

    public void Activate()
    {
        active = true;
        rangeSprite.gameObject.SetActive(false);
    }

    public void StopSpell()
    {
        //Later pool these objects
        Destroy(gameObject);
    }
}
