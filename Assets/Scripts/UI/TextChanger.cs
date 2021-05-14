using UnityEngine;
using UnityEngine.UI;

public class TextChanger : MonoBehaviour {

    [SerializeField] private float timer;
    [SerializeField] private Text currentText;
    [SerializeField] private string defaultText;

    private string targetText;
    private float timerStore;
    private bool rebuild;
    private bool remove;
    private int rebuildStep;

    private void Start()
    {

        timerStore = timer;

    }

    private void Update()
    {

        timer -= Time.deltaTime;

        if (timer <= 0)
        {

            timer = timerStore;

            if (rebuild)
            {

                string newText = currentText.text;

                newText = newText.Substring(0, newText.Length - 1);

                newText += targetText[rebuildStep];

                rebuildStep++;

                newText += "_";

                if (newText.Substring(0, newText.Length - 1) == targetText)
                {

                    newText = newText.Substring(0, newText.Length - 1);

                    rebuild = false;

                    rebuildStep = 0;

                    timer = timerStore;

                }

                currentText.text = newText;

            }
            else if (remove)
            {

                string newText = currentText.text;

                newText = newText.Substring(0, newText.Length - 2) + "_";

                currentText.text = newText;

                if (newText.Length <= 1)
                {

                    rebuild = true;

                    remove = false;

                }

            }

        }

    }

    public void ChangeText(string newText)
    {

        targetText = newText;

        remove = true;

        rebuild = false;

        rebuildStep = 0;

        timer = timerStore;

        currentText.text = defaultText;

        defaultText = newText;

    }

}
