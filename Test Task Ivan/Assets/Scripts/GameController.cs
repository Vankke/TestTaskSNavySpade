using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* THIS SCRIPT CONTROLS GAME LOGIC
 */
public class GameController : MonoBehaviour
{
    PlayerController PC;
    InterfaceController IC;

    [SerializeField] GameObject EnemyPrefab, GemPrefab;
    [SerializeField] GameObject EnemyDestroyParts, GemDestroyParts;
    public GameObject ClosestGemObject, ClosestEnemyObject;
    public List<GameObject> AllGems, AllEnemies;

    [SerializeField] Transform[] SpawnPoints;      //YOU CAN ADD SPAWNPOINTS AT THIS ARRAY TO MAKE MORE SPAWNPOINTS

    public int MaxGems;                            //MODIFY THIS VARIABLE TO CHANGE MAXIMUM QUANTITY OF GEMS IN PLAYABLE AREA
    public int MaxEnemies;                         //MODIFY THIS VARIABLE TO CHANGE MAXIMUM QUANTITY OF ENEMIES IN PLAYABLE AREA
    [SerializeField] float EnemyRespawnTimer;      //TIME TO SPAWN ENEMY
    [SerializeField] float GemRespawnTimer;        //TIME TO SPAWN GEM
    public long CurrentScore, HighScore;
    float enemyT, gemT;  //local timers

    public bool GameStarted;

    const string HighScorePref = "HIGHSCORE";

    private void Awake()
    {
        PC = FindObjectOfType<PlayerController>();
        IC = FindObjectOfType<InterfaceController>();
        ReadData();
    }

    private void Update()
    {
        if (GameStarted)
        {
            ManageCooldowns();
            DetermineClosestEnemy();
            DetermineClosestGem();
        }
    }
    public void StartGameMethod()
    {
        GameStarted = true;

        SpawnEnemy();
        for (int i = 0; i < 5; i++)
        {
            SpawnGem();
        }
    }

    public void EndGameMethod()
    {
        GameStarted = false;
        if(CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
        }
        SaveData();
        IC.EndGameInterfaceMethod();
    }

    void SpawnEnemy()
    {
        var spawnedEnemy = Instantiate(EnemyPrefab, SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform.position, Quaternion.identity);
        AllEnemies.Add(spawnedEnemy);
        enemyT = EnemyRespawnTimer;
    }
    void SpawnGem()
    {
        var spawnPos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        LayerMask mask = LayerMask.GetMask("Obstacle");

        var Obstacles = Physics.OverlapSphere(spawnPos, 1, mask);
        if (Obstacles.Length > 0)
        {
            return;
        }
        var spawnedGem = Instantiate(GemPrefab, spawnPos, Quaternion.identity);
        AllGems.Add(spawnedGem);
        gemT = GemRespawnTimer;
    }
    void ManageCooldowns()
    {
        if (AllEnemies.Count < MaxEnemies)
        {
            if (enemyT <= 0)
            {
                SpawnEnemy();
            }
            else
            {
                enemyT -= Time.deltaTime;
            }
        }
        if (AllGems.Count < MaxGems)
        {
            if (gemT <= 0)
            {
                SpawnGem();
            }
            else
            {
                gemT -= Time.deltaTime;
            }
        }
    }

    void DetermineClosestGem()
    {
        float dist = 0;
        int count = AllGems.Count;
        for (int i = 0; i < count; i++)
        {
            var currGem = AllGems[i];
            float localDist = Vector3.Distance(PC.transform.position, currGem.transform.position);
            if (dist == 0 || localDist < dist)
            {
                dist = localDist;
                ClosestGemObject = currGem;
            }
        }
    }
    void DetermineClosestEnemy()
    {
        float dist = 0;
        int count = AllEnemies.Count;
        for (int i = 0; i < count; i++)
        {
            var currEnemy = AllEnemies[i];
            float localDist = Vector3.Distance(PC.transform.position, currEnemy.transform.position);
            if (dist == 0 || localDist < dist)
            {
                dist = localDist;
                ClosestEnemyObject = currEnemy;
            }
        }
    }

    public void DestroyGem(GameObject gem)
    {
        var parts = Instantiate(GemDestroyParts, gem.transform.position, GemDestroyParts.transform.rotation);
        Destroy(parts, 1);
        AllGems.Remove(gem);
        Destroy(gem);
    }
    public void DestroyEnemy(GameObject enemy)
    {
        var parts = Instantiate(EnemyDestroyParts, enemy.transform.position, EnemyDestroyParts.transform.rotation);
        Destroy(parts, 1);
        AllEnemies.Remove(enemy);
        Destroy(enemy);
    }

    public void ReadData()
    {
        var readString = "0";
        if (PlayerPrefs.HasKey(HighScorePref)) 
        {
            readString = PlayerPrefs.GetString(HighScorePref);
        }
        HighScore = long.Parse(readString);
    }
    public void SaveData()
    {
        PlayerPrefs.SetString(HighScorePref, HighScore.ToString());
    }
}
