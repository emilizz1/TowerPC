using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    public EnemyMovement movement;
    [SerializeField] Image healthBar; 
    [SerializeField] Image armorBar; 
    [SerializeField] Image shieldBar; 
    [SerializeField] Image skillBar; 


    [Header("Stats")]
    [SerializeField] float health;
    [SerializeField] float armor;
    [SerializeField] float shield;
    [SerializeField] float speed = 3;
    [SerializeField] int damage;
    //Add special powers

    float currentHealth;
    float currentArmor;
    float currentShield;
    float currentSkill;

    private void Start()
    {
        movement.enemy = this;

        movement.movementSpeed = speed;
        currentHealth = health;
        currentArmor = armor;
        currentShield = shield;
        currentSkill = 0;

        UpdateBars();
    }

    private void OnDestroy()
    {
        EnemyManager.instance.EnemyRemoved(this);
    }

    public void UpdateBars()
    {
        healthBar.fillAmount = currentHealth / health;
        armorBar.fillAmount = currentArmor / armor;
        shieldBar.fillAmount = currentShield / shield;
        skillBar.fillAmount = currentSkill / 1f;
    }

    public void ReachedEnd()
    {
        PlayerLife.instance.ChangeHealthAmount(-damage);
        DestroyImmediate(gameObject);
    }

    public void DealDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
            return;
        }
        UpdateBars();        
    }
}
