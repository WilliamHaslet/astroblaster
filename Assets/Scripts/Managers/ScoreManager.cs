using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour {

    [SerializeField] private Text scoreText;
    [SerializeField] private Text highscoreText;
    [SerializeField] private Text moneyText;
    [SerializeField] private Animator canvasAnimator;
    [SerializeField] private float scorePerSecond;
    [SerializeField] private float scoreAddTimer;
    [SerializeField] private float moneyAddTimer;
    [SerializeField] private float scoreAddRate;
    [SerializeField] private float moneyAddRate;
    [SerializeField] private float stopAddingScorePoint;
    [SerializeField] private float stopAddingMoneyPoint;
    [SerializeField] [Range(0, 1)] private float scoreToMoneyRate;

    private float score;
    private float money;
    private float scoreToBeAdded;
    private float scoreAddTimerStore;
    private float moneyAddTimerStore;
    private float moneyToBeAdded;
    private bool addingMoney;
    private bool canAddPoints = true;

    private void Start()
    {

        canvasAnimator.Play("FadeIn");

        scoreAddTimerStore = scoreAddTimer;

        moneyAddTimerStore = moneyAddTimer;

    }

    private void Update()
    {

        score += scorePerSecond * Time.deltaTime;

        scoreAddTimer -= Time.deltaTime;

        if (scoreAddTimer <= 0)
        {

            if (canAddPoints)
            {

                float addition = scoreToBeAdded * scoreAddRate;

                scoreToBeAdded -= addition;

                if (scoreToBeAdded < stopAddingScorePoint)
                {

                    score += addition + scoreToBeAdded;

                    scoreToBeAdded = 0;

                }
                else
                {

                    score += addition;

                }

                scoreText.text = score.ToString("f0");

            }

            scoreAddTimer = scoreAddTimerStore;

        }

        if (addingMoney)
        {

            moneyAddTimer -= Time.deltaTime;

            if (moneyAddTimer <= 0)
            {

                float addition = moneyToBeAdded * moneyAddRate;

                moneyToBeAdded -= addition;

                if (moneyToBeAdded < stopAddingMoneyPoint)
                {

                    money += addition + moneyToBeAdded;

                    moneyToBeAdded = 0;

                    addingMoney = false;

                }
                else
                {

                    money += addition;

                }

                moneyText.text = "$" + money.ToString("f0");

                PlayerPrefs.SetInt("Money", Mathf.RoundToInt(money));

                moneyAddTimer = moneyAddTimerStore;

            }

        }

    }

    public void AddScore(float scoreToAdd)
    {

        if (canAddPoints)
        {

            scoreToBeAdded += scoreToAdd;

        }
        
    }

    public void StartScore()
    {

        canAddPoints = true;

    }

    public void StopScore()
    {

        canAddPoints = false;

    }

    public void EndLevel()
    {
        
        canAddPoints = false;

        score += Mathf.RoundToInt(scoreToBeAdded);

        scoreText.text = score.ToString("f0");

        if (score > PlayerPrefs.GetInt("Highscore", 0))
        {

            PlayerPrefs.SetInt("Highscore", Mathf.RoundToInt(score));

            highscoreText.text = "New Highscore!";

        }
        else
        {

            highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore", 0).ToString();

        }

        money = PlayerPrefs.GetInt("Money", 0);

        moneyText.text = "$" + money.ToString();

        moneyToBeAdded = score * scoreToMoneyRate;

        addingMoney = true;
        
        canvasAnimator.Play("ShowButtons");
        
    }

    public void ReturnToMenu()
    {
        
        StartCoroutine(LoadMenu());

    }

    private IEnumerator LoadMenu()
    {

        canvasAnimator.Play("FadeOut");

        yield return new WaitForSeconds(1);

        SceneManager.LoadSceneAsync("Menu");
        
    }
    
    public void ResetScore()
    {

        scoreToBeAdded = 0;

        score = 0;

        canAddPoints = true;

        addingMoney = false;  

    }

    public void Fade()
    {

        canvasAnimator.Play("FadeOut");

        canvasAnimator.SetTrigger("fadeIn");

    }

}
