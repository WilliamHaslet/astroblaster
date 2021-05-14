using UnityEngine;

public class AddOnController : MonoBehaviour {
    
    public float fireTimer;

    [SerializeField] private Transform[] firePoints;
    [SerializeField] private string poolName;
    [SerializeField] private bool isPlayer;

    [HideInInspector] public float bulletDamageMultiplier = 1;
    [HideInInspector] public float fireTimerStore;

    private ObjectPooler lazerPool;
    private ObjectPooler hitEffectPool;
    private Animator animator;

    private void Awake()
    {

        fireTimerStore = fireTimer;

    }

    private void OnEnable()
    {

        fireTimer = fireTimerStore;

    }

    private void Start()
    {

        animator = GetComponent<Animator>();

        if (animator != null)
        {

            animator.SetFloat("animSpeed", 1 / fireTimerStore);

        }
        
        ObjectPooler[] objectPoolers = FindObjectsOfType<ObjectPooler>();

        foreach (ObjectPooler objectPooler in objectPoolers)
        {

            if (objectPooler.name == poolName)
            {

                lazerPool = objectPooler;
                
            }
            else if (objectPooler.name == "HitEffectPool")
            {

                hitEffectPool = objectPooler;

            }

        }

    }

    private void Update()
    {

        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0)
        {

            if (animator != null)
            {

                animator.SetTrigger("fire");

            }

            foreach (Transform firePoint in firePoints)
            {

                Fire(firePoint);

            }

            fireTimer = fireTimerStore;

        }

    }

    private void Fire(Transform firePoint)
    {

        GameObject newLazer = lazerPool.GetPooledObject();

        newLazer.transform.position = firePoint.position;

        newLazer.transform.rotation = firePoint.rotation;

        newLazer.SetActive(true);

        BulletController bulletController = newLazer.GetComponent<BulletController>();

        bulletController.isPlayer = isPlayer;

        bulletController.lazerPool = lazerPool;

        bulletController.hitEffectPool = hitEffectPool;

        bulletController.damageMultiplier = bulletDamageMultiplier;
        
    }

}
