using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{

    [SerializeField] private GameObject pooledObject;

    private Queue<GameObject> avaliblePool;

    private void Start()
    {

        avaliblePool = new Queue<GameObject>();
        
    }

    private void AddNewObjectToPool()
    {

        GameObject newObject = Instantiate(pooledObject);

        ReturnToPoolTimer returnTimer = newObject.GetComponent<ReturnToPoolTimer>();

        if (returnTimer != null)
        {

            returnTimer.pool = this;

        }

        newObject.SetActive(false);

        avaliblePool.Enqueue(newObject);

    }
    
    public GameObject GetPooledObject()
    {
        
        if (avaliblePool.Count <= 0)
        {

            AddNewObjectToPool();

        }

        GameObject returnObject = avaliblePool.Dequeue();

        returnObject.SetActive(true);
        
        return returnObject;

    }

    public void ReturnToPool(GameObject returnObject)
    {

        returnObject.SetActive(false);

        avaliblePool.Enqueue(returnObject);
        
    }

}
