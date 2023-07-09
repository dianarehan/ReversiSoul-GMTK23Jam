using UnityEngine;

public class LevelValues : MonoBehaviour
{
    public int targetKills = 10;
    public int targetSpawner = 5;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void Reset()
    {
        targetKills = 10;
        targetSpawner = 5;
    }
}