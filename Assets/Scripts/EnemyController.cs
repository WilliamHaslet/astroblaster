using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float hitDamage;

    [SerializeField] private float pointTimer;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float health;
    [SerializeField] private float explosionScale;
    [SerializeField] private float fireStartPoint;
    [SerializeField] private float[] addAddOnDifficulties;
    [SerializeField] private int value;
    [SerializeField] private GameObject[] addOns;
    [SerializeField] TrailRenderer[] trails;
    
    [HideInInspector] public ObjectPooler myPool;

    private PlayerController thePlayer;
    private Vector3 target;
    private bool dead;
    private float pointTimerStore;
    private float healthStore;
    private ScoreManager scoreManager;
    private Vector2 screenSize;
    private Camera mainCamera;
    private ObjectPooler explosionPool;
    private DifficultyManager difficultyManager;

    private void Awake()
    {
        
        pointTimerStore = pointTimer;

        healthStore = health;

        difficultyManager = FindObjectOfType<DifficultyManager>();

    }

    private void OnEnable()
    {

        for (int i = 0; i < addAddOnDifficulties.Length; i++)
        {

            if (difficultyManager.difficulty >= addAddOnDifficulties[i])
            {

                addOns[i].SetActive(true);

            }
            else
            {

                addOns[i].SetActive(false);

            }

        }

        foreach (TrailRenderer trail in trails)
        {

            trail.emitting = true;

        }

        dead = false;

        health = healthStore;
        
        pointTimer = pointTimerStore;
        
    }
    
    private void Start()
    {

        thePlayer = FindObjectOfType<PlayerController>();

        scoreManager = FindObjectOfType<ScoreManager>();

        screenSize = new Vector2(Screen.width, Screen.height);

        mainCamera = Camera.main;

        ObjectPooler[] objectPoolers = FindObjectsOfType<ObjectPooler>();

        foreach (ObjectPooler objectPooler in objectPoolers)
        {

            if (objectPooler.tag == "ExplosionPool")
            {

                explosionPool = objectPooler;

                break;

            }

        }

    }

    private void Update()
    {

        pointTimer -= Time.deltaTime;

        transform.position += transform.up * moveSpeed * Time.deltaTime;

        if (pointTimer > 0 && thePlayer != null)
        {

            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(transform.position.y - thePlayer.transform.position.y, transform.position.x - thePlayer.transform.position.x) * Mathf.Rad2Deg + 90, Vector3.forward);

        }

        if (transform.position.y > mainCamera.ScreenToWorldPoint(screenSize).y + 15 ||
            transform.position.y < mainCamera.ScreenToWorldPoint(Vector3.zero).y - 10 ||
            transform.position.x > mainCamera.ScreenToWorldPoint(screenSize).x + 10 ||
            transform.position.x < mainCamera.ScreenToWorldPoint(Vector3.zero).x - 10)
        {

            if (!dead)
            {
                
                dead = true;

                foreach (TrailRenderer trail in trails)
                {

                    trail.emitting = false;

                    trail.Clear();

                }

                myPool.ReturnToPool(gameObject);

            }
            
        }

    }
    
    public void DestroyEnemy()
    {

        if (!dead)
        {

            GameObject newExplosion = explosionPool.GetPooledObject();

            newExplosion.transform.localScale = new Vector3(explosionScale, explosionScale, explosionScale);

            newExplosion.transform.position = transform.position;

            scoreManager.AddScore(value * difficultyManager.difficulty);
            
            dead = true;

            foreach (TrailRenderer trail in trails)
            {

                trail.emitting = false;

                trail.Clear();
                
            }

            myPool.ReturnToPool(gameObject);

        }

    }
    
    public void AlterHealth(float damage)
    {

        health -= damage;

        if (health <= 0)
        {

            DestroyEnemy();

        }

    }
    
}
