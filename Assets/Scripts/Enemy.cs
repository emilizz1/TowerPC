using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    public EnemyMovement movement;
    public EnemyDebuffIconController debuffIcons;
    [SerializeField] Image healthBar; 
    [SerializeField] Image armorBar; 
    [SerializeField] Image shieldBar; 
    [SerializeField] Image skillBar;
    [SerializeField] MeshRenderer myRenderer;

    [Header("Stats")]
    [SerializeField] List<float> health;
    [SerializeField] float speed = 3;
    [SerializeField] int damage;
    [SerializeField] EnemySpecial special;
    [SerializeField] int moneyOnKill;

    internal List<float> currentHealth;
    float currentSkill;
    Color rendererColor;

    void Start()
    {
        rendererColor = myRenderer.materials[1].color;
    }

    void Update()
    {
        if (special != null)
        {
            if (!special.currentlyInUse)
            {
                currentSkill += Time.deltaTime / special.cooldown;
                if (currentSkill >= 1f)
                {
                    special.UseSpecial();
                    currentSkill = 0f;
                }
                UpdateBars();
            }
        }
    }

    public void ResetEnemy()
    {
        movement.enemy = this;
        if(rendererColor == null)
        {
            rendererColor = myRenderer.materials[1].color;
        }
        myRenderer.materials[1].color = rendererColor;

        movement.movementSpeed = speed;
        currentHealth = new List<float>();
        currentHealth.Add(health[0]);
        currentHealth.Add(health[1]);
        currentHealth.Add(health[2]);
        currentSkill = 0;
        special.SpecialFinished();

        UpdateBars();

        movement.ResetEnemy();
    }

    public void UpdateBars()
    {
        healthBar.fillAmount = currentHealth[0] / health[0];
        armorBar.fillAmount = currentHealth[1] / health[1];
        shieldBar.fillAmount = currentHealth[2] / health[2];
        skillBar.fillAmount = currentSkill / 1f;
    }

    public void ReachedEnd()
    {
        PlayerLife.instance.ChangeHealthAmount(-damage);
        ObjectPools.instance.GetPool(ObjectPools.PoolNames.basicEnemy).ReturnObject(gameObject);
        EnemyManager.instance.EnemyRemoved(this);
    }

    public void DealDamage(List<float> damages)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        float damagePercentageDone = 1f;

        for (int i = 2; i >= 0; i--)
        {
            if (damagePercentageDone > 0)
            {
                //Debug.Log("Starting - per:" + damagePercentageDone + " hp: " + currentHealth[i] + " dmg: " + damages[i]);

                float currentDamage = damages[i] * damagePercentageDone;
                float damageDone = Mathf.Min(currentHealth[i], currentDamage);

                currentHealth[i] -= damageDone;
                damagePercentageDone *= 1f - damageDone / currentDamage;
                //Debug.Log("final - per:" + damagePercentageDone + " hp: " + currentHealth[i] + " dmg: " + damages[i]);
            }
        }

        if(currentHealth[0] <= 0)
        {
            Money.instance.AddCurrency(moneyOnKill, false);
            ObjectPools.instance.GetPool(ObjectPools.PoolNames.basicEnemy).ReturnObject(gameObject);
            EnemyManager.instance.EnemyRemoved(this);
            return;
        }
        else
        {
            StartCoroutine(HitAnimation());
        }
        UpdateBars();        
    }

    IEnumerator HitAnimation()
    {
        myRenderer.materials[1].color = Color.red;
        yield return new WaitForSeconds(0.25f);
        myRenderer.materials[1].color = rendererColor;
        yield return new WaitForSeconds(0.1f);
        myRenderer.materials[1].color = Color.red;
        yield return new WaitForSeconds(0.1f);
        myRenderer.materials[1].color = rendererColor;
    }
}
