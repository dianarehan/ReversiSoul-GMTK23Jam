using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private Animator animator;

    private readonly string nameMoveX = "MoveX";
    private readonly string nameMoveY = "MoveY";
    private readonly string nameIsMove = "IsMove";
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
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
}
