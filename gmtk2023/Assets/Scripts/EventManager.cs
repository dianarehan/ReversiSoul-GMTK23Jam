using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public Action<CharacterController, CharacterController> OnCollision;
    public Action<CharacterController> OnPlayerDie;
    public Action OnWin;
    public Action OnLose;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

}
