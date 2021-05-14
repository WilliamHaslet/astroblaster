using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float maxHealth;
    [SerializeField] private float healthRecovery;
    [SerializeField] private float[] fireRateLevels;
    [SerializeField] private GameObject[] addOns;
    [SerializeField] private GameObject nextShip;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distanceAboveTouch;
    [SerializeField] private float spinTimer;
    [SerializeField] private float explosionScale;
    [SerializeField] private float launchForce;
    
    private EnvironmentManager environmentManager;
    private float health;
    private float spinTimerStore;
    private float launchTimer;
    private float launchTimerStore;
    private bool playerInControl = true;
    private bool spinning;
    private bool launching;
    private Vector3 launchPoint;
    private EnemySpawner enemySpawner;
    private ScoreManager scoreManager;
    private ObjectPooler explosionPool;
    private UpgradeManager upgradeManager;
    private HealthBar healthBar;
    private int fireRateLevel;
    private int addOnLevel;
    private bool canMove = true;

    private void Start()
    {

        ObjectPooler[] objectPoolers = FindObjectsOfType<ObjectPooler>();

        foreach (ObjectPooler objectPooler in objectPoolers)
        {

            if (objectPooler.tag == "ExplosionPool")
            {

                explosionPool = objectPooler;

                break;

            }

        }

        launchTimer = spinTimer;
        spinTimerStore = spinTimer;
        launchTimerStore = launchTimer;

        health = maxHealth;

        environmentManager = FindObjectOfType<EnvironmentManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        upgradeManager = FindObjectOfType<UpgradeManager>();
        healthBar = FindObjectOfType<HealthBar>();

        healthBar.SetBars(maxHealth, health);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        healthBar.ShowHealthBar(true);

    }
    
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;

        }
        
        if (playerInControl && canMove)
        {
            
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            targetPosition.y += distanceAboveTouch;

            targetPosition.z = 0;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);

        }
        
        if (spinning)
        {

            spinTimer -= Time.deltaTime;
            
            transform.Rotate(new Vector3(0, 0, 360 * Time.deltaTime / spinTimerStore));
            
            if (spinTimer <= 0)
            {

                spinTimer = spinTimerStore;

                playerInControl = true;

                spinning = false;
                
                transform.rotation = Quaternion.identity;
                
            }

        }

        if (launching)
        {

            launchTimer -= Time.deltaTime;

            transform.position += new Vector3((launchPoint.x - transform.position.x) * -launchForce, (launchPoint.y - transform.position.y) * -launchForce, 0) * Time.deltaTime;

            if (launchTimer <= 0)
            {

                launchTimer = launchTimerStore;
                
                launching = false;
                
            }

        }

	}

    public void SetCanMove(bool move)
    {

        canMove = move;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy") && health > 0)
        {
            
            EnemyController enemy = collision.GetComponentInParent<EnemyController>();

            AlterHealth(enemy.hitDamage);

            ShakePlayer(0.15f);

            playerInControl = false;

            launchPoint = collision.transform.position;

            launching = true;

            enemy.DestroyEnemy();

        }

    }

    public void AlterHealth(float damage)
    {

        health -= damage;

        healthBar.SetBars(maxHealth, health);

        if (health <= 0)
        {

            EndLevel();

        }

    }

    public void ShakePlayer(float amount)
    {

        environmentManager.CameraShake(10, amount);

        spinning = true;

    }

    private void EndLevel()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameObject newExplosion = explosionPool.GetPooledObject();

        newExplosion.transform.localScale = new Vector3(explosionScale, explosionScale, explosionScale);

        newExplosion.transform.position = transform.position;
        
        environmentManager.CameraShake(10, 0.2f);

        scoreManager.EndLevel();
        
        enemySpawner.EndGame();

        FindObjectOfType<DifficultyManager>().SetGameEnded();

        healthBar.ShowHealthBar(false);

        gameObject.SetActive(false);
        
    }

    public void ResetPlayer()
    {

        health = maxHealth;
        
        transform.rotation = Quaternion.identity;
        
        playerInControl = true;

        gameObject.SetActive(true);

        spinning = false;

        launching = false;

    }

    public void RecoverHealth()
    {

        health = Mathf.Clamp(health + healthRecovery, 0, maxHealth);

        healthBar.SetBars(maxHealth, health);

    }

    public void UpgradeFireRate()
    {

        if (fireRateLevel < fireRateLevels.Length - 1)
        {

            fireRateLevel++;

            foreach (GameObject addOn in addOns)
            {

                addOn.GetComponent<AddOnController>().fireTimer *= fireRateLevels[fireRateLevel];

                addOn.GetComponent<AddOnController>().fireTimerStore *= fireRateLevels[fireRateLevel];

            }

        }
        else
        {

            upgradeManager.HideFireRateButton();

        }

    }

    public void UpgradeAddOns()
    {

        if (addOnLevel < addOns.Length - 1)
        {

            addOnLevel++;

            addOns[addOnLevel].SetActive(true);

            if (addOnLevel >= addOns.Length - 1 && nextShip == null)
            {

                upgradeManager.HideShipUpgradeButton();

            }

        }
        else
        {

            if (nextShip != null)
            {

                upgradeManager.NewShip(nextShip);

            }

        }

    }

}
