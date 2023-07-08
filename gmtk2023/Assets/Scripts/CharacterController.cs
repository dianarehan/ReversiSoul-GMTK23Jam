using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterAnimationController))]
public class CharacterController : MonoBehaviour
{
    public float Speed = 5f;
    public float AttackCooldown = 0.5f;
    public float HP = 4;
    public float Damage = 1;

    public bool IsPlayer;

    private bool canAttack = true;
    private BoxCollider2D boxCollider;
    private Vector3 moveDelta;
    private RaycastHit2D hit;
    private Vector3 initialScale;
    private CharacterAnimationController characterAnimationController;
    private WaitForSeconds bodySwapCooldownDelay;
    private Coroutine attackCooldownCoroutine;
    
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        characterAnimationController = GetComponent<CharacterAnimationController>();
        initialScale = transform.localScale;
        bodySwapCooldownDelay = new WaitForSeconds(AttackCooldown);

    }

    private void Update()
    {
        if (!IsPlayer)
        {
            characterAnimationController.SetIsMove(false);
            //AI logic
        }
    }

    public void Move(bool spaceInput, bool mouseInput)
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        moveDelta = new Vector3(x, y, 0);
        moveDelta *= Speed;

        /*
                //swap sprite direction (flip)
                if (moveDelta.x > 0)
                    transform.localScale = Vector3.one;
                // transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                else if (moveDelta.x < 0)
                    transform.localScale = new Vector3(-1, 1, 1);

                //transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        */

        if (moveDelta.magnitude == 0)
        {
            characterAnimationController.SetIsMove(false);
        }
        else
        {
            characterAnimationController.Move(moveDelta.normalized.x, moveDelta.normalized.y);
            characterAnimationController.SetIsMove(true);
        }

        // make sure that we can move in this directon by casting a box there first if the box returns null we are free to move
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y),
            Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //Make this thing move
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }
        else if (hit.collider.gameObject.GetComponent<CharacterController>() != null)
        {
            if (IsPlayer && spaceInput)
                EventManager.instance.OnCollision?.Invoke(this,
                    hit.collider.gameObject.GetComponent<CharacterController>());
            else if (mouseInput && canAttack)
            {
                hit.collider.gameObject.GetComponent<CharacterController>().ReceiveDamage(Damage);
                canAttack = false;

                if (attackCooldownCoroutine != null)
                {
                    StopCoroutine(AttackDelay());
                }

                attackCooldownCoroutine = StartCoroutine(AttackDelay());
            }
        }


        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0),
            Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //Make this thing move
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
        else if (hit.collider.gameObject.GetComponent<CharacterController>() != null)
        {
            if (IsPlayer && spaceInput)
                EventManager.instance.OnCollision?.Invoke(this,
                    hit.collider.gameObject.GetComponent<CharacterController>());
            else if (mouseInput && canAttack)
            {
                hit.collider.gameObject.GetComponent<CharacterController>().ReceiveDamage(Damage);
                canAttack = false;
                if (attackCooldownCoroutine != null)
                {
                    StopCoroutine(AttackDelay());
                }

                attackCooldownCoroutine = StartCoroutine(AttackDelay());
            }
        }
    }
    
    private IEnumerator AttackDelay()
    {
        yield return bodySwapCooldownDelay;
        canAttack = true;
    }

    public void ReceiveDamage(float damage)
    {
        HP -= damage;
        Debug.Log("DAMAGE");

        if (HP <= 0)
        {
            Debug.Log("DIE");
            //die
        }
    }
}