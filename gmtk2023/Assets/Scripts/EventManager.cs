using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public Action<CharacterController, CharacterController> OnCollision;
    public Action OnPlayerDie;
    
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
