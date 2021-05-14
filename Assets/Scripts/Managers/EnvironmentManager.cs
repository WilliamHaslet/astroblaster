using System.Collections;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour {
    
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float screenWidth;
    [SerializeField] private Transform environmentSpawnPoint;
    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private Transform playerStartPoint;
    [SerializeField] private float cameraGrowthRate;
    [SerializeField] private float cameraPanSpeed;
    [SerializeField] private float cameraSmoothSpeed;
    [SerializeField] private float shiftThreshold;
    [SerializeField] private float cameraSize;
    [SerializeField] private GameObject playerStartingShip;
    
    [HideInInspector] public float shakeDuration;
    [HideInInspector] public float shakeMagnitude;

    private Camera mainCamera;
    private PlayerController thePlayer;
    private UpgradeManager upgradeManager;
    private float environmentSpawnPointYPosition;
    private float enemySpawnPointYPosition;
    private ScoreManager scoreManager;
    private EnemySpawner enemySpawner;
    private DifficultyManager difficultyManager;
    private Vector3 velocityRef;

    private void Start()
    {

        mainCamera = Camera.main;

        GameObject newPlayer = Instantiate(playerStartingShip, mainCamera.transform);
        newPlayer.transform.position = playerStartPoint.position;
        thePlayer = newPlayer.GetComponent<PlayerController>();

        upgradeManager = FindObjectOfType<UpgradeManager>();
        upgradeManager.SetPlayer(thePlayer);

        environmentSpawnPointYPosition = environmentSpawnPoint.transform.localPosition.y;
        enemySpawnPointYPosition = enemySpawnPoint.transform.localPosition.y;

        scoreManager = FindObjectOfType<ScoreManager>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        difficultyManager = FindObjectOfType<DifficultyManager>();

    }

    public void NewShip(GameObject ship)
    {

        GameObject newPlayer = Instantiate(ship, mainCamera.transform);

        newPlayer.transform.position = thePlayer.transform.position;

        Destroy(thePlayer.gameObject);

        thePlayer = newPlayer.GetComponent<PlayerController>();

        upgradeManager.SetPlayer(thePlayer);

    }
	
	private void Update()
    {
        
        if (thePlayer != null)
        {

            Vector3 smoothedPosition = Vector3.SmoothDamp(mainCamera.transform.position, new Vector3(thePlayer.transform.position.x * cameraPanSpeed, 0, -2), ref velocityRef, cameraSmoothSpeed);

            smoothedPosition.y = mainCamera.transform.position.y + (cameraSpeed * Time.deltaTime);

            mainCamera.transform.position = smoothedPosition;
            
            float xOffset = -mainCamera.transform.position.x;

            environmentSpawnPoint.transform.localPosition = new Vector3(xOffset, environmentSpawnPointYPosition, 0);

            enemySpawnPoint.transform.localPosition = new Vector3(xOffset, enemySpawnPointYPosition, 0);
            
        }
        
        if (mainCamera.orthographicSize < cameraSize)
        {
            
            mainCamera.orthographicSize += cameraGrowthRate * Time.deltaTime;

            if (mainCamera.orthographicSize > cameraSize)
            {

                mainCamera.orthographicSize = cameraSize;
                
            }

        }
        
    }

    public void CameraShake(int duration, float magnitude)
    {

        shakeDuration = duration;

        shakeMagnitude = magnitude;

        StartCoroutine(C_CameraShake());

    }

    private IEnumerator C_CameraShake()
    {
        
        while (shakeDuration > 0)
        {

            mainCamera.transform.position = new Vector3(
                mainCamera.transform.position.x + Random.Range(-1f, 1f) * shakeMagnitude, 
                mainCamera.transform.position.y + Random.Range(-1f, 1f) * shakeMagnitude, -2);

            mainCamera.transform.Rotate(0, 0, Random.Range(-3f, 3f) * shakeMagnitude);

            yield return new WaitForSeconds(0.005f);

            shakeDuration -= 1;

        }
        
        mainCamera.transform.rotation = new Quaternion(0, 0, 0, 0);

        yield return null;

    }

    public void RestartGame()
    {

        StartCoroutine(Restart());

    }

    private IEnumerator Restart()
    {

        yield return new WaitForSeconds(1);

        mainCamera.orthographicSize = 3;

        mainCamera.transform.position = new Vector3(0, 0, -2);

        difficultyManager.ResetDifficulty();

        scoreManager.ResetScore();
        
        enemySpawner.ResetSpawner();

        SpawnStartingShip();

        upgradeManager.SetButtonsActive();

        BackgroundManager[] backgroundManagers = FindObjectsOfType<BackgroundManager>();

        foreach (BackgroundManager backgroundManager in backgroundManagers)
        {

            backgroundManager.ResetBackground();

        }

    }

    private void SpawnStartingShip()
    {

        Destroy(thePlayer.gameObject);

        GameObject newPlayer = Instantiate(playerStartingShip, mainCamera.transform);

        newPlayer.transform.position = playerStartPoint.position;

        thePlayer = newPlayer.GetComponent<PlayerController>();

        upgradeManager.SetPlayer(thePlayer);

    }

}
