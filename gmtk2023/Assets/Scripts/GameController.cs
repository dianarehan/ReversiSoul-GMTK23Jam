using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public TMP_Text targetText;
    public int target = 20;
    private int enemiesKilled = 0;

    private void Start()
    {
        EventManager.instance.OnPlayerDie += OnPlayerDie;
    }

    private void OnPlayerDie(CharacterController obj)
    {
        if (obj.IsPlayer)
        {
            EventManager.instance.OnLose?.Invoke();
        }
        else
        {
            enemiesKilled++;
            targetText.text = $"{enemiesKilled}/{target}";
            if (enemiesKilled == target)
                EventManager.instance.OnWin?.Invoke();
        }
    }
}