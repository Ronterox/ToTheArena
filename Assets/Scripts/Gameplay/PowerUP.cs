using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUP : MonoBehaviour
{
    [SerializeField] float speedBoost = 5f;
    [SerializeField] float bonusSize = 1f;
    [SerializeField] float explosionPowerTimesIncrement = 2f;
    [SerializeField] float explosionRangeIncrement = 1f;
    [SerializeField] float explosionRateIncrement = 1f;
    [SerializeField] float bulletTimeIncrement = 0.1f;

    [SerializeField] GameObject popUP = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            int randomBonus = Random.Range(0, 16);
            GameObject spawnedPopUp = Instantiate(popUP, new Vector2(0, 0), Quaternion.identity);
            switch (randomBonus)
            {
                case 0:
                case 1:
                case 2:
                    if (!Constants.FindObjectInChilds(collision.gameObject, "Weapon 2").activeSelf)
                        Constants.FindObjectInChilds(collision.gameObject, "Weapon 2").SetActive(true);
                    else
                        goto Explosion;
                    spawnedPopUp.GetComponent<TextMesh>().text = "DUAL WEAPONS!";
                    spawnedPopUp.GetComponent<TextMesh>().color = Color.magenta;

                    if (!GameManager.instance.themePlaying) 
                    {
                        AudioManager.instance.Play("theme");
                        GameManager.instance.themePlaying = true;
                    }
                    break;
                case 3:
                case 4:
                case 5:
                Explosion:
                    randomBonus = Random.Range(0, 3);
                    if (!BeExplosive(collision.gameObject, randomBonus))
                    {
                        spawnedPopUp.GetComponent<TextMesh>().text = "EKUSUPLOSHON!";
                        AudioManager.instance.Play("explosion");
                    }
                    else
                    {
                        switch (randomBonus)
                        {
                            case 1:
                                spawnedPopUp.GetComponent<TextMesh>().text = "EXPLOSION RANGE!";
                                break;
                            case 2:
                                spawnedPopUp.GetComponent<TextMesh>().text = "FASTER EXPLOSIONS!";
                                break;
                            default:
                                spawnedPopUp.GetComponent<TextMesh>().text = "EXPLOSION POWERRR!";
                                break;
                        }
                    }
                    spawnedPopUp.GetComponent<TextMesh>().color = Color.red;

                    if (!GameManager.instance.themePlaying)
                    {
                        AudioManager.instance.Play("theme");
                        GameManager.instance.themePlaying = true;
                    }
                    break;
                case 6:
                    if (!Gun(collision.gameObject))
                    {
                        spawnedPopUp.GetComponent<TextMesh>().text = "WATER GUN!";
                        spawnedPopUp.GetComponent<TextMesh>().color = Color.cyan;
                        AudioManager.instance.Play("water");
                    }
                    else
                    {
                        spawnedPopUp.GetComponent<TextMesh>().text = "MORE WATER!";
                        spawnedPopUp.GetComponent<TextMesh>().color = Color.cyan;
                        AudioManager.instance.Play("water");
                    }

                    if (!GameManager.instance.themePlaying)
                    {
                        AudioManager.instance.Play("theme");
                        GameManager.instance.themePlaying = true;
                    }
                    break;
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    speedBoost:
                    IncrementMoveSpeed(collision.gameObject);
                    spawnedPopUp.GetComponent<TextMesh>().text = "MORE SPEED!";
                    break;
                case 12:
                case 13:
                    IncreaseSize(collision.gameObject);
                    spawnedPopUp.GetComponent<TextMesh>().text = "BIGGER, STRONGER!";
                    break;
                case 14:
                case 15:
                    if (GiveDash(collision.gameObject))
                        spawnedPopUp.GetComponent<TextMesh>().text = "DASH WITH SHIFT!";
                    else
                        goto speedBoost;
                    break;
            }
            AudioManager.instance.Play("bass");
            Destroy(gameObject);
        }
    }

    private void IncreaseSize(GameObject obj)
    {
        HealthController playerHealth = obj.GetComponent<HealthController>();
        playerHealth.maxHp += (int)(bonusSize * 100);
        playerHealth.healthbar.setMaxHealth(playerHealth.maxHp);

        Vector3 newSize = new Vector3(obj.transform.localScale.x + bonusSize, obj.transform.localScale.y + bonusSize, obj.transform.localScale.z + bonusSize);
        obj.transform.localScale = newSize;

        obj.GetComponent<PlayerController2D>().GetComponent<PlayerDamageDealer>().damageMinimum = 0;

        obj.GetComponent<PlayerDamageDealer>().attackRange += bonusSize;
    }

    private void IncrementMoveSpeed(GameObject obj)
    {
        obj.gameObject.GetComponent<PlayerController2D>().movementSpeed += speedBoost;
    }

    private bool BeExplosive(GameObject obj, int increment)
    {
        Explosive explosive = obj.GetComponent<Explosive>();

        if (!explosive.enabled)
        {
            explosive.enabled = true;
            return false;
        }
        else
        {
            switch (increment)
            {
                case 1:
                    explosive.explosionRange += explosionRangeIncrement;
                    break;
                case 2:
                    explosive.cooldown -= explosionRateIncrement;
                    break;
                default:
                    explosive.power *= explosionPowerTimesIncrement;
                    break;
            }

            return true;
        }
    }

    private bool Gun(GameObject obj)
    {
        Gun gun = obj.GetComponent<Gun>();
        SpriteRenderer weaponSprite = Constants.FindObjectInChilds(obj, "Weapon").GetComponent<SpriteRenderer>();

        if (!gun.enabled)
        {
            gun.enabled = true;
            weaponSprite.color = Color.cyan;
            return false;
        }
        else
            gun.bulletTime += bulletTimeIncrement;

        return true;
    }

    private bool GiveDash(GameObject obj)
    {
        PlayerController2D playerController = obj.GetComponent<PlayerController2D>();

        if (playerController != null)
        {
            if (!playerController.canDash)
            {
                playerController.canDash = true;
                return true;
            }
        }

        return false;
    }
}
