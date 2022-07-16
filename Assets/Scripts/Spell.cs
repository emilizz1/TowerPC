using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;
    [SerializeField] Transform rangeSprite;
    [SerializeField] SphereCollider sphere;
    [SerializeField] float range;


    const float DEFAULT_RANGE_SPRITE_RADIUS = 4f;

    float timePassed;
    bool active;

    private void Start()
    {
        float rangeSpriteScale = range / DEFAULT_RANGE_SPRITE_RADIUS;
        rangeSprite.localScale = new Vector3(rangeSpriteScale, rangeSpriteScale, 1f);
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
