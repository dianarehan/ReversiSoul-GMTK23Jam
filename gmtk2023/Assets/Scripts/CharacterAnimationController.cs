using System.Collections;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private Animator animator;

    private readonly string nameMoveX = "MoveX";
    private readonly string nameMoveY = "MoveY";
    private readonly string nameIsMove = "IsMove";
    private readonly string nameHurt = "Hurt";
    private readonly string nameAttack = "Attack";

    private float hurtLayerOffCooldown = 0.5f;
    private float attackLayerOffCooldown = 0.5f;
    
    private WaitForSeconds hurtCooldownDelay;
    private Coroutine hurtCooldownCoroutine;
    
    private Coroutine attackCooldownCoroutine;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        hurtCooldownDelay = new WaitForSeconds(hurtLayerOffCooldown);

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
        if (hurtCooldownCoroutine != null)
        {
            StopCoroutine(SetHurtLayerOff());
        }

        animator.SetLayerWeight(1, 1);
        animator.SetTrigger(nameHurt);
        
        hurtCooldownCoroutine = StartCoroutine(SetHurtLayerOff());
    }
    
    private IEnumerator SetHurtLayerOff()
    {
        yield return hurtCooldownDelay;
        animator.SetLayerWeight(1, 0);

    }
    
    public void SetAttack()
    {
        animator.SetTrigger(nameAttack);
    }
}
