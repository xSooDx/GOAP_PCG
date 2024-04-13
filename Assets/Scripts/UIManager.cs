using PCC.ContentRepresentation.Sample;
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
    public CampSpawner CampSpawner;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        Sample s = PCPCGManager.Instance.GetSample();
        float bf = s.GetSampleValue(PCPCGManager.BRANCH_FEATURE).Item2.floatVal;
        LevelGenerator.generatorSettings.branchingFactor = bf;
        LevelGenerator.GenerateLevelWithSeed(System.DateTime.Now.Millisecond);
        float ed = s.GetSampleValue(PCPCGManager.ENEMY_DENSITY_FEATURE).Item2.floatVal;
        CampSpawner.EnemyDensity = ed;
        EndGameScreen.SetActive(false);
        LevelGenerator.CallGeneratorListeners();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            EndGame(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void SubmitFeedback(bool like)
    {
        
        Debug.Log("Feedback: " + like);
        PCPCGManager.Instance.RecordSample(like);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
