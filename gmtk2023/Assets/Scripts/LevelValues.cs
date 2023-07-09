using UnityEngine;

public class LevelValues : MonoBehaviour
{
    public int targetKills = 10;
    public int targetSpawner = 5;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }
}
