using UnityEngine;

public class ReturnToPoolTimer : MonoBehaviour {

    [SerializeField] private float lifetime;

    [HideInInspector] public ObjectPooler pool;

    private float lifetimeStore;

    private void Start()
    {

        lifetimeStore = lifetime;

    }

    private void Update()
    {

        lifetime -= Time.deltaTime;

        if (lifetime <= 0)
        {

            lifetime = lifetimeStore;

            pool.ReturnToPool(gameObject);
            
        }

    }

}
