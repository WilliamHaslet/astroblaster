using UnityEngine;

public class UpgradeManager : MonoBehaviour
{

    [SerializeField] private GameObject upgradeShipButton;
    [SerializeField] private GameObject fireRateButton;

    private PlayerController player;
    private EnvironmentManager environmentManager;

    private void Start()
    {

        environmentManager = FindObjectOfType<EnvironmentManager>();

    }

#if UNITY_EDITOR || DEBUG

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            player.UpgradeAddOns();

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            player.RecoverHealth();

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {

            player.UpgradeFireRate();

        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {

            player.AlterHealth(20);

        }

    }

#endif

    public void RecoverHealth()
    {

        player.RecoverHealth();

    }

    public void UpgradeFireRate()
    {

        player.UpgradeFireRate();

    }

    public void UpgradeAddOns()
    {

        player.UpgradeAddOns();

    }

    public void SetPlayer(PlayerController playerRef)
    {

        player = playerRef;

    }

    public void HideFireRateButton()
    {

        fireRateButton.SetActive(false);

    }

    public void HideShipUpgradeButton()
    {

        upgradeShipButton.SetActive(false);

    }

    public void NewShip(GameObject ship)
    {

        SetButtonsActive();

        environmentManager.NewShip(ship);

    }

    public void SetButtonsActive()
    {

        fireRateButton.SetActive(true);

        upgradeShipButton.SetActive(true);

    }

}
