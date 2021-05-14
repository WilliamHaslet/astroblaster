using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private GameObject barParent;
    [SerializeField] private Image background;
    [SerializeField] private Image healthBar;
    [SerializeField] private float maxBarHealth;

    public void SetBars(float maxHealth, float currentHealth)
    {

        background.fillAmount = maxHealth / maxBarHealth;

        healthBar.fillAmount = currentHealth / maxBarHealth;

    }

    public void ShowHealthBar(bool show)
    {

        barParent.SetActive(show);

    }

}
