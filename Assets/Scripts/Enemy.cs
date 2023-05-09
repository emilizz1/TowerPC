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
    [SerializeField] Image healthBarBg;
    [SerializeField] Image armorBar; 
    [SerializeField] Image armorBarBg; 
    [SerializeField] Image shieldBar; 
    [SerializeField] Image shieldBarBg; 
    [SerializeField] Image skillBar;
    [SerializeField] Image skillBarBg;
    [SerializeField] Image healthBarCollums;
    [SerializeField] Image armorBarCollums;
    [SerializeField] Image shieldBarCollums;
    [SerializeField] MeshRenderer myRenderer;
    public AudioSource audioSource;
    [SerializeField] List<AudioClip> deathClips;
    public List<AudioClip> specialClips;
    [SerializeField] DamageNumbers damageNumbers;

    [Header("Stats")]
    public int moneyOnKill;
    public List<float> maxHealth;
    public float speed = 3;
    public int damage;
    [SerializeField] EnemySpecial special;
    [SerializeField] ObjectPools.PoolNames poolName;

    internal List<float> currentHealth;
    internal List<float> tempMaxHealth;
    internal bool returning;
    internal bool summmoned;
    internal bool invincable;
    float currentSkill;
    List<Color> rendererColors;
    bool firstHitTaken;
    float passiveDamageTimer;
    List<float> passiveDamage = new List<float>();
    internal int currentMoneyOnKill;

    void Start()
    {
        if (rendererColors == null)
        {
            SetupRendererColors();
        }

        passiveDamage.Add(1f);
        passiveDamage.Add(1f);
        passiveDamage.Add(1f);
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
                    audioSource.PlayOneShot(specialClips[Random.Range(0, specialClips.Count)]);
                    special.UseSpecial();
                    currentSkill = 0f;
                }
                UpdateBars();
            }
        }

        if (GlobalConditionHolder.enemyDamage)
        {
            passiveDamageTimer += Time.deltaTime;
            if(passiveDamageTimer >= 1f)
            {
                passiveDamageTimer = 0f;
                DealDamage(passiveDamage, Color.grey);
            }
        }
    }

    public void ResetEnemy()
    {
        returning = false;
        summmoned = false;
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

        tempMaxHealth = new List<float>();
        tempMaxHealth.Add(0);
        tempMaxHealth.Add(0);
        tempMaxHealth.Add(0);

        currentHealth = new List<float>();
        currentHealth.Add(maxHealth[0]);
        currentHealth.Add(maxHealth[1]);
        currentHealth.Add(maxHealth[2]);
        currentSkill = 0;
        if (special != null)
        {
            special.SpecialFinished();
        }

        UpdateBars();

        debuffIcons.ResetIcons();
        movement.ResetEnemy();

        foreach(Debuff debuff in GetComponents<Debuff>())
        {
            Destroy(debuff);
        }

        currentMoneyOnKill = 0;
        invincable = false;

        firstHitTaken = false;
    }

    public void UpdateBars()
    {
        healthBar.fillAmount = currentHealth[0] / (maxHealth[0] + tempMaxHealth[0]);
        healthBarBg.fillAmount = healthBar.fillAmount > 0 ? 0.04f *  (1f - healthBar.fillAmount) + healthBar.fillAmount : 0f;
        healthBarCollums.pixelsPerUnitMultiplier = (float)(maxHealth[0] + tempMaxHealth[0]) / 40f;

        armorBar.fillAmount = currentHealth[1] / (maxHealth[1] + tempMaxHealth[1]);
        armorBarBg.fillAmount = armorBar.fillAmount > 0 ? 0.04f * (1 - armorBar.fillAmount) + armorBar.fillAmount : 0f;
        armorBarCollums.pixelsPerUnitMultiplier = (float)(maxHealth[1] + tempMaxHealth[1]) / 40f;

        shieldBar.fillAmount = currentHealth[2] / (maxHealth[2] + tempMaxHealth[2]);
        shieldBarBg.fillAmount = shieldBar.fillAmount > 0 ? 0.04f * (1 - shieldBar.fillAmount) + shieldBar.fillAmount : 0f;
        shieldBarCollums.pixelsPerUnitMultiplier = (float)(maxHealth[2] + tempMaxHealth[2]) / 40f;

        skillBar.fillAmount = currentSkill / 1f;
        skillBarBg.fillAmount = skillBar.fillAmount > 0 ? 0.04f * (1 - skillBar.fillAmount) + skillBar.fillAmount : 0f;
    }

    public virtual void ReachedEnd()
    {
        if (!returning)
        {
            PlayerLife.instance.ChangeHealthAmount(-damage);
            StartCoroutine(ReturnAfterTimer());
            EnemyManager.instance.EnemyRemoved(this);
            AchievementManager.EnemiesFinished();
        }
    }

    public virtual void DealDamage(List<float> damages, Color damageColor, int additionalGoldOnKill = 0)
    {
        if (!gameObject.activeInHierarchy || currentHealth[0] <= 0f || invincable)
        {
            return;
        }

        if (GlobalConditionHolder.noGold)
        {
            currentMoneyOnKill -= moneyOnKill;
        }

        if(GlobalConditionHolder.firstHitDoubleDamage && !firstHitTaken)
        {
            for (int i = 0; i < 3; i++)
            {
                damages[i] += damages[i];
            }
        }
        firstHitTaken = true;

        float damagePercentageDone = 1f;

        float damageToShow = 0f;

        for (int i = 2; i >= 0; i--)
        {
            if (damagePercentageDone > 0)
            {
                //Debug.Log("Starting - per:" + damagePercentageDone + " hp: " + currentHealth[i] + " dmg: " + damages[i]);

                float currentDamage = damages[i] * damagePercentageDone;
                float damageDone = Mathf.Min(currentHealth[i], currentDamage);

                damageToShow += damageDone;
                currentHealth[i] -= damageDone;
                damagePercentageDone *= 1f - damageDone / currentDamage;
                //Debug.Log("final - per:" + damagePercentageDone + " hp: " + currentHealth[i] + " dmg: " + damages[i]);
            }
        }

        damageNumbers.ShowDamage(damageToShow, damageColor);

        if (currentHealth[0] <= 0)
        {
            audioSource.PlayOneShot(deathClips[Random.Range(0, deathClips.Count)]);
            UpdateBars();
            healthBar.fillAmount = 0f;
            if (!summmoned)
            {
                Money.instance.AddCurrency(currentMoneyOnKill + additionalGoldOnKill, true);
                EnemyManager.instance.enemiesKilled++;
                AchievementManager.KilledEnemies();
            }
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
