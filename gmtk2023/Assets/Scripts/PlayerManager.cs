using System.Collections;
using Cinemachine;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float BodySwapCooldown = 2;
    private bool canBodySwap = true;
    public static PlayerManager instance;

    public CharacterController startCharacter;
    public CharacterController currentCharacter;

    private WaitForSeconds bodySwapCooldownDelay;
    private Coroutine bodySwapCooldownCoroutine;
    private bool spaceInput;
    private bool mouseInput;
    private float xAxis;
    private float yAxis;
    
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
        
        currentCharacter = startCharacter;
        currentCharacter.Speed *= 1.5f;
        currentCharacter.GetComponent<Rigidbody2D>().mass = 10;
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
        currentCharacter.Speed /= 1.5f;
        currentCharacter.GetComponent<Rigidbody2D>().mass = 1;


        currentCharacter = newTarget;
        
        currentCharacter.Speed *= 1.5f;
        currentCharacter.GetComponent<Rigidbody2D>().mass = 10;


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
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
    }
    
    private void FixedUpdate()
    {
        if (currentCharacter != null)
        {
            currentCharacter.Move(xAxis, yAxis);
        }
    }

    private IEnumerator BodySwap()
    {
        yield return bodySwapCooldownDelay;
        canBodySwap = true;
    }
}