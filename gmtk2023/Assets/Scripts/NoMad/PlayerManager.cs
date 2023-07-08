using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //  public CinemachineVirtualCamera virtualCamera;
    //  private CinemachineBasicMultiChannelPerlin noise;
    public float BodySwapCooldown = 2;
    private bool canBodySwap = true;

    public CharacterController startCharacter;
    private CharacterController currentCharacter;

    private WaitForSeconds shakeScreenLength;
    private Coroutine shakeScreenCoroutine;

    private WaitForSeconds bodySwapCooldownDelay;
    private Coroutine bodySwapCooldownCoroutine;

    private void Awake()
    {
        currentCharacter = startCharacter;
    }

    private void Start()
    {
        EventManager.instance.OnCollision += OnCollision;
        //   noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        bodySwapCooldownDelay = new WaitForSeconds(BodySwapCooldown);
        shakeScreenLength = new WaitForSeconds(1);
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

        if (shakeScreenCoroutine != null)
        {
            StopCoroutine(shakeScreenCoroutine);
        }

        //    shakeScreenCoroutine = StartCoroutine(ShakeScreen());
        //    virtualCamera.Follow = currentCharacter.transform;
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

    //  private IEnumerator ShakeScreen()
//  {
//  noise.m_AmplitudeGain = 1;
//   yield return shakeScreenLength;
//   noise.m_AmplitudeGain = 0;
//  }
}