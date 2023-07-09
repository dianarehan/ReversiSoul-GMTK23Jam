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

    public Action OnDie;

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
        if (isDie) return;
        var character = collision.gameObject.GetComponent<CharacterController>();
        if (character == null) return;

        HandleBoxCastOnYAxis();
        HandleBoxCastOnXAxis();
    }

    private void HandleBoxCastOnYAxis()
    {
        hit = CreateBoxCast(new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), "Actor", "Blocking");
        HandleCollision();
    }

    private void HandleBoxCastOnXAxis()
    {
        hit = CreateBoxCast(new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), "Actor", "Blocking");
        HandleCollision();
    }

    private RaycastHit2D CreateBoxCast(Vector2 direction, float distance, params string[] layers)
    {
        var layerMask = LayerMask.GetMask(layers);
        return Physics2D.BoxCast(transform.position, boxCollider.size, 0, direction, distance, layerMask);
    }

    private void HandleCollision()
    {
        var collidedCharacter = hit.collider?.gameObject.GetComponent<CharacterController>();
        if (collidedCharacter == null) return;

        if (IsPlayer && spaceInput)
        {
            EventManager.instance.OnCollision?.Invoke(this, collidedCharacter);
        }
        else if (canAttack && (IsPlayer && mouseInput || collidedCharacter.IsPlayer))
        {
            ApplyDamageTo(collidedCharacter);
        }
    }

    private void ApplyDamageTo(CharacterController target)
    {
        if (isDie) return;
        target.ReceiveDamage(Damage);
        canAttack = false;

        if (attackCooldownCoroutine != null)
        {
            StopCoroutine(AttackDelay());
        }

        attackCooldownCoroutine = StartCoroutine(AttackDelay());
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
            OnDie?.Invoke();
            EventManager.instance.OnPlayerDie?.Invoke(this);
            GetComponent<SpriteRenderer>().DOFade(0, 0.5f).OnComplete(() =>
            {
                if (!IsPlayer)
                    Destroy(gameObject);
            });
        }
    }
}