using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] GameObject tanker;
    [SerializeField] GameObject ranger;
    [SerializeField] GameObject soldier;
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject healthPowerUp;
    [SerializeField] GameObject speedPowerUp;
    [SerializeField] GameObject[] powerupSpawns;
    [SerializeField] Text levelText;
    [SerializeField] Text Victory;
    [SerializeField] int maxPowerUps = 3;
    [SerializeField] int finalLevel = 10;
    bool gameOver = false;
    int currentlevel;
    float generatedSpawnTime = 1;
    float currentSpawnTime = 0;
    float powerUpSpawnTime = 30;
    float currentPowerUpSpawnTime = 0;
    GameObject newEnemy;
    int powerups = 0;
    GameObject newPowerUp;

    List<EnemyHealth> enemies = new List<EnemyHealth>();
    List<EnemyHealth> killedEnemies = new List<EnemyHealth>();

    public void RegisterEnemy(EnemyHealth enemy)
    {
        enemies.Add(enemy);
    }
    public void KilledEnemy(EnemyHealth enemy)
    {
        killedEnemies.Add(enemy);
    }
    public void RegisterPowerUp()
    {
        powerups++;
    }
    public bool GameOver
    {
        get { return gameOver; }
    }
    public GameObject Arrow
    {
        get { return arrow; }
    }
    public GameObject Player
    {
        get { return player; }
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Victory.GetComponent<Text>().enabled = false;
        StartCoroutine(Spawn());
        StartCoroutine(powerUpSpawn());
        currentlevel = 1;
    }

    void Update()
    {
        currentSpawnTime += Time.deltaTime;
        currentPowerUpSpawnTime += Time.deltaTime;
    }
    public void PlayerHit(int currentHP)
    {
        if(currentHP > 0)
        {
            gameOver = false;
        }
        else
        {
            gameOver = true;
            StartCoroutine(endGame("Defeat)"));
        }
    }
    IEnumerator Spawn()
    {
        if (currentSpawnTime >= generatedSpawnTime)
        {
            currentSpawnTime = 0;
            if (enemies.Count < currentlevel)
            {
                int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                GameObject spawnLocation = spawnPoints[randomNumber];
                int randomEnemy = Random.Range(0, 3);
                if(randomEnemy == 0)
                {
                    newEnemy = Instantiate(soldier) as GameObject;
                }
                else if (randomEnemy == 1)
                {
                    newEnemy = Instantiate(ranger) as GameObject;
                }
                else if (randomEnemy == 2)
                {
                    newEnemy = Instantiate(tanker) as GameObject;
                }
                newEnemy.transform.position = spawnLocation.transform.position;
            }
            if(killedEnemies.Count == currentlevel && currentlevel != finalLevel)
            {
                enemies.Clear();
                killedEnemies.Clear();
                yield return new WaitForSeconds(2f);
                currentlevel++;
                levelText.text = "Level " + currentlevel;
            }
            if(killedEnemies.Count == finalLevel)
            {
                StartCoroutine(endGame("Victory)"));
            }
        }
        yield return null;
        StartCoroutine(Spawn());
    }
    IEnumerator powerUpSpawn()
    {
        if (currentPowerUpSpawnTime >= powerUpSpawnTime)
        {
            currentPowerUpSpawnTime = 0;
            if (powerups < maxPowerUps)
            {
                int randomNumber = Random.Range(0, powerupSpawns.Length - 1);
                GameObject spawnLoc = powerupSpawns[randomNumber];
                int randomPowerUp = Random.Range(0, 2);
                if (randomPowerUp == 0)
                {
                    newPowerUp = Instantiate(healthPowerUp) as GameObject;
                }
                else if(randomPowerUp == 1)
                {
                    newPowerUp = Instantiate(speedPowerUp) as GameObject;
                }
                newPowerUp.transform.position = spawnLoc.transform.position;
            }
        }
        yield return null;
        StartCoroutine(powerUpSpawn());
    }
    IEnumerator endGame(string outcome)
    {
        Victory.text = outcome;
        Victory.GetComponent<Text>().enabled = true;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
    }
}
