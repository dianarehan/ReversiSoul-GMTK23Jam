using Cinemachine;
using System.Threading.Tasks;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private CinemachineBasicMultiChannelPerlin noise;
    private Rigidbody2D characterRb;
    private Vector2 moveDir;

    // --------------------------------------------------------

    private void Awake()
    {
        characterRb = GetComponent<Rigidbody2D>();
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        // Taking Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        moveDir = new Vector2(horizontal, vertical).normalized;
    }

    private void FixedUpdate()
    {
        // Movement Logic
        if (moveDir.magnitude == 0)
        {
            characterRb.velocity = Vector2.zero;
        }
        else
        {
            characterRb.velocity = moveDir * moveSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ShakeScreen();
    }

    // --------------------------------------------------------

    private async void ShakeScreen()
    {
        noise.m_AmplitudeGain = 1;
        await Task.Delay(1000);
        noise.m_AmplitudeGain = 0;
    }
}
