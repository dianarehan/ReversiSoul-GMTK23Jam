using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CharacterAnimationController))]
public class CharacterController : MonoBehaviour
{
    public float Speed = 5f;
    public float AttackCooldown = 0.5f;
    public float HP = 4;
    public float Damage = 1;

    public bool IsPlayer;

    private float distanceToPlayer;
    private bool canAttack = true;
    private BoxCollider2D boxCollider;
    private RaycastHit2D hit;
    private Vector3 moveDelta;
    private Rigidbody2D rigidbody;
    private CharacterAnimationController characterAnimationController;
    private WaitForSeconds bodySwapCooldownDelay;
    private Coroutine attackCooldownCoroutine;
    private bool isDie;
    private bool spaceInput;
    private bool mouseInput;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        characterAnimationController = GetComponent<CharacterAnimationController>();
        bodySwapCooldownDelay = new WaitForSeconds(AttackCooldown);
    }

    private void Update()
    {
        if (isDie) return;

        spaceInput = Input.GetKey(KeyCode.Space);
        mouseInput = Input.GetKey(KeyCode.Mouse0);
    }

    private void FixedUpdate()
    {
        if (isDie) return;

        if (!IsPlayer)
        {
            var playerPos = PlayerManager.instance.currentCharacter.transform.position;
            var position = transform.position;
            distanceToPlayer = Vector2.Distance(position, playerPos);
            Vector2 direction = playerPos - position;
            direction.Normalize();
            Move(direction.x, direction.y);
        }
    }

    public void Move(float x, float y)
    {
        if (isDie) return;

        moveDelta = new Vector2(x, y);
        moveDelta *= Speed;

        if (moveDelta.magnitude == 0)
        {
            rigidbody.velocity = Vector2.zero;
            characterAnimationController.SetIsMove(false);
        }
        else
        {
            rigidbody.velocity = moveDelta * Time.deltaTime;
            characterAnimationController.Move(moveDelta.normalized.x, moveDelta.normalized.y);
            characterAnimationController.SetIsMove(true);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Collision(collision);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Collision(col);
    }

    private void Collision(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CharacterController>() != null)
        {
            hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y),
                Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));

            if ((hit.collider != null ? hit.collider.gameObject.GetComponent<CharacterController>() : null) != null)
            {
                if (IsPlayer && spaceInput)
                    EventManager.instance.OnCollision?.Invoke(this,
                        hit.collider.gameObject.GetComponent<CharacterController>());
                else if (!(IsPlayer && spaceInput) &&
                         ((IsPlayer && mouseInput && canAttack) ||
                         (canAttack && hit.collider.gameObject.GetComponent<CharacterController>().IsPlayer)))
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
            if ((hit.collider != null ? hit.collider.gameObject.GetComponent<CharacterController>() : null) != null)
            {
                if (IsPlayer && spaceInput)
                    EventManager.instance.OnCollision?.Invoke(this,
                        hit.collider.gameObject.GetComponent<CharacterController>());
                else if ((IsPlayer && mouseInput && canAttack) ||
                         (canAttack && hit.collider.gameObject.GetComponent<CharacterController>().IsPlayer))
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
    }

    private IEnumerator AttackDelay()
    {
        yield return bodySwapCooldownDelay;
        canAttack = true;
    }

    public void ReceiveDamage(float damage)
    {
        HP -= damage;
        characterAnimationController.SetHurt();

        if (HP <= 0)
        {
            isDie = true;
            EventManager.instance.OnPlayerDie?.Invoke();
            GetComponent<SpriteRenderer>().DOFade(0, 0.5f).OnComplete(() => Destroy(gameObject));
        }
    }
}