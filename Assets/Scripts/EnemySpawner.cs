using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private float spawnTimer;
    [SerializeField] private float spawnRange;
    [SerializeField] private int[] addShipDifficulty;
    [SerializeField] private ObjectPooler[] enemyPools;

    private float spawnTimerStore;
    private bool shouldSpawn = true;
    private DifficultyManager difficultyManager;

    private void Start()
    {
        
        spawnTimerStore = spawnTimer;

        difficultyManager = FindObjectOfType<DifficultyManager>();

    }

    private void Update()
    {
        
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0 && shouldSpawn)
        {

            spawnTimer = spawnTimerStore;

            SpawnEnemy();

        }

    }

    private void SpawnEnemy()
    {
        
        Vector3 spawnPosition = new Vector3(enemySpawnPoint.position.x + Random.Range(-spawnRange, spawnRange), enemySpawnPoint.position.y, 0);

        int possibleShips = 0;
        
        for (int i = 0; i < addShipDifficulty.Length; i++)
        {

            if (difficultyManager.difficulty >= addShipDifficulty[i])
            {

                possibleShips++;

            }
            
        }

        int poolNum = Random.Range(0, possibleShips);

        GameObject newEnemy = enemyPools[poolNum].GetPooledObject();

        newEnemy.transform.position = spawnPosition;

        newEnemy.GetComponent<EnemyController>().myPool = enemyPools[poolNum];
        
    }

    public void EndGame()
    {

        shouldSpawn = false;

    }

    public void ResetSpawner()
    {

        shouldSpawn = true;

        spawnTimer = spawnTimerStore;

    }

    public void SetShouldSpawn(bool spawn)
    {

        shouldSpawn = spawn;

    }

}
