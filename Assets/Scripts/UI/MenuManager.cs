using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    
    [SerializeField] private Animator buttonAnimator;
    [SerializeField] private Animator blackAnimator;
    [SerializeField] private TextChanger titleTextChanger;

    [HideInInspector] public string activeScene;
    
    public void Play()
    {

        StartCoroutine(LoadSpace());

    }

    private IEnumerator LoadSpace()
    {
        
        blackAnimator.Play("BlackFadeIn");

        yield return new WaitForSeconds(1);

        SceneManager.LoadSceneAsync("Space");

    }

    public void QuitGame()
    {

        Application.Quit();

    }

}
