using UnityEngine;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour
{

    [SerializeField] private GameObject[] pooledObjects;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int maxObjects;
    [SerializeField] private float spawnTimer;
    [SerializeField] private float spawnTimerRandomRange;
    [SerializeField] private float spawnRange;
    [SerializeField] private float scaleMin;
    [SerializeField] private float scaleMax;

    private List<GameObject> avaliblePool;
    private Queue<GameObject> activePool;
    private float spawnTimerStore;
    
    private void Start()
    {

        spawnTimerStore = spawnTimer;

        activePool = new Queue<GameObject>();
        avaliblePool = new List<GameObject>();

        foreach (GameObject item in pooledObjects)
        {

            for (int i = 0; i < maxObjects / pooledObjects.Length; i++)
            {

                GameObject newObject = Instantiate(item);

                newObject.SetActive(false);
                
                avaliblePool.Add(newObject);

            }

        }
        
    }

    private void Update()
    {

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {

            spawnTimer = Random.Range(spawnTimerStore - spawnTimerRandomRange, spawnTimerStore + spawnTimerRandomRange);

            GameObject newObject = avaliblePool[Random.Range(0, avaliblePool.Count)];

            avaliblePool.Remove(newObject);

            activePool.Enqueue(newObject);

            Vector3 spawnPos = new Vector3(spawnPoint.position.x + Random.Range(-spawnRange, spawnRange), spawnPoint.position.y, 0);

            float scale = Random.Range(scaleMin, scaleMax);

            newObject.transform.position = spawnPos;

            newObject.transform.localScale = new Vector3(scale, scale, 1);

            newObject.transform.Rotate(new Vector3(0, 0, Random.Range(0f, 360f)));

            newObject.SetActive(true);

            if (avaliblePool.Count < maxObjects / 2)
            {

                GameObject returnObject = activePool.Dequeue();

                returnObject.SetActive(false);

                avaliblePool.Add(returnObject);

            }
            
        }
        
    }

    public void ResetBackground()
    {
        
        foreach (GameObject item in activePool)
        {

            item.SetActive(false);

            avaliblePool.Add(item);

        }

        activePool.Clear();

    }
    
}
