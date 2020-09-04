using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int maxHp = 100;
    public int currentHp = 100;

    public HealthBar healthbar = null;

    [SerializeField] float secondsShowHealth = 3f;

    [SerializeField] GameObject onDeathDrop = null;

    [SerializeField] GameObject armor = null;

    private bool hasArmor;

    private bool isDead = false;
    private float timer;

    private void Start()
    {
        currentHp = maxHp;
        healthbar.setMaxHealth(maxHp);
        healthbar.setHealth(maxHp);

        if (armor != null)
            hasArmor = true;
    }

    private void Update()
    {
        if (timer >= 0)
            timer -= Time.deltaTime;

        if (healthbar.gameObject.activeSelf && timer < 0)
            healthbar.gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHp -= damage;

        if (currentHp < 1 && hasArmor)
        {
            hasArmor = false;
            armor.SetActive(false);
            currentHp = maxHp;
            if (AudioManager.instance != null)
                AudioManager.instance.Play("break armor");
        }
        else if (currentHp < 1)
            Die();

        healthbar.setHealth(currentHp);

        if (damage > 0)
            Instantiate(onDeathDrop, transform.position, Random.rotationUniform);

        AudioManager.instance.Play("oof");

        timer = secondsShowHealth;
    }

    public void Heal(int healing)
    {
        if (currentHp == maxHp || isDead)
            return;

        currentHp += healing;

        if (currentHp > maxHp)
            currentHp = maxHp;

        healthbar.setHealth(currentHp);

        timer = secondsShowHealth;
    }

    private void Die()
    {
        isDead = true;
        AudioManager.instance.Stop("theme");
        AudioManager.instance.Play("squish");
        AudioManager.instance.Play("boo");
        GameObject.Find("reset_button").GetComponent<Animator>().SetTrigger("died");
        HighScoreManager.instance.CheckData();
        Destroy(gameObject);
    }
}
