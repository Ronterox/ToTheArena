using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] spawners = null;
    public int waves = 1;
    public int enemyIncrementPerWave = 2;
    public int maxEnemiesPerWave = 1;
    public float timeBTWEnemies = 1;

    [SerializeField] GameObject[] enemies = new GameObject[0];

    private float timerBTWEnemies = 0;
    private int enemiesCounter = 0;
    [HideInInspector]
    public int wavesCounter = 1;

    private GameObject lastSpawner;
    private int nextEnemy; //red 0, green 1, blue 2 : purplePusher 3, purpleSkeleton 4, purpleRange 5 : redBig 6, greenBig 7, blueBig 8

    private bool isLastWave = false;
    private int totalEnemies;

    private void Start()
    {
        AudioManager.instance.Play("cheer");
        Move();
        totalEnemies = maxEnemiesPerWave;
        AudioManager.instance.Play("crowd");
    }
    private void Update()
    {
        if (wavesCounter < waves)
        {
            if (GameManager.instance.kills >= totalEnemies)
            {
                enemiesCounter = 0;
                wavesCounter++;
                maxEnemiesPerWave += enemyIncrementPerWave;
                totalEnemies += maxEnemiesPerWave;
                GameManager.instance.wavesText.GetComponent<Animator>().SetTrigger("POP");
                if (wavesCounter == waves)
                {
                    GameManager.UpdateWave("Final Wave");
                    isLastWave = true;
                }
                else
                    GameManager.UpdateWave(wavesCounter);

                AudioManager.instance.Play("cheer");
            }
        }

        if (enemiesCounter < maxEnemiesPerWave && timerBTWEnemies <= 0)
        {
            if (enemiesCounter == maxEnemiesPerWave - 1 && isLastWave)
            {
                SpawnEnemy(nextEnemy);
                Constants.FindObjectInChilds(lastSpawner, "torch_fire").GetComponentInChildren<SpriteRenderer>().color = Color.white;
                Constants.FindObjectInChilds(lastSpawner, "torch_fire_2").GetComponentInChildren<SpriteRenderer>().color = Color.white;
            }
            else
            {
                SpawnEnemy(nextEnemy);
                Move();
            }
            timerBTWEnemies = timeBTWEnemies;
        }
        if (timerBTWEnemies > 0)
            timerBTWEnemies -= Time.deltaTime;

        if (GameManager.instance.kills == totalEnemies)
        {
            GameManager.instance.WinState();
        }
    }

    private void Move()
    {
        if (wavesCounter <= 5)
            nextEnemy = Random.Range(0, 3);
        else if (wavesCounter <= 10)
            nextEnemy = Random.Range(0, 6);
        else
            nextEnemy = Random.Range(0, enemies.Length);

        if (lastSpawner != null)
        {
            Constants.FindObjectInChilds(lastSpawner, "torch_fire").GetComponentInChildren<SpriteRenderer>().color = Color.white;
            Constants.FindObjectInChilds(lastSpawner, "torch_fire_2").GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }

        int randomSpawner = Random.Range(0, spawners.Length);
        lastSpawner = spawners[randomSpawner];

        switch (nextEnemy)
        {
            case 0:
            case 6:
                Constants.FindObjectInChilds(lastSpawner, "torch_fire").GetComponentInChildren<SpriteRenderer>().color = Color.red;
                Constants.FindObjectInChilds(lastSpawner, "torch_fire_2").GetComponentInChildren<SpriteRenderer>().color = Color.red;
                break;
            case 1:
            case 7:
                Constants.FindObjectInChilds(lastSpawner, "torch_fire").GetComponentInChildren<SpriteRenderer>().color = Color.green;
                Constants.FindObjectInChilds(lastSpawner, "torch_fire_2").GetComponentInChildren<SpriteRenderer>().color = Color.green;
                break;
            case 2:
            case 8:
                Constants.FindObjectInChilds(lastSpawner, "torch_fire").GetComponentInChildren<SpriteRenderer>().color = Color.blue;
                Constants.FindObjectInChilds(lastSpawner, "torch_fire_2").GetComponentInChildren<SpriteRenderer>().color = Color.blue;
                break;
            case 3:
            case 4:
            case 5:
                Constants.FindObjectInChilds(lastSpawner, "torch_fire").GetComponentInChildren<SpriteRenderer>().color = Color.magenta;
                Constants.FindObjectInChilds(lastSpawner, "torch_fire_2").GetComponentInChildren<SpriteRenderer>().color = Color.magenta;
                break;
        }

        Vector3 newPos = lastSpawner.transform.position;
        transform.position = newPos;
    }

    public void SpawnEnemy()
    {
        int randomEnemy = Random.Range(0, enemies.Length);
        Instantiate(enemies[randomEnemy], transform.position, Quaternion.identity);

        lastSpawner.GetComponent<Animator>().SetTrigger("open");

        enemiesCounter++;

        AudioManager.instance.Play("door");
    }
    public void SpawnEnemy(int enemyInArray)
    {
        Instantiate(enemies[enemyInArray], transform.position, Quaternion.identity);

        lastSpawner.GetComponent<Animator>().SetTrigger("open");

        enemiesCounter++;

        AudioManager.instance.Play("door");
    }

    public void ResetSpawner()
    {
        gameObject.SetActive(false);
        isLastWave = false;
        wavesCounter = 1;
        enemiesCounter = 0;
        timerBTWEnemies = 0;
        totalEnemies = maxEnemiesPerWave;
        gameObject.SetActive(true);
    }
}
