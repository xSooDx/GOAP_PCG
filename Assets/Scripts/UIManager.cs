using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    int EnemiesAlive;
    int WoodCount;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI enemyWoodText;
    public TextMeshProUGUI pickupText;
    public TextMeshProUGUI enemyCount;
    public GameObject EndGameScreen;
    public TextMeshProUGUI endGameText;
    public int WoodCountToWin = 100;
    public LevelGenerator LevelGenerator;

    public void AddWood()
    {
        WoodCount++;
        enemyWoodText.text = "Enemy Wood: " + WoodCount + "/" + WoodCountToWin;
        if(WoodCount >= WoodCountToWin)
        {
            EndGame(false);
        }
    }

    public void EndGame(bool win)
    {
        Time.timeScale = 0f;
        endGameText.text = win ? "You Win" : "You Loose";
        EndGameScreen.SetActive(true);
    }

    public void RegisterEnemy()
    {
        EnemiesAlive++;
        enemyCount.text = "Enemey Count: " + EnemiesAlive;
    }

    public void EnemeyDead()
    {
        EnemiesAlive--;
        enemyCount.text = "Enemey Count: " + EnemiesAlive;
        if(EnemiesAlive <= 0)
        {
            EndGame(true);
        }
    }

    private void Awake()
    {
        Instance = this;
        EnemiesAlive = 0;
        WoodCount++;
        Time.timeScale = 1f;
        LevelGenerator.GenerateLevelWithSeed(System.DateTime.Now.Millisecond);
        LevelGenerator.CallGeneratorListeners();
        EndGameScreen.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void SubmitFeedback(bool like)
    {
        Debug.Log("Feedback: " + like);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
