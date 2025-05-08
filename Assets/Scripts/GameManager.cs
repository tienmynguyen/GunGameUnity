
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    [SerializeField] private int currentEnergy;
    private int coin;
    [SerializeField] private Transform spawmBossPoint;
    [SerializeField] private int currentBanana;
    [SerializeField] private int energyThreshold = 7;
    [SerializeField] private int bananaThreshold = 3;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject AppleBoss;
    [SerializeField] protected Image energyBar;
    [SerializeField] protected Image bananaBar;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject enermySpawner;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private GameObject ShopMenu;

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private TextMeshProUGUI currentCoin;


    public bool dash = false; //cd 5S, dich chuyen 1 doan ngan
    public bool energyBallSkill = false; //cd 5s, ban mot tia beam gay x3 dmg


    public bool explosionSkill = true;


    [SerializeField] private bool bossCalled = false;

    void Start()
    {

        AppleBoss.SetActive(false);
        currentEnergy = 0;
        currentBanana = 0;
        coin = 0;
        UpdateEnergyBar();
        UpdateBananaBar();
        MainMenu();
        audioManager.StopAudio();
        cam.m_Lens.OrthographicSize = 5f;
    }
    void Update()
    {
        if (currentBanana == bananaThreshold)
        {
            if (bossCalled)
            {
                return;
            }
            CallAppleBoss();
        }
        if (currentEnergy == energyThreshold)
        {
            if (bossCalled)
            {
                return;
            }
            CallBoss();
        }
    }
    public void AddBanana()
    {
        currentBanana += 1;
        UpdateBananaBar();
    }
    public void AddEnergy()
    {
        currentEnergy += 1;
        UpdateEnergyBar();


    }
    public void AddCoin()
    {
        coin += 1;
        currentCoin.SetText(coin.ToString() + " " + "Coin");
    }
    private void CallBoss()
    {
        bossCalled = true;
        Instantiate(boss, spawmBossPoint.position, Quaternion.identity);
        boss.SetActive(true);
        gameUI.SetActive(false);
        audioManager.PlayBossAudio();
        cam.m_Lens.OrthographicSize = 10f;
    }
    private void CallAppleBoss()
    {
        // enermySpawner.SetActive(false);
        bossCalled = true;
        AppleBoss.SetActive(true);
        gameUI.SetActive(false);
        audioManager.PlayBossAudio();
        cam.m_Lens.OrthographicSize = 10f;

    }
    public void BossDefeat()
    {
        Debug.Log("BossDefeat được gọi!");
        currentEnergy = 0;
        UpdateEnergyBar();
        bossCalled = false;
        gameUI.SetActive(true);
        audioManager.PlayDefaulAudio();
        cam.m_Lens.OrthographicSize = 5f;
    }
    private void UpdateEnergyBar()
    {

        if (energyBar != null)
        {
            float fillAmount = Mathf.Clamp01((float)currentEnergy / (float)energyThreshold);
            energyBar.fillAmount = fillAmount;
        }
    }
    private void UpdateBananaBar()
    {

        if (bananaBar != null)
        {
            float fillAmount = Mathf.Clamp01((float)currentBanana / (float)bananaThreshold);
            bananaBar.fillAmount = fillAmount;
        }
    }
    public void MainMenu()
    {
        mainMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        winMenu.SetActive(false);
        ShopMenu.SetActive(false);
        Time.timeScale = 0f;
    }
    public void GameOverMenu()
    {
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        pauseMenu.SetActive(false);
        winMenu.SetActive(false);
        ShopMenu.SetActive(false);
        Time.timeScale = 0f;
    }
    public void PauseGameMenu()
    {
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(true);
        winMenu.SetActive(false);
        ShopMenu.SetActive(false);
        Time.timeScale = 0f;
    }
    public void StartGame()
    {
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        winMenu.SetActive(false);
        ShopMenu.SetActive(false);
        Time.timeScale = 1f;
        audioManager.PlayDefaulAudio();
    }
    public void ResumeGame()
    {
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        winMenu.SetActive(false);
        ShopMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void Wingame()
    {
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        winMenu.SetActive(true);
        ShopMenu.SetActive(false);
        Time.timeScale = 0f;
    }
    public void OpenChest()
    {

        ShopMenu.SetActive(true);
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        winMenu.SetActive(false);
        Time.timeScale = 0f;
    }
    public void SelectSkill(int skillId)
    {
        switch (skillId)
        {
            case 0:
                dash = true;
                break;
            case 1:
                energyBallSkill = true;
                break;
            case 2:
                explosionSkill = true;
                break;
        }
    }



}
