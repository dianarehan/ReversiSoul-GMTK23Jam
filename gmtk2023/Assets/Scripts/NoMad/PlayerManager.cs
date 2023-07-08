using System.Collections;
using Cinemachine;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float BodySwapCooldown = 2;
    private bool canBodySwap = true;

    public CharacterController startCharacter;
    private CharacterController currentCharacter;

    private WaitForSeconds bodySwapCooldownDelay;
    private Coroutine bodySwapCooldownCoroutine;

    private void Awake()
    {
        currentCharacter = startCharacter;
    }

    private void Start()
    {
        EventManager.instance.OnCollision += OnCollision;
        bodySwapCooldownDelay = new WaitForSeconds(BodySwapCooldown);
    }

    private void OnCollision(CharacterController first, CharacterController second)
    {
        if (!canBodySwap)
            return;

        canBodySwap = false;
        
        if (first.IsPlayer)
        {
            first.IsPlayer = false;
            second.IsPlayer = true;
            TryChangeTarget(second);
        }
        else if (second.IsPlayer)
        {
            first.IsPlayer = true;
            second.IsPlayer = false;
            TryChangeTarget(first);
        }
        else
        {
            canBodySwap = true;
        }
    }

    private void TryChangeTarget(CharacterController newTarget)
    {
        currentCharacter = newTarget;
        
        if (bodySwapCooldownCoroutine != null)
        {
            StopCoroutine(BodySwap());
        }

        bodySwapCooldownCoroutine = StartCoroutine(BodySwap());
        
        virtualCamera.Follow = currentCharacter.transform;
        //Do smth on change
    }

    private void Update()
    {
        if (currentCharacter != null)
        {
            currentCharacter.Move();
        }
    }

    private IEnumerator BodySwap()
    {
        yield return bodySwapCooldownDelay;
        canBodySwap = true;
    }
}