using System.Collections;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private Animator animator;

    private readonly string nameMoveX = "MoveX";
    private readonly string nameMoveY = "MoveY";
    private readonly string nameIsMove = "IsMove";
    private readonly string nameHurt = "Hurt";

    private float hurtLayerOffCooldown = 0.5f;
    
    private WaitForSeconds bodySwapCooldownDelay;
    private Coroutine bodySwapCooldownCoroutine;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        bodySwapCooldownDelay = new WaitForSeconds(hurtLayerOffCooldown);

    }

    public void SetIsMove(bool state)
    {
        animator.SetBool(nameIsMove, state);
    }

    public void Move(float x, float y)
    {
        animator.SetFloat(nameMoveX, x);
        animator.SetFloat(nameMoveY, y);
    }

    public void SetHurt()
    {
        if (bodySwapCooldownCoroutine != null)
        {
            StopCoroutine(SetHurtLayerOff());
        }

        animator.SetLayerWeight(1, 1);
        animator.SetTrigger(nameHurt);
        
        bodySwapCooldownCoroutine = StartCoroutine(SetHurtLayerOff());
    }
    
    private IEnumerator SetHurtLayerOff()
    {
        yield return bodySwapCooldownDelay;
        animator.SetLayerWeight(1, 0);

    }
}
