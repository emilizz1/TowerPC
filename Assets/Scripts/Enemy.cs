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
    [SerializeField] MeshRenderer myRenderer;
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> deathClips;
    [SerializeField] List<AudioClip> specialClips;
    [SerializeField] DamageNumbers damageNumbers;

    [Header("Stats")]
    public int moneyOnKill;
    [SerializeField] List<float> health;
    [SerializeField] float speed = 3;
    [SerializeField] int damage;
    [SerializeField] EnemySpecial special;
    [SerializeField] ObjectPools.PoolNames poolName;

    internal List<float> currentHealth;
    internal List<float> tempMaxHealth;
    internal bool returning;
    internal bool summmoned;
    float currentSkill;
    List<Color> rendererColors;
    float startingVolumeValue;

    void Start()
    {
        startingVolumeValue = audioSource.volume;
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
                    audioSource.volume = startingVolumeValue * SettingsHolder.sound;
                    audioSource.PlayOneShot(specialClips[Random.Range(0, specialClips.Count)]);
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
        healthBar.fillAmount = currentHealth[0] / (health[0] + tempMaxHealth[0]);
        healthBarBg.fillAmount = healthBar.fillAmount > 0 ? 0.04f *  (1f - healthBar.fillAmount) + healthBar.fillAmount : 0f;
      
        armorBar.fillAmount = currentHealth[1] / (health[1] + tempMaxHealth[1]);
        armorBarBg.fillAmount = armorBar.fillAmount > 0 ? 0.04f * (1 - armorBar.fillAmount) + armorBar.fillAmount : 0f;

        shieldBar.fillAmount = currentHealth[2] / (health[2] + tempMaxHealth[2]);
        shieldBarBg.fillAmount = shieldBar.fillAmount > 0 ? 0.04f * (1 - shieldBar.fillAmount) + shieldBar.fillAmount : 0f;

        skillBar.fillAmount = currentSkill / 1f;
        skillBarBg.fillAmount = skillBar.fillAmount > 0 ? 0.04f * (1 - skillBar.fillAmount) + skillBar.fillAmount : 0f;
    }

    public void ReachedEnd()
    {
        PlayerLife.instance.ChangeHealthAmount(-damage);
        StartCoroutine(ReturnAfterTimer());
        EnemyManager.instance.EnemyRemoved(this);
    }

    public void DealDamage(List<float> damages, Color damageColor)
    {
        if (!gameObject.activeInHierarchy || healthBar.fillAmount == 0f)
        {
            return;
        }

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
            audioSource.volume = startingVolumeValue * SettingsHolder.sound;
            audioSource.PlayOneShot(deathClips[Random.Range(0, deathClips.Count)]);
            UpdateBars();
            healthBar.fillAmount = 0f;
            if (!summmoned)
            {
                Money.instance.AddCurrency(moneyOnKill, true);
                EnemyManager.instance.enemiesKilled++;
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
