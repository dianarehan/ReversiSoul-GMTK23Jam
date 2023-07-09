using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public TMP_Text targetText;
    public Spawner[] spawners;
    public GameObject winWindow;
    public GameObject loseWindow;
    private LevelValues levelValues;
    private int enemiesKilled;

    private void Start()
    {
        EventManager.instance.OnPlayerDie += OnPlayerDie;
        levelValues = FindObjectOfType<LevelValues>();
        InitLevel();
    }

    public void InitLevel()
    {
        enemiesKilled = 0;
        foreach (var spawner in spawners)
        {
            spawner.maxSpawnedCount =  levelValues.targetSpawner;
        }
        
        targetText.text = $"{enemiesKilled}/{levelValues.targetKills}";
    }

    public void LoadMenu()
    {
        levelValues.Reset();
        SceneManager.LoadScene("MainMenu");
    }
    
    public void StartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level");
    }

    private void OnPlayerDie(CharacterController obj)
    {
        if (obj.IsPlayer)
        {
            EventManager.instance.IsGameDone = true;
            loseWindow.SetActive(true);
            loseWindow.GetComponent<CanvasGroup>().alpha = 0;
            loseWindow.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        }
        else
        {
            enemiesKilled++;
            targetText.text = $"{enemiesKilled}/{levelValues.targetKills}";
            if (enemiesKilled == levelValues.targetKills)
            {
                EventManager.instance.IsGameDone = true;
                levelValues.targetKills += 10;
                levelValues.targetSpawner += 1;
                winWindow.SetActive(true);
                winWindow.GetComponent<CanvasGroup>().alpha = 0;
                winWindow.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
            }
        }
    }
}