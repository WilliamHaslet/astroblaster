using UnityEngine;

public class BulletController : MonoBehaviour {

    [SerializeField] private float damage;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float playerSpeedAddition;
    [SerializeField] private float enemySpeedAddition;
    [SerializeField] private Color hitEffectColor;

    [HideInInspector] public float damageMultiplier = 1;
    [HideInInspector] public bool isPlayer;
    [HideInInspector] public ObjectPooler lazerPool;
    [HideInInspector] public ObjectPooler hitEffectPool;

    private Vector2 screenSize;
    private float modifiedBulletSpeed;
    private Camera mainCamera;

    private void OnEnable()
    {
        
        if (isPlayer)
        {

            modifiedBulletSpeed = bulletSpeed + playerSpeedAddition;

        }
        else
        {

            modifiedBulletSpeed = bulletSpeed - enemySpeedAddition;

        }

    }

    private void Start()
    {

        screenSize = new Vector2(Screen.width, Screen.height);

        mainCamera = Camera.main;
        
    }

    private void Update()
    {

        transform.position += transform.up * modifiedBulletSpeed * Time.deltaTime;

        if (transform.position.y > mainCamera.ScreenToWorldPoint(screenSize).y + 1 ||
            transform.position.y < mainCamera.ScreenToWorldPoint(Vector3.zero).y - 1 ||
            transform.position.x > mainCamera.ScreenToWorldPoint(screenSize).x + 1 ||
            transform.position.x < mainCamera.ScreenToWorldPoint(Vector3.zero).x - 1)
        {

            RemoveBullet();

        }

    }

    private void RemoveBullet()
    {

        lazerPool.ReturnToPool(gameObject);
        
    }

    private void SpawnHitEffect()
    {
        
        GameObject newHitEffect = hitEffectPool.GetPooledObject();
        
        var particleSystem = newHitEffect.GetComponent<ParticleSystem>().main;

        particleSystem.startColor = hitEffectColor;

        newHitEffect.transform.position = transform.position;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (isPlayer)
        {

            if (collision.CompareTag("Enemy"))
            {

                collision.GetComponent<EnemyController>().AlterHealth(damage * damageMultiplier);

                SpawnHitEffect();

                RemoveBullet();

            }

        }
        else
        {

            if (collision.CompareTag("Player"))
            {

                PlayerController hitPlayer = collision.GetComponent<PlayerController>();
                
                hitPlayer.AlterHealth(damage);

                hitPlayer.ShakePlayer(0.1f);

                SpawnHitEffect();

                RemoveBullet();

            }

        }

    }
    
}
