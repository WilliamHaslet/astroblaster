using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DifficultyManager : MonoBehaviour {

    [HideInInspector] public int difficulty = 1;
    [HideInInspector] public bool difficultyincreasing;

    [SerializeField] private float difficultyUpTimeStart;
    [SerializeField] private float difficultyUpTimeIncrease;
    [SerializeField] private float difficultyUpWaitTime;
    [SerializeField] private Animator canvasAnimator;
    [SerializeField] private Animator upgradeAnimator;
    [SerializeField] private Text stageText;

    private float gameTime;
    private float difficultyUpTime;
    private EnemySpawner enemySpawner;
    private ScoreManager scoreManager;
    private PlayerController player;
    private HealthBar healthBar;
    private bool gameEnded = false;

    private void Start()
    {
        
        difficultyincreasing = true;

        enemySpawner = FindObjectOfType<EnemySpawner>();

        scoreManager = FindObjectOfType<ScoreManager>();

        healthBar = FindObjectOfType<HealthBar>();

        difficultyUpTime = difficultyUpTimeStart;

    }

    private void Update()
    {

        if (difficultyincreasing && !gameEnded)
        {

            gameTime += Time.deltaTime;

            if (gameTime >= difficultyUpTime)
            {

                StartCoroutine(DifficultyUp());

            }

        }
        
    }

    public void SetGameEnded()
    {

        gameEnded = true;

        upgradeAnimator.Play("HideUpgradeButtons");

    }

    public void ResetDifficulty()
    {

        difficulty = 1;

        gameTime = 0;

        difficultyUpTime = difficultyUpTimeStart;

        difficultyincreasing = true;

        gameEnded = false;

        upgradeAnimator.Play("HideUpgradeButtons");

    }

    private IEnumerator DifficultyUp()
    {

        if (!gameEnded)
        {

            enemySpawner.SetShouldSpawn(false);

            difficultyincreasing = false;

        }

        yield return new WaitForSeconds(difficultyUpWaitTime);

        if (!gameEnded)
        {

            healthBar.ShowHealthBar(false);

            scoreManager.StopScore();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (player == null)
            {

                player = FindObjectOfType<PlayerController>();

            }

            player.SetCanMove(false);

            stageText.text = "Stage: " + difficulty.ToString();

            canvasAnimator.Play("DifficultyUp");

            upgradeAnimator.Play("ShowUpgradeButtons");

        }

    }

    public void ContinueGame()
    {

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        if (player == null)
        {

            player = FindObjectOfType<PlayerController>();

        }

        player.SetCanMove(true);

        gameTime = 0;

        difficulty++;

        difficultyUpTime += difficultyUpTimeIncrease;

        enemySpawner.SetShouldSpawn(true);

        scoreManager.StartScore();

        difficultyincreasing = true;

        upgradeAnimator.Play("HideUpgradeButtons");

        healthBar.ShowHealthBar(true);

    }

}
