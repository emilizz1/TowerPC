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
    public int moneyOnKill;
    [SerializeField] List<float> health;
    [SerializeField] float speed = 3;
    [SerializeField] int damage;
    [SerializeField] EnemySpecial special;
    [SerializeField] ObjectPools.PoolNames poolName;

    internal List<float> currentHealth;
    internal bool returning;
    float currentSkill;
    List<Color> rendererColors;

    void Start()
    {
        if (rendererColors == null)
        {
            SetupRendererColors();
        } 
    }

    private void SetupRendererColors()
    {
        rendererColors = new List<Color>();
        foreach (Material material in myRenderer.materials)
        {
            rendererColors.Add(material.color);
        }
    }

    void Update()
    {
        if (returning)
        {
            return;
        }

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
        returning = false;
        movement.enemy = this;
        if(rendererColors == null)
        {
            SetupRendererColors();
        }
        for (int i = 0; i < rendererColors.Count; i++)
        {
            myRenderer.materials[i].color = rendererColors[i];
        }

        movement.movementSpeed = speed;
        currentHealth = new List<float>();
        currentHealth.Add(health[0]);
        currentHealth.Add(health[1]);
        currentHealth.Add(health[2]);
        currentSkill = 0;
        if (special != null)
        {
            special.SpecialFinished();
        }

        UpdateBars();

        movement.ResetEnemy();
        debuffIcons.ResetIcons();
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
        StartCoroutine(ReturnAfterTimer());
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
            healthBar.fillAmount = 0f;
            Money.instance.AddCurrency(moneyOnKill, true);
            StartCoroutine(ReturnAfterTimer());
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
        for (int i = 0; i < rendererColors.Count; i++)
        {
            myRenderer.materials[i].color = Color.red;
        }
        yield return new WaitForSeconds(0.25f);
        for (int i = 0; i < rendererColors.Count; i++)
        {
            myRenderer.materials[i].color = rendererColors[i];
        }
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < rendererColors.Count; i++)
        {
            myRenderer.materials[i].color = Color.red;
        }
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < rendererColors.Count; i++)
        {
            myRenderer.materials[i].color = rendererColors[i];
        }
    }

    IEnumerator ReturnAfterTimer()
    {
        returning = true;
        yield return new WaitForSeconds(0.33f);
        ObjectPools.instance.GetPool(poolName).ReturnObject(gameObject);
    }
}
